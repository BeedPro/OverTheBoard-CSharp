using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverTheBoard.Data;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.UnitTests.Queueing
{
    [TestClass]
    public class RankedGameQueueUnitTest
    {
        private RankedGameQueueBuilder builder;

        [TestInitialize]
        public void Initialize()
        {
            builder = new RankedGameQueueBuilder();
        }


        [TestMethod]
        public async Task GetQueueGame_Should_Queue_The_RankedGame()
        {
            var result = builder.Build().GetQueueGame(new TournamentQueueItem()
                {Identifier = Guid.NewGuid(), UserId = GameConstantsTest.UserId, Level = 1});

            result.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task GetQueueGame_Should_Return_Paired_QueueItem()
        {
            var sut = builder.Build();

            var result1 = sut.GetQueueGame(new TournamentQueueItem()
                { Identifier = Guid.NewGuid(), UserId = GameConstantsTest.UserId, Level = 1 });

            var result2 = sut.GetQueueGame(new TournamentQueueItem()
                { Identifier = Guid.NewGuid(), UserId = GameConstantsTest.OpponentUserId, Level = 1 });

            result2.Count.Should().Be(2);
        }
        
        [TestMethod]
        public async Task GetQueueGame_Should_Not_pair_Same_User()
        {
            var sut = builder.Build();

            var result1 = sut.GetQueueGame(new TournamentQueueItem()
                { Identifier = Guid.NewGuid(), UserId = GameConstantsTest.UserId, Level = 1 });

            var result2 = sut.GetQueueGame(new TournamentQueueItem()
                { Identifier = Guid.NewGuid(), UserId = GameConstantsTest.UserId, Level = 1 });

            result2.Count.Should().Be(0);
        }

        
        [TestMethod]
        public async Task GetQueueGame_Should_Not_pair_Different_Level()
        {
            var sut = builder.Build();

            var result1 = sut.GetQueueGame(new TournamentQueueItem()
                { Identifier = Guid.NewGuid(), UserId = GameConstantsTest.UserId, Level = 1 });

            var result2 = sut.GetQueueGame(new TournamentQueueItem()
                { Identifier = Guid.NewGuid(), UserId = GameConstantsTest.OpponentUserId, Level = 2 });

            result2.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task GetQueueGame_Should_Remove_When_Paird()
        {
            var sut = builder.Build();

            var result1 = sut.GetQueueGame(new TournamentQueueItem()
                { Identifier = Guid.NewGuid(), UserId = GameConstantsTest.UserId, Level = 1 });

            var result2 = sut.GetQueueGame(new TournamentQueueItem()
                { Identifier = Guid.NewGuid(), UserId = GameConstantsTest.OpponentUserId, Level = 1});

            var result3 = sut.GetQueueGame(new TournamentQueueItem()
                { Identifier = Guid.NewGuid(), UserId = GameConstantsTest.OpponentUserId, Level = 1 });


            result3.Count.Should().Be(0);
        }

    }



    internal class RankedGameQueueBuilder
    {

        public TournamentQueue Build()
        {
            WithDefault();
            IRepository<TournamentQueueEntity> repository = new Repository<TournamentQueueEntity>(ContextMock.GetApplicationDbContext());
            var sut = new TournamentQueue(repository);
            return sut;
        }

        public RankedGameQueueBuilder WithDefault()
        {
            return this;
        }
    }


}

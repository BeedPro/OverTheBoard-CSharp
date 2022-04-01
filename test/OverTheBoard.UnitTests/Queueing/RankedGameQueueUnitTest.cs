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
using OverTheBoard.Infrastructure.Services;
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
        public async Task AddQueueAsync_Should_Add_To_The_Queue()
        {
            var result = await builder.Build().AddQueueAsync(new TournamentQueueItem() {UserId = GameConstantsTest.UserId, Level = 1});
            result.Should().BeTrue();
        }


        [TestMethod]
        public async Task GetAvailableLevels_Should_Return_All_Available_Levels_In_The_Queue()
        {
            var result = await builder.WithTwoLevels().Build().GetAvailableLevels();
            result.Count.Should().Be(2);
            result.FirstOrDefault().Should().Be(1);
            result.LastOrDefault().Should().Be(2);
        }

        [TestMethod]
        public async Task HasRequiredPlayersInLevel_Should_Return_True_If_Enough_Player_To_Create_Tournament()
        {
            var result = await builder.WithTwoLevels().Build().HasRequiredPlayersInLevel(4, 1);
            result.Should().BeTrue();

        }

        [TestMethod]
        public async Task HasRequiredPlayersInLevel_Should_Return_False_If_There_Is_No_Enough_Player_To_Create_Tournament()
        {
            var result = await builder.WithTwoLevels().Build().HasRequiredPlayersInLevel(4, 2);
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task GetGameQueueItems_Should_Return_Requested_Playes_For_A_Level()
        {
            var results = await builder.WithTwoLevels().Build().GetGameQueueItems(4, 1);
            results.Count.Should().Be(4);
        }

        [TestMethod]
        public async Task GetGameQueueItems_Should_Return_Empty_Playes_If_No_Enough_Queued()
        {
            var results = await builder.WithTwoLevels().Build().GetGameQueueItems(4, 2);
            results.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task RemoveGameQueueItems_Should_Remove_Players_From_Queue()
        {
            var sut = builder.WithTwoLevels().Build();
            var results = await sut.GetGameQueueItems(4, 1);
            await sut.RemoveGameQueueItems(results);
            var result = await sut.HasRequiredPlayersInLevel(4, 1);
            result.Should().BeFalse();
        }



    }



    internal class RankedGameQueueBuilder
    {
        private ApplicationDbContext _applicationDbContext;

        public RankedGameQueueBuilder()
        {
            _applicationDbContext = ContextMock.GetApplicationDbContext();
        }

        public TournamentQueue Build()
        {
            WithDefault();
            IRepository<TournamentQueueEntity> repository = new Repository<TournamentQueueEntity>(_applicationDbContext);
            var sut = new TournamentQueue(repository);
            return sut;
        }

        public RankedGameQueueBuilder WithDefault()
        {
            return this;
        }
        public RankedGameQueueBuilder WithTwoLevels()
        {
            _applicationDbContext.TournamentQueues.Add(new TournamentQueueEntity() {Level = 1});
            _applicationDbContext.TournamentQueues.Add(new TournamentQueueEntity() {Level = 1});
            _applicationDbContext.TournamentQueues.Add(new TournamentQueueEntity() {Level = 1});
            _applicationDbContext.TournamentQueues.Add(new TournamentQueueEntity() {Level = 1});
            _applicationDbContext.TournamentQueues.Add(new TournamentQueueEntity() {Level = 1});
            _applicationDbContext.TournamentQueues.Add(new TournamentQueueEntity() {Level = 2});
            _applicationDbContext.TournamentQueues.Add(new TournamentQueueEntity() {Level = 2});
            _applicationDbContext.SaveChanges();
            return this;
        }
        
    }


}

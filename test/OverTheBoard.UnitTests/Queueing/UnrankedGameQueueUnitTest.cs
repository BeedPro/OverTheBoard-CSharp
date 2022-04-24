using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.UnitTests.Queueing
{
    [TestClass]
    public class UnrankedGameQueueUnitTest
    {
        private UnrankedGameQueueBuilder builder;

        [TestInitialize]
        public void Initialize()
        {
            builder = new UnrankedGameQueueBuilder();
        }


        [TestMethod]
        public async Task GetQueueGame_Should_Add_To_The_Queue_If_There_Is_No_Maching_Player()
        {
            var sut = builder.Build();
            var result = sut.GetQueueGame(new UnrankedGameQueueItem() {ConnectionId = "abc1", UserId = "User01"});
            result.Count.Should().Be(0);
            sut.Queue.Count.Should().Be(1);
        }

        [TestMethod]
        public async Task GetQueueGame_Should_Return_Matching_Players_And_Remove_From_Queue()
        {
            var sut = builder.Build();
            sut.GetQueueGame(new UnrankedGameQueueItem() { ConnectionId = "abc1", UserId = "User01" });
            var result = sut.GetQueueGame(new UnrankedGameQueueItem() { ConnectionId = "abc2", UserId = "User02" });
            result.Count.Should().Be(2);
            sut.Queue.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task GetQueueGame_Should_Not_Match_Same_Player()
        {
            var sut = builder.Build();
            sut.GetQueueGame(new UnrankedGameQueueItem() { ConnectionId = "abc1", UserId = "User01" });
            var result = sut.GetQueueGame(new UnrankedGameQueueItem() { ConnectionId = "abc2", UserId = "User01" });
            result.Count.Should().Be(0);
            sut.Queue.Count.Should().Be(1);
        }

        [TestMethod]
        public async Task RemoveQueueGame_Should_Remove_From_The_Queue()
        {
            var sut = builder.Build();
            sut.GetQueueGame(new UnrankedGameQueueItem() { ConnectionId = "abc1", UserId = "User01" });
            sut.RemoveQueueGame("abc1");
            sut.Queue.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task RemoveQueueGame_Should_Return_False_If_Not_Found_In_The_Queue()
        {
            var sut = builder.Build();
            sut.GetQueueGame(new UnrankedGameQueueItem() { ConnectionId = "abc1", UserId = "User01" });
            var result = sut.RemoveQueueGame("abc2");
            result.Should().BeFalse();
        }


    }



    internal class UnrankedGameQueueBuilder
    {

        public UnrankedGameQueueBuilder()
        {
        }

        public UnrankedGameQueue Build()
        {
            WithDefault();

            var sut = new UnrankedGameQueue();


            return sut;
        }

        public UnrankedGameQueueBuilder WithDefault()
        {
            return this;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverTheBoard.Infrastructure.Common;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.UnitTests.Common
{
    [TestClass]
    public class RoundRobinEngineUnitTest
    {
        private RoundRobinEngineBuilder builder;

        [TestInitialize]
        public void Initialize()
        {
            builder = new RoundRobinEngineBuilder();
        }
        
        [TestMethod]
        public async Task BuildMatches_With_Empty_List_Should_Return_Empty_Division()
        {
            var result = builder.Build().BuildMatches(new List<GameQueueItem>(), 1);
            result.Count().Should().Be(0);
        }

        [TestMethod]
        [DataRow(2, 1, 1)]
        [DataRow(3, 1, 3)]
        [DataRow(4, 1, 6)]
        [DataRow(5, 1, 10)]
        [DataRow(6, 1, 15)]
        [DataRow(2, 2, 2)]
        [DataRow(3, 2, 6)]
        [DataRow(4, 3, 18)]
        [DataRow(5, 4, 40)]
        [DataRow(6, 5, 75)]
        public async Task BuildMatches_With_Players_Should_Return_Games(int numberOfPlayers, int numberOfGames, int expectedMatches)
        {
            var result = builder.Build().BuildMatches(builder.GetGameQueueItems(numberOfPlayers), numberOfGames);
            result.Count().Should().Be(expectedMatches);
            var isBothSameColours = result.Any(e => e.GameQueueItems[0].Colour == e.GameQueueItems[1].Colour);
            isBothSameColours.Should().BeFalse();
        }
        
        

    }



    internal class RoundRobinEngineBuilder
    {

        public RoundRobinEngineBuilder()
        {
        }

        public RoundRobinEngine Build()
        {
            WithDefault();

            var sut = new RoundRobinEngine();


            return sut;
        }

        public RoundRobinEngineBuilder WithDefault()
        {
            return this;
        }
        
        public List<GameQueueItem> GetGameQueueItems(int count)
        {
            var retvalue = new List<GameQueueItem>();

            for (int i = 1; i <= count; i++)
            {
                retvalue.Add(new GameQueueItem() { UserId = $"P{i:00}" });
            }

            //
            return retvalue;
        }
    }


}

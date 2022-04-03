using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Common
{
    public class RoundRobinEngine
    {
        private static readonly List<string> Colours = new List<string> { "white", "black" };

        public List<DivisionItem> BuildMatches<TType>(List<TType> players, int numberOfIteration) where TType : GameQueueItem
        {
            var matches = new List<DivisionItem>();
            int startRound = 0;
            for (int index = 0; index < numberOfIteration; index++)
            {
                var items = BuildMatches(players, startRound, index, numberOfIteration);
                if (!items.Any())
                {
                    return matches;
                }
                matches.AddRange(items);
                startRound = items.Max(e => e.RoundNumber) + 1;
            }

            return matches;
        }


        public List<DivisionItem> BuildMatches<TType>(List<TType> players, int startRound, int iterationIndex, int numberOfIteration) where TType : GameQueueItem
        {
            var matches = new List<DivisionItem>();
            if (players == null || players.Count < 2)
            {
                return matches;
            }

            var restPlayers = new List<GameQueueItem>(players.Skip(1));
            var playersCount = players.Count;
            if (players.Count % 2 != 0)
            {
                restPlayers.Add(default);
                playersCount++;
            }

            for (var round = startRound; round < playersCount + startRound - 1; round++)
            {
                if (restPlayers[round % restPlayers.Count]?.Equals(default) == false)
                {
                    var items = new List<GameQueueItem>()
                    {
                        Clone(players[0]),
                        Clone(restPlayers[round % restPlayers.Count])
                    };

                    SetColour(items, iterationIndex, numberOfIteration);

                    matches.Add(new DivisionItem()
                    {
                        RoundNumber = round,
                        GameQueueItems = items
                    });
                }

                for (var index = 1; index < playersCount / 2; index++)
                {
                    var firstPlayer = restPlayers[(round + index) % restPlayers.Count];
                    var secondPlayer = restPlayers[(round + restPlayers.Count - index) % restPlayers.Count];
                    if (firstPlayer?.Equals(default) == false && secondPlayer?.Equals(default) == false)
                    {
                        var items = new List<GameQueueItem>()
                        {
                            Clone(firstPlayer),
                            Clone(secondPlayer)
                        };

                        SetColour(items, iterationIndex, numberOfIteration);
                        matches.Add(new DivisionItem()
                        {
                            RoundNumber = round,
                            GameQueueItems = items
                        });
                    }
                }
            }

            return matches;
        }

        private GameQueueItem Clone(GameQueueItem team)
        {
            return new GameQueueItem() { UserId = team.UserId, Colour = team.Colour };
        }

        private void SetColour(List<GameQueueItem> queueItems, int colourIndex, int numberOfGame)
        {
            var index = colourIndex % 2;

            if (colourIndex + 1 == numberOfGame && (numberOfGame % 2) != 0)
            {
                Random rand = new Random();
                index = rand.Next(Colours.Count);
            }

            foreach (var item in queueItems)
            {
                item.Colour = Colours[index];
                index = (index + 1) % 2;
            }
        }
    }
}

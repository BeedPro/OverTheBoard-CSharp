using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Services
{
    public class EloService : IEloService
    {
        //TODO Fix EloServices Async methods

        public async Task<int[]> CalculateEloAsync(int playerOneRating, int playerTwoRating, EloOutcomesType gameOutcomeForPlayerOne, EloOutcomesType gameOutcomeForPlayerTwo, int kFactorA = GameConstants.EloK, int kFactorB = GameConstants.EloK)
        {
            var expectedResults = await PredictResultAsync(playerOneRating, playerTwoRating);
            var deltaPlayerOne = kFactorA * (GetEloOutcomeValue(gameOutcomeForPlayerOne) - expectedResults[0]);
            var deltaPlayerTwo = kFactorB * (GetEloOutcomeValue(gameOutcomeForPlayerTwo)- expectedResults[1]);

            var newEloPlayerOneRating = (int)(playerOneRating + deltaPlayerOne);
            var newEloPlayerTwoRating = (int)(playerTwoRating + deltaPlayerTwo);
            return new int[] { newEloPlayerOneRating, newEloPlayerTwoRating };
        }

        public async Task<decimal[]> PredictResultAsync(int playerOneRating, int playerTwoRating)
        {
            var eloPlayerOne = 1 / (1 + (decimal)Math.Pow(10, (double)(playerTwoRating - playerOneRating) / 400));
            var eloPlayerTwo = 1 / (1 + (decimal)Math.Pow(10, (double)(playerOneRating - playerTwoRating) / 400));
            return new decimal[] { eloPlayerOne, eloPlayerTwo };
        }

        public async Task<List<GamePlayerEloOutcomes>> CalculateEloRatingChangeAsync(int currentPlayerRating, int opponentPlayerRating)
        {
            var outcomes = new List<GamePlayerEloOutcomes>()
            {
                new GamePlayerEloOutcomes()
                {
                    Type   = EloOutcomesType.Win,
                    Value =  await CalculateDeltaElo(currentPlayerRating, opponentPlayerRating,
                        GetEloOutcomeValue(EloOutcomesType.Win))
                },
                new GamePlayerEloOutcomes()
                {

                    Type   = EloOutcomesType.Lose,
                    Value =  await CalculateDeltaElo(currentPlayerRating, opponentPlayerRating,
                        GetEloOutcomeValue(EloOutcomesType.Lose))
                },

                new GamePlayerEloOutcomes()
                {

                    Type   = EloOutcomesType.Draw,
                    Value =  await CalculateDeltaElo(currentPlayerRating, opponentPlayerRating,
                        GetEloOutcomeValue(EloOutcomesType.Draw))
                }
            };

            return outcomes;
        }
        private async Task<int> CalculateDeltaElo(int currentPlayerRating, int opponentPlayerRating, decimal gameOutcomeForCurrentPlayer, int kFactor = GameConstants.EloK)
        {
            var expectedResults = await PredictResultAsync(currentPlayerRating, opponentPlayerRating);
            var delta = (int)(kFactor * (gameOutcomeForCurrentPlayer - expectedResults[0]));
            return delta;
        }
        private decimal GetEloOutcomeValue(EloOutcomesType type)
        {
            switch (type)
            {
                case EloOutcomesType.Win:
                    return (decimal) 1.0;
                case EloOutcomesType.Draw:
                    return (decimal) 0.5;
                case EloOutcomesType.Lose:
                    return 0;
                default:
                    throw new Exception("EloOutcomeType not valid");
            }
        }
    }
}
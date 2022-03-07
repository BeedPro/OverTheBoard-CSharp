using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Services
{
    public class EloService : IEloService
    {
        public async Task<GamePlayerRatings> CalculateEloAsync(int playerWhiteRating, int playerBlackRating, EloOutcomesType gameOutcomeForPlayerWhite, EloOutcomesType gameOutcomeForPlayerBlack, int kFactorA = GameConstants.EloK, int kFactorB = GameConstants.EloK)
        {
            var expectedResults = await PredictResultAsync(playerWhiteRating, playerBlackRating);
            var deltaPlayerWhite = kFactorA * (GetEloOutcomeValue(gameOutcomeForPlayerWhite) - expectedResults[0]);
            var deltaPlayerBlack = kFactorB * (GetEloOutcomeValue(gameOutcomeForPlayerBlack)- expectedResults[1]);

            var newEloWhiteRating = (int)(playerWhiteRating + deltaPlayerWhite);
            var newEloBlackRating = (int)(playerBlackRating + deltaPlayerBlack);
            return new GamePlayerRatings() { WhitePlayerRating = newEloWhiteRating, BlackPlayerRating = newEloBlackRating };
        }

        //TODO: Make sure to return Array into an object with whitePlayerRating, blackPlayerRating or use ref it
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
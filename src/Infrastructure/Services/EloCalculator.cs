using System;
using System.Collections.Generic;
using System.Text;
using OverTheBoard.Data.Entities.Applications;

namespace OverTheBoard.Infrastructure.Services
{
    //TODO Delete this class
    public class EloCalculator
    {
        public static int CalculateDeltaElo(int currentPlayerRating, int opponentPlayerRating, decimal gameOutcomeForCurrentPlayer,int kFactor = GameConstants.EloK)
        {
            var expectedResults = PredictResult(currentPlayerRating, opponentPlayerRating);
            var delta = (int) (kFactor * (gameOutcomeForCurrentPlayer - expectedResults[0]));
            return delta;
        }

        public static int[] CalculateElo(int playerOneRating, int playerTwoRating, decimal gameOutcomeForPlayerOne, decimal gameOutcomeForPlayerTwo, int kFactorA = GameConstants.EloK, int kFactorB = GameConstants.EloK)
        {
            var expectedResults = PredictResult(playerOneRating, playerTwoRating);
            var deltaPlayerOne = kFactorA * (gameOutcomeForPlayerOne - expectedResults[0]);
            var deltaPlayerTwo = kFactorB * (gameOutcomeForPlayerTwo - expectedResults[1]);

            var newEloPlayerOneRating = (int)(playerOneRating + deltaPlayerOne);
            var newEloPlayerTwoRating = (int)(playerTwoRating + deltaPlayerTwo);
            return new int[] { newEloPlayerOneRating, newEloPlayerTwoRating };
        }

        public static decimal[] PredictResult(int playerOneRating, int playerTwoRating)
        {
            var eloPlayerOne = 1 / (1 + (decimal)Math.Pow(10, (double)(playerTwoRating - playerOneRating) / 400));
            var eloPlayerTwo = 1 / (1 + (decimal)Math.Pow(10, (double)(playerOneRating - playerTwoRating) / 400));
            return new decimal[] { eloPlayerOne, eloPlayerTwo };
        }
    }
}
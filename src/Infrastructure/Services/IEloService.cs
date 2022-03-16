
using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Services
{
    public interface IEloService
    {
        Task<GamePlayerRatings> CalculateEloAsync(int playerOneRating, int playerTwoRating, EloOutcomesType gameOutcomeForPlayerOne,
            EloOutcomesType gameOutcomeForPlayerTwo, int kFactorA = GameConstants.EloK,
            int kFactorB = GameConstants.EloK);

        Task<decimal[]> PredictResultAsync(int playerOneRating, int playerTwoRating);
        Task<List<GamePlayerEloOutcomes>> CalculateEloRatingChangeAsync(int currentPlayerRating, int opponentPlayerRating);
    }
}
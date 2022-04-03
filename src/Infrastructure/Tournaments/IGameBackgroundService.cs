using System.Threading.Tasks;

namespace OverTheBoard.Infrastructure.Tournaments
{
    public interface IGameBackgroundService
    {
        Task<bool> ExecuteAsync();
    }
    
   
}
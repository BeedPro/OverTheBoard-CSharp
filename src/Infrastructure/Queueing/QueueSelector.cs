using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface IQueueSelector
    {
        IGameQueue GetQueue(string type);
    }

    public class QueueSelector : IQueueSelector
    {
        private readonly IEnumerable<IGameQueue> _gameQueues;

        public QueueSelector(IEnumerable<IGameQueue> gameQueues)
        {
            _gameQueues = gameQueues;
        }

        public IGameQueue GetQueue(string type)
        {
            return _gameQueues.FirstOrDefault(e => e.Name.Equals(type, StringComparison.OrdinalIgnoreCase));
        }
    }

    public interface IGameQueue
    {
        string Name { get; }
        List<GamePlayer> GetQueueGame(string userId, InitialisationMessage message);
    }
}

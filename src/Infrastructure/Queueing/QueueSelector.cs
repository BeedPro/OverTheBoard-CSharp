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

    public class UnrankedGameQueue : IGameQueue
    {
        public string Name => GameType.Unranked.ToString();

        public Queue<InitialisationMessage> Queue = new Queue<InitialisationMessage>();

        public List<GamePlayer> GetQueueGame(string userId, InitialisationMessage message)
        {
            if(Queue.Any())
            {
                var item = Queue.Dequeue();
                if (!item.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
                {
                    return new List<GamePlayer>()
                    {
                        new GamePlayer()
                        {
                            UserId = message.UserId, GameId = message.GameId,
                            ConnectionId = message.ConnectionId, Colour = "white"
                        },
                        new GamePlayer()
                        {
                            UserId = item.UserId, GameId = item.GameId, ConnectionId = item.ConnectionId,
                            Colour = "black"
                        }
                    };
                }
            }

            message.UserId = userId;
            Queue.Enqueue(message);
            return null;
        }
    }
}

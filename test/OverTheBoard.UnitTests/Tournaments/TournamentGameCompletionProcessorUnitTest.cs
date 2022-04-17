using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OverTheBoard.Data.Entities;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.Infrastructure.Tournaments.Processors;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.UnitTests.Tournaments
{
    [TestClass]
    public class TournamentGameCompletionProcessorUnitTest
    {
        private TournamentGameCompletionProcessorBuilder builder;

        [TestInitialize]
        public void Initialize()
        {
            builder = new TournamentGameCompletionProcessorBuilder();
        }

        [TestMethod]
        public async Task ProcessAsync_Should_Not_Process_If_Tournament_Is_Not_Finished()
        {
            var result = await builder.WithHasFinished(false).Build().ProcessAsync(new GameCompletionQueueItem());
            result.Should().BeFalse();
        }
        
        [TestMethod]
        public async Task ProcessAsync_Should_Process_If_Tournament_Is_Not_Finished()
        {
            var result = await builder.WithHasFinished(true).WithTournament().Build().ProcessAsync(new GameCompletionQueueItem());
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task ProcessAsync_Ranked_Up_For_The_Top_Scorer()
        {
            var result = await builder
                .WithHasFinished(true)
                .WithTournament()
                .Build().ProcessAsync(new GameCompletionQueueItem(){Level = 1});

            builder.tournamentQueueMock.Verify(m=>m.AddQueueAsync(It.Is<TournamentQueueItem>(e=>e.Level == 2)), Times.Once);
            builder.userServiceMock.Verify(m=>m.UpdateUserRankAsync(It.IsAny<string>(), It.Is<UserRank>(e=>e == UserRank.Level02)), Times.Once);
        }
        
        [TestMethod]
        public async Task ProcessAsync_Ranked_Stayed_Same_For_The_Bottom_Scorer_if_In_rank_1()
        {
            var result = await builder
                .WithHasFinished(true)
                .WithTournament()
                .Build().ProcessAsync(new GameCompletionQueueItem(){Level = 1});

            builder.tournamentQueueMock.Verify(m=>m.AddQueueAsync(It.Is<TournamentQueueItem>(e=>e.Level == 1)), Times.Exactly(3));
            builder.userServiceMock.Verify(m=>m.UpdateUserRankAsync(It.IsAny<string>(), It.Is<UserRank>(e=>e == UserRank.Level01)), Times.Once);
        }
        
        [TestMethod]
        public async Task ProcessAsync_Ranked_Down_For_The_Bottom_Scorer()
        {
            var result = await builder
                .WithHasFinished(true)
                .WithTournament()
                .Build().ProcessAsync(new GameCompletionQueueItem(){Level = 4});

            builder.tournamentQueueMock.Verify(m=>m.AddQueueAsync(It.Is<TournamentQueueItem>(e=>e.Level == 3)), Times.Once);
            builder.userServiceMock.Verify(m=>m.UpdateUserRankAsync(It.IsAny<string>(), It.Is<UserRank>(e=>e == UserRank.Level03)), Times.Once);
        }



    }



    internal class TournamentGameCompletionProcessorBuilder
    {
        public Mock<IGameService> gameServiceMock;
        public Mock<ITournamentService> tournamentServiceMock;
        public Mock<ITournamentQueue> tournamentQueueMock;
        public Mock<IUserService> userServiceMock;
        public Mock<ILogger<TournamentGameCompletionProcessor>> loggerMock;

        public TournamentGameCompletionProcessorBuilder()
        {
            gameServiceMock = new Mock<IGameService>();
            tournamentServiceMock = new Mock<ITournamentService>();
            tournamentQueueMock = new Mock<ITournamentQueue>();
            userServiceMock = new Mock<IUserService>();
            loggerMock = new Mock<ILogger<TournamentGameCompletionProcessor>>();
        }

        public TournamentGameCompletionProcessor Build()
        {
            WithDefault();

            var sut = new TournamentGameCompletionProcessor(
                gameServiceMock.Object,
                tournamentServiceMock.Object,
                tournamentQueueMock.Object,
                userServiceMock.Object,
                loggerMock.Object);

            return sut;
        }

        public TournamentGameCompletionProcessorBuilder WithDefault()
        {
            gameServiceMock.Setup(m => m.GetChessGameWithPlayersAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new ChessGame(){Identifier = GameConstantsTest.Identifier, TournamentId = GameConstantsTest.TournamentIdentifier }));

            return this;
        }
        
        public TournamentGameCompletionProcessorBuilder WithHasFinished(bool expected)
        {
            gameServiceMock.Setup(m => m.HasAllGameFinishedForTournamentAsync(It.IsAny<string>())).Returns(Task.FromResult(expected));

            return this;
        }
        
        public TournamentGameCompletionProcessorBuilder WithTournament()
        {
            var tournament = new Tournament();
            tournament.Players = new List<TournamentPlayer>()
            {
                new TournamentPlayer(){UserId = "User01"},
                new TournamentPlayer(){UserId = "User02"},
                new TournamentPlayer(){UserId = "User03"},
                new TournamentPlayer(){UserId = "User04"},
            };

            tournamentServiceMock.Setup(m => m.GetTournamentAsync(It.IsAny<string>())).Returns(Task.FromResult(tournament));

            var chessGames = new List<ChessGame>();
            chessGames.Add(new ChessGame(){Players = new List<GamePlayer>(){new GamePlayer(){UserId = "User01", Outcome = EloOutcomesType.Win.ToString()}, new GamePlayer(){ UserId = "User02", Outcome = EloOutcomesType.Lose.ToString() } }});
            chessGames.Add(new ChessGame(){Players = new List<GamePlayer>(){new GamePlayer(){UserId = "User03", Outcome = EloOutcomesType.Win.ToString()}, new GamePlayer(){ UserId = "User04", Outcome = EloOutcomesType.Lose.ToString() } }});
            chessGames.Add(new ChessGame(){Players = new List<GamePlayer>(){new GamePlayer(){UserId = "User01", Outcome = EloOutcomesType.Win.ToString()}, new GamePlayer(){ UserId = "User03", Outcome = EloOutcomesType.Lose.ToString() } }});
            chessGames.Add(new ChessGame(){Players = new List<GamePlayer>(){new GamePlayer(){UserId = "User02", Outcome = EloOutcomesType.Draw.ToString()}, new GamePlayer(){ UserId = "User04", Outcome = EloOutcomesType.Draw.ToString() } }});
            chessGames.Add(new ChessGame(){Players = new List<GamePlayer>(){new GamePlayer(){UserId = "User02", Outcome = EloOutcomesType.Draw.ToString()}, new GamePlayer(){ UserId = "User03", Outcome = EloOutcomesType.Draw.ToString() } }});


            gameServiceMock.Setup(m => m.GetMatchesByTournamentAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(chessGames));



            return this;
        }
    }


}

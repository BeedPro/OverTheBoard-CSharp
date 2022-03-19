(function ($) {
    $.fn.chessmove = function (options) {
        var $self = $(this);
        const userRatingDiv = document.getElementById('currRating');
        const oppRatingDiv = document.getElementById('oppRating');

        $self.settings = $.extend({ Id: '#checkConnection'}, options);
        $self.control = $($self.settings.Id);

        $self.connection = new signalR.HubConnectionBuilder().withUrl("/piece-move").build();
        $self.connection.start()
            .then(function () {
                $($self.settings.Id).show();
            })
            .then(function () {

                var obj = {
                    GameId: $self.control.data("instance-id"),
                    type: $self.control.data("type"),
                    connectionId: $self.connection.connectionId
                };

                $self.connection.invoke("Initialise", obj).catch(function (err) {
                    return console.error(err.toString());
                });

            })
            .catch(function (err) {
                $($self.settings.Id).show();
                return console.error(err.toString());
            });

        $self.connection.on("Receive", function (move) {
            $($self.settings.Id).trigger("chess_moved", move);
        });

        $($self.settings.Id).once("chess_move", function (event, move) {
            $self.connection.invoke("Send", move).catch(function (err) {
                return console.error(err.toString());
            });
        });

        $($self.settings.Id).once("send_gameStatus", function (event, gameOverStatus) {
            gameOverStatus.GameId = $self.control.data("instance-id");
            gameOverStatus.ConnectionId = $self.connection.connectionId;
            $self.connection.invoke("SendGameStatus", gameOverStatus).catch(function (err) {
                return console.error(err.toString());
            });
        });

        //TODO get a receive from the HUB.
        $self.connection.on("ReceiveRatings", function (gameRatings) {
            userRatingDiv.textContent = gameRatings.WhitePlayerRating.toString();
            oppRatingDiv.textContent = gameRatings.BlackPlayerRating.toString();
        });

        $self.connection.on("Initialised", function (move) {
            $($self.settings.Id).trigger("chess_init", move);
            $($self.settings.Id).hide();
        });


        return this;
    };
}(jQuery));

$(function () {
    $.fn.chessmove();
});

/*R ando m athe ath aba wetha athlawhgt*/
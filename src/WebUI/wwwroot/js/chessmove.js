(function ($) {
    $.fn.chessmove = function (options) {
        var $self = $(this);

        $self.settings = $.extend({Id: '#divProcessor'}, options);
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

        $self.connection.on("Receive", function (user, message) {
            $($self.settings.Id).trigger("chess_moved", message);
        });

        $($self.settings.Id).once("chess_move", function (event, message) {
            $self.connection.invoke("Send", "user", message).catch(function (err) {
                return console.error(err.toString());
            });
        });

        $self.connection.on("Initialised", function (message) {
            $($self.settings.Id).trigger("chess_init", message);
            $($self.settings.Id).hide();
        });


        return this;
    };
}(jQuery));
(function ($) {
    $.fn.chessmove = function (options) {
        var $self = $(this);

        $self.settings = $.extend({}, options);

        $self.connection = new signalR.HubConnectionBuilder().withUrl("/piece-move").build();

        $self.connection.on("Receive", function (user, message) {
            $(document).trigger("chess_moved", message);
        });


        $self.connection.start()
            .then(function () {

            })
            .catch(function (err) {
                return console.error(err.toString());
            });

        $(document).once("chess_move", function (event, message) {
            $self.connection.invoke("Send", "user", message).catch(function (err) {
                return console.error(err.toString());
            });
        });

        return this;
    };
}(jQuery));
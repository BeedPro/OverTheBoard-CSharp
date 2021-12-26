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
                $('#connectionId').html($self.connection.connectionId);
            })
            .catch(function (err) {
                return console.error(err.toString());
            });

        $(document).once("chess_move", function (event, message) {
            $self.connection.invoke("Send", "user", message).catch(function (err) {
                return console.error(err.toString());
            });
        });

        $self.connection.on("Registered", function (message) {
            $('#connectionId').html(message);
        });


        $('#clickButton').click(function () {

            $self.connection.invoke("Register", "Annoor is good programmer!!!").catch(function (err) {
                return console.error(err.toString());
            })
        });

        return this;
    };
}(jQuery));
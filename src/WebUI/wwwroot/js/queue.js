(function ($) {
    $.fn.queueuser = function (options) {
        var $self = $(this);

        $self.settings = $.extend({}, options);

        $self.connection = new signalR.HubConnectionBuilder().withUrl("/queue").build();

        $self.connection.on("Play", function (message) {
            window.location = "/play/game/" + message;
        });


        $self.connection.start()
            .then(function () {

            })
            .catch(function (err) {
                return console.error(err.toString());
            });

        $('#btnQueue').click(function (event) {
            $self.connection.invoke("Queue", $self.connection.connectionId).catch(function (err) {
                return console.error(err.toString());
            });
            $('#divProgress').show();
            event.preventDefault();
        });
        return this;
    };
}(jQuery));

$(function () {
    $.fn.queueuser();
});
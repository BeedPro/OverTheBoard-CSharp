(function ($) {
    $.fn.queueuser = function (options) {
        var $self = $(this);

        $self.settings = $.extend({}, options);

        $self.connection = new signalR.HubConnectionBuilder().withUrl("/queue").build();

        $self.connection.on("Receive", function (user, message) {
            $(document).trigger("queue_user", message);
        });


        $self.connection.start()
            .then(function () {

            })
            .catch(function (err) {
                return console.error(err.toString());
            });

        $(document).once("queue_user", function (event, message) {
            $self.connection.invoke("Send", "user", message).catch(function (err) {
                return console.error(err.toString());
            });
        });

        return this;
    };
}(jQuery));
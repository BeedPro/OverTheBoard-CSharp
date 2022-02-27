// JS for gameTimer
(function ($) {
    $.fn.gameTimer = function (options) {
        var $self = $(this);


        $self.settings = $.extend({
            initial_time : 10 * 60 * 15,
            player1css : 'player1clock',
            player2css : 'player2clock'
        }, options);

        $self.gameTimer = null;
        $self.formatTime = function (tenths) {
            var minutes = String(Math.floor(tenths / 600));
            var seconds = String(Math.floor(tenths / 10) % 60);
            if (seconds.length == 1) { seconds = '0' + seconds; }
            var decimal = String(tenths % 10);

            // if we have 0 minutes then timer is formated to "00:[Seconds].[decimal]"
            return (minutes != '0') ? (minutes + ':' + seconds) : ("00:" + seconds + '.' + decimal);
        }

        var Player = function (clock_id, initial_time, gameTimer) {
            this.clock = document.getElementById(clock_id);
            this.initial_time = initial_time;
            this.time = this.initial_time;
            this.gameTimer = gameTimer;
            this.opponent = null;
            this.play = function () {
                var opponent = this.opponent;
                var gameTimer = this.gameTimer;
                if (!gameTimer.gameTimer_over) {
                    clearTimeout(gameTimer.timer_loop);
                    gameTimer.timer_loop = setInterval(function () {
                        opponent.time -= 1;
                        if (opponent.time == 0) {
                            clearTimeout(gameTimer.timer_loop);
                            gameTimer.gameTimer_over = true;
                            $($self).trigger('gameFlagged');
                        }
                        gameTimer.displayTimers();
                    }, 100);
                    opponent.clock.className = 'now_playing';
                    this.clock.className = '';
                }
            }
        }

        var GameTimer = function (initial_time, p1_clock_id, p2_clock_id) {
            this.player1 = new Player(p1_clock_id, initial_time, this);
            this.player2 = new Player(p2_clock_id, initial_time, this);
            this.player1.opponent = this.player2;
            this.player2.opponent = this.player1;
            this.timer_loop = null;
            this.gameTimer_over = false;

            this.resetClocks = function () {
                clearTimeout(this.timer_loop);
                this.player1.clock.className = '';
                this.player2.clock.className = '';
                this.player1.time = this.player1.initial_time;
                this.player2.time = this.player2.initial_time;
                this.gameTimer_over = false;
                this.displayTimers();
            }

            this.displayTimers = function () {
                this.player1.clock.innerHTML = $self.formatTime(this.player1.time);
                this.player2.clock.innerHTML = $self.formatTime(this.player2.time);
            }
            this.changePlay = function (timerInfo) {

                var playerColour = '';
                if (timerInfo.orientation === 'black' && timerInfo.turn == 'b') {
                    playerColour = 'w';
                }
                else if (timerInfo.orientation === 'black' && timerInfo.turn === 'w') {
                    playerColour = 'b';
                } else {
                    playerColour = timerInfo.turn;
                }

                // Player1 Move then start timer of player2
                if ('w'.indexOf(playerColour) != -1) {
                    this.player1.play();
                }
                // Player2 Move then start timer of player1
                else if (("b".indexOf(playerColour) != -1)) {
                    this.player2.play();
                }
            }
        }

        $(this).once("change_colour", function (event, playerColour) {
            gameTimer.changePlay(playerColour);
        });

        gameTimer = new GameTimer($self.settings.initial_time, $self.settings.player1css, $self.settings.player2css);
        gameTimer.resetClocks();
        return this;
    }

    $.fn.once = function (a, b) {
        return this.each(function () {
            $(this).off(a).on(a, b);
        });
    };
}(jQuery));

$(function () {
    $('#checkConnection').gameTimer({
        initial_time: 9000,
        player1css: 'player1clock',
        player2css: 'player2clock'
    });
});
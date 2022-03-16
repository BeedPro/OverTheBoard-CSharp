// JS for gameTimer
(function ($) {
    $.fn.gameTimer = function (options) {
        var $self = $(this);


        $self.settings = $.extend({
            initial_time : 10 * 60 * 15,
            whiteTimercss: 'whiteTimercss',
            blackTimercss: 'blackTimercss'
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
                        if (opponent.time <= 0) {
                            clearTimeout(gameTimer.timer_loop);
                            gameTimer.gameTimer_over = true;
                            $($self).trigger('gameFlagged');
                            opponent.time = 0;
                        }
                        gameTimer.displayTimers();
                    }, 100);
                    opponent.clock.className = 'now_playing';
                    this.clock.className = '';
                }
            }
        }

        var GameTimer = function (initial_time, p1_clock_id, p2_clock_id) {
            this.whiteTimer = new Player(p1_clock_id, initial_time, this);
            this.blackTimer = new Player(p2_clock_id, initial_time, this);
            this.whiteTimer.opponent = this.blackTimer;
            this.blackTimer.opponent = this.whiteTimer;
            this.timer_loop = null;
            this.gameTimer_over = false;

            this.resetClocks = function (times) {
                clearTimeout(this.timer_loop);
                this.whiteTimer.clock.className = '';
                this.blackTimer.clock.className = '';
                this.whiteTimer.time = times.whiteTime;
                this.blackTimer.time = times.blackTime;
                this.gameTimer_over = false;
                this.displayTimers();
                this.changePlay({
                    orientation: times.orientation,
                    turn: times.turn
                })
            }


            
            this.displayTimers = function () {
                this.whiteTimer.clock.innerHTML = $self.formatTime(this.whiteTimer.time);
                this.blackTimer.clock.innerHTML = $self.formatTime(this.blackTimer.time);
            }
            this.changePlay = function (timerInfo) {

                var playerColour = timerInfo.turn;
                // whiteTimer Move then start timer of blackTimer
                if ('b'.indexOf(playerColour) != -1) {
                    this.whiteTimer.play();
                }
                // blackTimer Move then start timer of whiteTimer
                else if (("w".indexOf(playerColour) != -1)) {
                    this.blackTimer.play();
                }
            }
        }

        $(this).once("init_timer", function (event, times) {
            gameTimer.resetClocks(times);
        });


        $(this).once("change_colour", function (event, timerInfo) {
            gameTimer.changePlay(timerInfo);
        });

        gameTimer = new GameTimer($self.settings.initial_time, $self.settings.whiteTimercss, $self.settings.blackTimercss);
        //gameTimer.resetClocks();
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
        whiteTimercss: 'whiteTimercss',
        blackTimercss: 'blackTimercss'
    });
});
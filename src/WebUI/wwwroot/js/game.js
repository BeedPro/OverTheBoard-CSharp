(function ($) {
    $.fn.play = function (options) {
        var $self = $(this);


        $self.settings = $.extend({
            Id: '#checkConnection',
            status: '#status',
            fen: '#fen',
            pgn: '#pgn',
            whiteSquareGrey: '#a9a9a9',
            blackSquareGrey: '#696969'
        }, options);

        $self.control = $($self.settings.Id);
        //$self.board = null;
        $self.game = new Chess();
        $self.gameFlagged = false;

        $self.onDragStart = function (source, piece, position, orientation) {
            if ($self.game.game_over() || $self.gameFlagged) {
                return false;
            }

            // only pick up pieces for the side to move
            if (($self.game.turn() === 'w' && piece.search(/^b/) !== -1) ||
                ($self.game.turn() === 'b' && piece.search(/^w/) !== -1)) {
                return false;
            }

            if ((orientation === 'white' && piece.search(/^b/) !== -1) ||
                (orientation === 'black' && piece.search(/^w/) !== -1)) {
                return false;
            }

        }

        $self.returnCorrectPromotion = function (input) {
            if (input === "queen") { return "q"; }
            else if (input === "knight") { return "n"; }
            else if (input === "bishop") { return "b"; }
            else if (input === "rook") { return "r"; }
            else { return input; }
        }

        $self.onDrop = function (source, target, piece) {
            var move = null;
            var promotion = null;
            const allowedPromotions = ["q", "n", "b", "k", "queen", "knight", "bishop", "rook"];
            if (target.slice(-1) === "8" && piece === "wP" || target.slice(-1) === "1" && piece === "bP") {
                promotion = prompt("Enter Queen, Knight, Bishop, Rook to promote pawn to").toLowerCase();
                while (!allowedPromotions.includes(promotion)) {
                    promotion = prompt("Previous value entered was wrong \n Please enter Queen, Knight, Bishop, Rook to promote pawn to").toLowerCase();
                }
                promotion = $self.returnCorrectPromotion(promotion);
            }
            
            // check if move is legal
            move = $self.game.move({
                from: source,
                to: target,
                promotion: promotion 
            });
            // illegal move
            if (move === null) return 'snapback';

            $self.updateStatus();

            var moveInfo = {
                fen: $self.game.fen(),
                gameId: $self.control.data("instance-id"),
                pgn: source + '-' + target
            };

            $($self.settings.Id).trigger("chess_move", moveInfo);
            $self.changeTimer();
        }

        $self.onSnapEnd = function () {
            $self.board.position($self.game.fen());
        }

        $self.changeTimer = function () {

            var timerInfo = {
                orientation: $self.board.orientation(),
                turn: $self.game.turn()
            };

            $($self.settings.Id).trigger('change_colour', timerInfo);
        }

        $self.updateStatus = function () {

            var status = '';

            var moveColor = 'White';
            if ($self.game.turn() === 'b') {
                moveColor = 'Black';
            }
            // checkmate?
            if ($self.game.in_checkmate()) {
                status = 'Game over ' + moveColor + ' is in checkmate.';
            }

            // draw?
            else if ($self.game.in_draw()) {
                status = 'Game over, drawn position';
            }

            // game still on
            else if ($self.gameFlagged) {
                status = 'Game over, ' + moveColor + ' flagged';
            } else {
                status = moveColor + ' to move';
                // check?
                if ($self.game.in_check()) {
                    status += ', ' + moveColor + ' is in check';
                }
            }
            $($self.settings.status).html(status);
        }

        $($self.settings.Id).once("chess_init", function (event, move) {

            //if (move.orientation === "black") {
            //    $($self.settings.status).html("Waiting for move");
            //}
            //else {
            //    $($self.settings.status).html("Move piece to start");
            //}

            $self.board.orientation(move.orientation);

            if (move.fen) {
                $self.game.load(move.fen);
                $self.board.position(move.fen);
            }
            $($self.settings.Id).trigger("init_timer", {
                whiteTime: move.whiteRemaining,
                blackTime: move.blackRemaining,
                orientation: $self.board.orientation(),
                turn: $self.game.turn()
            });

        });

        $($self.settings.Id).once("chess_moved", function (event, move) {
            $self.game.load(move.fen);
            $self.board.position(move.fen);
            $self.updateStatus();
            $self.changeTimer();
        });

        
        $($self.settings.Id).once('gameFlagged', function (event) {
            $self.gameFlagged = true;
            $self.updateStatus();
        });

        var config = {
            draggable: true,
            pieceTheme: '/img/chesspieces/merida/{piece}.svg',
            position: 'start',
            onDragStart: $self.onDragStart,
            onDrop: $self.onDrop,
            onSnapEnd: $self.onSnapEnd
        };

        $self.board = Chessboard('myBoard', config);
        return this;
    };

    $.fn.once = function (a, b) {
        return this.each(function () {
            $(this).off(a).on(a, b);
        });
    };

}(jQuery));

$(function () {
    $.fn.play();
});



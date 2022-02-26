


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
        $self.board = null;
        $self.game = new Chess();

        $self.onDragStart = function (source, piece, position, orientation) {
            if ($self.game.game_over()) return false;

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

        $self.onDrop = function (source, target) {
            // see if the move is legal
            var move = $self.game.move({
                from: source,
                to: target,
                promotion: 'q' // NOTE: always promote to a queen for example simplicity
            });
            // illegal move
            if (move === null) return 'snapback';

            var playerColour = '';
            if ($self.board.orientation() === 'black' && $self.game.turn() == 'b') {
                playerColour = 'w';
            }
            else if ($self.board.orientation() === 'black' && $self.game.turn() === 'w') {
                playerColour = 'b';
            } else {
                playerColour = $self.game.turn();
            }


            $self.updateStatus();
            var move = { fen: $self.game.fen(), gameId: $self.control.data("instance-id"), playerColour: playerColour };
            $($self.settings.Id).trigger("chess_move", move);
        }

        $self.onMouseoverSquare = function (square, piece) {
            // get list of possible moves for this square
            var moves = $self.game.moves({
                square: square,
                verbose: true
            });

            // exit if there are no moves available for this square
            if (moves.length === 0) return;

        }

        $self.onMouseoutSquare = function (square, piece) {
            //$self.removeGreySquares();
        }

        $self.onSnapEnd = function () {
            $self.board.position($self.game.fen());
        }

        $self.updateStatus = function () {

            var status = '';

            var moveColor = 'White';
            if ($self.game.turn() === 'b') {
                moveColor = 'Black';
            }

            // checkmate?
            if ($self.game.in_checkmate()) {
                status = 'Game over, ' + moveColor + ' is in checkmate.';
            }

            // draw?
            else if ($self.game.in_draw()) {
                status = 'Game over, drawn position';
            }

            // game still on
            else {
                status = moveColor + ' to move';

                // check?
                if ($self.game.in_check()) {
                    status += ', ' + moveColor + ' is in check';
                }
            }

            $($self.settings.status).html(status);
            $($self.settings.fen).html($self.game.fen());
            $($self.settings.pgn).html($self.game.pgn());
        }

        $($self.settings.Id).once("chess_init", function (event, move) {
            $($self.settings.status).html(move.colour);
            $self.board.orientation(move.colour);
            if (move.fen) {
                $self.game.load(move.fen);
                $self.board.position(move.fen);
            }
        });

        $($self.settings.Id).once("chess_moved", function (event, move) {
            $self.game.load(move.fen);
            $self.board.position(move.fen);

            if ($self.board.orientation() === 'black' && $self.game.turn() == 'b') {
                $($self.settings.Id).trigger('change_colour', 'w');
            }
            else if ($self.board.orientation() === 'black' && $self.game.turn() === 'w') {
                $($self.settings.Id).trigger('change_colour', 'b');
            } else {
                $($self.settings.Id).trigger('change_colour', $self.game.turn());
            }

            //$self.updateStatus();
        });

        var config = {
            draggable: true,
            pieceTheme: '/img/chesspieces/merida/{piece}.svg',
            position: 'start',
            onDragStart: $self.onDragStart,
            onDrop: $self.onDrop,
            onMouseoutSquare: $self.onMouseoutSquare,
            onMouseoverSquare: $self.onMouseoverSquare,
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



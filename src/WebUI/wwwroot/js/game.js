

(function ($) {
    $.fn.play = function (options) {
        var $self = $(this);


        $self.settings = $.extend({
            status: '#status',
            fen: '#fen',
            pgn: '#pgn',
            whiteSquareGrey: '#a9a9a9',
            blackSquareGrey: '#696969'
        }, options);

        $self.board = null;
        $self.game = new Chess();

        $self.onDragStart = function (source, piece, position, orientation)
        {
            if ($self.game.game_over()) return false;

            // only pick up pieces for the side to move
            if (($self.game.turn() === 'w' && piece.search(/^b/) !== -1) ||
                ($self.game.turn() === 'b' && piece.search(/^w/) !== -1)) {
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

            
            $self.updateStatus();
            var fen = $self.game.fen();
            $(document).trigger("chess_move", fen);
        }

        $self.onMouseoverSquare = function (square, piece) {
            // get list of possible moves for this square
            var moves = $self.game.moves({
                square: square,
                verbose: true
            });

            // exit if there are no moves available for this square
            if (moves.length === 0) return;

            // highlight the square they moused over
            //$self.greySquare(square);

            // highlight the possible squares for this piece
            for (var i = 0; i < moves.length; i++) {
                //$self.greySquare(moves[i].to);
            }
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

        $(document).once("chess_moved", function (event, message) {
            $self.game.load(message);
            $self.board.position(message);
            //$self.updateStatus();
        });

        var config = {
            draggable: true,
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
    $.fn.chessmove();
});


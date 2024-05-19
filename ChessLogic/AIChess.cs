namespace ChessLogic
{
    public class AIChess
    {
        public static AIChess Instance { get; } = new AIChess();
        private static readonly Random Random = new Random();
        private readonly int maxDepth;

        private AIChess()
        {
            maxDepth = 3; // Example depth, you can adjust this
        }

        public Move GetBestMove(GameState gameState)
        {
            var bestMove = Minimax(gameState, maxDepth, int.MinValue, int.MaxValue, true);
            return bestMove.move;
        }

        private (Move move, int score) Minimax(GameState gameState, int depth, int alpha, int beta, bool maximizingPlayer)
        {
            if (depth == 0 || gameState.IsGameOver())
            {
                return (null, EvaluateBoard(gameState.Board, gameState.CurrentPlayer));
            }

            IEnumerable<Move> legalMoves = gameState.AllLegalMovesFor(gameState.CurrentPlayer);
            Move bestMove = null;

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;

                foreach (var move in legalMoves)
                {
                    GameState newState = SimulateMove(gameState, move);
                    int eval = Minimax(newState, depth - 1, alpha, beta, false).score;

                    if (eval > maxEval)
                    {
                        maxEval = eval;
                        bestMove = move;
                    }

                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return (bestMove, maxEval);
            }
            else
            {
                int minEval = int.MaxValue;

                foreach (var move in legalMoves)
                {
                    GameState newState = SimulateMove(gameState, move);
                    int eval = Minimax(newState, depth - 1, alpha, beta, true).score;

                    if (eval < minEval)
                    {
                        minEval = eval;
                        bestMove = move;
                    }

                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return (bestMove, minEval);
            }
        }

        private GameState SimulateMove(GameState gameState, Move move)
        {
            GameState newState = new GameState(gameState.CurrentPlayer, gameState.Board.Copy());
            newState.MakeMove(move);
            newState.SwitchPlayer();
            return newState;
        }

        private int EvaluateBoard(Board board, Player player)
        {
            int score = 0;

            foreach (var pos in board.PiecePositions())
            {
                Piece piece = board[pos];
                int pieceValue = GetPieceValue(piece);

                if (piece.Color == player)
                {
                    score += pieceValue;
                }
                else
                {
                    score -= pieceValue;
                }
            }

            return score;
        }

        private int GetPieceValue(Piece piece)
        {
            return piece.Type switch
            {
                PieceType.Pawn => 1,
                PieceType.Knight => 3,
                PieceType.Bishop => 3,
                PieceType.Rook => 5,
                PieceType.Queen => 9,
                PieceType.King => 1000,
                _ => 0
            };
        }
    }
}

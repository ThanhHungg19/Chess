namespace ChessLogic
{
    public class AIChess
    {
        // Singleton instance of AIChess, ensuring there's only one instance
        public static AIChess Instance { get; } = new AIChess();

        // Random number generator, could be used for move selection in case of tie or random move generation
        private static readonly Random Random = new Random();

        // The maximum depth for the Minimax algorithm. This determines how many moves ahead the AI will consider
        private readonly int maxDepth;

        // Private constructor for the singleton pattern
        private AIChess()
        {
            maxDepth = 3; // Can only maximum depth is 3
        }

    
        public Move GetBestMove(GameState gameState)
        {
            // Call the Minimax algorithm to get the best move, starting at the maximum depth and assuming the AI is the maximizing player
            var bestMoveResult = Minimax(gameState, maxDepth, int.MinValue, int.MaxValue, true);
            Move bestMove = bestMoveResult.move;

            // If no best move, AI will make a random move
            if (bestMove == null)
            {
                bestMove = GetRandomMove(gameState);
            }
            return bestMove; 
        }

        // Minimax algorithm with alpha-beta pruning to find the best move
        // 'depth' determines how deep the algorithm goes, 'alpha' and 'beta' are used for pruning, 'maximizingPlayer' indicates if it's AI's turn
        private (Move move, int score) Minimax(GameState gameState, int depth, int alpha, int beta, bool maximizingPlayer)
        {
            // Base case: if depth is 0 or the game is over, evaluate the board
            if (depth == 0 || gameState.IsGameOver())
            {
                return (null, EvaluateBoard(gameState.Board, gameState.CurrentPlayer));
            }

            // Get all legal moves for the current player
            IEnumerable<Move> legalMoves = gameState.AllLegalMovesFor(gameState.CurrentPlayer);
            Move bestMove = null;

            if (maximizingPlayer)
            {
                // Maximizing player's turn (AI's turn)
                int maxEval = int.MinValue;

                // Iterate over all legal moves
                foreach (var move in legalMoves)
                {
                    // Simulate the move and get the new game state
                    GameState newState = SimulateMove(gameState, move);

                    // Recursively call Minimax for the new state, reducing the depth and switching to the minimizing player
                    int eval = Minimax(newState, depth - 1, alpha, beta, false).score;

                    // Update maxEval and bestMove if a better evaluation is found
                    if (eval > maxEval)
                    {
                        maxEval = eval;
                        bestMove = move;
                    }

                    // Update alpha for alpha-beta pruning
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break; // Prune the search tree
                    }
                }

                return (bestMove, maxEval);
            }
            else
            {
                // Minimizing player's turn (opponent's turn)
                int minEval = int.MaxValue;

                // Iterate over all legal moves
                foreach (var move in legalMoves)
                {
                    // Simulate the move and get the new game state
                    GameState newState = SimulateMove(gameState, move);

                    // Recursively call Minimax for the new state, reducing the depth and switching to the maximizing player
                    int eval = Minimax(newState, depth - 1, alpha, beta, true).score;

                    // Update minEval and bestMove if a better evaluation is found
                    if (eval < minEval)
                    {
                        minEval = eval;
                        bestMove = move;
                    }

                    // Update beta for alpha-beta pruning
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break; // Prune the search tree
                    }
                }

                return (bestMove, minEval);
            }
        }

        private GameState SimulateMove(GameState gameState, Move move)
        {
            // Copy the game state and update it
            GameState newState = new GameState(gameState.CurrentPlayer, gameState.Board.Copy());

            // Make the move on the new game state
            newState.MakeMove(move);

            // Switch the player to the opponent
            newState.SwitchPlayer();

            // Check is game over 
            newState.IsGameOver();
            return newState;
        }

        // Evaluate the board and return a score indicating the favorability for the current player
        private int EvaluateBoard(Board board, Player player)
        {
            int score = 0;

            // Iterate over all positions on the board
            foreach (var pos in board.PiecePositions())
            {
                Piece piece = board[pos];
                int pieceValue = GetPieceValue(piece);

                // Add to score if the piece belongs to the current player, subtract otherwise
                if (piece.Color == player)
                {
                    score += pieceValue;
                }
                else
                {
                    score -= pieceValue;
                }
            }

            return score; // Return the calculated score
        }

        private Move GetRandomMove(GameState gameState)
        {
            IEnumerable<Move> legalMoves = gameState.AllLegalMovesFor(gameState.CurrentPlayer);
            return legalMoves.ElementAt(Random.Next(legalMoves.Count()));
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

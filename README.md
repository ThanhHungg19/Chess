
# ChessAI Minimax with Alpha Beta Pruning - Author: Nguyễn Thành Hưng




## Overview
ChessAIMinimax is a C# project that implements a chess game with an AI opponent using the Minimax algorithm and alpha-beta pruning.
## Getting Started
### Prerequisites
- Visual Studio (2019 or later)
- .NET Framework

### Installation
1. Clone the repository:
```bash
 git clone https://github.com/ThanhHungg19/ChessAIMinimax.git
```

2. Open the solution file `Chess.sln` in Visual Studio.

### Running the Project
1. Set ChessUI as the startup project.
2. Build the solution by clicking Build > Build Solution.
3. Run the project by clicking Debug > Start Debugging or pressing F5.

## Minimax Algorithm
### Overview
The Minimax algorithm is a decision-making algorithm used in turn-based games. It operates by simulating all possible moves and their outcomes, assuming both players play optimally. The AI aims to maximize its score while minimizing the opponent's score.

### Alpha-Beta Pruning
Alpha-beta pruning enhances the Minimax algorithm by eliminating branches in the search tree that don't influence the final decision. It maintains two values, alpha and beta, representing the minimum score the maximizing player is assured and the maximum score the minimizing player is assured, respectively. Pruning occurs when these values indicate that a subtree cannot influence the final decision.


## Time Complexity
- Minimax: O(b^d), where b is the branching factor and d is the depth of the tree.
- Alpha-Beta Pruning: O(b^(d/2)) in the best case, effectively doubling the search depth achievable in the same amount of time as Minimax.

## Space Complexity
Both Minimax and Alpha-Beta Pruning have a space complexity of O(bd), where `b` is the branching factor and `d` is the depth of the tree, due to the need to store the entire search tree.



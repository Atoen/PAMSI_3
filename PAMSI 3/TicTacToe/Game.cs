using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Dumpify;

namespace PAMSI_3.TicTacToe;

public class Game
{
    private char _playerSymbol;
    private Tile _last = Tile.Player;

    public ContentControl[,] Buttons = default!;

    private readonly Board _board;

    public Game(int size, int streakToWin, char playerSymbol)
    {
        _board = new Board(size, streakToWin);
        _playerSymbol = playerSymbol;
    }

    public void Click(int column, int row)
    {
        if (_board[column, row] != Tile.Empty) return;
        Buttons[column, row].Content = "X";
        _board[column, row] = Tile.Player;
        
        var winner = _board.CheckWinner();
        if (winner != Winner.None)
        {
            MessageBox.Show($"{winner} has won", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var (c, r) = MakeMoveWithMinimax(Tile.Player);

        if (c != -1 && r != -1)
        {
            Buttons[c, r].Content = "O";
            _board[c, r] = Tile.Opponent;
        }
        
        winner = _board.CheckWinner();
        if (winner != Winner.None)
        {
            MessageBox.Show($"Player {winner} has won", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
    }
    
    public (int column, int row) MakeMoveWithMinimax(Tile currentPlayer)
    {
        var bestScore = int.MinValue;
        var bestMoveColumn = -1;
        var bestMoveRow = -1;

        for (var column = 0; column < _board.Size; column++)
        {
            for (var row = 0; row < _board.Size; row++)
            {
                if (_board[column, row] != Tile.Empty) continue;
                
                _board[column, row] = currentPlayer;

                var score = MinMax.GetScore(_board, MinMax.GetOpponent(currentPlayer), 0);

                _board[column, row] = Tile.Empty;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMoveColumn = column;
                    bestMoveRow = row;
                }
            }
        }

        return (bestMoveColumn, bestMoveRow);
    }
}

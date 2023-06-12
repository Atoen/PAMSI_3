using System;
using System.Windows;
using System.Windows.Controls;

namespace PAMSI_3.TicTacToe;

public class Game
{
    public ContentControl[,] Buttons = default!;

    private readonly Board _board;
    private readonly string _opponentSymbol;
    private readonly string _playerSymbol;
    private bool _isGameOver;

    public Game(int size, int streakToWin, string playerSymbol, string opponentSymbol)
    {
        _board = new Board(size, streakToWin);
        _playerSymbol = playerSymbol;
        _opponentSymbol = opponentSymbol;
    }

    public void Click(int column, int row)
    {
        if (_board[column, row] != Tile.Empty || _isGameOver) return;

        if (PlayerMove(column, row))
        {
            ShowMessageBox("Player has won");
            _isGameOver = true;
            return;
        }

        if (CheckTie()) return;

        // if (OpponentMove())
        // {
        //     ShowMessageBox("Opponent has won");
        //     _isGameOver = true;
        //     return;
        // }

        CheckTie();
    }

    private bool PlayerMove(int column, int row)
    {
        Buttons[column, row].Content = _playerSymbol;
        _board[column, row] = Tile.Player;

        return _board.CheckWinner() == Winner.Player;
    }

    private bool OpponentMove()
    {
        var (column, row) = MinMax.FindBestMove(_board);

        if (column == -1 || row == -1) throw new Exception();

        Buttons[column, row].Content = _opponentSymbol;
        _board[column, row] = Tile.Opponent;

        return _board.CheckWinner() == Winner.Opponent;
    }

    private bool CheckTie()
    {
        if (!_board.AreAllTilesTaken) return false;

        ShowMessageBox("Tie");
        return true;
    }

    private void ShowMessageBox(string message)
    {
        MessageBox.Show(message, "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}

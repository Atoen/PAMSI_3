using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PAMSI_3.TicTacToe;

public class Game
{
    private readonly Board _board;
    private readonly string _opponentSymbol;
    private readonly string _playerSymbol;
    private bool _isGameOver;
    private readonly ContentControl[,] _buttons;
    private bool _movingNow;

    public Game(int size, int streakToWin, string playerSymbol, string opponentSymbol)
    {
        _board = new Board(size, streakToWin);
        _playerSymbol = playerSymbol;
        _opponentSymbol = opponentSymbol;
        _buttons = new ContentControl[size, size];
    }

    public void AddButton(Button button, int column, int row) => _buttons[column, row] = button;

    public async void Click(int column, int row)
    {
        if (_board[column, row] != Tile.Empty || _isGameOver || _movingNow) return;

        _movingNow = true;

        await Task.Run(() =>
        {
            if (PlayerMove(column, row))
            {
                ShowMessageBox("Player has won");
                _isGameOver = true;
                return;
            }

            if (CheckTie()) return;

            if (OpponentMove())
            {
                ShowMessageBox("Opponent has won");
                _isGameOver = true;
                return;
            }

            CheckTie();
        });

        _movingNow = false;
    }

    private bool PlayerMove(int column, int row)
    {
        var button = _buttons[column, row];
        button.Dispatcher.Invoke(() => button.Content = _playerSymbol);
        _board[column, row] = Tile.Player;

        return _board.CheckWinner() == Winner.Player;
    }

    private bool OpponentMove()
    {
        var (column, row) = MinMax.FindBestMove(_board);

        if (column == -1 || row == -1) throw new Exception();

        var button = _buttons[column, row];
        button.Dispatcher.Invoke(() => button.Content = _opponentSymbol);

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

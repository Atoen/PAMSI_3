using System;
using System.Linq;

namespace PAMSI_3.TicTacToe;

public class Board
{
    private readonly Tile[,] _tiles;
    private readonly int _size;
    private readonly int _streakToWin;

    public bool AreAllTilesTaken => _tiles.Cast<Tile>().All(tile => tile != Tile.Empty);

    public Tile this[int column, int row]
    {
        get => _tiles[column, row];
        set => _tiles[column, row] = value;
    }

    public int Size => _size;

    public Board(int size, int streakToWin)
    {
        _size = size;
        _tiles = new Tile[size, size];
        _streakToWin = streakToWin;
    }

    public Winner CheckWinner()
    {
        if (_size < _streakToWin) return Winner.None;

        var horizontal = Horizontal();
        if (horizontal != Winner.None) return horizontal;

        var vertical = Vertical();
        if (vertical != Winner.None) return vertical;

        var diagonal = Diagonal(DiagonalDirection.Up);
        if (diagonal != Winner.None) return diagonal;

        diagonal = Diagonal(DiagonalDirection.Down);
        if (diagonal != Winner.None) return diagonal;

        return Winner.None;
    }
    
    private Winner Horizontal()
    {
        for (var row = 0; row < _size; row++)
        {
            var streakStartState = Tile.Empty;
            var streak = 1;

            for (var column = 0; column < _size; column++)
            {
                var tileState = _tiles[column, row];
                if (tileState == Tile.Empty) continue;

                if (tileState == streakStartState)
                {
                    streak++;

                    if (streak >= _streakToWin) return (Winner) tileState;
                }
                else
                {
                    streakStartState = tileState;
                    streak = 1;
                }
            }
        }

        return Winner.None;
    }

    private Winner Vertical()
    {
        for (var column = 0; column < _size; column++)
        {
            var streakStartState = Tile.Empty;
            var streak = 1;

            for (var row = 0; row < _size; row++)
            {
                var tileState = _tiles[column, row];
                if (tileState == Tile.Empty) continue;

                if (tileState == streakStartState)
                {
                    streak++;
                    if (streak >= _streakToWin) return (Winner) tileState;
                }
                else
                {
                    streakStartState = tileState;
                    streak = 1;
                }
            }
        }

        return Winner.None;
    }

    private Winner Diagonal(DiagonalDirection direction)
    {
        var maxOffset = _size - _streakToWin;

        for (var columnOffset = 0; columnOffset <= maxOffset; columnOffset++)
        for (var rowOffset = 0; rowOffset <= maxOffset; rowOffset++)
        {
            if (columnOffset != 0 && rowOffset != 0) continue;

            var diagonalLength = _size - Math.Max(columnOffset, rowOffset);
            var streakStartState = Tile.Empty;
            var streak = 1;

            for (var pos = 0; pos < diagonalLength; pos++)
            {
                Tile tileState;
                if (direction == DiagonalDirection.Up)
                {
                    var row = _size - pos - 1;
                    tileState = _tiles[pos + columnOffset, row - rowOffset];
                }
                else
                {
                    tileState = _tiles[pos + columnOffset, pos + rowOffset];
                }

                if (tileState == Tile.Empty) continue;

                if (tileState == streakStartState)
                {
                    streak++;
                    if (streak >= _streakToWin) return (Winner) streakStartState;
                }
                else
                {
                    streakStartState = tileState;
                    streak = 1;
                }
            }
        }

        return Winner.None;
    }
}

public enum Tile
{
    Empty,
    Player,
    Opponent
}

public enum Winner
{
    None,
    Player,
    Opponent
}

public enum DiagonalDirection
{
    Up,
    Down
}
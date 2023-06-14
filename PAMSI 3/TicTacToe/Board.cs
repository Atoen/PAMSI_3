using System;

namespace PAMSI_3.TicTacToe;

public class Board
{
    private readonly Tile[,] _tiles;

    public Board(int size, int streakToWin)
    {
        Size = size;
        StreakToWin = streakToWin;
        _tiles = new Tile[size, size];
    }
    
    public bool AreAllTilesTaken
    {
        get
        {
            foreach (var tile in _tiles)
            {
                if (tile == Tile.Empty) return false;
            }

            return true;
        }
    }

    public Tile this[int column, int row]
    {
        get => _tiles[column, row];
        set => _tiles[column, row] = value;
    }

    public int Size { get; }
    public int StreakToWin { get; }

    public Winner CheckWinner()
    {
        if (Size < StreakToWin) return Winner.None;

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
        for (var row = 0; row < Size; row++)
        {
            var streakStartState = Tile.Empty;
            var streak = 1;

            for (var column = 0; column < Size; column++)
            {
                var tileState = _tiles[column, row];

                if (tileState != Tile.Empty && tileState == streakStartState)
                {
                    streak++;

                    if (streak >= StreakToWin) return (Winner) tileState;
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
        for (var column = 0; column < Size; column++)
        {
            var streakStartState = Tile.Empty;
            var streak = 1;

            for (var row = 0; row < Size; row++)
            {
                var tileState = _tiles[column, row];

                if (tileState != Tile.Empty && tileState == streakStartState)
                {
                    streak++;
                    if (streak >= StreakToWin) return (Winner) tileState;
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
        var maxOffset = Size - StreakToWin;

        for (var columnOffset = 0; columnOffset <= maxOffset; columnOffset++)
        for (var rowOffset = 0; rowOffset <= maxOffset; rowOffset++)
        {
            if (columnOffset != 0 && rowOffset != 0) continue;

            var diagonalLength = Size - Math.Max(columnOffset, rowOffset);
            var streakStartState = Tile.Empty;
            var streak = 1;

            for (var pos = 0; pos < diagonalLength; pos++)
            {
                Tile tileState;
                if (direction == DiagonalDirection.Up)
                {
                    var row = Size - pos - 1;
                    tileState = _tiles[pos + columnOffset, row - rowOffset];
                }
                else
                {
                    tileState = _tiles[pos + columnOffset, pos + rowOffset];
                }

                if (tileState != Tile.Empty && tileState == streakStartState)
                {
                    streak++;
                    if (streak >= StreakToWin) return (Winner) streakStartState;
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

    public long HashCode
    {
        get
        {
            unchecked
            {
                long hash = 17;

                foreach (var tile in _tiles)
                {
                    hash = hash * 23 + tile.GetHashCode();
                }

                return hash;
            }
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Board otherBoard) return false;

        if (otherBoard.Size != Size) return false;

        for (var i = 0; i < Size; i++)
        for (var j = 0; j < Size; j++)
        {
            if (_tiles[i, j] != otherBoard[i, j])
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        return System.HashCode.Combine(_tiles, Size, StreakToWin);
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
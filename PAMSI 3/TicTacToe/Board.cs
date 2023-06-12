namespace PAMSI_3.TicTacToe;

public class Board
{
    private readonly Tile[,] _tiles;
    private readonly int _size;
    private readonly int _streakToWin;

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

    public int Size => _size;

    public Board(int size, int streakToWin)
    {
        _size = size;
        _tiles = new Tile[size, size];
        _streakToWin = streakToWin;
    }

    public Winner CheckWinner()
    {

        for (var row = 0; row < _size; row++)
        {
            for (var column = 0; column <= _size - _streakToWin; column++)
            {
                var winner = GetStreakWinner(column, row, 1, 0);
                if (winner != Winner.None) return winner;
            }
        }
        
        for (var column = 0; column < _size; column++)
        {
            for (var row = 0; row <= _size - _streakToWin; row++)
            {
                var winner = GetStreakWinner(column, row, 0, 1);
                if (winner != Winner.None) return winner;
            }
        }
        
        for (var column = 0; column <= _size - _streakToWin; column++)
        {
            for (var row = 0; row <= _size - _streakToWin; row++)
            {
                // Main diagonal
                var diagonalWinner = GetStreakWinner(column, row, 1, 1);
                if (diagonalWinner != Winner.None) return diagonalWinner;

                // Anti-diagonal
                var antiDiagonalWinner = GetStreakWinner(column + _streakToWin - 1, row, -1, 1);
                if (antiDiagonalWinner != Winner.None) return antiDiagonalWinner;
            }

        }

        return Winner.None;
    }

    private Winner GetStreakWinner(int startColumn, int startRow, int columnStep, int rowStep)
    {
        var streakStartState = _tiles[startColumn, startRow];
        var streak = 1;

        for (var i = 0; i < _streakToWin; i++)
        {
            var column = startColumn + i * columnStep;
            var row = startRow + i * rowStep;
            var tileState = _tiles[column, row];

            if (tileState == Tile.Empty || tileState != streakStartState)
            {
                return Winner.None;
            }

            streak++;
        }

        return (Winner) streakStartState;
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
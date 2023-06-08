using System;

namespace PAMSI_3.TicTacToe;

public static class MinMax
{
    public const int MaxDepth = 5;

    public static int GetScore(Board board, Tile player, int depth)
    {
        var winner = board.CheckWinner();

        if (winner == Winner.Player) return 10 - depth;
        if (winner == Winner.Opponent) return depth - 10;
        if (winner == Winner.None && board.AreAllTilesTaken || depth >= MaxDepth) return 0;

        var bestScore = player == Tile.Player ? int.MinValue : int.MaxValue;

        for (var column = 0; column < board.Size; column++)
        for (var row = 0; row < board.Size; row++)
        {
            if (board[column, row] != Tile.Empty) continue;

            board[column, row] = Tile.Player;

            var nextPlayer = player == Tile.Player ? Tile.Opponent : Tile.Player;

            var score = GetScore(board, nextPlayer, depth + 1);

            bestScore = player == Tile.Player
                ? Math.Max(score, bestScore)
                : Math.Min(score, bestScore);

            board[column, row] = Tile.Empty;
        }

        return bestScore;
    }

    public static (int column, int row) FindBestMove(Board board)
    {
        var bestScore = int.MaxValue;
        var bestMove = (-1, -1);

        for (var column = 0; column < board.Size; column++)
        for (var row = 0; row < board.Size; row++)
        {
            if (board[column, row] != Tile.Empty) continue;

            board[column, row] = Tile.Opponent;
            var score = GetScore(board, Tile.Player, 0);
            board[column, row] = Tile.Empty;

            if (score < bestScore)
            {
                bestMove = (column, row);
                bestScore = score;
            }
        }

        return bestMove;
    }
}
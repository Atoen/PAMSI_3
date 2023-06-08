using System;

namespace PAMSI_3.TicTacToe;

public static class MinMax
{
    public const int MaxDepth = 3;
    
    public static int GetScore(Board board, Tile currentPlayer, int depth)
    {
        var winner = board.CheckWinner();
        
        if (winner == Winner.None && board.AreAllTilesTaken || depth >= MaxDepth) return 0;

        if (winner == Winner.Player)
        {
            return currentPlayer == Tile.Player ? 10 : -10;
        }

        if (winner == Winner.Opponent)
        {
            return currentPlayer == Tile.Opponent ? 10 : -10;
        }
        
        var bestScore = currentPlayer == Tile.Player ? int.MinValue : int.MaxValue;

        for (var column = 0; column < board.Size; column++)
        for (var row = 0; row < board.Size; row++)
        {
            if (board[column, row] != Tile.Empty) continue;

            board[column, row] = currentPlayer;

            var score = GetScore(board, GetOpponent(currentPlayer), depth + 1);

            if (currentPlayer == Tile.Player) bestScore = Math.Max(bestScore, score);
            else bestScore = Math.Min(bestScore, score);

            board[column, row] = Tile.Empty;
        }

        return bestScore;
    }

    public static Tile GetOpponent(Tile player)
    {
        return player == Tile.Player ? Tile.Opponent : Tile.Player;
    }
}
using System;
using System.Collections.Generic;

namespace PAMSI_3.TicTacToe;

public static class MinMax
{
    public const int MaxDepth = 7;
    private static readonly Dictionary<long, int> TranspositionTable = new();

    public static int GetScore(Board board, Tile currentPlayer, int depth, int alpha, int beta)
    {
        var winner = board.CheckWinner();

        if (winner == Winner.Player) return 15 - depth;
        if (winner == Winner.Opponent) return depth - 15;
        if (depth >= MaxDepth || winner == Winner.None && board.AreAllTilesTaken) return 0;

        var hash = board.HashCode;

        var bestScore = currentPlayer == Tile.Player ? int.MinValue : int.MaxValue;

        for (var column = 0; column < board.Size; column++)
        {
            for (var row = 0; row < board.Size; row++)
            {
                if (board[column, row] != Tile.Empty) continue;

                board[column, row] = currentPlayer;

                var score = GetScore(board, GetOpponent(currentPlayer), depth + 1, alpha, beta);
                bestScore = currentPlayer == Tile.Player
                    ? Math.Max(score, bestScore)
                    : Math.Min(score, bestScore);

                board[column, row] = Tile.Empty;

                if (currentPlayer == Tile.Player) alpha = Math.Max(alpha, bestScore);
                else beta = Math.Min(beta, bestScore);

                if (alpha >= beta)
                {
                    return bestScore;
                }
            }
        }
        
        return bestScore;
    }

    private static Tile GetOpponent(Tile currentPlayer) => currentPlayer == Tile.Player ? Tile.Opponent : Tile.Player;

    public static (int column, int row) FindBestMove(Board board)
    {
        var bestScore = int.MaxValue;
        var bestMove = (-1, -1);

        for (var column = 0; column < board.Size; column++)
        for (var row = 0; row < board.Size; row++)
        {
            if (board[column, row] != Tile.Empty) continue;

            board[column, row] = Tile.Opponent;

            var score = GetScore(board, Tile.Player, 0, int.MinValue, int.MaxValue);
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
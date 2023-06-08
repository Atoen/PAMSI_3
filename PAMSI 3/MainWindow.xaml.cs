using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PAMSI_3.TicTacToe;

namespace PAMSI_3;

public partial class MainWindow
{
    private readonly Regex _regex = NumericRegex();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void SymbolSelectButton_OnClick(object sender, RoutedEventArgs e)
    {
        SymbolSelectButton.Content = (string) SymbolSelectButton.Content == "X" ? "O" : "X";
    }

    private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (_regex.IsMatch(e.Text))
        {
            e.Handled = true;
            return;
        }

        ((TextBox) sender).Text = e.Text[0].ToString();
        e.Handled = true;
    }

    private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox) return;

        if (textBox.Text.Length == 0) textBox.Text = "3";
        else if (!int.TryParse(textBox.Text, out _)) textBox.Text = "3";
        else if (textBox.Text.Length > 1) textBox.Text = textBox.Text[^1].ToString();
    }

    [GeneratedRegex("[^2-9]+")]
    private static partial Regex NumericRegex();

    private void PLayButton_OnClick(object sender, RoutedEventArgs e)
    {
        var columns = int.Parse(GridWidthTextBox.Text);
        var winningStreak = int.Parse(WinConditionTextBox.Text);

        var playerSymbol = (string) SymbolSelectButton.Content;
        var opponentSymbol = playerSymbol == "X" ? "O" : "X";

        var game = new Game(columns, winningStreak, playerSymbol, opponentSymbol);

        var buttons = CreateGrid(columns, game);
        game.Buttons = buttons;
    }

    private ContentControl[,] CreateGrid(int size, Game game)
    {
        GameGrid.ColumnDefinitions.Clear();
        GameGrid.RowDefinitions.Clear();

        for (var i = 0; i < size; i++)
        {
            GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            GameGrid.RowDefinitions.Add(new RowDefinition());
        }

        var buttons = new ContentControl[size, size];

        for (var col = 0; col < size; col++)
        for (var row = 0; row < size; row++)
        {
            var cellButton = new Button
            {
                FontSize = 38,
                Foreground = Brushes.Black
            };

            buttons[col, row] = cellButton;

            var col1 = col;
            var row1 = row;
            cellButton.Click += (_, _) => game.Click(col1, row1);

            var cellBorder = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = cellButton
            };

            GameGrid.Children.Add(cellBorder);
            Grid.SetRow(cellBorder, row);
            Grid.SetColumn(cellBorder, col);
        }

        return buttons;
    }
}
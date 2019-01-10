using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game TTT_Game = new Game();
        int player1 = 0;
        int player2 = 0;
        int tie = 0;

        AI_PLayer AI = new AI_PLayer();


        public MainWindow()
        {
            InitializeComponent();
            DrawGridlines();
        }
        void DrawGridlines()
        {
            for (int i = 0; i <= Game.ROWS - 1; i++)
            {
                for (int j = 0; j <= Game.COLS - 1; j++)
                {
                    Border b = new Border();
                    b.BorderBrush = Brushes.Black;
                    b.BorderThickness = new Thickness(1);

                    PlayGrid.Children.Add(b);
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);
                }
            }
        }

        void DrawX(int row, int col)
        {
            const int CELL_PADDING = 20;
            SolidColorBrush solid = new SolidColorBrush();
            solid.Color = Color.FromRgb(0, 0, 150);
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = Brushes.Black;
            ellipse.Height = PlayGrid.ActualHeight / Game.ROWS - CELL_PADDING;
            ellipse.Width = PlayGrid.ActualWidth / Game.COLS - CELL_PADDING;
            ellipse.StrokeThickness = 3;
            ellipse.Fill = solid;
            PlayGrid.Children.Add(ellipse);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, col);
        }
        void DrawO(int row, int col)
        {
            const int CELL_PADDING = 20;
            SolidColorBrush solid = new SolidColorBrush();
            solid.Color = Color.FromRgb(200, 0, 0);
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = Brushes.Black;
            ellipse.Height = PlayGrid.ActualHeight / Game.ROWS - CELL_PADDING;
            ellipse.Width = PlayGrid.ActualWidth / Game.COLS - CELL_PADDING;
            ellipse.StrokeThickness = 3;
            ellipse.Fill = solid;
            PlayGrid.Children.Add(ellipse);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, col);

        }

    

        private void MarkBoard(int row, int col)
         {
            if (TTT_Game.CellIsEmpty(row, col))
            {
                if (TTT_Game.getTurn() == 'X' )
                {
                    DrawX(row, col);
                    TTT_Game.MarkGameBoard(row, col);
                }
                else if (TTT_Game.getTurn() == 'O' )
                {
                    DrawO(row, col);
                    TTT_Game.MarkGameBoard(row, col);
                }
            }

            char winner = TTT_Game.checkWinner().Item1;
            if (winner != ' ')
            {
                List<Tuple<int, int>> winningSpots = TTT_Game.checkWinner().Item2;
                string NameOfWinner = "";

                if (winner == 'X')
                {
                    NameOfWinner = txtPlayer1.Text;
                    player1++;
                    lblPlayer1.Content = player1.ToString();
                }
                else
                {
                    NameOfWinner = txtPlayer2.Text;
                    player2++;
                    lblPlayer2.Content = player2.ToString();
                }

                foreach (var child in PlayGrid.Children)
                {
                    var shape = child as Shape;

                    if (shape != null)
                    {
                        foreach (var spot in winningSpots)
                        {
                            if (Grid.GetRow(shape) == spot.Item1 && Grid.GetColumn(shape) == spot.Item2)
                            {
                                shape.Stroke = Brushes.Green;
                                shape.StrokeThickness = 6;
                            }
                        }
                    }
                }
                MessageBox.Show(string.Format("Winner is {0}!", NameOfWinner));
                reset();
            }
            else if (TTT_Game.BoardIsFull())
            {
                tie++;
                lblTie.Content = tie.ToString();
                MessageBox.Show(string.Format("No winner"));
                reset();
            }

        }
        
        void reset()
        {
            PlayGrid.Children.Clear();
            DrawGridlines();
            TTT_Game = new Game(TTT_Game.getTurn());
            PlayGrid.Children.Add(lblPlayer1);
            PlayGrid.Children.Add(lblPlayer2);
            PlayGrid.Children.Add(lblTie);
            PlayGrid.Children.Add(Label1);
            PlayGrid.Children.Add(Label2);
            PlayGrid.Children.Add(Label3);
            PlayGrid.Children.Add(Label4);
            PlayGrid.Children.Add(Label5);
            PlayGrid.Children.Add(txtPlayer1);
            PlayGrid.Children.Add(txtPlayer2);
            PlayGrid.Children.Add(btnReset);
            PlayGrid.Children.Add(rbHuman);
            PlayGrid.Children.Add(rbAIPlayer);
            PlayGrid.Children.Add(rbHard);
            PlayGrid.Children.Add(rbMedium);
            PlayGrid.Children.Add(rbEasy);

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
            txtPlayer1.Text = "Player 1";
            txtPlayer2.Text = "Player 2";
            lblPlayer1.Content = "0";
            lblPlayer2.Content = "0";
            lblTie.Content = "0";
            player1 = 0;
            player2 = 0;
            tie = 0;
            rbHuman.IsChecked = true;
            rbEasy.IsChecked = true;

        }

        private void PlayGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(PlayGrid);
            
            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // calc row mouse was over
            foreach (var rowDefinition in PlayGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // calc col mouse was over
            foreach (var columnDefinition in PlayGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

           for (int i = Game.ROWS - 1; i >= 0; i--)
            {
                if (TTT_Game.CellIsEmpty(i, col))
                {
                    MarkBoard(i, col);
                    char winner = TTT_Game.checkWinner().Item1;

                    if (rbHuman.IsChecked == true)
                    {
                        return;
                    }
                    else if (rbAIPlayer.IsChecked == true)
                    {
                        if (winner == ' ')
                        {
                            if (rbEasy.IsChecked == true)
                            {
                                Spot S = AI.GetBestMove(TTT_Game, 2);
                                //    //MessageBox.Show(string.Format("Score is {0}!", score));
                                MarkBoard(S.row, S.col);
                            }
                            else if (rbMedium.IsChecked == true)
                            {
                                Spot S = AI.GetBestMove(TTT_Game, 4);
                                //    //MessageBox.Show(string.Format("Score is {0}!", score));
                                MarkBoard(S.row, S.col);
                            }
                            else if (rbHard.IsChecked == true)
                            {
                                Spot S = AI.GetBestMove(TTT_Game, 5);
                                //    //MessageBox.Show(string.Format("Score is {0}!", score));
                                MarkBoard(S.row, S.col);
                            }
                        }
                    }
                    break;
                }
            }
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {

        }

        private void rbAIPlayer_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}

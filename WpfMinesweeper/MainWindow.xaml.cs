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

namespace WpfMinesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// TODO:
    /// - Add Right-Click for Flags.
    /// - Add option to restart.
    /// - Add window to set Height, Width and Mine count.
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        const int COLUMN_COUNT = 20;
        const int ROW_COUNT = 20;
        const int MINE_COUNT = 10;
        const int TILE_COUNT = COLUMN_COUNT * ROW_COUNT;
        const int BUTTON_SIZE = 25;

        static bool IsGenerated = false;
        static MineSweeper ms = new MineSweeper(COLUMN_COUNT, ROW_COUNT, MINE_COUNT);
        static Button[,] buttonGrid = new Button[COLUMN_COUNT, ROW_COUNT];

        public MainWindow()
        {
            InitializeComponent();

            this.Width = (double)(COLUMN_COUNT * BUTTON_SIZE + 20.0 + 15.0);
            this.Height = (double)(ROW_COUNT * BUTTON_SIZE + 20.0 + 35.0);
            grid.Width = (double)(COLUMN_COUNT * BUTTON_SIZE);
            grid.Height = (double)(ROW_COUNT * BUTTON_SIZE);

            for (int rr = 0; rr < ROW_COUNT; rr++)
            {
                for (int cc = 0; cc < COLUMN_COUNT; cc++)
                {
                    buttonGrid[cc,rr] = new Button();

                    buttonGrid[cc, rr].Content = "";
                    buttonGrid[cc, rr].Name = "Tile_" + cc.ToString() + "_" + rr.ToString();
                    buttonGrid[cc, rr].Height = BUTTON_SIZE;
                    buttonGrid[cc, rr].Width = BUTTON_SIZE;
                    buttonGrid[cc, rr].HorizontalAlignment = HorizontalAlignment.Left;
                    buttonGrid[cc, rr].VerticalAlignment = VerticalAlignment.Top;
                    buttonGrid[cc, rr].Margin = new Thickness((double)(cc * BUTTON_SIZE), (double)(rr * BUTTON_SIZE), 0.0, 0.0);
                    buttonGrid[cc, rr].Background = Brushes.LightPink;
                    buttonGrid[cc, rr].Click += Tile_Left_Click;

                    //buttonGrid[cc, rr].InputBindings.Add(new InputBinding(Binding PressLetterCommand, Tile_Right_Click));
                    //<Button.InputBindings>
                    //<MouseBinding Gesture="RightClick" Command="{Binding PressLetterCommand}" />

                    grid.Children.Add(buttonGrid[cc, rr]);
                }
            }
        }
        
        //private void Tile_Right_Click(object sender, RoutedEventArgs e)
        //{
        //    Button btn = sender as Button;
        //}

        private void Tile_Left_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            List<string> title = new List<string>(btn.Name.Split('_'));
            int sel_col = int.Parse(title.ElementAt(1));
            int sel_row = int.Parse(title.ElementAt(2));

            //if (((MouseButtonEventArgs)e).RightButton == MouseButtonState.Pressed) {
            //    btn.Background = btn.Background == Brushes.DarkGray ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.DarkGray;
            //    btn.Content = "F";
            //}
            //else {
                //btn.Background = btn.Background = Brushes.DarkGray;
                btn.IsEnabled = false;
                
                btn.Content = "0";

                if (!IsGenerated)
                {
                    ms.constructGrid(sel_col, sel_row);
                }

                ms.checkTile(sel_col, sel_row);

                for (int rr = 0; rr < ROW_COUNT; rr++)
                {
                    for (int cc = 0; cc < COLUMN_COUNT; cc++)
                    {
                        //if (Grid[cc, rr].Display == DisplayType.Flagged)
                        //{
                        //}
                        //else if (grid[cc, rr].Display == DisplayType.Question)
                        //{
                        //}
                        if (ms.Grid[cc, rr].Display == DisplayType.Swept)
                        {
                            buttonGrid[cc, rr].Content = ms.Grid[cc, rr].Value;
                            buttonGrid[cc, rr].IsEnabled = false;
                        }
                        else if (ms.Grid[cc, rr].Display != DisplayType.Flagged)
                        {
                            buttonGrid[cc, rr].Content = "";
                        }
                    }
                }

                IsGenerated = true;

                if (ms.State == GameState.Lose)
                {
                    MessageBoxResult result = MessageBox.Show("YOU'RE LOSER !", "A loser is you!", MessageBoxButton.OK, MessageBoxImage.None);
                    if (result == MessageBoxResult.OK)
                    {
                        Application.Current.Shutdown();
                    }
                }
                else if (ms.State == GameState.Win)
                {
                    MessageBoxResult result = MessageBox.Show("YOU'RE WINNER !", "A winner is you!", MessageBoxButton.OK, MessageBoxImage.None);
                    if (result == MessageBoxResult.OK)
                    {
                        Application.Current.Shutdown();
                    }
                }
            //}
        }
    }
}

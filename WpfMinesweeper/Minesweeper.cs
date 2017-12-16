using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMinesweeper
{
    public enum GameState
    {
        Tentative,
        Win,
        Lose
    }

    class MineSweeper
    {
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public int MineCount { get; set; }
        public GameState State { get; set; }
        public Tile[,] Grid { get; private set; }
        public int SweepCount { get; private set; }
        public List<Tile> MineList { get; private set; }

        static List<Tile> flags = new List<Tile>();

        // Initalise blank grid
        public MineSweeper(int columnCount, int rowCount, int mineCount)
        {
            this.ColumnCount = columnCount;
            this.RowCount = rowCount;
            this.MineCount = mineCount;
            this.State = GameState.Tentative;
            this.Grid = new Tile[ColumnCount, RowCount];
            this.SweepCount = 0;

            for (int cc = 0; cc < ColumnCount; cc++)
            {
                for (int rr = 0; rr < RowCount; rr++)
                    Grid[cc, rr] = new Tile(cc, rr);
            }
        }

        // Check if the tile is a mine and check neighbouring tiles.
        public void checkTile(int sel_col, int sel_row)
        {
            if (Grid[sel_col, sel_row].Value == 9) // Mine selected = Game over
            {
                State = GameState.Lose;
            }
            sweepMine(sel_col, sel_row);
            if (Grid[sel_col, sel_row].Value == 0)
            {
                recurseSweepNeighbour(sel_col, sel_row);
            }

            if ((State != GameState.Lose) && (ColumnCount * RowCount - SweepCount == MineCount))
            {
                State = GameState.Win;
            }
        }

        // Set tile to Swept state and increment sweep counter
        private void sweepMine(int sel_col, int sel_row)
        {
            Grid[sel_col, sel_row].Display = DisplayType.Swept;
            SweepCount++;
        }

        // Recursively check neighbours of the selected tile
        private void recurseSweepNeighbour(int sel_col, int sel_row)
        {
            //foreach (Tile neighbour in Grid[sel_col, sel_row].getUnvalidatedNeighbours())
            foreach (Tile neighbour in neighbouringTiles(sel_col, sel_row))
            {
            //    try
            //    {
                    if ((Grid[neighbour.Column, neighbour.Row].Display != DisplayType.Swept) && (Grid[neighbour.Column, neighbour.Row].Value == 0))
                    {
                        sweepMine(neighbour.Column, neighbour.Row);
                        recurseSweepNeighbour(neighbour.Column, neighbour.Row);
                    }
                    else if (Grid[neighbour.Column, neighbour.Row].Display != DisplayType.Swept)
                    {
                        sweepMine(neighbour.Column, neighbour.Row);
                    }
            //    }
            //    catch (System.IndexOutOfRangeException e) { } // Do nothing
            }
        }

        // Initialise mine tiles.
        // Calculate the number of mines in neighbouring tiles.
        // Cater for edge cases to avoid out-of-bounds array checking.
        public void constructGrid(int sel_col, int sel_row)
        {
            Tile tempTile;
            int num_mines;
            this.MineList = new List<Tile>();

            Random random = new Random();

            // Initialise mine tiles
            for (int ii = 0; ii < MineCount; ii++)
            {
                Tile mineTile;
                do
                {
                    mineTile = new Tile(random.Next(0, ColumnCount), random.Next(0, RowCount));
                }
                while (MineList.Any(item => (item.Equals(mineTile))) || (sel_col == mineTile.Column && sel_row == mineTile.Row));
                MineList.Add(mineTile);
                Grid[mineTile.Column, mineTile.Row].Value = 9;
            }

            // Initialise neighbouring mines count
            for (int cc = 0; cc < ColumnCount; cc++)
            {
                for (int rr = 0; rr < RowCount; rr++)
                {
                    if (Grid[cc, rr].Value != 9)
                    {
                        tempTile = new Tile(cc, rr);
                        num_mines = 0;
                        
                        foreach (Tile neighbour in neighbouringTiles(tempTile.Column, tempTile.Row))
                        {
                            if (Grid[neighbour.Column, neighbour.Row].Value == 9)
                            {
                                num_mines++;
                            }
                        }

                        Grid[cc, rr].Value = num_mines;
                    }
                }
            }
        }

        private List<Tile> neighbouringTiles(int sel_col, int sel_row)
        {
            List<Tile> neighbours = new List<Tile>();
            if (sel_col > 0) // Not Left
            {
                neighbours.Add(new Tile(sel_col - 1, sel_row));
                if (sel_row > 0) // Not Left, Top
                {
                    neighbours.Add(new Tile(sel_col-1, sel_row-1));
                }
                if (sel_row < RowCount - 1) // Not Left, Bottom
                {
                    neighbours.Add(new Tile(sel_col-1, sel_row+1));
                }
            }
            if (sel_col < ColumnCount - 1) // Not Right
            {
                neighbours.Add(new Tile(sel_col + 1, sel_row));
                if (sel_row > 0) // Not Right, Top
                {
                    neighbours.Add(new Tile(sel_col+1, sel_row-1));
                }
                if (sel_row < RowCount - 1) // Not Right, Bottom
                {
                    neighbours.Add(new Tile(sel_col+1, sel_row+1));
                }
            }
            //if ((sel_col < ColumnCount-1) && (sel_col > 0)) // Centre, assume valid input
            //{
                if (sel_row > 0) // Centre, Top
                {
                    neighbours.Add(new Tile(sel_col, sel_row-1));
                }
                if (sel_row < RowCount-1) // Centre, Bottom
                {
                    neighbours.Add(new Tile(sel_col, sel_row+1));
                }
            //}
            return neighbours;
        }
    }
}

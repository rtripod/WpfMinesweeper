using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMinesweeper
{
    public enum DisplayType
    {
        Unknown,
        Swept,
        Flagged
    }

    class Tile
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public int Value { get; set; }
        public DisplayType Display { get; set; }

        public Tile(int column, int row)
        {
            this.Column = column;
            this.Row = row;
            this.Value = 0;
            this.Display = DisplayType.Unknown;
        }

        public List<Tile> getUnvalidatedNeighbours()
        {
            List<Tile> unvalidatedNeighbours = new List<Tile>();
            for (int dx = -1; dx < 2; dx++)
            {
                for (int dy = -1; dy < 2; dy++)
                    unvalidatedNeighbours.Add(new Tile(this.Column + dx, this.Row + dy));
            }
            return unvalidatedNeighbours;
        }

        public bool Equals(Tile other)
        {
            if (other == null) return false;
            return (this.Column.Equals(other.Column) && this.Row.Equals(other.Row));
        }
    }
}

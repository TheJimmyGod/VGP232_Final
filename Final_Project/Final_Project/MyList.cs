using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    // Tile type to input into a list<Is_Map>
    public enum Tile_Type
    {
        Empty, One, Two, Three, Four
    }
    public class Is_Map
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Tile_Type GetTileNumber { get; set; }
        public string TileName { get; set; }
    }
    // Button types
    public enum Types
    {
        First, Second, Third, Fourth, Erase
    }

    public class Tile_info
    {
        public string First_tile { get; set; }
        public string Second_tile { get; set; }
        public string Third_tile { get; set; }
        public string Fourth_tile { get; set; }
        public string ImagePath1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
    }
}
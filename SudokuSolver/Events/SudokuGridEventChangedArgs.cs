using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.Events
{
    public class SudokuGridEventChangedArgs : EventArgs
    {
        public SudokuGridEventChangedArgs(int x, int y, int value)
        {
            X = x;
            Y = y;
            Value = value;            
        }


        public int X { get; }
        public int Y { get; }
        public int Value { get; }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolverTest
{
    [TestClass]
    public class SudokuGridTests
    {
        public int[,] _validGrid = new int[9, 9] {
                { 2,0,4, 0,0,6, 0,0,9 },
                { 8,0,0, 0,0,0, 5,0,0 },
                { 0,3,0, 1,5,0, 7,2,0 },

                { 0,6,2, 0,8,0, 0,0,1 },
                { 3,9,0, 0,4,0, 0,5,0 },
                { 7,0,0, 0,0,0, 0,3,0 },

                { 0,4,0, 0,0,8, 0,0,7 },
                { 0,0,0, 9,2,0, 1,0,5 },
                { 5,0,0, 0,0,0, 6,0,0 }
            };
        public int[] _validList = new int[]{
                  2,0,4, 0,0,6, 0,0,9  ,
                  8,0,0, 0,0,0, 5,0,0  ,
                  0,3,0, 1,5,0, 7,2,0  ,

                  0,6,2, 0,8,0, 0,0,1  ,
                  3,9,0, 0,4,0, 0,5,0  ,
                  7,0,0, 0,0,0, 0,3,0  ,

                  0,4,0, 0,0,8, 0,0,7  ,
                  0,0,0, 9,2,0, 1,0,5  ,
                  5,0,0, 0,0,0, 6,0,0
        };

        public int[,] _invalidGrid = new int[9, 9] {
                { 2,0,4, 0,0,6, 0,0,9 },
                { 8,0,0, 0,0,0, 5,0,0 },
                { 0,3,0, 1,50,0, 7,2,0 },

                { 0,6,2, 0,8,0, 0,0,1 },
                { 3,9,0, 0,4,0, 0,5,0 },
                { 7,0,0, 0,0,0, 0,3,0 },

                { 0,4,0, 0,0,8, 0,0,7 },
                { 0,0,0, 9,2,0, 1,0,5 },
                { 5,0,0, 0,0,0, 6,0,0 }
            };
        public int[] _invalidList = new int[] {
                  2,0,4, 0,0,6, 0,0,9  ,
                  8,0,0, 0,0,0, 5,0,0  ,
                  0,3,0, 1,50,0, 7,2,0  ,

                  0,6,2, 0,8,0, 0,0,1  ,
                  3,9,0, 0,4,0, 0,5,0  ,
                  7,0,0, 0,0,0, 0,3,0  ,

                  0,4,0, 0,0,8, 0,0,7  ,
                  0,0,0, 9,2,0, 1,0,5  ,
                  5,0,0, 0,0,0, 6,0,0  
            };

        [TestMethod]
        public void CreateEmptyGrid()
        {
            var grid = new SudokuGrid();
            Assert.IsNotNull(grid);
        }

        [TestMethod]
        public void CreateGrids()
        {
            var result = new List<SudokuGrid>();
            foreach(var item in Enumerable.Range(3,10))
            {
                var grid = new SudokuGrid(item * item);
                Assert.IsNotNull(grid);
                result.Add(grid);

            }            
        }

        [TestMethod]
        public void CreateInvalidEmptyGrid()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var grid = new SudokuGrid(8);
            });
        }

        [TestMethod]
        public void CreatePrefilledGrid()
        {
            var grid = new SudokuGrid(_validGrid);
            Assert.IsNotNull(grid);
        }

        [TestMethod]
        public void CreatePrefilledGridFromList()
        {
            var grid = new SudokuGrid(_validList);
            
            for(int x=0; x< grid.Size;x++)
            {
                for (int y = 0; y < grid.Size; y++)
                {
                    Assert.AreEqual(_validGrid[x, y], grid.Grid[ x,y]);
                }
            }

            Assert.IsNotNull(grid);
        }

        [TestMethod]
        public void CreatePrefilledInvalidGridFromList()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var grid = new SudokuGrid(_invalidList);
            });
        }

        [TestMethod]
        public void CreateInvalidPrefilledGrid()
        {            
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var grid = new SudokuGrid(_invalidGrid);
            });
        }

        [TestMethod]
        public void GetRows()
        {
            var grid = new SudokuGrid(_validGrid);
            var row0 = grid.GetRow(0);
            Assert.IsTrue(row0.SequenceEqual(new[] { 2, 0, 4, 0, 0, 6, 0, 0, 9 }));
            var row1 = grid.GetRow(1);
            Assert.IsTrue(row1.SequenceEqual(new[] { 8, 0, 0, 0, 0, 0, 5, 0, 0 }));
            var row2 = grid.GetRow(2);
            Assert.IsTrue(row2.SequenceEqual(new[] { 0, 3, 0, 1, 5, 0, 7, 2, 0 }));
            var row3 = grid.GetRow(3);
            Assert.IsTrue(row3.SequenceEqual(new[] { 0, 6, 2, 0, 8, 0, 0, 0, 1 }));
        }

        [TestMethod]
        public void GetColumns()
        {
            var grid = new SudokuGrid(_validGrid);
            var column0 = grid.GetColumn(0);
            Assert.IsTrue(column0.SequenceEqual(new[] { 2, 8, 0, 0, 3, 7, 0, 0, 5 }));
            var column1 = grid.GetColumn(1);
            Assert.IsTrue(column1.SequenceEqual(new[] { 0, 0, 3, 6, 9, 0, 4, 0, 0 }));
            var column2 = grid.GetColumn(2);
            Assert.IsTrue(column2.SequenceEqual(new[] { 4, 0, 0, 2, 0, 0, 0, 0, 0 }));
            var column3 = grid.GetColumn(3);
            Assert.IsTrue(column3.SequenceEqual(new[] { 0, 0, 1, 0, 0, 0, 0, 9, 0 }));
        }

        [TestMethod]
        public void GetFromBlock()
        {
            var grid = new SudokuGrid(_validGrid);

            var block00 = grid.GetFromCurrentBlock(1, 1);
            Assert.IsTrue(block00.SequenceEqual(new[] { 2, 0, 4, 8, 0, 0, 0, 3, 0 }));
            var block01 = grid.GetFromCurrentBlock(2, 5);
            Assert.IsTrue(block01.SequenceEqual(new[] { 0, 0, 6, 0, 0, 0, 1, 5, 0 }));
            var block11 = grid.GetFromCurrentBlock(4, 5);
            Assert.IsTrue(block11.SequenceEqual(new[] { 0, 8, 0, 0, 4, 0, 0, 0, 0 }));
        }

        [TestMethod]
        public void GetPossible()
        {
            var grid = new SudokuGrid(_validGrid);
            var possible = grid.GetPossibleNumbers(2,0);
            Assert.IsTrue(possible.SequenceEqual(new[] {6,9 }));
        }

        [TestMethod]
        public void GetGridString()
        {
            var result = new SudokuGrid(_validGrid).GetGridString();
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }



        [TestMethod]
        public void SolveGrid()
        {
            var grid = new SudokuGrid(_validGrid);
            var result = grid.SolveGrid();            

            Assert.IsTrue(result,Environment.NewLine+grid.GetGridString());
        }
    }
}

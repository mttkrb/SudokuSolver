using SudokuSolver.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SudokuSolver
{
    public class SudokuGrid: INotifyPropertyChanged
    {
        

        private const int _defaultGridSize = 9;
        public SudokuGrid():this(_defaultGridSize) { }

        public SudokuGrid(int size)
        {
            var blocksize = (int)Math.Sqrt(size);

            if(blocksize * blocksize != size || blocksize<3)
            {
                throw new ArgumentException("given grid is invalid");
            }


            Size = size;
            _blockSize = blocksize;
        }

        public SudokuGrid(int[,] given ) : this((int)Math.Sqrt(given.Length))
        {
            Grid = new int[Size, Size];
            
            if (given.Rank!=Grid.Rank || given.Length!= Grid.Length )
            {
                throw new ArgumentException($"array must be [{Size},{Size}]");
            }            

            for(int x=0;x< Size; x++)
            {
                for(int y =0;y< Size; y++)
                {
                    if(given[x,y]>Size)
                    {
                        throw new ArgumentException($"numbers must be less or equal {Size}");
                    }
                    Grid[x, y] = given[x, y];
                    if(given[x, y]>0)
                    {
                        SolvedFields++;
                    }
                }
            }            
        }

        public SudokuGrid(IEnumerable<int> given) : this(given.ToArray()) { }
        public SudokuGrid(int[] given) : this(To2dArray(given)) { }


        private static int[,] To2dArray(int[] items)
        {
            var size = (int)Math.Sqrt(items.Length);
            var result = new int[size,size];
            var index = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    result[x, y] = items[index++];
                }
            }

            return result;
        }

        public int [,] Grid { get; private set; }        


        public int Size;
        private int _blockSize;
        private bool _isSolving;
        
        public int SolvedFields { get; private set; }
        public int OpenFields => Grid.Length - SolvedFields;

        public void SolveField(int x, int y, int value)
        {
            var old = Grid[x, y];
            if (!_isSolving && value<Size && old!=value)
            {                
                Grid[x, y] = value;
                if(value==0)
                {
                    SolvedFields--;
                }
                else
                {
                    SolvedFields++;
                }
                OnPropertyChanged("SolvedFields");
                OnPropertyChanged("OpenFields");
            }
        }
        

        public bool IsGridSolved()
        {
            return OpenFields == 0;
        }

        public bool SolveGrid(int previousMultipleFields=0)
        {            
            int multipleFields = 0;
            if(IsGridSolved())
            {
                return true;
            }

            _isSolving = true;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if(Grid[x,y]==0)
                    {
                        var numbers = GetPossibleNumbers(x, y);                        
                        if (numbers.Count == 1)
                        {
                            var value = numbers.First();
                            Grid[x, y] = value;
                            SolvedFields++;
                            OnRaiseValueChanged(x, y, value);                                                        
                        }
                        else
                        {
                            multipleFields++;
                        }
                    }
                }
            }
            _isSolving = false;
            if (previousMultipleFields!= multipleFields)
            {                
                return SolveGrid(multipleFields);
            }
            else
            {                
                return false;
            }

            
        }

        private void MinimizePossibleItems(int x,int y, int value)
        {
                        
        }        

        public IEnumerable<int> GetRow(int rowIndex)
        {
            for (int i = 0; i < Size; i++)
            {
                yield return Grid[rowIndex, i];
            }
        }

        public IEnumerable<int> GetColumn(int columnIndex)
        {
            for (int i = 0; i < Size; i++)
            {
                yield return Grid[i, columnIndex];
            }
        }

        public IEnumerable<int> GetFromCurrentBlock(int x,int y)
        {
            var blockx = x / _blockSize;
            var blocky = y / _blockSize;

            var startx = _blockSize * blockx;
            var endx = startx + _blockSize;
            var starty = _blockSize* blocky;
            var endy = starty + _blockSize;

            for (int bx =startx;bx<endx;bx++)
            {
                for (int by = starty; by < endy; by++)
                {
                    yield return Grid[bx, by];
                }
            }
        }

        private List<int> GetPossibleFromRow(int rowIndex)
        {
            return new List<int>(Enumerable.Range(1, Size).Except(GetRow(rowIndex)));
        }

        private List<int> GetPossibleFromColumn(int columnIndex)
        {
            return new List<int>(Enumerable.Range(1, Size).Except(GetColumn(columnIndex)));            
        }

        private List<int> GetPossibleFromBlock(int x, int y)
        {
            return new List<int>(Enumerable.Range(1, Size).Except(GetFromCurrentBlock(x,y)));            
        }


        public List<int> GetPossibleNumbers(int x,int y)
        {
            if(Grid[x,y]!=0)
            {
                return new List<int>();
            }
            
            return new List<int>(
                GetPossibleFromRow(x)
                .Concat(GetPossibleFromColumn(y))
                .Concat(GetPossibleFromBlock(x, y))
                .GroupBy(g => g)
                .Where(w => w.Count() == 3)
                .Select(s => s.Key));           
        }

        public string GetGridString()
        {
            var sb = new StringBuilder();
            for (int x = 0; x < Size; x++)
            {                
                for (int y = 0; y < Size; y++)
                {
                    
                    if(y==Size-1 && x!= Size-1)
                    {
                        sb.Append(Grid[x, y]);
                        sb.Append(Environment.NewLine);
                    }
                    else
                    {
                        sb.Append($"{Grid[x, y]} ");
                    }
                }
            }

            return sb.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<SudokuGridEventChangedArgs> OnValueChanged;
        private void OnRaiseValueChanged(int x,int y, int value)
        {
            OnPropertyChanged("SolvedFields");
            OnPropertyChanged("OpenFields");
            OnValueChanged?.Invoke(this, new SudokuGridEventChangedArgs(x, y, value));
        }        
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SudokuSolver.Gui
{
    /// <summary>
    /// Interaktionslogik für SudokuGridControl.xaml
    /// </summary>
    public partial class SudokuGridControl : UserControl, IDisposable
    {
        public SudokuGridControl()
        {
            InitializeComponent();
        }

        public SudokuGrid SudokuGrid
        {
            get { return (SudokuGrid)GetValue(SudokuGridProperty); }
            set { SetValue(SudokuGridProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SudokuGrid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SudokuGridProperty =
            DependencyProperty.Register("SudokuGrid", typeof(SudokuGrid), typeof(SudokuGridControl), new PropertyMetadata(null, (s, e) => {
                if (s is SudokuGridControl ctrl )
                {
                    if (e.OldValue is SudokuGrid sdGrid)
                    {
                        ctrl.CleanUp(sdGrid);
                    }
                    if (e.NewValue is SudokuGrid sdgrid)
                    {
                        ctrl.setGrid(sdgrid);              
                    }
                }
            }));

        internal void setGrid(SudokuGrid grid)
        {
            grid.OnValueChanged += Grid_OnValueChanged;
            for (int i = 0; i < grid.Size; i++)
            {
                Sudoku.RowDefinitions.Add(new RowDefinition());
                Sudoku.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int x = 0; x < grid.Size; x++)
            {
                for (int y = 0; y < grid.Size; y++)
                {
                    var numberItem = new NumberItem();
                    Sudoku.Children.Add(numberItem);
                    numberItem.SetValue(Grid.ColumnProperty, y);
                    numberItem.SetValue(Grid.RowProperty, x);
                    numberItem.NumberLength = grid.Size;
                    numberItem.Number = grid.Grid[x, y];
                    numberItem.PossibleNumbers = grid.GetPossibleNumbers(x, y).ToArray();
                    numberItem.PropertyChanged += NumberItem_PropertyChanged;
                    numberItem.IsEnabled = grid.Grid[x, y]==0;
                }
            }
        }

        private void NumberItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Number" && sender is NumberItem ni)
            {
                var x = (int)ni.GetValue(Grid.RowProperty);
                var y = (int)ni.GetValue(Grid.ColumnProperty);                
                SudokuGrid.SolveField(x, y, ni.Number);
                UpdatePossibleNumbers();
            }
        }

        private void Grid_OnValueChanged(object sender, Events.SudokuGridEventChangedArgs e)
        {            
            var ctrl = Sudoku.Children.OfType<NumberItem>().FirstOrDefault(f => f.GetValue(Grid.RowProperty) as int?==e.Y && f.GetValue(Grid.ColumnProperty) as int?==e.X );
            if(ctrl!=null)
            {
                ctrl.Number = e.Value;
                foreach (var item in Sudoku.Children.OfType<NumberItem>())
                {
                    var y = (int)item.GetValue(Grid.ColumnProperty);
                    var x = (int)item.GetValue(Grid.RowProperty);
                    item.Number = SudokuGrid.Grid[x, y];
                    item.PossibleNumbers=SudokuGrid.GetPossibleNumbers(x,y).ToArray();
                }
            }
        }        

        private void UpdatePossibleNumbers()
        {
            foreach (var item in Sudoku.Children.OfType<NumberItem>())
            {
                var y = (int)item.GetValue(Grid.ColumnProperty);
                var x = (int)item.GetValue(Grid.RowProperty);
                item.PossibleNumbers = SudokuGrid.GetPossibleNumbers(x, y).ToArray();
            }
        }

        public void Dispose()
        {
            CleanUp(SudokuGrid);
        }

        internal void CleanUp(SudokuGrid sudokuGrid)
        {
            foreach (var item in Sudoku.Children.OfType<NumberItem>())
            {
                item.PropertyChanged -= NumberItem_PropertyChanged;

            }
            Sudoku.Children.Clear();
            sudokuGrid.OnValueChanged -= Grid_OnValueChanged;
        }
    }
}

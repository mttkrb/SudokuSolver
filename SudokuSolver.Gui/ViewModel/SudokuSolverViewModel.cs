using SudokuSolver.Gui.Commands;
using SudokuSolver.Loader;
using SudokuSolver.Loader.ZeitDe;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SudokuSolver.Gui.ViewModel
{
    public class SudokuSolverViewModel : INotifyPropertyChanged, IDisposable
    {
        public SudokuSolverViewModel()
        {        
            Size = 9;
            _sudokuLoader = new ZeitDeLoader();
            GenerateGridCommand = new RelayCommand<object>(async o => { if (Size == 9) SudokuGrid = new SudokuGrid(await _sudokuLoader.GetItemsAsync()); else SudokuGrid = new SudokuGrid(Size); });
            SolveGridCommand = new RelayCommand<object>(o => SudokuGrid?.SolveGrid());
        }
        
        private LoaderBase _sudokuLoader;
        private int _size;

        public int Size
        {
            get { return _size; }
            set { _size = value;OnPropertyChanged(); }
        }

        private SudokuGrid _sudokuGrid;

        public SudokuGrid SudokuGrid
        {
            get { return _sudokuGrid; }
            set { _sudokuGrid = value;OnPropertyChanged(); }
        }        

        public RelayCommand<object> GenerateGridCommand { get; }
        public RelayCommand<object> SolveGridCommand { get; }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
         
        }
    }
}

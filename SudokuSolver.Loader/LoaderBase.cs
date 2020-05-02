using System.Collections.Generic;
using System.Threading.Tasks;

namespace SudokuSolver.Loader
{
    public abstract class LoaderBase
    {
        public LoaderBase(int size)
        {
            Size = size;
        }
        public int Size { get;  }

        public abstract Task<ICollection<int>> GetItemsAsync();
    }
}

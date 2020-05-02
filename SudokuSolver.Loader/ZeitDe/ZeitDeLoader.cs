using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SudokuSolver.Loader.ZeitDe
{   
    public class ZeitDeLoader : LoaderBase
    {
        private HttpClient _client;
        
        public ZeitDeLoader() : this(DateTime.Today,Difficulty.Medium)
        {
            
        }

        public ZeitDeLoader(DateTime date, Difficulty difficulty) : base(9)
        {
            _date = date;
            _difficulty = difficulty;
            _client = new HttpClient();

        }

        private DateTime _date;
        private Difficulty _difficulty;

        public override async Task<ICollection<int>> GetItemsAsync()
        {   

            //var request = await _client.GetAsync("https://sudoku.zeit.de/sudoku/level/3/2020-5-1");
            var request = await _client.GetAsync($"https://sudoku.zeit.de/sudoku/level/{(int)_difficulty}/{_date:yyyy-M-d}");
            var result = new List<int>();
            if (request.IsSuccessStatusCode)
            {
                var httpResult = await request.Content.ReadAsStringAsync();
                var type = JsonConvert.DeserializeAnonymousType(httpResult, new { game = "", solve = "", id = 0 });

                foreach (var item in type.game)
                {
                    if (item == '.')
                    {
                        result.Add(0);
                    }
                    else
                    {
                        result.Add(int.Parse(item.ToString()));
                    }
                }
            }

            return result;
        }
    }
}

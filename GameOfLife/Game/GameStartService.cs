using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameOfLife.Game
{
    public class GameStartService
    {

        GameStartWrapper game;
        public GameStartService()
        {

            game = new GameStartWrapper(10);

            game.ToggleCell(5, 5);
            game.ToggleCell(6, 5);
            game.ToggleCell(7, 5);

            game.ToggleCell(6, 6);

            //game.ToggleCell(1, 1);
            //game.ToggleCell(2, 1);
            //game.ToggleCell(3, 2);

            //game.BeginGeneration();
            //game.Wait();
        }

        public class Game
        {
            public int name { get; set; }
            public int size { get; set; }
            public Dictionary<Tuple<int, int>, bool> points { get; set; }
        }

        public List<Game> GetResults()
        {
            game.Update();
            game.Wait();

            var v = new Dictionary<Tuple<int, int>, bool>();

            for (var y = 0; y < game.Size; y++)
            {
                for (var x = 0; x < game.Size; x++)
                {
                    v.Add(new Tuple<int, int>(x, y), game[x, y]);
                }

            }

            var _gameParams = new List<Game>();
            if (v.Any(x => x.Value))
            {
                _gameParams.Add(new Game()
                {
                    name = 1,//game.Generation,
                    size = game.Size,
                    points = v
                });
            }

            return _gameParams;
            
        }

        public List<Game> GetResultsMain()
        {
            game.BeginGeneration();
            game.Wait();

            var v = new Dictionary<Tuple<int, int>, bool>();

            for (var y = 0; y < game.Size; y++)
            {
                for (var x = 0; x < game.Size; x++)
                {
                    v.Add(new Tuple<int, int>(x, y), game[x, y]);
                }

            }

            var _gameParams = new List<Game>();
            if (v.Any(x => x.Value))
            {
                _gameParams.Add(new Game()
                {
                    name = 0,
                    size = game.Size,
                    points = v
                });
            }

            return _gameParams;

        }
    }
}
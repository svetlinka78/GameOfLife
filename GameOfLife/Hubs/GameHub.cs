using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Threading;
using GameOfLife.Game;

namespace GameOfLife.Hubs
{
    public class GameHub : Hub
    {
        //public CancellationTokenSource _tokenSource;
        public bool _cancel;
        public GameHub()
        {
           // StartGameCollection();
        }

        public void Send(string message)
        {
            Clients.All.Message(Context.User.Identity.Name + " says " + message);

        }
        void StartGameCollection()
        {
            var task = Task.Factory.StartNew(async () =>
            {
                var gameService = new GameService();
                while (true)
                {
                    var results = gameService.GetResults();
                    Clients.All.newPoints(results);
                    await Task.Delay(2000);
                }

            }, TaskCreationOptions.LongRunning);
        }

       public void StartGame()
        {
            
            var gameService = new GameStartService();

            var firstresult = gameService.GetResultsMain();
            Clients.All.outputBoard(firstresult);
     
            //_tokenSource = new CancellationTokenSource()           

            var task = Task.Factory.StartNew( () =>
            {
             
                var cnt = 1;
                while (cnt > 0)
                {
                    var results = gameService.GetResults();
                    Clients.All.outputBoard(results);
                    //await Task.Delay(2000);
                    cnt = results.Count();

                };
            },TaskCreationOptions.LongRunning );
            //}, _tokenSource.Token, TaskCreationOptions.LongRunning,TaskScheduler.Default);

            //try
            //{
            //    task.Wait();
            //}
            //catch (AggregateException e)
            //{
            //    foreach (var v in e.InnerExceptions)
            //        Console.WriteLine(e.Message + " " + v.Message);
            //}
            //finally
            //{
            //    _tokenSource.Dispose();
            //}


        }
        // int _connections = 0;
        public void StopGame() {
            // _tokenSource.Cancel();
            

        }
    }
  }
      

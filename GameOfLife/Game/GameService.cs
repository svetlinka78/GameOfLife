using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;


namespace GameOfLife.Game
{
    public class GameService
    {
        List<GameWrapper> _counters;
        public GameService()
        {
            _counters = new List<GameWrapper>();
            _counters.Add(new GameWrapper("Processor", "Processor", "% Processor Time", "_Total"));
            _counters.Add(new GameWrapper("Paging", "Memory", "Pages/sec"));
            _counters.Add(new GameWrapper("Disk", "PhysicalDisk", "% Disk Time", "_Total"));
        }

        public dynamic GetResults()
        {
            return _counters.Select(x => new { name = x.Name, value = x.Value });         
        }

    }
}






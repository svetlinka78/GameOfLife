using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace GameOfLife.Game
{
    public class GameWrapper
    {
        public string Name { get; protected set; }
        PerformanceCounter _counter;
        public float Value
        {
           get {
                return _counter.NextValue();
            }
        }
        public GameWrapper(string name, string category,string counter, string instance = "")
        {
            Name = name;
            _counter = new PerformanceCounter(category,counter,instance, readOnly: true);       
        }

    }
}



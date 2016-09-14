using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekDelegate
{
    public class EventSource
    {
        public event Action<int> FirstEventSource;
        public event Action<int, double> SecondEventSource;
        public event Action<int, double, int> ThirdEventSource;
        public event Action<int, double, int, int> FourthEventSource;
        public event Action<int, int, int, int> FifthEventSource;

        public void CallFirstEventSource()
        {
            FirstEventSource?.Invoke(1);
        }

        public void CallSecondEventSource()
        {
            SecondEventSource?.Invoke(1, 2);
        }
        public void CallThirdEventSource()
        {
            ThirdEventSource?.Invoke(1, 2, 3);
        }
        public void CallFourthEventSource()
        {
            FourthEventSource?.Invoke(1, 2, 3, 4);
        }
        public void CallFifthEventSource()
        {
            FifthEventSource?.Invoke(1, 2, 3, 4);
        }
    }
}

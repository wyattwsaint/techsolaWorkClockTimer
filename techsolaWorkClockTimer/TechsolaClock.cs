using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace techsolaWorkClockTimer
{
    class TechsolaClock : ObservableObject
    {
        private string timer;

        public string Timer
        {
            get { return timer; }
            set 
            { 
                timer = value; 
                OnPropertyChanged(Timer);
            }
        }
        public TechsolaClock()
        {
        }

        public void StartClock()
        {
            Task.Run(() =>
            {
                int time = 0;
                while (true)
                {
                    time += 1;
                    Timer = "";
                    Timer = time.ToString();
                    Thread.Sleep(1000);
                }
            });
        }
    }
}

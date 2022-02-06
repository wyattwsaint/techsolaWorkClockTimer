using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System;

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
            Task.Run(() =>
            {
                while (true)
                {
                    int hours = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        int minutes = 0;
                        for (int j = 0; j < 60; j++)
                        {
                            int seconds = -1;
                            for (int k = 0; k < 60; k++)
                            {
                                seconds += 1;
                                Timer = "";
                                Timer = $"{hours.ToString("0#")}:{minutes.ToString("0#")}:{seconds.ToString("0#")}";
                                Thread.Sleep(1000);
                            }
                            minutes += 1;
                        }
                        hours += 1;
                    }
                }
            });
        }
    }
}

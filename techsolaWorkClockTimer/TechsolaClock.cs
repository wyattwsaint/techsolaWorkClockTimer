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

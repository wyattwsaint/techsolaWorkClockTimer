using System;
using System.Threading;
using System.Threading.Tasks;

namespace techsolaWorkClockTimer
{
    class TechsolaClock : ObservableObject
    {
        private CancellationTokenSource? cancellationTokenSource;

        private string timer;
        public string Timer { get => timer; private set => Set(ref timer, value); }

        public bool IsRunning => cancellationTokenSource is not null;

        public void Start()
        {
            cancellationTokenSource = new();
            OnPropertyChanged(nameof(IsRunning));

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        var hours = 0;
                        for (var i = 0; i < 8; i++)
                        {
                            var minutes = 0;
                            for (var j = 0; j < 60; j++)
                            {
                                var seconds = -1;
                                for (var k = 0; k < 60; k++)
                                {
                                    seconds += 1;
                                    Timer = $"{hours:00}:{minutes:00}:{seconds:00}";
                                    await Task.Delay(1000, cancellationTokenSource.Token);
                                }
                                minutes += 1;
                            }
                            hours += 1;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                }
            });
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();

            cancellationTokenSource = null;
            OnPropertyChanged(nameof(IsRunning));
        }
    }
}

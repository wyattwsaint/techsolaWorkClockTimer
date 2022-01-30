using System;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace techsolaWorkClockTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string test = "Test1234";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Test 
        { 
            get 
            { 
                return test; 
            } 
            set 
            { 
                test = value;
                OnPropertyChanged(value);
            } 
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void startStopClock_Click(object sender, RoutedEventArgs e)
        {
            Test = "Test4321";
        }
    }
}

﻿using System.Windows;

namespace techsolaWorkClockTimer
{
    /// <summary>
    /// Interaction logic for WorkdayComplete.xaml
    /// </summary>
    public partial class WorkdayComplete : Window
    {
        public WorkdayComplete()
        {
            InitializeComponent();
            DataContext = App.Clock;

            var clock = (TechsolaClock)DataContext;

        }
    }
}

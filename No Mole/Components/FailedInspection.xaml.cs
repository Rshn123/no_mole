﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace No_Mole.Components
{
    /// <summary>
    /// Interaction logic for FailedInspection.xaml
    /// </summary>
    public partial class FailedInspection : Window
    {
        private readonly Button? _mainButton;
        private MainWindow _mainWindow;

        private DispatcherTimer _timer;        // Timer for updating the clock
        private TimeSpan _elapsedTime;        // Tracks elapsed time
        private TimeSpan _durationLimit;      // Duration limit (15 seconds)
        public FailedInspection(Button button, MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainButton = button!;
        }

        private void PassButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FailButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            _mainButton!.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AD42AD"));
            _mainWindow.ChangeText("Start Inspection");
            _mainWindow.ChangeButtonImage("Resources/Icons/play.png");
            _mainWindow.ChangeRecordingVisibility(false);
            _mainWindow.StopTimer();
            this.Close();
        }
    }
}

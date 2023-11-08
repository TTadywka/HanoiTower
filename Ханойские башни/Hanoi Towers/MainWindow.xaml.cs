using System;
using System.Diagnostics;
using System.Windows;

namespace Hanoi_Towers
{

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameSettings gameSettings = new GameSettings();

            //Запуск формы с игрой
            ManualGame manual = new ManualGame(gameSettings);
            manual.Owner = this;
            manual.ShowDialog();
        }
    }
}

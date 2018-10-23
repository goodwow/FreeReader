using FreeReader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FreeReader
{
    public partial class App : Application
    {
        public void AppStartup(object sender, StartupEventArgs args)
        {
            SettingsManager.Instance.LoadSettings();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SettingsManager.Instance.SaveSettings();
        }
    }
}

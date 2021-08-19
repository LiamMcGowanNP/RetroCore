using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Reflection;
using System.Diagnostics;
using Ini;

namespace RetroCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    internal delegate void Invoker();

    public partial class App : Application
    {
        public App()
        {
            // Populate global variables
            LoadAppVariables gen = new LoadAppVariables();
            gen.LoadApplication();

            ApplicationInitialize = _applicationInitialize;
        }
        public static new App Current
        {
            get { return Application.Current as App; }
        }

        internal delegate void ApplicationInitializeDelegate(SplashScreen splashWindow);

        internal ApplicationInitializeDelegate ApplicationInitialize;

        private void _applicationInitialize(SplashScreen splashWindow)
        {
            string strEvent = "Local Variables Loaded";
            // Step through usual opening processes

            for (int i = 1; i < strEvent.Length+1; i++)
            {
                splashWindow.SetProgress(strEvent.Substring(0,i));
                Thread.Sleep(100);
            }
            Thread.Sleep(500);

            strEvent = "Checking Version";

            for (int i = 1; i < strEvent.Length + 1; i++)
            {
                splashWindow.SetProgress(strEvent.Substring(0, i));
                Thread.Sleep(100);
            }
            // Check version numbers
            Global.CheckVersions();
            Thread.Sleep(500);

            strEvent = "Checking Connection";

            for (int i = 1; i < strEvent.Length + 1; i++)
            {
                splashWindow.SetProgress(strEvent.Substring(0, i));
                Thread.Sleep(100);
            }
            // Check if db is online
            #region DB Online
            if (!Global.IsServerConnected(Global.ConnectionString))
            {
                MessageBox.Show(Global.ApplicationName + " is not available.  Please try again later.  If the problem persists, please report the error to the IT Helpdesk", Global.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                System.Threading.Thread.CurrentThread.Abort();
                Application.Current.Shutdown();
                Environment.Exit(0);
            }
            #endregion DB Online
            Thread.Sleep(500);

            strEvent = "Checking User";
            for (int i = 1; i < strEvent.Length + 1; i++)
            {
                splashWindow.SetProgress(strEvent.Substring(0, i));
                Thread.Sleep(100);
            }
            // Get user details            
            User user = new User();
            Global.FullName = user.FullName;
            Global.PID = user.PID;
            Thread.Sleep(500);

            strEvent = "Loading Application";
            for (int i = 1; i < strEvent.Length + 1; i++)
            {
                splashWindow.SetProgress(strEvent.Substring(0, i));
                Thread.Sleep(100);
            }
            Thread.Sleep(500);

            Global.blnmainWindow = false;

            // Create the main window, but on the UI thread.
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                if (Global.blnmainWindow != true) //Window already opened
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Global.blnmainWindow = true;

                }
            });
        }
    }
}

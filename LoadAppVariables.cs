using System;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using Ini;

namespace RetroCore
{
    class LoadAppVariables
    {
        public static string GlobalFile;
        public static string LocalFile;
        public static int MainPageSize;
        public static string BGColour;
        public static string Colour1 = "#C9DBEA";
        public static string Colour2 = "#B6D2EA";
        public static string Colour3 = "#83BAEA";
        public void LoadApplication()
        {

            // get path to local file
            #region Local FIle
            LocalFile = AppDomain.CurrentDomain.BaseDirectory + "lclRetroCore.ini";
            IniFile iniLocal = new IniFile(LocalFile);
            GlobalFile = iniLocal.IniReadValue("Paths", "GlobalFile");
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "lclRetroCore.ini"))
            {
                try
                {
                    MainPageSize = Convert.ToInt16(iniLocal.IniReadValue("UserSettings", "MainPageSize"));
                }
                catch
                {
                    MainPageSize = 100;
                }
                try
                {
                    BGColour = Convert.ToString(iniLocal.IniReadValue("UserSettings", "BGColour"));
                }
                catch
                {
                    BGColour = "White";
                }
            }
            else
            {
                MessageBox.Show("Unable to find the Local settings file.  The application will now close.", Global.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Application.Current.Shutdown();
                Environment.Exit(1);
            }
            #endregion

            // Check global file exists
            #region Global File
            if (File.Exists(GlobalFile))
            {
                // Do nothing;
            }
            else
            {
                MessageBox.Show("Unable to find the Global settings file.  The application will now close.", Global.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Application.Current.Shutdown();
                Environment.Exit(1);
            }
            #endregion

            IniFile iniGlobal = new IniFile(GlobalFile);
            int iTimeOut;

            // read info from Global file
            Global.ConnectionString = iniGlobal.IniReadValue("System", "ConnectString");
            Global.CurrentVersion = iniGlobal.IniReadValue("AppDetails", "CurrentVersion");
            Global.InstallPath = iniGlobal.IniReadValue("AppDetails", "InstallPath");

            // Application Name, default to Assembly Name
            Global.ApplicationName = iniGlobal.IniReadValue("AppDetails", "ApplicationName");
            Global.ApplicationName = Global.ApplicationName == "" ? System.Reflection.Assembly.GetCallingAssembly().GetName().Name : Global.ApplicationName;

            // Application Name, default to Assembly Name
            Global.BDAppNo = iniGlobal.IniReadValue("AppDetails", "BDAppNo");
            Global.BDAppNo = Global.BDAppNo == "" ? "000000" : Global.BDAppNo;

            // Application Name, default to Assembly Name
            Global.ResolverGroup = iniGlobal.IniReadValue("AppDetails", "ResolverGroup");
            Global.ResolverGroup = Global.ApplicationName == "" ? "HMRC B Local Compliance" : Global.ResolverGroup;


            // External option to adjust connection timeout.
            try
            {
                iTimeOut = int.Parse(iniGlobal.IniReadValue("System", "TimeOut"));
            }
            catch
            {
                iTimeOut = 30;  // default
            }
            Global.TimeOut = iTimeOut < 30 ? 30 : iTimeOut; // avoid any possibility of zero by setting minimum at 10

        }


        public string getAppVersion(string strLocalIniFile)
        {
            IniFile iniLocal = new IniFile(strLocalIniFile);
            string strLocalVersionNo = iniLocal.IniReadValue("Version", "CurrentVersion");

            return strLocalVersionNo;
        }

    }
}

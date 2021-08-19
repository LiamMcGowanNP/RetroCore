using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows;

namespace RetroCore
{

    public class StringSplitters
    {
        public string[] SplitCamelCase(string source)
        {
            return Regex.Split(source, @"(?<!^)(?=[A-Z])");
        }
    }

    public class Title
    {
        public string Description { get; set; }
        public int ID { get; set; }
    }

    // Define a class to receive parsed values
    public static class Options
    {

        public static bool Debug
        {
            get; set;

        }

        public static string PID
        {
            get; set;

        }

        public static bool IsInVisualStudio
        {
            get
            {
                bool inIDE = false;
                string[] args = System.Environment.GetCommandLineArgs();
                if (args != null && args.Length > 0)
                {
                    string prgName = args[0].ToUpper();
                    inIDE = prgName.EndsWith("VSHOST.EXE");
                }
                return inIDE;
            }
        }
    }

    class Global
    {

        private static string _connectionstring;
        private static string _applicationnamestring;
        private static string _currentVersion;
        private static string _bdappNo;
        private static string _resolverGroup;
        private static string _installPath;
        private static int _timeOut;
        private static string _pid;
        private static string _fullname;
        private static string _accesslevel;
        private static string _popcodename = "";
        private static bool _admin;
        private static bool _baadmin;

        public static string PID
        {
            get
            {
                return _pid;
            }
            set
            {
                _pid = value;
            }
        }

        public static string FullName
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
            }
        }

        public static string AccessLevel
        {
            get
            {
                return _accesslevel;
            }
            set
            {
                _accesslevel = value;
            }
        }

        public static bool Admin
        {
            get
            {
                return _admin;
            }
            set
            {
                _admin = value;
            }
        }

        public static bool BA_Admin
        {
            get
            {
                return _baadmin;
            }
            set
            {
                _baadmin = value;
            }
        }

        public static string Pop_Code_Name
        {
            get
            {
                return _popcodename;
            }
            set
            {
                _popcodename = value;
            }
        }

        public static int TimeOut
        {
            get
            {
                // Reads are usually simple
                return _timeOut;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                _timeOut = value;
            }
        }


        public static string ConnectionString
        {
            get
            {
                // Reads are usually simple
                return _connectionstring;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                _connectionstring = value;
            }
        }

        public static string CurrentVersion
        {
            get
            {
                return _currentVersion;
            }
            set
            {
                _currentVersion = value;
            }
        }

        public static string BDAppNo
        {
            get
            {
                return _bdappNo;
            }
            set
            {
                _bdappNo = value;
            }
        }

        public static string ResolverGroup
        {
            get
            {
                return _resolverGroup;
            }
            set
            {
                _resolverGroup = value;
            }
        }

        public static string InstallPath
        {
            get
            {
                return _installPath;
            }
            set
            {
                _installPath = value;
            }
        }

        public static String ApplicationName
        {
            get
            {
                // Reads are usually simple
                return _applicationnamestring;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                _applicationnamestring = value;
            }
        }

        public static void CheckVersions()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            int _currentVersion = Convert.ToInt16(CurrentVersion);
            int _userVersion = Convert.ToInt16(fvi.FileMajorPart.ToString() + fvi.FileMinorPart.ToString() + fvi.FileBuildPart.ToString() + fvi.FilePrivatePart.ToString());

            if (_currentVersion > _userVersion)
            {
                MessageBoxResult result = MessageBox.Show("There is a new version available.  You will need to update the application", Global.ApplicationName, MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);

                if (result == MessageBoxResult.OK)
                {
                    updateApplication();
                }
                else
                {
                    MessageBox.Show("The application will now exit", Global.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                // We need to close the application whether the user updated or not
                Environment.Exit(1);

            }
        }

        public static bool blnmainWindow;

        public static void updateApplication()
        {
            // Update applicaton
            try
            {
                Process.Start(Global.InstallPath);
                Environment.Exit(1);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
                Environment.Exit(1);
            }
        }

        public static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace RetroCore
{
    public class User
    {
        public String Input;
        public User()
        {
            try
            {
                PID = Convert.ToInt32(Environment.UserName).ToString();

            }
            catch
            {
                PID = "0000000";
            }

            try
            {
                var varUserName = Regex.Match(Environment.UserName, @"\d+", RegexOptions.RightToLeft).Value;
                PID = Convert.ToInt32(varUserName).ToString();

            }
            catch
            {
                PID = "0000000";
            }

            try
            {
                if (!String.IsNullOrEmpty(Options.PID))
                {
                    PID = Options.PID;
                }
            }
            catch
            {
                PID = "0000000";
            }

            try
            {
                FullName = Environment.UserDomainName.ToString();

            }
            catch
            {
                FullName = "Unknown";
            }

            // PID for dev purposes only
            if ((System.Diagnostics.Debugger.IsAttached == true) && (PID == "0000000"))
            {
                PID = "1234567";
            }


            GetUserInfo();

        }


        public User(string _pid)
        {
            PID = _pid;
            GetUserInfo();
        }

        private string _pid = "1234567";
        private string _fullname = "";
        private bool _active = false; // 0 = No, 1 = Yes

        public string PID
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

        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }

        public string FullName
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

        public void GetUserInfo()
        {

            SqlConnection con = new SqlConnection(Global.ConnectionString);
            SqlCommand cmd = new SqlCommand("qryGetUserDetails", con);
            cmd.CommandTimeout = Global.TimeOut;
            cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter prmPID = cmd.Parameters.Add("@PID", SqlDbType.Int);

            prmPID.Value = PID;

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            #region Recordset
            if (reader.HasRows)
            {
                reader.Read();
                {
                    FullName = reader["Full Name"].ToString();
                }
            }
            else
            {
                {
                    MessageBox.Show("You are not know by this application." + "\n" + "\n" + "The application will now close.", Global.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    Environment.Exit(0);
                }
            }

            #endregion

        }

        public void SaveUserInfo(string FullName)
        {
            try
            {
                SqlConnection con = new SqlConnection(Global.ConnectionString);
                SqlCommand cmd = new SqlCommand("qryAddNewUser", con);
                cmd.CommandTimeout = Global.TimeOut;
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.Parameters.Clear();
                cmd.CommandText = "qryAddNewUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nPID", PID);
                cmd.Parameters.AddWithValue("@nFullName", FullName);
                cmd.ExecuteNonQuery();

                con.Close();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            catch
            {
                MessageBox.Show("There was an error. Your details has not been saved", Global.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }



    }
}

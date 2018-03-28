using System;
using System.Data.SqlClient;

namespace SJBCS.SMS.Implementation
{
    public class DatabaseImpl
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static SqlConnection open()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source = PHMANLAPANGAN1;" +
                "Initial Catalog=AMS;" +
                "User id=sa;" +
                "Password=Infor123;";
            connection.Open();

            return connection;
        }

        public static void close(SqlConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public static int updateAttendanceSMSID(string attendanceID, string smsID)
        {
            int rowsAffected = 0;
            SqlConnection connection = null;
            try
            {
                string columnName = null;
                connection = DatabaseImpl.open();

                if (connection == null)
                {
                    return 0;
                }

                SqlCommand selectCommand = new SqlCommand("SELECT TimeIn, TimeOut FROM Attendance WHERE AttendanceID = @0", connection);
                selectCommand.Parameters.Add(new SqlParameter("0", attendanceID));

                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!String.IsNullOrEmpty(reader[0].ToString()) && String.IsNullOrEmpty(reader[1].ToString()))
                        {
                            columnName = "TimeInSMSID";
                        }
                        else if (!String.IsNullOrEmpty(reader[0].ToString()) && !String.IsNullOrEmpty(reader[1].ToString()))
                        {
                            columnName = "TimeOutSMSID";
                        }
                    }
                }

                if (columnName != null)
                {
                    SqlCommand updateCommand = new SqlCommand("UPDATE Attendance SET " + columnName + " = @0 WHERE AttendanceID = @1", connection);
                    updateCommand.Parameters.Add(new SqlParameter("0", smsID));
                    updateCommand.Parameters.Add(new SqlParameter("1", attendanceID));
                    rowsAffected = updateCommand.ExecuteNonQuery();
                }
            }
            catch(SqlException e)
            {
                Logger.Error("Failed to update database", e);
            }
            catch (Exception e)
            {
                Logger.Error("Exception encountered", e);
            }
            finally
            {
                DatabaseImpl.close(connection);
            }

            return rowsAffected;
        }

        public static int updateAttendaceSMSStatus(string smsID, string status)
        {
            int rowsAffected = 0;
            SqlConnection connection = null;
            try
            {
                string columnName = null;
                connection = DatabaseImpl.open();

                if (connection == null)
                {
                    return 0;
                }

                SqlCommand selectCommand = new SqlCommand("SELECT TimeInSMSID, TimeOutSMSID FROM Attendance WHERE TimeInSMSID = @0 OR TimeOutSMSID = @0", connection);
                selectCommand.Parameters.Add(new SqlParameter("0", smsID));

                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!String.IsNullOrEmpty(reader[0].ToString()) && reader[0].Equals(smsID))
                        {
                            columnName = "TimeInSMSStatus";
                        }
                        else if (!String.IsNullOrEmpty(reader[1].ToString()) && reader[1].Equals(smsID))
                        {
                            columnName = "TimeOutSMSStatus";
                        }
                    }
                }

                if (columnName != null)
                {
                    SqlCommand updateCommand = new SqlCommand("UPDATE Attendance SET " + columnName + " = @0 WHERE TimeInSMSID = @1 OR TimeOutSMSID = @1", connection);
                    updateCommand.Parameters.Add(new SqlParameter("0", status));
                    updateCommand.Parameters.Add(new SqlParameter("1", smsID));
                    rowsAffected = updateCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Logger.Error("Failed to update database", e);
            }
            catch (Exception e)
            {
                Logger.Error("Exception encountered", e);
            }
            finally
            {
                DatabaseImpl.close(connection);
            }

            return rowsAffected;
        }
    }
}
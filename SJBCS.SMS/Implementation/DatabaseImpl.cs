using Newtonsoft.Json;
using SJBCS.Data;
using SJBCS.Services.Repository;
using System;
using System.Configuration;
using System.IO;

namespace SJBCS.SMS.Implementation
{
    public class DatabaseImpl
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IAttendancesRepository attendanceRepository;

        public DatabaseImpl()
        {
            ConnectionHelper.Config = SetConfiguration();
            attendanceRepository = new AttendancesRepository();
        }

        public DatabaseImpl(IAttendancesRepository attendanceRepository)
        {
            ConnectionHelper.Config = SetConfiguration();
            this.attendanceRepository = attendanceRepository;
        }

        private Config SetConfiguration()
        {
            Config config = ConnectionHelper.Config;
            if (config == null)
            {
                string json = File.ReadAllText(ConfigurationManager.AppSettings["configPath"]);
                config = JsonConvert.DeserializeObject<Config>(json);
            }
            return config;
        }

        public bool UpdateAttendanceSMSID(string attendanceID, bool isTimeIn, string smsID)
        {
            try
            {
                Attendance attendance = attendanceRepository.GetAttendanceByID(Guid.Parse(attendanceID));

                if (isTimeIn)
                {
                    attendance.TimeInSMSID = smsID;
                }
                else
                {
                    attendance.TimeOutSMSID = smsID;
                }

                attendanceRepository.UpdateAttendance(attendance);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Exception encountered when updating sms ID", e);
                return false;
            }
        }

        public bool UpdateAttendaceSMSStatus(string smsID, string status)
        {
            try
            {
                if ((status != "Sent" && status != "Delivered"))
                {
                    return false;
                }

                Attendance attendance = attendanceRepository.GetAttendanceBySMSID(smsID);

                if (attendance == null)
                {
                    return false;
                }

                if (!String.IsNullOrEmpty(attendance.TimeInSMSID) && attendance.TimeInSMSID.Equals(smsID))
                {
                    attendance.TimeInSMSStatus = status;
                }
                else if (!String.IsNullOrEmpty(attendance.TimeOutSMSID) && attendance.TimeOutSMSID.Equals(smsID))
                {
                    attendance.TimeOutSMSStatus = status;
                }

                attendanceRepository.UpdateAttendance(attendance);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Exception encountered when updating sms status", e);
                return false;
            }
        }
    }
}
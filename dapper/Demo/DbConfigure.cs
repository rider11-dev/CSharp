using MyFrame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class DbConfigure
    {
        const string Key_DbType = "dbtype";
        const string Key_ConnectionStrings = "dbconn";

        static ConnectionStringSettings _connectionStringSettings = null;

        public static string GetDbType()
        {
            return AppSettingHelper.Get(Key_DbType);
        }

        public static string ConnectString
        {
            get
            {
                return ConnectionStrings.ConnectionString;
            }
        }

        public static string Provider
        {
            get
            {
                return ConnectionStrings.ProviderName;
            }
        }

        public static ConnectionStringSettings ConnectionStrings
        {
            get
            {
                if (_connectionStringSettings == null)
                {
                    _connectionStringSettings = ConfigurationManager.ConnectionStrings[Key_ConnectionStrings];
                }
                return _connectionStringSettings;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class DbConnectionProvider
    {
        static DbProviderFactory _dbProviderFactory = null;

        public static IDbConnection Create(string connectString = "")
        {
            if (_dbProviderFactory == null)
            {
                try
                {
                    _dbProviderFactory = DbProviderFactories.GetFactory(DbConfigure.Provider);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("无法创建指定类型的DbProviderFactory实例", ex);
                }
                if (_dbProviderFactory == null)
                {
                    throw new ArgumentException("无法创建指定类型的DbProviderFactory实例");
                }
            }
            try
            {
                IDbConnection conn = _dbProviderFactory.CreateConnection();
                if (string.IsNullOrEmpty(connectString))
                {
                    conn.ConnectionString = DbConfigure.ConnectString;
                }
                return conn;
            }
            catch (Exception ex)
            {
                throw new Exception("获取数据库连接失败！", ex);
            }
        }
    }
}

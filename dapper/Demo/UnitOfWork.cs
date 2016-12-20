using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class UnitOfWork : IDisposable
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }

        public UnitOfWork(IDbConnection connection)
        {
            Connection = connection;
        }

        public void DoQuery(Action<IDbConnection, IDbTransaction> queryAction)
        {
            if (queryAction == null)
            {
                return;
            }

            queryAction(Connection, Transaction);

        }

        public void Begin()
        {
            if (Connection == null)
            {
                throw new Exception("Connection属性不能为空");
            }

            if (Transaction != null)
            {
                return;
            }
            if (Connection.State == ConnectionState.Closed ||
                Connection.State == ConnectionState.Broken)
            {
                Connection.Open();
            }
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }

            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MiddleTier_DapperCommand.DataAccess.Inventory
{
    public class GetProducts
    {
        // This could be a parameter for a base class's constructor.
        private const string ProcedureName = "Inventory.GetProducts";

        // Public data members represent our parameters.
        public string NameSearch { get; set; }

        // The query method.  Returns results, and optionally includes a transaction.
        // **** WILL IT WORK?? ****
        public IEnumerable<Result> Query(IDbConnection connection, IDbTransaction transaction = null)
        {
            return
                connection.Query<Result>(
                    ProcedureName,
                    commandType: CommandType.StoredProcedure,
                    param: this,
                    transaction: transaction);
        }

        // Our query result type.  This could be generated from the sproc.
        public class Result
        {
            public int Id { get; set; }
            public string name { get; set; }
        }
    }
}

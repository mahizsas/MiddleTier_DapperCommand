using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleTier_DapperCommand.DataAccess.Inventory
{
    public class SaveProduct
    {
        // This could be a parameter for a base class's constructor.
        private const string ProcedureName = "Inventory.SaveProduct";

        // Public data members represent our parameters.
        // These could be generated from our metadata.
        public int? Id { get; set; }
        public string Name { get; set; }

        // **** INTERESTING.
        // We could have put this in the Query example, too, and just returned
        // new DynamicParameters(this).
        private DynamicParameters GetParameters()
        {
            // "Template" initialization: just use the current instance.
            DynamicParameters parameters = new DynamicParameters(this);

            // Now override parameters as needed.
            parameters.Add(
                "@Id",
                value: Id,
                direction: ParameterDirection.InputOutput);

            return parameters;
        }

        // **** WILL IT WORK?? ****
        public int Execute(IDbConnection connection, IDbTransaction transaction = null)
        {
            DynamicParameters parameters = GetParameters();

            connection.Execute(
                ProcedureName,
                commandType: CommandType.StoredProcedure,
                param: parameters,
                transaction: transaction); // *** SPECIFICALLY, WILL THIS WORK???

            // Map our return value.
            return parameters.Get<int>("@Id");
        }
    }
}

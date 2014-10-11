using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleTier_DapperCommand.DataAccess.Inventory
{
    // As an experiment, I copied an already-working SaveProduct.
    // Here is what changed.
    public class DeleteProduct
    {
        // The procedure name.
        private const string ProcedureName = "Inventory.DeleteProduct";

        // Our parameters/properties.
        public int? Id { get; set; }
        public bool? Result { get; set; }

        // Parts of this thing:
        private DynamicParameters GetParameters()
        {
            // Same as save.
            DynamicParameters parameters = new DynamicParameters(this);

            // This looks different, but it's the same form:
            parameters.Add(
                // *** This changed.
                "@Result",
                // *** This changed too.
                value: Result,
                // This looked identical.
                direction: ParameterDirection.InputOutput);

            return parameters;
        }

        // Only one change here: mapping our output parameter to a boolean.
        public bool Execute(IDbConnection connection, IDbTransaction transaction = null)
        {
            DynamicParameters parameters = GetParameters();

            connection.Execute(
                ProcedureName,
                commandType: CommandType.StoredProcedure,
                param: parameters,
                transaction: transaction);

            // *** THIS IS THE ONLY LINE THAT CHANGED
            return parameters.Get<bool>("@Result");
        }
    }
}

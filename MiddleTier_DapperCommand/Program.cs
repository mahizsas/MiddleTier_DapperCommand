using MiddleTier_DapperCommand.DataAccess.Inventory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleTier_DapperCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            using(IDbConnection connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["Test1"].ConnectionString))
            {
                connection.Open();

                WithoutTransaction(connection);
                //WithTransaction(connection);
            }
        }

        static void WithTransaction(IDbConnection connection)
        {
            int okayProductId = (new SaveProduct { Name = "Thing with a transaction" }).Execute(connection);

            using (IDbTransaction transaction = connection.BeginTransaction())
            {
                int newProductId = (new SaveProduct { Name = "Thing with a transaction" }).Execute(connection, transaction);
                
                var results = (new GetProducts { NameSearch = string.Empty }).Query(connection, transaction);

                transaction.Rollback();
            }

            var moreResults = (new GetProducts { NameSearch = string.Empty }).Query(connection);

            bool wasDeleted = (new DeleteProduct { Id = okayProductId }).Execute(connection);
        }

        static void WithoutTransaction(IDbConnection connection)
        {
            int newProductId = (new SaveProduct { Name = "Thing without a transaction" }).Execute(connection);

            bool wasDeleted = (new DeleteProduct { Id = newProductId }).Execute(connection);

            var results = (new GetProducts { NameSearch = string.Empty }).Query(connection);
        }
    }
}

using Dapper;
using Quantum_Leap.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Quantum_Leap.Data
{
    public class LeaperRepository
    {
        const string ConnectionString = "Server = localhost; Database = QuantumLeaper; Trusted_Connection = True;";

        public Leaper AddLeaper(string leaperName, int age, decimal budgetAmount)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newLeaper = db.QueryFirstOrDefault<Leaper>(@"Insert into leapers (leaperName,age, budgetAmount)
                                                            Output inserted.*
                                                            Values(@leaperName,@age, @budgetAmount)",
                                                             new { leaperName, age, budgetAmount });
                if (newLeaper != null)
                {
                    return newLeaper;
                }
            }

            throw new Exception("No Leaper is created");
        }

        public IEnumerable<Leaper> GetAllLeapers()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = "Select * from leapers";
                var leapers = db.Query<Leaper>(sql).ToList();

                return leapers;
            }
            throw new Exception("No Leaper found");
        }

        public void DeleteLeaper(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            { 
                var deleteQuery = "Delete From Leapers where Id = @id";

                var parameter = new { id };

                var rowsAffected = db.Execute(deleteQuery, parameter);

                if (rowsAffected != 1)
                {
                    throw new Exception("Didn't do right");
                }
            }
        }

    }
}

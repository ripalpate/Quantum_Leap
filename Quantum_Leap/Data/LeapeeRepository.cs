using Dapper;
using Quantum_Leap.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Quantum_Leap.Data
{
    public class LeapeeRepository
    {
        const string ConnectionString = "Server = localhost; Database = QuantumLeaper; Trusted_Connection = True;";

        public Leapee AddLeapee(string leapeeName, string profession, string gender)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newLeapee = db.QueryFirstOrDefault<Leapee>(@"Insert into leapees (leapeeName, profession, gender)
                                                            Output inserted.*
                                                            Values(@leapeeName, @profession, @gender)",
                                                             new { leapeeName, profession, gender});
                if (newLeapee != null)
                {
                    return newLeapee;
                }
            }

            throw new Exception("No Leapee is created");
        }

        public IEnumerable<Leapee> GetAllLeapees()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = "Select * from Leapees";
                var leapees = db.Query<Leapee>(sql);
                return leapees;
            }
            throw new Exception("No Leapee found");
        }

        public void DeleteLeapee(int leapeeId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var deleteQuery = "Delete From Leapees where Id = @leapeeId";

                var parameter = new { leapeeId };

                var rowsAffected = db.Execute(deleteQuery, parameter);

                if (rowsAffected != 1)
                {
                    throw new Exception("Didn't do right");
                }
            }
        }
    }
}

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

        public Leapee AddLeapee(string name, string profession, string gender, int leaperId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newLeapee = db.QueryFirstOrDefault<Leapee>(@"Insert into leapees (name, profession, gender, leaperId)
                                                            Output inserted.*
                                                            Values(@name, @profession, @gender, @leaperId)",
                                                             new { name, profession, gender, leaperId });
                if (newLeapee != null)
                {
                    return newLeapee;
                }
            }

            throw new Exception("No Leapee is created");
        }
    }
}

using Dapper;
using Quantum_Leap.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Quantum_Leap.Data
{
    public class LeapRepository
    {
        const string ConnectionString = "Server = localhost; Database = QuantumLeaper; Trusted_Connection = True;";

        public Leap AddLeap(int leaperId, int leapeeId, int eventId, DateTime date, decimal cost)
        {
            using (var db = new SqlConnection(ConnectionString))
            {

                var newLeap = db.QueryFirstOrDefault<Leap>(@"Insert into leap (leaperId, leapeeId, eventId, date, cost)
                                                            Output inserted.*
                                                            Values(@leaperId, @leapeeId, @eventId, @date, @cost)",
                                                             new { leaperId, leapeeId, eventId, date, cost });
                if (newLeap != null)
                {
                    return newLeap;
                }
            }

            throw new Exception("No Leap is created");
        }

    }
}

using Dapper;
using Quantum_Leap.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Quantum_Leap.Data
{
    public class EventRepository
    {
        const string ConnectionString = "Server = localhost; Database = QuantumLeaper; Trusted_Connection = True;";

        public Event AddEvent(string name, string description, DateTime date, string location, bool isCorrected, int leapeeId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newEvent = db.QueryFirstOrDefault<Event>(@"Insert into events (name, description, date, location, isCorrected, leapeeId)
                                                            Output inserted.*
                                                            Values(@name, @description, @date, @location, @isCorrected, @leapeeId)",
                                                             new { name, description, date, location, isCorrected, leapeeId });
                if (newEvent != null)
                {
                    return newEvent;
                }
            }

            throw new Exception("No Event is created");
        }
    }
}

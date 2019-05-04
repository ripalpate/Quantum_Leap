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

        public Event AddEvent(string name, string description, DateTime date, string location, bool isCorrected)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newEvent = db.QueryFirstOrDefault<Event>(@"Insert into events (name, description, date, location, isCorrected)
                                                            Output inserted.*
                                                            Values(@name, @description, @date, @location, @isCorrected)",
                                                             new { name, description, date, location, isCorrected });
                if (newEvent != null)
                {
                    return newEvent;
                }
            }

            throw new Exception("No Event is created");
        }

        public IEnumerable<Event> GetAllEvents()
        {
     
            using (var db = new SqlConnection(ConnectionString))
            {
                var events = db.Query<Event>("Select * from Events").ToList();

                return events;
            }
            throw new Exception("No event found");
        }
    }
}

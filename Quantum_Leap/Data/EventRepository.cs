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

        public Event AddEvent(string eventName, string description, DateTime date, string location, bool isCorrected, int leapeeId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newEvent = db.QueryFirstOrDefault<Event>(@"Insert into events (eventName, description, date, location, isCorrected, leapeeId)
                                                            Output inserted.*
                                                            Values(@eventName, @description, @date, @location, @isCorrected, @leapeeId)",
                                                             new { eventName, description, date, location, isCorrected, leapeeId });
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

        public void DeleteEvent(int eventId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var deleteQuery = "Delete From Events where Id = @eventId";

                var parameter = new { eventId };

                var rowsAffected = db.Execute(deleteQuery, parameter);

                if (rowsAffected != 1)
                {
                    throw new Exception("Didn't do right");
                }
            }
        }
    }
}

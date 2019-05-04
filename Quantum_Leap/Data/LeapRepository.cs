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
                var getRandomLeaper = db.QueryFirstOrDefault<Leaper>(@"Select TOP(1) lr.* 
                                                                       From Leapers as lr
                                                                       Order By NEWID()");

                var getRandomEvent = db.QueryFirstOrDefault<Event>(@"Select TOP(1) e.* 
                                                                     From Events as e
                                                                     Where e.isCorrected = 0
                                                                     Order By NEWID()");

                var getRandomLeapee = db.QueryFirstOrDefault<Event>(@"Select TOP(1) le.* 
                                                                     From Leapees as le
                                                                     Order By NEWID()");

                if (getRandomLeaper.BudgetAmount > cost)
                {

                    //var newLeap = db.QueryFirstOrDefault<Leap>(@"Insert into leap (leaperId, leapeeId, eventId, date, cost)
                    //                                        Output inserted.*
                    //                                        Values(@leaperId, @leapeeId, @eventId, @date, @cost)",
                    //                                             new { leaperId, leapeeId, eventId, date, cost });

                    //if (newLeap != null)
                    //{
                    //    return newLeap;
                    //}
                }
                else
                {
                    throw new Exception("you don't have enough budget");
                }
            }

            throw new Exception("No Leap is created");
        }

        public IEnumerable<object> GetLeap()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"Select l.Date, l.Cost, lr.*, e.*, le.*
                           From leap as l
                           Join Leapers as lr
                           On l.LeaperId = lr.Id
                           Join Leapees as le
                           On l.LeapeeId = le.Id
                           Join Events as e
                           On l.EventId = e.Id";
                var getLeap = db.Query<object>(sql);
                return getLeap;
            }
            throw new Exception("No leap found");
        }

    }
}

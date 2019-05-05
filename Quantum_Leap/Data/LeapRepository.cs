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

        public Leap AddLeap(int leaperId, int leapeeId, int eventId, decimal cost)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var getRandomLeaper = db.QueryFirstOrDefault<Leaper>(@"Select Top(1) lr.* 
                                                                       From Leapers as lr
                                                                       Order By NEWID()");

                var getRandomLeapee = db.QueryFirstOrDefault<Leapee>(@"Select Top(1) le.Id 
                                                                        From Leapees as le 
                                                                        Where Id in (Select leapeeId from events where isCorrected = 0) 
                                                                        Order By NEWID();");

                @leaperId = getRandomLeaper.Id;
                @leapeeId = getRandomLeapee.Id;

                var getEventAssociatedWithLeapee = db.QueryFirstOrDefault<Event>(@"Select Top(1) e.Id
                                                                                   From Events as e
                                                                                   Where e.LeapeeId = @leapeeId and e.IsCorrected = 0 
                                                                                   and e.Id Not In(Select EventId from Leap)", 
                                                                                   new { leapeeId});

                @eventId = getEventAssociatedWithLeapee.Id;


                if (getRandomLeaper.BudgetAmount > cost)
                {
                    var newLeap = db.QueryFirstOrDefault<Leap>(@"Insert into leap (leaperId, leapeeId, eventId, cost)
                                                            Output inserted.*
                                                            Values(@leaperId, @leapeeId, @eventId, @cost)",
                                                                 new { leaperId, leapeeId, eventId, cost });
                    if (newLeap != null)
                    {
                        var updateLeaper = db.Execute(@"Update Leapers Set BudgetAmount = BudgetAmount - @cost Where Id = @leaperId", new { leaperId, cost});
                        return newLeap;
                    }
                }
                else
                {
                    throw new Exception("you can not leap beacuase you don't have enough budget");
                }
            }

            throw new Exception("No Leap is created");
        }

        public IEnumerable<object> GetLeap()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"Select l.Id, l.Cost as LeapCost, lr.LeaperName, le.LeapeeName, e.EventName, e.Description, e.Date, e.Location
                            From leap as l
                            Join Leapers as lr
                            On l.LeaperId = lr.Id
                            Join Leapees as le
                            On l.LeapeeId = le.Id
                            Join Events as e
                            On l.EventId = e.Id;";
                var getLeap = db.Query<object>(sql);
                return getLeap;
            }
            throw new Exception("No leap found");
        }

    }
}

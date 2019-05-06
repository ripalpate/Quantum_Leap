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

        //public Leap AddLeap(int leaperId, int leapeeId, int eventId, decimal cost)
        //{
        //    using (var db = new SqlConnection(ConnectionString))
        //    {
        //        @leaperId = getRandomLeaper().Id;
        //        @leapeeId = getRandomLeapee().Id;

        //        @eventId = getEventAssociatedWithLeapee(leapeeId) != null? getEventAssociatedWithLeapee(leapeeId).Id : 0; 


        //        if (@eventId != 0) {
        //            if (getRandomLeaper().BudgetAmount > cost)
        //            {
        //                insertLeapAndUpdateBudget(leaperId, leapeeId, eventId, cost);
        //            }
        //            else
        //            {
        //                throw new Exception ("you can not leap because you don't have enough budget");
        //            }
        //        } else { 
        //            throw new Exception("Event already exist in another leap for that leapee");
        //        }
        //    }

        //    throw new Exception("No Leap is created");
        //}

        public IEnumerable<object> GetAllLeap()
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

        public Leaper getRandomLeaper() {

            using (var db = new SqlConnection(ConnectionString))
            {
                var randomLeaper = db.QueryFirstOrDefault<Leaper>(@"Select Top(1) lr.* 
                                                                       From Leapers as lr
                                                                       Order By NEWID()");
                return randomLeaper;
            }    
        }

        public Leapee getRandomLeapee()
        {

            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"Select Top(1) le.Id 
                            From Leapees as le 
                            Where Id in (Select leapeeId from events where isCorrected = 0) 
                            Order By NEWID();";
                var randomLeapee = db.QueryFirstOrDefault<Leapee>(sql);
                return randomLeapee;
            }
        }

        public Event getEventAssociatedWithLeapee(int leapeeId)
        {

            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"Select Top(1) e.Id
                            From Events as e
                            Where e.LeapeeId = @leapeeId and e.IsCorrected = 0 
                            And e.Id Not In(Select EventId from Leap)";
                var parameter = new { leapeeId };
                var leapeeEvent = db.QueryFirstOrDefault<Event>(sql,parameter);
                return leapeeEvent;
            }
        }

        public Leap insertLeapAndUpdateBudget(int leaperId, int leapeeId, int eventId, decimal cost)
        {

            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"Insert into leap (leaperId, leapeeId, eventId, cost)
                            Output inserted.*
                            Values(@leaperId, @leapeeId, @eventId, @cost)";
                var parameter = new { leaperId, leapeeId, eventId, cost };
                var newLeap = db.QueryFirstOrDefault<Leap>(sql, parameter);
                if (newLeap != null)
                {
                    var updateSql = @"Update Leapers 
                                      Set BudgetAmount = BudgetAmount - @cost 
                                      Where Id = @leaperId";
                    var updateParameter = new { leaperId, cost };
                    var updateLeaper = db.Execute(updateSql, updateParameter);
                }
                return newLeap;
            }
        }
    }
}

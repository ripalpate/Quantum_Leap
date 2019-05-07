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

        public Leaper GetRandomLeaper()
        {

            using (var db = new SqlConnection(ConnectionString))
            {
                var randomLeaper = db.QueryFirstOrDefault<Leaper>(@"Select Top(1) lr.* 
                                                                       From Leapers as lr
                                                                       Order By NEWID()");
                return randomLeaper;
            }
        }

        public Leapee GetRandomLeapee()
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

        public Event GetEventAssociatedWithLeapee(int leapeeId)
        {

            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"Select Top(1) e.Id
                            From Events as e
                            Where e.LeapeeId = @leapeeId and e.IsCorrected = 0 
                            And e.Id Not In(Select EventId from Leap)";
                var parameter = new { leapeeId };
                var leapeeEvent = db.QueryFirstOrDefault<Event>(sql, parameter);
                return leapeeEvent;
            }
        }

        public Leap AddLeapAndUpdateBudget(int leaperId, int leapeeId, int eventId, decimal cost)
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

        public object GetSingleLeap(int leapId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"Select l.Id, l.Cost as LeapCost, lr.LeaperName, le.LeapeeName, e.EventName, e.Description as 'Event Description', e.Date as 'Event Date', e.Location as 'Event Location'
                            From leap as l
                            Join Leapers as lr
                            On l.LeaperId = lr.Id
                            Join Leapees as le
                            On l.LeapeeId = le.Id
                            Join Events as e
                            On l.EventId = e.Id
                            Where l.Id = @leapId;";
                var parameter = new { leapId };
                var getSingleLeap = db.QueryFirstOrDefault<object>(sql, parameter);
                return getSingleLeap;
            }
            throw new Exception("No single leap found");
        }
    }
}

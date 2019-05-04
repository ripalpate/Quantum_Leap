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

        //public Leap AddLeap(int leaperId, int leapeeId, int eventId, DateTime date, decimal cost)
        //{
        //    using (var db = new SqlConnection(ConnectionString))
        //    {
        //        var getLeaper = db.Query(@"Select l.Date, l.Cost, lr.BudgetAmount
        //                                     From leap as l
        //                                     Join Leapers as lr
        //                                     On l.LeaperId = lr.Id;");
        //        if (getLeaper[BudgetAmount] > 0)
        //        {

        //            var newLeap = db.QueryFirstOrDefault<Leap>(@"Insert into leap (leaperId, leapeeId, eventId, date, cost)
        //                                                    Output inserted.*
        //                                                    Values(@leaperId, @leapeeId, @eventId, @date, @cost)",
        //                                                         new { leaperId, leapeeId, eventId, date, cost });

        //            if (newLeap != null)
        //            {
        //                return newLeap;
        //            }
        //        }
        //        else {
        //            throw new Exception("you don't have enough budget");
        //        }
        //    }

        //    throw new Exception("No Leap is created");
        //}

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

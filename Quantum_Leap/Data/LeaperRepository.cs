﻿using Dapper;
using Quantum_Leap.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Quantum_Leap.Data
{
    public class LeaperRepository
    {
        const string ConnectionString = "Server = localhost; Database = QuantumLeaper; Trusted_Connection = True;";

        public Leaper AddLeaper(string name, int age, decimal budgetAmount)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newLeaper = db.QueryFirstOrDefault<Leaper>(@"Insert into leapers (name,age, budgetAmount)
                                                            Output inserted.*
                                                            Values(@name,@age, @budgetAmount)",
                                                             new { name, age, budgetAmount });
                if (newLeaper != null)
                {
                    return newLeaper;
                }
            }

            throw new Exception("No Leaper is created");
        }

        public IEnumerable<Leaper> GetAllLeapers()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = "Select * from leapers";
                var leapers = db.Query<Leaper>(sql).ToList();

                return leapers;
            }
            throw new Exception("No Leaper found");
        }
    }
}

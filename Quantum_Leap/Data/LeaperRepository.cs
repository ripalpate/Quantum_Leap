using Dapper;
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

        public Leaper AddLeaper(string name, int age)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newLeaper = db.QueryFirstOrDefault<Leaper>(@"Insert into leapers (name,age)
                                                            Output inserted.*
                                                            Values(@name,@age)",
                                                             new { name, age });
                if (newLeaper != null)
                {
                    return newLeaper;
                }
            }

            throw new Exception("No Leaper is created");
        }
    }
}

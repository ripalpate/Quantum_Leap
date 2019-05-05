﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantum_Leap.Models
{
    public class CreateLeaperRequest
    {
        public string LeaperName { get; set; }
        public int Age { get; set; }
        public decimal BudgetAmount { get; set; }
    }
}

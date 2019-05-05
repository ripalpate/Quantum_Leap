using Quantum_Leap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantum_Leap.Validators
{
    public class CreateLeaperRequestValidator
    {
        public bool Validate(CreateLeaperRequest requestToValidate)
        {
            return string.IsNullOrEmpty(requestToValidate.LeaperName);
            
        }
    }
}

using Quantum_Leap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quantum_Leap.Validators
{
    public class CreateLeapeeRequestValidator
    {
        public bool Validate(CreateLeapeeRequest requestToValidate)
        {
            return (string.IsNullOrEmpty(requestToValidate.LeapeeName) ||
                string.IsNullOrEmpty(requestToValidate.Profession));
        }
    }
}

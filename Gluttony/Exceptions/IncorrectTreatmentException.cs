using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gluttony.Exceptions
{
    public class IncorrectTreatmentException:Exception
    {
        public IncorrectTreatmentException(ParameterTreatment treatment, Exception innerException) : base(MessageBuild(treatment), innerException) { }

        private static string MessageBuild(ParameterTreatment treatment)
        {
            return $"ParameterTreatment {treatment} is not valid.";
        }
    }
}

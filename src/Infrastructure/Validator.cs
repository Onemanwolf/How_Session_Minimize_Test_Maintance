using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingControllersSample.Infrastructure
{
    public class Validator<T> : IValidatorStrategy<T>
    {
        public bool IsValid(T context)
        {
            var results = Validate(context);

            if (results.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private IList<ValidationResult> Validate(T model)
        {
            var results = new List<ValidationResult>();

            var context = new ValidationContext(model);

            Validator.TryValidateObject(model, context, results, true);

            return results;
        }

    }

    public interface IValidatorStrategy<T>
    {
        bool IsValid(T context);

    }
}

using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        /// <summary>
        /// Use the static method because we don't want to create the { ValidationHelper } class create the object
        /// </summary>
        /// <param name="obj"> Any type of class receive </param>
        /// <exception cref="ArgumentException"> Return the error type of message  </exception>
        internal static void ModelValidation(object obj)
        {
            //Model validation
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationsResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationsResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationsResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}

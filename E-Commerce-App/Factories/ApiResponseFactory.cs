using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens.Experimental;
using Shared.ErrorModels;
using System.Net;
using ValidationError = Shared.ErrorModels.ValidationError;

namespace E_Commerce_App.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        { 
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Any())
                .Select(e => new ValidationError
                {
                   Field  = e.Key,
                    Error = e.Value.Errors.Select(er => er.ErrorMessage)
                });
            var response = new ValidationErrorRespons
            {
                StatusCode = (int) HttpStatusCode.BadRequest,
                ErrorMessage = "Validation Errors Occurred",
                Errors = errors
            };
            return new BadRequestObjectResult(response);
        }
    }
}

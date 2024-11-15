using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryWebApp.AuthService.API.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var validationErrors = new Dictionary<string, string[]>();

            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value is null)
                {
                    continue;
                }

                var model = argument.Value;

                var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
                var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

                if (validator != null)
                {
                    var validationContext = new ValidationContext<object>(model);

                    ValidationResult validationResult = validator.Validate(validationContext);

                    if (!validationResult.IsValid)
                    {
                        var errors = validationResult.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                        foreach (var error in errors)
                        {
                            if (validationErrors.ContainsKey(error.Key))
                            {
                                validationErrors[error.Key] = validationErrors[error.Key].Concat(error.Value).ToArray();
                            }
                            else
                            {
                                validationErrors[error.Key] = error.Value;
                            }
                        }
                    }
                }
            }

            if (validationErrors.Any())
            {
                context.Result = new BadRequestObjectResult(validationErrors);
            }
        }
    }
}
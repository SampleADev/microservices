using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace RdErp
{
    public static class ValidationExtensions
    {
        public static async Task<IEnumerable<ValidationResult>> ValidateExtAsync<TElement>(
            this IValidator<TElement> validator,
            IEnumerable<TElement> collection,
            string propertyPathPrefix = null,
            CancellationToken cancellationToken = default(CancellationToken),
            IValidatorSelector validatorSelector = null,
            string ruleSet = null
        )
        {
            var rootContext = new ValidationContext(collection);

            var results = await Task.WhenAll(collection
                .Select((element, index) =>
                {
                    var elementPrefix = AddPropertyPrefix($"[{index}]", propertyPathPrefix);

                    return validator.ValidateExtAsync(
                        element,
                        elementPrefix,
                        cancellationToken,
                        validatorSelector,
                        ruleSet);
                })
                .ToArray()
            );

            return results;
        }

        public static async Task<ValidationResult> ValidateExtAsync<TObject>(
            this IValidator<TObject> validator,
            TObject instance,
            string propertyPathPrefix = null,
            CancellationToken cancellationToken = default(CancellationToken),
            IValidatorSelector validatorSelector = null,
            string ruleSet = null
        )
        {
            var result = await validator.ValidateAsync(
                instance,
                cancellationToken,
                validatorSelector,
                ruleSet
            );

            if (result.IsValid)
            {
                return result;
            }

            if (!String.IsNullOrEmpty(propertyPathPrefix))
            {
                result.Errors
                    .ToList()
                    .ForEach(e =>
                    {
                        e.PropertyName = AddPropertyPrefix(e.PropertyName, propertyPathPrefix);
                    });

                return result;
            }
            else
            {
                return result;
            }
        }

        public static void AndThrow(this ValidationResult validationResult)
        {
            validationResult = validationResult
                ??
                throw new ArgumentNullException(nameof(validationResult));

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        public static async Task AndThrow(this Task<ValidationResult> validationResult)
        {
            (await validationResult).AndThrow();
        }

        public static void AndThrow(this IEnumerable<ValidationResult> validationResults)
        {
            validationResults = validationResults ?? Enumerable.Empty<ValidationResult>();
            if (validationResults.Any(r => !r.IsValid))
            {
                throw new ValidationException(validationResults.SelectMany(r => r.Errors));
            }
        }

        public static async Task AndThrow(this Task<IEnumerable<ValidationResult>> validationResults)
        {
            (await validationResults).AndThrow();
        }

        public static ValidationResult ToValidationResult(this Exception ex)
        {
            if (ex is ValidationException vex)
            {
                return new ValidationResult(vex.Errors);
            }

            return new ValidationResult(new []
            {
                new ValidationFailure("", ex.Message)
            });
        }

        private static string AddPropertyPrefix(string propertyPath, string prefix)
        {
            if (String.IsNullOrWhiteSpace(prefix))
            {
                return propertyPath;
            }

            if (String.IsNullOrWhiteSpace(propertyPath))
            {
                return prefix;
            }

            if (propertyPath.StartsWith("["))
            {
                return prefix + propertyPath;
            }
            else
            {
                return prefix + "." + propertyPath;
            }

        }

        public static void Inherit<TAncestor, TBase>(
            this AbstractValidator<TAncestor> target,
            AbstractValidator<TBase> source
        ) where TAncestor : TBase
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var rule in source)
            {
                target.AddRule(rule);
            }
        }
    }

}
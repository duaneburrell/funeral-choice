using Obsequy.Utility;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
    #region Property Validation Failure
    public class PropertyValidationFailure : ValidationFailure
    {
        // Summary:
        //     The property name.
        public string Property { get { return FormatHelper.ToCamlCase(this.PropertyName); } }

        // Summary:
        //     The validation status code.
        public string Status { get; set; }

        // Summary:
        //     The validation severity code.
        public string Severity { get; set; }

        // Summary:
        //     The error reason (error message).
        public string Reason { get { return this.ErrorMessage; } }

        // Summary:
        //     Creates a new property validation failure.
        public PropertyValidationFailure(string propertyName, string error) 
            : base(propertyName, error)
        {
        }

        // Summary:
        //     Creates a new property validation failure.
        public PropertyValidationFailure(string propertyName, string error, string attemptedValue) 
            : base(propertyName, error, attemptedValue)
        {
        }
    }
    #endregion

    #region Model Validation Result
    public class ModelValidationResult : ValidationResult
    {
		// Summary:
		//     The validation mode.
		protected ValidationMode ValidationMode { get; set; }

		// Summary:
        //     Does this model have any required warnings?
		public virtual bool HasRequiredWarnings { get; protected set; }

        // Summary:
        //     The Id of the model.
        public string Id { get; set; }

		// Summary:
		//     Is this response stale?
		public bool IsStale { get; set; }

        // Summary:
        //     Creates a new model validation result.
        public ModelValidationResult() : base()
        {
			this.ValidationMode = ValidationMode.None;
        }

        // Summary:
        //     Creates a new model validation result.
        public ModelValidationResult(IEnumerable<ValidationFailure> failures)
            : base(failures)
        {
			this.ValidationMode = ValidationMode.None;
        }

        // Summary:
        //     Creates a new model validation result.
		public ModelValidationResult(ValidationResult validationResult)
			: base()
		{
			this.ValidationMode = ValidationMode.None;

			if (validationResult is ModelValidationResult)
				this.Id = (validationResult as ModelValidationResult).Id;

			foreach (var error in validationResult.Errors)
				this.Errors.Add(error);

			// Summary:
			//     Is this response stale?
			foreach (var error in this.Errors.Where(item => item is PropertyValidationFailure))
				if ((error as PropertyValidationFailure).Status.ToLower() == ValidationStatus.Stale.ToString().ToLower())
					IsStale = true;
		}

		// Summary:
		//     Creates a new model validation result.
		public ModelValidationResult(ValidationResult validationResult, ValidationMode validationMode)
			: base()
		{
			this.ValidationMode = validationMode;

			if (validationResult is ModelValidationResult)
				this.Id = (validationResult as ModelValidationResult).Id;

			foreach (var error in validationResult.Errors)
				this.Errors.Add(error);
		}
    }
    #endregion

    #region Model Abstract Validator
    public class ModelAbstractValidator<T> : AbstractValidator<T>
	{
		#region Properties

		#region Account Session
		public AccountSession AccountSession
		{
			get;
			protected set; 
		}
		#endregion

		#region Validation Mode
		public ValidationMode ValidationMode
		{
			get;
			protected set;
		}
		#endregion

		#endregion

		#region Constructor
		protected ModelAbstractValidator(AccountSession accountSession, ValidationMode validationMode)
            : base()
        {
			this.AccountSession = accountSession;
			this.ValidationMode = validationMode;

            // stop validating when the first failure is discovered
            this.CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;
        }
        #endregion

        #region Validate
        public override FluentValidation.Results.ValidationResult Validate(T instance)
        {
            // create the validation result
            var modelValidationResult = new ModelValidationResult()
            {
                Id = GetId(instance)
            };

            // get the validation results from the model
            var validationResult = base.Validate(instance);
            if (validationResult.Errors.Any())
            {
                // note: this shouldn't be necessary with the StopOnFirstFailure flag set
                var errors = DistinctPropertyErrors(validationResult.Errors);
                foreach (var error in errors)
                {
                    modelValidationResult.Errors.Add(new PropertyValidationFailure(error.PropertyName, error.ErrorMessage)
                    {
                        Status = GetStatus(error.CustomState),
                        Severity = GetSeverity(error.CustomState)
                    });
                }
            }

            return modelValidationResult;
        }
        #endregion

        #region Distinct Property Errors
        protected IEnumerable<ValidationFailure> DistinctPropertyErrors(IEnumerable<ValidationFailure> errors)
        {
            var distinctErrors = new List<ValidationFailure>();
            var propertyNames = errors.Select(i => i.PropertyName).Distinct();

            foreach (var propertyName in propertyNames)
                distinctErrors.Add(errors.Where(i => i.PropertyName == propertyName).First());
            
            return distinctErrors;
        }
        #endregion

        #region Get Id
        protected string GetId(T instance)
        {
			if (instance != null)
			{
				var property = instance.GetType().GetProperty("Id");
				if (property != null)
					return Convert.ToString(property.GetValue(instance, null));
			}
			return string.Empty;
        }
        #endregion

        #region Get Status
        protected string GetStatus(object customState)
        {
            if (customState != null && customState is object[])
            {
                if ((customState as object[]).Length >= 1 && ((customState as object[])[0] is ValidationStatus))
                {
                    var validationStatus = (ValidationStatus)((customState as object[])[0]);
                    return FormatHelper.ToCamlCase(validationStatus.ToString());
                }
            }
            return string.Empty;
        }
        #endregion

        #region Get Severity
        protected string GetSeverity(object customState)
        {
            if (customState != null && customState is object[])
            {
                if ((customState as object[]).Length >= 2 && ((customState as object[])[1] is ValidationSeverity))
                {
                    var validationSeverity = (ValidationSeverity)((customState as object[])[1]);
                    return FormatHelper.ToCamlCase(validationSeverity.ToString());
                }
            }
            return string.Empty;
        }
        #endregion
    }
    #endregion
}

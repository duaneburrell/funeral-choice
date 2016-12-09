using FluentValidation;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Obsequy.Model
{
    public static class FluentValidationExtensions
    {
        #region Definitions

		private static int MIN_PASSWORD_LENGTH = 6;
        public static double MIN_QUOTE_VALUE = 300F;
        private static string REGEX_DIGITS_ONLY_PATTERN = "^[0-9]+$";
        
        #endregion

        #region With Validation Context
        // https://fluentvalidation.codeplex.com/discussions/355890
        public static IRuleBuilderOptions<T, TProperty> WithValidationContext<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, ValidationStatus validationStatus, ValidationSeverity validationSeverity)
        {
            return rule.WithState(i =>
            {
                return new object[2] { validationStatus, validationSeverity };
            });
        }

        public static IRuleBuilderOptions<T, TProperty> WithValidationContext<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, ValidationStatus validationStatus)
        {
            return rule.WithState(i =>
            {
                return new object[2] { validationStatus, ValidationSeverity.Error };
            });
        }

        public static IRuleBuilderOptions<T, TProperty> WithValidationContext<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, ValidationSeverity validationSeverity)
        {
            return rule.WithState(i =>
            {
                return new object[2] { ValidationStatus.Invalid, validationSeverity };
            });
        }
        #endregion

        #region Not Null Or Empty
        public static IRuleBuilderOptions<T, string> NotNullOrEmpty<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Must(i =>
            {
                return !string.IsNullOrEmpty(i);
            });
        }
        #endregion

		#region Ensure Account Type
		public static IRuleBuilderOptions<T, AccountType> EnsureAccountType<T>(this IRuleBuilder<T, AccountType> rule)
        {
            // make sure this account type is valid
            return rule.Must(value =>
            {
				return (value == AccountType.Consumer || value == AccountType.Provider);
            });
        }
        #endregion

		#region Ensure Account Type Administrator
		public static IRuleBuilderOptions<T, string> EnsureAccountTypeAdministrator<T>(this IRuleBuilder<T, string> rule, AccountSession accountSession)
		{
			// make sure this account type is valid
			return rule.Must(value =>
			{
				return (accountSession.AccountType == AccountType.Administrator ? true : false);
			});
		}
		#endregion

		#region Can Delete Administrator Member
		public static IRuleBuilderOptions<T, string> CanDeleteAdministratorMember<T>(this IRuleBuilder<T, string> rule, AccountSession accountSession)
		{
			// verify they aren't deleting the current administrator member and it's not the last one
			return rule.Must(value =>
			{
				using (var db = new MongoDbContext())
				{
					var members = db.GetAdministratorMembers();
					if (members.Count == 1)
						return false;

					var memberId = (value as string);
					if (accountSession.MemberId == memberId)
						return false;

					return true;
				}
			});
		}
		#endregion

        #region Ensure Unique Email
		public static IRuleBuilderOptions<T, string> EnsureUnusedEmail<T>(this IRuleBuilder<T, string> rule, AccountSession accountSession)
        {
			// TODO: verify this works when full Mongo DB integration

            // check if this email is NOT currently in use
            return rule.Must(value =>
            {
				if (DatabaseHelper.UserExists(value))
					return false;

                return true;
            });
        }
        #endregion

		#region Ensure Member Email
		public static IRuleBuilderOptions<T, string> EnsureMemberEmail<T>(this IRuleBuilder<T, string> rule, string memberId)
		{
			// TODO: verify this works when full Mongo DB integration

			// check if this email is NOT currently in use
			return rule.Must(value =>
			{
				// check to see if the email address is currently assigned to this member
				if (DatabaseHelper.IsMemberEmail(value, memberId))
					return true;
				
				// if not, they are changing it, so make sure the new one does not exist
				if (DatabaseHelper.UserExists(value))
					return false;

				return true;
			});
		}
		#endregion

		#region Ensure Valid Email
		public static IRuleBuilderOptions<T, string> EnsureValidEmail<T>(this IRuleBuilder<T, string> rule)
        {
			// TODO: verify this works when full Mongo DB integration

            // check if this email is currently in use
            return rule.Must(value =>
            {
				if (!DatabaseHelper.UserExists(value))
					return false;

				return true;
            });
        }
        #endregion

		#region Ensure Email Format
		public static IRuleBuilderOptions<T, string> EnsureEmailFormat<T>(this IRuleBuilder<T, string> rule)
		{
			// TODO: verify this works when full Mongo DB integration

			// check if this email is currently in use
			return rule.Must(value =>
			{
				return true;
			});
		}
		#endregion

        #region Ensure Password Strength
        public static IRuleBuilderOptions<T, string> EnsurePasswordStrength<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Must(value =>
            {
                if (string.IsNullOrEmpty(value))
                    return false;

				bool meetsLengthRequirements = value.Length >= MIN_PASSWORD_LENGTH;
				if (meetsLengthRequirements)
					return true;

                return false;
            });
        }
        #endregion

        #region Ensure State
        public static IRuleBuilderOptions<T, string> EnsureState<T>(this IRuleBuilder<T, string> rule, bool isLimited)
        {
            return rule.Must(i =>
            {
				if (!string.IsNullOrEmpty(i as string))
				{
					// get the state abbreviation
					var abbreviation = Convert.ToString(i).ToUpper();

					// check against the appropriate list
					if (isLimited)
						return StateArray.LimitedAbbreviations.Contains(abbreviation);
					else
						return StateArray.AllAbbreviations.Contains(abbreviation);
				}

				return false;
            });
        }
        #endregion

        #region Ensure Zip
        public static IRuleBuilderOptions<T, string> EnsureZip<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Must(i =>
            {
                if (string.IsNullOrEmpty(i))
                    return false;
                if (i.Length != 5)
                    return false;
                if (!Regex.IsMatch(i, REGEX_DIGITS_ONLY_PATTERN))
                    return false;

                return true;
            });
        }
        #endregion

        #region Ensure Phone Number
        public static IRuleBuilderOptions<T, string> EnsurePhoneNumber<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Must(i =>
            {
                var value = Convert.ToString(i);

                // not sure how to really do this, but for now, as long as there are 10 digits it's ok
                if (!string.IsNullOrEmpty(value))
                {
                    var numDigits = 0;
                    for (int idx = 0; idx < value.Length; idx++)
                    {
                        int tmp = 0;
                        if (Int32.TryParse(value[idx].ToString(), out tmp))
                            numDigits += 1;
                    }

                    return (numDigits == 10 ? true : false);
                }
                return false;
            });
        }
        #endregion

        #region Ensure Geo Distance Id
        public static IRuleBuilderOptions<T, int> EnsureGeoDistanceId<T>(this IRuleBuilder<T, int> rule)
        {
            return rule.Must(value =>
            {
				//using (var db = new AppDbContext())
				//{
				//	return db.GeoDistances.Where(gd => gd.Id == value).Any();
				//}
				return true;
            });
        }
        #endregion

        #region Ensure Character String
        public static IRuleBuilderOptions<T, string> EnsureCharacterString<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Must(i => {
                var value = Convert.ToString(i);
                
                if (string.IsNullOrEmpty(value))
                    return false;

                // TODO: devis a good regular expression. blanks would probably be ok too
                //if (!Regex.IsMatch(value, @"^[a-zA-Z]+$"))
                //    return false;

                return true; 
            });
        }
        #endregion

        #region Ensure Currency
		public static IRuleBuilderOptions<T, double?> EnsureCurrency<T>(this IRuleBuilder<T, double?> rule)
        {
            return rule.Must(value =>
            {
                if (value != null)
					return true;
				return false;
            });
        }
        #endregion

        #region Ensure Quote
        public static IRuleBuilderOptions<T, double?> EnsureQuote<T>(this IRuleBuilder<T, double?> rule)
        {
            return rule.Must(value =>
            {
                if (value >= MIN_QUOTE_VALUE)
                    return true;
                return false;
            });
        }
        #endregion

        #region Ensure Date Is After Today
        public static IRuleBuilderOptions<T, DateTime?> EnsureDateIsAfterToday<T>(this IRuleBuilder<T, DateTime?> rule)
		{
			return rule.Must(value =>
			{
                if (value != null)
                {
                    var tomorrow = DateTimeHelper.Tomorrow;
                    var checkMe = DateTimeHelper.ClipAsDate(value);

                    if (checkMe >= tomorrow)
                        return true;
                }
				return false;
			});
		}
		#endregion

		#region Ensure Url
		public static IRuleBuilderOptions<T, string> EnsureUrl<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Must(value =>
            {
				if (!string.IsNullOrEmpty(value))
				{
					var url = value as string;

					if (!url.StartsWith("http://"))
						return false;
					if (!url.Contains("."))
						return false;
				}

				return true;
            });
        }
		#endregion

		#region Ensure Has Accepted EULA
		public static IRuleBuilderOptions<T, bool?> EnsureHasAcceptedEULA<T>(this IRuleBuilder<T, bool?> rule)
		{
			return rule.Must(i =>
			{
				return Convert.ToBoolean(i);
			});
		}
		#endregion

		#region Has Price
		public static IRuleBuilderOptions<T, decimal?> HasPrice<T>(this IRuleBuilder<T, decimal?> rule)
		{
			return rule.Must(price =>
			{
				if (price == null || Double.Equals(price, 0M) || price <= 0)
					return false;
				return true;
			});
		}
		#endregion

		#region Has Agreement
		public static IRuleBuilderOptions<T, AgreementTypes> HasAgreement<T>(this IRuleBuilder<T, AgreementTypes> rule, ProviderResponseScheme response, string propertyName)
		{
			return rule.Must(value =>
			{
				using (var db = new MongoDbContext())
				{
					var agreement = response.Agreement.AsAgreementType(propertyName);
					var alternate = response.Alternate.AsGenericType(propertyName);
					var preference = (db.GetConsumerPortfolio(response.ConsumerPortfolioId)).Preference.AsGenericType(propertyName);

					// check to see if the consumer has actually specified a value for this property
					if (preference.Value != GenericTypes.NA && preference.Value != GenericTypes.None)
					{
						// has the provider selected a value for the agreement?
						if (agreement == AgreementTypes.NA)
							return false;

						// check to see if the provider has decided to select an alternate value
						if (agreement == AgreementTypes.Alternate)
						{
							// the provider has not chosen a valid alternate value
							if (alternate.Value == GenericTypes.NA)
								return false;

							// check to see if the provider has added a valid alternate user defined value
							if (alternate.Value == GenericTypes.UserSpecified)
								return !string.IsNullOrEmpty(Convert.ToString(alternate.Specified));
						}
					}

					return true;
				}
			});
		}
		#endregion

		#region Has Agreement Date
		public static IRuleBuilderOptions<T, AgreementTypes> HasAgreementDate<T>(this IRuleBuilder<T, AgreementTypes> rule, ProviderResponseScheme response, string propertyName)
		{
			return rule.Must(value =>
			{
				using (var db = new MongoDbContext())
				{
					var agreement = (propertyName == "WakeDate" ? response.Agreement.WakeDate : response.Agreement.CeremonyDate);
					var alternate = (propertyName == "WakeDate" ? response.Alternate.WakeDate : response.Alternate.CeremonyDate);
					var schedule = (db.GetConsumerPortfolio(response.ConsumerPortfolioId)).Schedule;
					var scheduleValue = (propertyName == "WakeDate" ? schedule.WakeDate : schedule.CeremonyDate);

					// check to see if the consumer has actually specified a value for this property
					if (scheduleValue.HasValue)
					{
						// has the provider selected a value for the agreement?
						if (agreement == AgreementTypes.NA)
							return false;

						// check to see if the provider has decided to select an alternate value
						if (agreement == AgreementTypes.Alternate)
						{
							// the provider has not chosen a valid alternate value
							if (alternate.Value == GenericTypes.NA)
								return false;

							// check to see if the provider has added a valid alternate user defined value
							if (alternate.Value == GenericTypes.UserSpecified)
							{
								// is this a valid date?
								var outDate = new DateTime();
								var result = DateTime.TryParse(Convert.ToString(alternate.Specified), out outDate);
								return result;
							}
						}
					}

					return true;
				}
			});
		}
		#endregion

		#region Has Agreement Text
		public static IRuleBuilderOptions<T, AgreementTypes> HasAgreementText<T>(this IRuleBuilder<T, AgreementTypes> rule, ProviderResponseScheme response, string propertyName)
		{
			return rule.Must(value =>
			{
				using (var db = new MongoDbContext())
				{
					var agreement = response.Agreement.ServicePreferences;
					var alternate = response.Alternate.ServicePreferences;
					var preference = (db.GetConsumerPortfolio(response.ConsumerPortfolioId)).Preference;
					var preferenceValue = preference.ServicePreferences;

					// check to see if the consumer has actually specified a value for this property
					if (!string.IsNullOrEmpty(preferenceValue))
					{
						// has the provider selected a value for the agreement?
						if (agreement == AgreementTypes.NA)
							return false;

						// check to see if the provider has decided to select an alternate value
						if (agreement == AgreementTypes.Alternate)
						{
							// the provider has not chosen a valid alternate value
							if (alternate.Value == GenericTypes.NA)
								return false;

							// check to see if the provider has added a valid alternate user defined value
							if (alternate.Value == GenericTypes.UserSpecified)
								return !string.IsNullOrEmpty(Convert.ToString(alternate.Specified));
						}
					}

					return true;
				}
			});
		}
		#endregion

		#region Has Preference
		public static IRuleBuilderOptions<T, object> HasPreference<T>(this IRuleBuilder<T, object> rule, ConsumerPreference instance, string propertyName)
		{
			return rule.Must(value =>
			{
				var preference = instance.AsGenericType(propertyName);

				// if this user has not selected anything:
				if (preference.Value == GenericTypes.NA)
					return false;

				// if user specified, make sure the consumer has typed in a value
				if (preference.Value == GenericTypes.UserSpecified)
					return !string.IsNullOrEmpty(Convert.ToString(preference.Specified));

				return true;
			});
		}
		#endregion
	}
}

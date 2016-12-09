using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Obsequy.Utility;

namespace Obsequy.Model
{
    public static class FluentValidationAuthorizations
	{
		#region Is Current Consumer Portfolio
		public static IRuleBuilderOptions<T, string> IsCurrentConsumerPortfolio<T>(this IRuleBuilder<T, string> rule, ConsumerPortfolio instance)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var portfolio = db.GetConsumerPortfolio(id);
					if (portfolio != null)
					{
						// verify the modified timestamps match
						if (instance.Modified.On == portfolio.Modified.On)
							return true;
					}
				}
				return false;
			});
		}
		#endregion

		#region Is Current Provider Portfolio
		public static IRuleBuilderOptions<T, string> IsCurrentProviderPortfolio<T>(this IRuleBuilder<T, string> rule, ProviderPortfolio instance)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var portfolio = db.GetProviderPortfolio(id);
					if (portfolio != null)
					{
						// verify the modified timestamps match
						if (instance.Modified.On == portfolio.Modified.On)
							return true;
					}
				}
				return false;
			});
		}
		#endregion

		#region Is Current Response
		public static IRuleBuilderOptions<T, string> IsCurrentResponse<T>(this IRuleBuilder<T, string> rule, ProviderResponseScheme instance)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var response = db.GetResponseById(id);
					if (response != null)
					{
						// verify the current timestamps match
						if (instance.Current.On == response.Current.On) 
							return true;
					}
				}
				return false;
			});
		}

		public static IRuleBuilderOptions<T, string> IsCurrentResponse<T>(this IRuleBuilder<T, string> rule, ResponseForm instance)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var response = db.GetResponseById(id);
					if (response != null)
					{
						// verify the current timestamps match
						if (instance.Current.On == response.Current.On)
							return true;
					}
				}
				return false;
			});
		}
		#endregion

		#region Owns Consumer Portfolio Id
		public static IRuleBuilderOptions<T, string> OwnsConsumerPortfolioId<T>(this IRuleBuilder<T, string> rule, AccountSession accountSession)
		{
			return rule.Must(id =>
			{
				if (accountSession.PortfolioIds.Contains(id))
					return true;

				return false;
			});
		}
		#endregion

		#region Owns Provider Portfolio Id
		public static IRuleBuilderOptions<T, string> OwnsProviderPortfolioId<T>(this IRuleBuilder<T, string> rule, AccountSession accountSession)
		{
			return rule.Must(id =>
			{
				if (accountSession.PortfolioIds.Contains(id))
					return true;

				return false;
			});
		}
		#endregion

		#region Owns Response Id
		public static IRuleBuilderOptions<T, string> OwnsResponseId<T>(this IRuleBuilder<T, string> rule, AccountSession accountSession, ValidationMode validationMode)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var response = db.GetResponseById(id);
					if (response != null)
					{
						// if this is a consumer account, verify their portfolio is assigned to this response
						if (accountSession.AccountType == AccountType.Consumer)
							return accountSession.PortfolioIds.Contains(response.ConsumerPortfolioId);

						// if this is a provider account, verify their portfolio is assigned to this response
						if (accountSession.AccountType == AccountType.Provider)
							return accountSession.PortfolioIds.Contains(response.ProviderPortfolioId);
					}
				}
				return false;
			});
		}
		#endregion

		#region Can Become Available
		public static IRuleBuilderOptions<T, string> CanBecomeAvailable<T>(this IRuleBuilder<T, string> rule)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var response = db.GetResponseById(id);
					if (response != null)
					{
						if (response.CanBecomeAvailable)
							return true;
					}
				}
				return false;
			});
		}
		#endregion

		#region Can Become Accepted
		public static IRuleBuilderOptions<T, string> CanBecomeAccepted<T>(this IRuleBuilder<T, string> rule)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var response = db.GetResponseById(id);
					if (response != null)
					{
						if (response.CanBecomeAccepted)
							return true;
					}
				}
				return false;
			});
		}
		#endregion

		#region Can Become Dismissed
		public static IRuleBuilderOptions<T, string> CanBecomeDismissed<T>(this IRuleBuilder<T, string> rule)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var response = db.GetResponseById(id);
					if (response != null)
					{
						if (response.CanBecomeDismissed)
							return true;
					}
				}
				return false;
			});
		}
		#endregion

		#region Can Become Pending
		public static IRuleBuilderOptions<T, string> CanBecomePending<T>(this IRuleBuilder<T, string> rule)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var response = db.GetResponseById(id);
					if (response != null)
					{
						if (response.CanBecomePending)
							return true;
					}
				}
				return false;
			});
		}
		#endregion

		#region Can Become Rejected
		public static IRuleBuilderOptions<T, string> CanBecomeRejected<T>(this IRuleBuilder<T, string> rule)
		{
			return rule.Must(id =>
			{
				using (var db = new MongoDbContext())
				{
					var response = db.GetResponseById(id);
					if (response != null)
					{
						if (response.CanBecomeRejected)
							return true;
					}
				}
				return false;
			});
		}
		#endregion
	}
}

using Obsequy.Utility;
using System;
using System.Collections.Generic;

namespace Obsequy.Model
{
	public class AccountSession
	{
		#region Properties

		#region Account Type
		private AccountType _accountType = AccountType.None;
		public AccountType AccountType
		{
			get	{ return _accountType; }
			set	{ _accountType = value; }
		}
		#endregion

		#region Has Member
		public bool HasMember
		{
			get { return !string.IsNullOrEmpty(_memberId); }
		}
		#endregion

		#region Has Portfolio
		public bool HasPortfolio
		{
			get { return !string.IsNullOrEmpty(_portfolioId); }
		}
		#endregion

		#region Is Authenticated
		public bool IsAuthenticated
		{
			get { return this.HasMember; }
		}
		#endregion

		#region Member Id
		private string _memberId;
		public string MemberId
		{
			get { return _memberId; }
			set { _memberId = value; }
		}
		#endregion

		#region Portfolio Id
		private string _portfolioId;
		public string PortfolioId 
		{
			get	{ return _portfolioId; }
			set { _portfolioId = value; if (!_portfolioIds.Contains(value)) { _portfolioIds.Add(value); } }
		}
		#endregion

		#region Portfolio Ids
		private List<string> _portfolioIds;
		public List<string> PortfolioIds
		{
			get { return _portfolioIds; }
		}
		#endregion

		#endregion

		#region Constructors

		public AccountSession()
		{
			_portfolioIds = new List<string>();
		}

		public AccountSession(System.Security.Principal.IPrincipal principal)
		{
			_portfolioIds = new List<string>();

			SetCurrentAccountSession(principal);
		}

		#endregion

		#region Methods
		protected void SetCurrentAccountSession(System.Security.Principal.IPrincipal principal)
		{
			var accountType = AccountType.None;
			var userId = string.Empty;
			var portfolioId = string.Empty;
			var portfolioIds = new List<string>();

			if (principal != null && principal.Identity.IsAuthenticated)
			{
				// note: we'll just read SQL directly until we get MongoDB web security figured out.
				var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

				using (var sqlConnection = new System.Data.SqlClient.SqlConnection(connectionString))
				{
					// open a connection to our SQL DB
					sqlConnection.Open();

					// find the Mongo Id for this email
					var query = new System.Data.SqlClient.SqlCommand(string.Format("SELECT MongoUserId FROM MEMBERSHIP WHERE Email = '{0}'", principal.Identity.Name), sqlConnection);
					var reader = query.ExecuteReader();

					try
					{
						// get our value
						reader.Read();
						userId = reader.GetValue(0).ToString();
					}
					catch
					{
					}
					finally
					{
						// close the connection
						sqlConnection.Close();
					}

					using (var mongoDbContext = new MongoDbContext())
					{
						var consumerMember = mongoDbContext.GetConsumerMember(userId);
						if (consumerMember != null)
						{
							accountType = AccountType.Consumer;
							portfolioIds.AddRange(consumerMember.PortfolioIds);
						}
						else
						{
							var providerMember = mongoDbContext.GetProviderMember(userId);
							if (providerMember != null)
							{
								accountType = AccountType.Provider;
								portfolioIds.AddRange(providerMember.PortfolioIds);
							}
							else
							{
								var administratorMember = mongoDbContext.GetAdministratorMember(userId);
								if (administratorMember != null)
								{
									accountType = AccountType.Administrator;
								}
							}

						}
					}
				}
			}

			// set properties
			_accountType = accountType;
			_memberId = userId;
			_portfolioId = portfolioId;
			_portfolioIds.AddRange(portfolioIds);
		}
		#endregion
	}
}

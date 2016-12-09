using MongoDB.Web.Providers;
using Obsequy.Data;
using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;

namespace Obsequy.Security
{
    public class AuthenticationSecurity
	{
		#region Properties

		#region Application Name
		private string _applicationName = string.Empty;
		protected string ApplicationName
		{
			get
			{
				if (string.IsNullOrEmpty(_applicationName))
					_applicationName = this.DatabaseName;

				return _applicationName;
			}
		}
		#endregion

		#region Connection String Name
		protected string ConnectionStringName
		{
			get { return "CUSTOMCONNSTR_MONGOLAB_URI"; }
		}
		#endregion

		#region Connection String
		protected string ConnectionString
		{
			get { return System.Environment.GetEnvironmentVariable(this.ConnectionStringName); }
		}
		#endregion

		#region Database Name
		private string _databaseName = string.Empty;
		protected string DatabaseName
		{
			get
			{
				if (string.IsNullOrEmpty(_databaseName))
				{
					var parts = this.ConnectionString.Split('/');
					_databaseName = parts[parts.Length - 1];
				}

				return _databaseName;
			}
		}
		#endregion

		#region Is Mongo Membership Enabled
		public bool IsMongoMembershipEnabled
		{
			get { return false; }
		}
		#endregion

		#region Logger
		protected NLog.Logger Logger
		{
			get;
			set;
		}
		#endregion

		#region Membership Provider
		public MongoDBMembershipProvider _membershipProvider;
		public MongoDBMembershipProvider MembershipProvider
		{
			get 
			{
				if (_membershipProvider == null)
				{
					_membershipProvider = new MongoDBMembershipProvider();
					_membershipProvider.Initialize(System.Web.Security.Membership.Provider.Name, this.MembershipProviderConfigSettings);
				}
				
				return _membershipProvider;
			}
		}
		#endregion

		#region Membership Provider Config Settings
		private NameValueCollection _membershipProviderConfigSettings;
		public NameValueCollection MembershipProviderConfigSettings
		{
			get
			{
				if (_membershipProviderConfigSettings == null)
				{
					_membershipProviderConfigSettings = new NameValueCollection();
					_membershipProviderConfigSettings.Add("connectionStringName", this.ConnectionStringName);
					_membershipProviderConfigSettings.Add("applicationName", this.ApplicationName);
					_membershipProviderConfigSettings.Add("database", this.DatabaseName);
					_membershipProviderConfigSettings.Add("collection", "Membership");
					_membershipProviderConfigSettings.Add("enablePasswordRetrieval", "false");
					_membershipProviderConfigSettings.Add("enablePasswordReset", "true");
					_membershipProviderConfigSettings.Add("requiresQuestionAndAnswer", "false");
					_membershipProviderConfigSettings.Add("requiresUniqueEmail", "false");
					_membershipProviderConfigSettings.Add("maxInvalidPasswordAttempts", "5");
					_membershipProviderConfigSettings.Add("minRequiredPasswordLength", "6");
					_membershipProviderConfigSettings.Add("minRequiredNonalphanumericCharacters", "0");
					_membershipProviderConfigSettings.Add("passwordAttemptWindow", "10");
				}

				return _membershipProviderConfigSettings;
			}
		}
		#endregion

		#region Mongo
		private MongoDbContext _mongo;
		protected MongoDbContext Mongo
		{
			get { return _mongo ?? (_mongo = new MongoDbContext()); }
		}
		#endregion

		#region Role Provider
		public MongoDBRoleProvider _roleProvider;
		public MongoDBRoleProvider RoleProvider
		{
			get
			{
				if (_roleProvider == null)
				{
					_roleProvider = new MongoDBRoleProvider();
					_roleProvider.Initialize("MongoDBRoleProvider", this.RoleProviderConfigSettings);
				}

				return _roleProvider;
			}
		}
		#endregion

		#region Role Provider Config Settings
		private NameValueCollection _roleProviderConfigSettings;
		public NameValueCollection RoleProviderConfigSettings
		{
			get
			{
				if (_roleProviderConfigSettings == null)
				{
					_roleProviderConfigSettings = new NameValueCollection();
					_roleProviderConfigSettings.Add("connectionStringName", this.ConnectionStringName);
					_roleProviderConfigSettings.Add("applicationName", this.ApplicationName);
					_roleProviderConfigSettings.Add("database", this.DatabaseName);
				}

				return _roleProviderConfigSettings;
			}
		}
		#endregion

		#region Uow
		public IObsequyUow Uow
		{
			get;
			set;
		}
		#endregion

		#endregion

		#region Construction / Initialization
		public AuthenticationSecurity()
		{
			Logger = NLog.LogManager.GetLogger(Definitions.Logger.Name);
		}

		public void InitializeDatabaseConnection()
		{
			if (!WebSecurity.Initialized)
			{
				WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Membership", "Id", "Email", autoCreateTables: false);

				Logger.Info("WebSecurity database initialized");
			}

			this.Mongo.Initialize();
		}
		#endregion

		#region Create Administrator Account
		public void CreateAdministratorAccount()
		{
			// TODO: figure out how we an create the UoW w/o doing this
			this.Uow = new ObsequyUow(new RepositoryProvider(new RepositoryFactories()));

			// ?? we should probably put this in the web config
			var form = new AdministratorRegistrationForm() 
			{
				Member = new AdminMember() 
				{
					Email = "admin@synchro-soft.com",
					FirstName = "Big",
					LastName = "Daddy",
					IsNotifiedOnConsumerRegistrations = true,
					IsNotifiedOnProviderRegistrations = true,
					IsNotifiedOnAcceptedResponses = true,
					IsNotifiedOnExceptions = false
				},
				Password = "doggo1",
				ConfirmPassword = "doggo1",
			};

			if (!WebSecurity.UserExists(form.Member.Email))
			{
				// register this account
				CreateMember(form);

				// add appropriate roles to the account
				var roles = new string[2] { Definitions.Account.Roles.Elevated, Definitions.Account.Roles.Administrator };

				AddRolesToUser(form.Member.Email, roles);
			}
		}
		#endregion

		#region Role Management
		public void CreateRole(string role)
		{
			if (this.IsMongoMembershipEnabled)
			{
				// add to user
				var allRoles = this.RoleProvider.GetAllRoles();

				if (!allRoles.Contains(role))
				{
					this.RoleProvider.CreateRole(role);

					Logger.Info(string.Format("Created role (MongoDB): {0}", role));
				}
			}
			else
			{
				// also set in web security
				var allRoles = Roles.GetAllRoles();

				if (!allRoles.Contains(role))
				{
					Roles.CreateRole(role);

					Logger.Info(string.Format("Created role (WebSecurity): {0}", role));
				}
			}
		}

		public void AddRoleToUser(string userName, string role)
		{
			// TODO: Finish me for Mongo
			Roles.AddUserToRole(userName, role);
		}

		public void AddRolesToUser(string userName, string[] roles)
		{
			// TODO: Finish me for Mongo
			Roles.AddUserToRoles(userName, roles);
		}

		public static string[] GetRoles(System.Security.Principal.IPrincipal principal)
		{
			if (principal.Identity.IsAuthenticated)
			{
				var roles = Roles.GetRolesForUser(principal.Identity.Name);
				if (roles.Length == 0)
				{
					// TODO: Read these roles from Mongo

					////// get from the DB directly
					////// note: I had to do this because by the time the index page loads, it's almost as if the user session hasn't loaded any roles yet.
					////using (var db = new AppDbContext())
					////{
					////	var user = db.Users.Where(u => u.Email == principal.Identity.Name).FirstOrDefault();
					////	if (user != null)
					////	{
					////		// not sure why this needed to be done
					////		var webpages_Roles = user.webpages_UsersInRoles.Select(i => i.webpages_Roles);
					////		var webpages_RoleNames = webpages_Roles.Select(i => i.RoleName).ToArray();

					////		// return the list of names
					////		return webpages_RoleNames;

					////		// old code	
					////		//return user.webpages_Roles.Select(i => i.RoleName).ToArray();
					////	}
					////}
				}

				return roles;
			}

			return new string[1];
		}
		#endregion

		#region Create Member (Administrator)
		public AdminMember CreateMember(AdministratorRegistrationForm form)
		{
			var userName = form.Member.Email.Scrub();
			var password = form.Password.Scrub();
			var model = AdminMember.Empty;

			if (this.IsMongoMembershipEnabled)
			{
				try
				{
					var status = MembershipCreateStatus.UserRejected;
					var membershipUser = this.MembershipProvider.CreateUser(userName, password, userName, "", "", true, null, out status);

					if (status == MembershipCreateStatus.Success)
					{
						// create a user account
						model = this.Uow.Administrators.CreateMember(form, membershipUser.ProviderUserKey.ToString());

						if (model == null)
						{
							throw new Exception("failed to create administrator account");
						}
					}
					else
					{
						Logger.Warn(string.Format("Failed to create administrator: {0}. Reason: {1}", userName, status));
					}
				}
				catch (Exception e)
				{
					Logger.Error(string.Format("Error creating administrator: {0}", userName), e);
					throw e;
				}
			}
			else
			{
				// create a user account
				model = this.Uow.Administrators.CreateMember(form, string.Empty);

				if (model == null)
				{
					throw new Exception("failed to create administrator account");
				}
			}

			try
			{
				WebSecurity.CreateUserAndAccount(userName, password, new
				{
					MongoUserId = model.Id
				});
			}
			catch (Exception e)
			{
				Logger.Error(string.Format("Error creating WebSecurity administrator: {0}", userName), e);
				throw e;
			}

			return model;
		}
		#endregion

		#region Update Member (Administrator)
		public AdminMember UpdateMember(AdminMember member)
		{
			var userName = member.Email;

			if (this.IsMongoMembershipEnabled)
			{
				//try
				//{
				//	var status = MembershipCreateStatus.UserRejected;
				//	var membershipUser = this.MembershipProvider.CreateUser(userName, password, userName, "", "", true, null, out status);

				//	if (status == MembershipCreateStatus.Success)
				//	{
				//		// create a user account
				//		model = this.Uow.Administrators.CreateMember(form, membershipUser.ProviderUserKey.ToString());

				//		if (model == null)
				//		{
				//			throw new Exception("failed to create administrator account");
				//		}
				//	}
				//	else
				//	{
				//		Logger.Warn(string.Format("Failed to create administrator: {0}. Reason: {1}", userName, status));
				//	}
				//}
				//catch (Exception e)
				//{
				//	Logger.Error(string.Format("Error creating administrator: {0}", userName), e);
				//	throw e;
				//}
			}
			else
			{
				// update the member account
				this.Uow.Administrators.UpdateMember(member);
			}

			try
			{
				// update the SQL user email
				var wasUpdated = DatabaseHelper.UpdateMemberEmail(userName, member.Id);
			}
			catch (Exception e)
			{
				Logger.Error(string.Format("Error deleting WebSecurity administrator: {0}", member.Email), e);
				throw e;
			}

			// return a fresh copy
			return this.Uow.Administrators.FindById(member.Id);
		}
		#endregion

		#region Delete Member (Administrator)
		public void DeleteMember(AdminMember member)
		{
			var userName = member.Email;

			if (this.IsMongoMembershipEnabled)
			{
				//try
				//{
				//	var status = MembershipCreateStatus.UserRejected;
				//	var membershipUser = this.MembershipProvider.CreateUser(userName, password, userName, "", "", true, null, out status);

				//	if (status == MembershipCreateStatus.Success)
				//	{
				//		// create a user account
				//		model = this.Uow.Administrators.CreateMember(form, membershipUser.ProviderUserKey.ToString());

				//		if (model == null)
				//		{
				//			throw new Exception("failed to create administrator account");
				//		}
				//	}
				//	else
				//	{
				//		Logger.Warn(string.Format("Failed to create administrator: {0}. Reason: {1}", userName, status));
				//	}
				//}
				//catch (Exception e)
				//{
				//	Logger.Error(string.Format("Error creating administrator: {0}", userName), e);
				//	throw e;
				//}
			}
			else
			{
				// delete the member account
				this.Uow.Administrators.DeleteMember(member);
			}

			try
			{
				if (Roles.GetRolesForUser(userName).Count() > 0)
					Roles.RemoveUserFromRoles(userName, Roles.GetRolesForUser(userName));

				((SimpleMembershipProvider)System.Web.Security.Membership.Provider).DeleteAccount(userName); // deletes record from webpages_Membership table
				((SimpleMembershipProvider)System.Web.Security.Membership.Provider).DeleteUser(userName, true); // deletes the record in the Membership table
			}
			catch (Exception e)
			{
				Logger.Error(string.Format("Error deleting WebSecurity administrator: {0}", member.Email), e);
				throw e;
			}
		}
		#endregion

		#region Create Member (Consumer)
		public ConsumerMember CreateMember(ConsumerRegistrationForm form)
		{
			var userName = form.Member.Email.Scrub();
			var password = form.Password.Scrub();
			var model = ConsumerMember.Empty;

			if (this.IsMongoMembershipEnabled)
			{
				try
				{
					var status = MembershipCreateStatus.UserRejected;
					var membershipUser = this.MembershipProvider.CreateUser(userName, password, userName, "", "", true, null, out status);

					if (status == MembershipCreateStatus.Success)
					{
						// create a user account
						model = this.Uow.ConsumerMembers.CreateMember(form, membershipUser.ProviderUserKey.ToString());

						if (model == null)
						{
							throw new Exception("failed to create consumer account");
						}
					}
					else
					{
						Logger.Warn(string.Format("Failed to create Member: {0}. Reason: {1}", userName, status));
					}
				}
				catch (Exception e)
				{
					Logger.Error(string.Format("Error creating Member: {0}", userName), e);
					throw e;
				}
			}
			else
			{
				// create a user account w/o the membership
				model = this.Uow.ConsumerMembers.CreateMember(form, string.Empty);

				if (model == null)
				{
					throw new Exception("failed to create consumer account");
				}
			}

			try
			{
				WebSecurity.CreateUserAndAccount(userName, password, new {
					MongoUserId = model.Id
				});
			}
			catch (Exception e)
			{
				Logger.Error(string.Format("Error creating WebSecurity user: {0}", userName), e);
				throw e;
			}

			return model;
		}
		#endregion

		#region Create Member (Provider)
		public ProviderMember CreateMember(ProviderRegistrationForm form)
		{
			var userName = form.Member.Email.Scrub();
			var password = form.Password.Scrub();
			var model = ProviderMember.Empty;

			if (this.IsMongoMembershipEnabled)
			{
				try
				{
					var status = MembershipCreateStatus.UserRejected;
					var membershipUser = this.MembershipProvider.CreateUser(userName, password, userName, "", "", true, null, out status);

					if (status == MembershipCreateStatus.Success)
					{
						// create a user account
						model = this.Uow.ProviderMembers.CreateMember(form, membershipUser.ProviderUserKey.ToString());

						if (model == null)
						{
							throw new Exception("failed to create provider account");
						}
					}
					else
					{
						Logger.Warn(string.Format("Failed to create Member: {0}. Reason: {1}", userName, status));
					}
				}
				catch (Exception e)
				{
					Logger.Error(string.Format("Error creating Member: {0}", userName), e);
					throw e;
				}
			}
			else
			{
				// create a user account
				model = this.Uow.ProviderMembers.CreateMember(form, string.Empty);

				if (model == null)
				{
					throw new Exception("failed to create provider account");
				}
			}

			try
			{
				WebSecurity.CreateUserAndAccount(userName, password, new
				{
					MongoUserId = model.Id
				});
			}
			catch (Exception e)
			{
				Logger.Error(string.Format("Error creating WebSecurity user: {0}", userName), e);
				throw e;
			}

			return model;
		}
		#endregion

		#region Login / Logout
		public static bool Login(string userName, string password, bool persistCookie = false)
		{
			return WebSecurity.Login(userName, password, persistCookie);
		}

		public static void Logout()
		{
			WebSecurity.Logout();
		}
		#endregion

		#region User Account Type
		public static AccountType UserAccountType(System.Security.Principal.IPrincipal principal)
		{
			if (principal.Identity.IsAuthenticated)
			{
				var roles = GetRoles(principal);

				if (roles.Contains(Definitions.Account.Roles.Administrator))
					return AccountType.Administrator;

				if (roles.Contains(Definitions.Account.Roles.Consumer))
					return AccountType.Consumer;

				if (roles.Contains(Definitions.Account.Roles.Provider))
					return AccountType.Provider;
			}

			return AccountType.None;
		}
		#endregion

		#region Registration Account Type Redirect
		public static string RegistrationAccountTypeRedirect(AccountType accountType)
		{
			if (accountType == AccountType.Consumer)
				return string.Format("{0}/#/home", Definitions.Controllers.Consumer.Route.Server);

			if (accountType == AccountType.Provider)
				return string.Format("{0}/#/home", Definitions.Controllers.Provider.Route.Server);

			return "/";
		}
		#endregion

		#region Generate Password Reset Token
		public static string GeneratePasswordResetToken(string userName)
		{
			var token = WebSecurity.GeneratePasswordResetToken(userName);

			return token;
		}
		#endregion

		#region Reset Password
		public static bool ResetPassword(string token, string password)
		{
			return WebSecurity.ResetPassword(token, password);
		}
		#endregion

		#region Change Password
		public static bool ChangePassword(string email, string currentPassword, string newPassword)
		{
			return WebSecurity.ChangePassword(email, currentPassword, newPassword);
		}
		#endregion
	}
}

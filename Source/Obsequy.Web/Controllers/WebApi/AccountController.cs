using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Security;
using Obsequy.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace Obsequy.Web
{
    public class AccountController : BaseApiController
    {
		#region Constuction
		public AccountController(IObsequyUow uow)
			: base(uow)
		{
		}
		#endregion

		#region Member
		[ActionName("member")]
		[HttpGet]
		public HttpResponseMessage Member()
		{
			try
			{
				if (this.AccountSession != null)
				{
					if (this.AccountSession.AccountType == AccountType.Consumer)
					{
						var results = this.Uow.ConsumerMembers.GetScheme(this.AccountSession.MemberId);

						// return the response for this member
						return CreateSuccessResponse(new { success = true, results = results });
					}
					else if (this.AccountSession.AccountType == AccountType.Provider)
					{
						var member = this.Uow.ProviderMembers.GetScheme(this.AccountSession.MemberId);

						// return the response for this member
						return CreateSuccessResponse(new { success = true, results = member });
					}
					else
					{
						// not sure what to do here. this could be the administrator, but they shouldn't need this. If they do, add in support
						return CreateInvalidResponse(new { invalid = true, results = string.Format("Can not find member for {0}", this.AccountSession.MemberId) });
					}
				}
				else
				{
					// not sure what to do here. this could be the administrator, but they shouldn't need this. If they do, add in support
					return CreateInvalidResponse(new { invalid = true });
				}
			}
			catch (Exception ex)
			{
				// log the user out
				AuthenticationSecurity.Logout();

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Register Consumer
		[ActionName("registerconsumer")]
		[HttpPost]
		[AllowAnonymous]
		[InitializeSimpleMembership]
		public HttpResponseMessage RegisterConsumer(ConsumerRegistrationForm registrationForm)
		{
			var formValidation = registrationForm.Validate(this.AccountSession, ValidationMode.Create);

			if (formValidation.IsValid)
			{
				var authenticationSecurity = new AuthenticationSecurity() 
				{
					Uow = this.Uow
				};

				try
				{
					// create the account
					var member = authenticationSecurity.CreateMember(registrationForm);

					// set the account session (user ID needed for create group)
					this.Uow.AccountSession = new Model.AccountSession() { MemberId = member.Id };

					// use the switchboard to handle any communications related to this new member
					this.Switchboard.ConsumerMemberCreated(member);

					// login to the account
					AuthenticationSecurity.Login(registrationForm.Member.Email, registrationForm.Password);

					// add appropriate roles to the account
					var roles = new string[2] {Definitions.Account.Roles.Elevated, Definitions.Account.Roles.Consumer};

					authenticationSecurity.AddRolesToUser(registrationForm.Member.Email, roles);

					// set redirect location based on user type
					var redirect = AuthenticationSecurity.RegistrationAccountTypeRedirect(AccountType.Consumer);

					return CreateSuccessResponse(new { success = true, redirect = redirect }, HttpStatusCode.Created);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to create consumer account and login for email {0}", registrationForm.Member.Email), ex);

					// log the user out
					AuthenticationSecurity.Logout();

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(formValidation);
		}
		#endregion

		#region Register Provider
		[ActionName("registerprovider")]
		[HttpPost]
		[AllowAnonymous]
		[InitializeSimpleMembership]
		public HttpResponseMessage RegisterProvider(ProviderRegistrationForm registrationForm)
		{
			var formValidation = registrationForm.Validate(this.AccountSession, ValidationMode.Create);

			if (formValidation.IsValid)
			{
				var authenticationSecurity = new AuthenticationSecurity()
				{
					Uow = this.Uow
				};

				try
				{
					// create the member account
					var member = authenticationSecurity.CreateMember(registrationForm);

					// set the accout session
					this.Uow.AccountSession = new Model.AccountSession() { MemberId = member.Id };

					// use the switchboard to handle any communications related to this new member
					this.Switchboard.ProviderMemberCreated(member);

					// login to the account
					AuthenticationSecurity.Login(registrationForm.Member.Email, registrationForm.Password);

					// add appropriate roles to the account
					var roles = new string[2] { Definitions.Account.Roles.Elevated, Definitions.Account.Roles.Provider };

					authenticationSecurity.AddRolesToUser(registrationForm.Member.Email, roles);

					// set redirect location based on user type
					var redirect = AuthenticationSecurity.RegistrationAccountTypeRedirect(AccountType.Provider);

					return CreateSuccessResponse(new { success = true, redirect = redirect }, HttpStatusCode.Created);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to create provider account and login for email {0}", registrationForm.Member.Email), ex);

					// log the user out
					AuthenticationSecurity.Logout();

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(formValidation);
		}
		#endregion

		#region Login
		[ActionName("login")]
		[HttpPost]
		[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		public HttpResponseMessage Login(LoginForm loginForm)
		{
			var formValidation = loginForm.Validate();

			if (formValidation.IsValid)
			{
				try
				{
					// attempt to login the user
					if (AuthenticationSecurity.Login(loginForm.Email, loginForm.Password, loginForm.RememberMe))
					{
						// set some arbitrary redirect path to a valid MVC route
						// note: this is ok since the client should do a redirect and the server will determine their correct path when the account session is updated
						// note: ideally we'd look up the account type and set some default path, but this works well enough as long as the redirect is done.
						var redirect = "/c/#/path-to-somewhere/";

						return CreateSuccessResponse(new { success = true, results = redirect });
					}
					else
					{
						// force invalid password error
						return CreateInvalidResponse(loginForm.AsInvalidPassword());
					}
				}
				catch (Exception ex)
				{
					// log the user out
					AuthenticationSecurity.Logout();

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(formValidation);
		}
		#endregion

		#region Retrieve Password
		[HttpPut]
		[AllowAnonymous]
		[ActionName("passwordrecovery")]
		public HttpResponseMessage PasswordRecovery(PasswordRecoveryForm form)
		{
			var formValidation = form.Validate();

			if (formValidation.IsValid)
			{
				try
				{
					// TODO: populate the recovery options based on the provided email
					form.CanAnswerSecurityQuestion = true;
					form.CanResetPassword = true;
					form.CanSendSmSCode = true;

					form.SecurityQuestion = "What is my dog's name?";

					return CreateSuccessResponse(new { success = true, results = form });
				}
				catch (Exception ex)
				{
					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(formValidation);
		}

		[HttpPost]
		[AllowAnonymous]
		[ActionName("recoverpassword")]
		public HttpResponseMessage RecoverPassword(PasswordRecoveryForm form)
		{
			var formValidation = form.Validate();

			if (formValidation.IsValid)
			{
				try
				{
					// TODO: finish this section
					if (form.IsAnsweringSecurityQuestion)
					{

					}
					else if (form.IsResettingPassword)
					{
						var host = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
						var token = AuthenticationSecurity.GeneratePasswordResetToken(form.Email);
						
						if (!string.IsNullOrWhiteSpace(host))
						{
							// create the reset message
							StringBuilder sb = new StringBuilder();
							var body1 = string.Format("You have requested your password to be reset. Follow the provided link:\n");
							//var body2 = string.Format("http://{0}/api/account/incomingtoken?param={1}", host, token);
							var body2 = string.Format("http://{0}/#/reset?param={1}", host, token);

							sb.Append(body1);
							sb.AppendLine();
							sb.AppendLine();
							sb.Append(body2);

							// send the password reset message
							//Obsequy.Communication.Email.PasswordReset(form.Email, "BuildShark Password Reset", sb.ToString());
							Obsequy.Communication.Mailer mailer = new Communication.Mailer();
							mailer.DeliverMessage(form.Email, "Password Reset", sb.ToString());
						}
					}
					else if (form.IsSendingSmSCode)
					{

					}


					// request successful
					return CreateSuccessResponse(new { success = true });
				}
				catch (Exception ex)
				{
					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(formValidation);
		}
		#endregion

		#region Incoming Token
		[AllowAnonymous]
		[HttpPost]
		[ActionName("incomingtoken")]
		public HttpResponseMessage IncomingToken(object dto)//string param, string password
		{
			JObject jObject = JObject.Parse(dto.ToString());

			string token = (string)jObject["token"];
			string password = (string)jObject["password"];
			
			//if (!IsSupportedBrowser())
			//	return BrowserUpgrade();

			if (string.IsNullOrWhiteSpace(token))
			{
				return CreateInvalidResponse("The specified token is invalid");
			}

			if (AuthenticationSecurity.ResetPassword(token, password))
			{
				return CreateSuccessResponse(new { success = true });
			}
			else
			{
				return CreateSuccessResponse(new { success = false });
			}
		}

		#endregion
	}

}

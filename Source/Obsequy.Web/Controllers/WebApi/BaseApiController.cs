using Obsequy.Agent;
using Obsequy.Communication;
using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Obsequy.Web
{
	public abstract class BaseApiController : ApiController
	{
		#region Properties

		#region Account Session
		private AccountSession _accountSession = null;
		public AccountSession AccountSession
		{
			get { return _accountSession ?? (_accountSession = new AccountSession(this.User)); }
		}
		#endregion

		#region Agent
		private WebAgent _agent = null;
		protected WebAgent Agent
		{
			get
			{
				if (_agent == null)
					_agent = new WebAgent();
				return _agent;
			}
		}
		#endregion

		#region Logger
		private NLog.Logger _logger;
		protected  NLog.Logger Logger
		{
			get { return _logger ?? (_logger = NLog.LogManager.GetLogger(Definitions.Logger.Name)); }
		}
		#endregion

		#region Switchboard
		private Switchboard _switchboard = null;
		protected Switchboard Switchboard
		{
			get
			{
				if (_switchboard == null)
					_switchboard = new Switchboard();
				return _switchboard;
			}
			private set
			{
				_switchboard = value;
			}
		}
		#endregion

		#region Uow
		protected IObsequyUow Uow
		{ 
			get; 
			set; 
		}
		#endregion

		#endregion

		#region Constructors

		public BaseApiController(IObsequyUow uow)
			: base()
		{
			this.Uow = uow;
			this.Uow.AccountSession = this.AccountSession;
			this.Uow.Switchboard = this.Switchboard;
		}

		#endregion

		#region Create Success Response
		protected HttpResponseMessage CreateSuccessResponse(object data, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
		{
			return this.Request.CreateResponse(httpStatusCode, data);
		}
		#endregion

		#region Create Invalid Response
		protected HttpResponseMessage CreateInvalidResponse(object validationResult)
		{
			return this.Request.CreateResponse(HttpStatusCode.OK, new
			{
				invalid = true,
				results = validationResult
			});
		}
		#endregion

		#region Create No Content Response
		protected HttpResponseMessage CreateNoContentResponse()
		{
			return this.Request.CreateResponse(HttpStatusCode.NoContent, new
			{
				invalid = true
			});
		}
		#endregion

		#region Create Error Response
		protected HttpResponseMessage CreateErrorResponse(Exception ex)
		{
			return this.Request.CreateResponse(HttpStatusCode.Forbidden, new
			{
				error = true,
				results = ex.Message
			});
		}
		#endregion
	}
}
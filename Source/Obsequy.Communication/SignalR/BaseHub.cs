using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obsequy.Communication
{
	public class BaseHub : Hub
	{
		#region Hub Identity
		public class HubIdentity
		{
			public AccountType AccountType { get; set; }
			public string ConnectionId { get; set; }
			public string PortfolioId { get; set; }
			public List<string> PortfolioIds { get; private set; }
			public string UserId { get; set; }

			public HubIdentity(AccountSession accountSession, HubCallerContext context)
			{
				this.PortfolioIds = new List<string>();
				
				this.ConnectionId = context.ConnectionId;
				this.AccountType = accountSession.AccountType;
				this.PortfolioId = accountSession.PortfolioId;
				this.PortfolioIds.AddRange(accountSession.PortfolioIds);
				this.UserId = accountSession.MemberId;
			}

			public override bool Equals(object obj)
			{
				return (this.ConnectionId == (obj as HubIdentity).ConnectionId);
				//return (this.AccountType == (obj as HubIdentity).AccountType &&
				//	this.ConnectionId == (obj as HubIdentity).ConnectionId &&
				//	this.PortfolioId == (obj as HubIdentity).PortfolioId &&
				//	this.UserId == (obj as HubIdentity).UserId);
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}
		#endregion

		#region Properties

		#region Account Session
		private AccountSession _accountSession = null;
		protected AccountSession AccountSession
		{
			get { return _accountSession ?? (_accountSession = new AccountSession(Context.User)); }
		}
		#endregion

		#region Identity
		private HubIdentity _identity = null;
		protected HubIdentity Identity
		{
			get { return _identity ?? (_identity = new HubIdentity(this.AccountSession, this.Context)); }
		}
		#endregion

		#region Identities
		private static List<HubIdentity> _identities;
		protected List<HubIdentity> Identities
		{
			get { return _identities ?? (_identities = new List<HubIdentity>()); }
		}
		#endregion

		#region Logger
		private NLog.Logger _logger;
		protected NLog.Logger Logger
		{
			get { return _logger ?? (_logger = NLog.LogManager.GetLogger(Definitions.Logger.Name)); }
		}
		#endregion

		#region Mongo
		private MongoDbContext _mongo;
		protected MongoDbContext Mongo
		{
			get { return _mongo ?? (_mongo = new MongoDbContext()); }
		}
		#endregion

		#endregion

		#region Connection Methods

		public override Task OnConnected()
		{
			// add the current identity if it's not already stored
			if (!this.Identities.Any(item => item.Equals(this.Identity)))
				this.Identities.Add(this.Identity);

			return base.OnConnected();
		}

		public override Task OnReconnected()
		{
			// add the current identity if it's not already stored
			if (!this.Identities.Any(item => item.Equals(this.Identity)))
				this.Identities.Add(this.Identity);

			return base.OnReconnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			// add the current identity if it's not already stored
			if (this.Identities.Any(item => item.Equals(this.Identity)))
				this.Identities.Remove(this.Identities.Where(item => item == this.Identity).FirstOrDefault());

			return base.OnDisconnected(stopCalled);
		}

		#endregion
	}
}
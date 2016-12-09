using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GeoJsonObjectModel;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class MongoDbContext : IDisposable
	{
		#region Constants

		private const double KM_MULTIPLIER = 111.0;
		private const double MI_MULTIPLIER = 69.0;
		private const double MI_MAX_DISTANCE = 100.0;
		private const int UNLIMITED_RESULTS = 10000;

		#endregion

		#region Properties

		#region Connection String
		private string _connectionString;
		protected string ConnectionString
		{
			get
			{
				if (string.IsNullOrEmpty(_connectionString))
					_connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;
				return _connectionString;
			}
		}
		#endregion

		#region Database
		private MongoDatabase _database;
		protected MongoDatabase Database
		{
			get	{ return _database ?? (_database = this.Server.GetDatabase(new MongoUrl(this.ConnectionString).DatabaseName)); }
		}
		#endregion

		#region Server
		private MongoServer _server;
		protected MongoServer Server
		{
			get { return _server ?? (_server = new MongoClient(this.ConnectionString).GetServer()); }
		}
		#endregion

		#region Geo Near Options
		private GeoNearOptionsBuilder _geoNearOptions;
		protected GeoNearOptionsBuilder GeoNearOptions
		{
			get
			{
				if (_geoNearOptions == null)
				{
					_geoNearOptions = new GeoNearOptionsBuilder();
					_geoNearOptions.SetDistanceMultiplier(MI_MULTIPLIER);
					_geoNearOptions.SetMaxDistance(MI_MAX_DISTANCE / MI_MULTIPLIER);
				}

				return _geoNearOptions;
			}
		}
		#endregion

		#region Admin Members
		protected MongoCollection<AdminMember> AdminMembers
		{
			get
			{
				return this.Database.GetCollection<AdminMember>(Definitions.Database.AdminMemberDocuments);
			}
		}
		#endregion

		#region Consumer Members
		protected MongoCollection<ConsumerMember> ConsumerMembers
		{
			get
			{
				return this.Database.GetCollection<ConsumerMember>(Definitions.Database.ConsumerMemberDocuments);
			}
		}
		#endregion

		#region Consumer Portfolios
		protected MongoCollection<ConsumerPortfolio> ConsumerPortfolios
		{
			get
			{
				return this.Database.GetCollection<ConsumerPortfolio>(Definitions.Database.ConsumerPortfolioDocuments);
			}
		}
		#endregion

		#region Provider Members
		protected MongoCollection<ProviderMember> ProviderMembers
		{
			get
			{
				return this.Database.GetCollection<ProviderMember>(Definitions.Database.ProviderMemberDocuments);
			}
		}
		#endregion

		#region Provider Portfolios
		protected MongoCollection<ProviderPortfolio> ProviderPortfolios
		{
			get
			{
				return this.Database.GetCollection<ProviderPortfolio>(Definitions.Database.ProviderPortfolioDocuments);
			}
		}
		#endregion

		#region Responses
		protected MongoCollection<Response> Responses
		{
			get
			{
				return this.Database.GetCollection<Response>(Definitions.Database.ResponseDocuments);
			}
		}
		#endregion

		#endregion

		#region Initialization Methods
		public void Initialize()
		{
			// consumer indexes
			this.ConsumerPortfolios.EnsureIndex(IndexKeys<ConsumerPortfolio>.GeoSpatial(item => item.Preference.Proximity.Location));
			this.ConsumerPortfolios.EnsureIndex(IndexKeys<ConsumerPortfolio>.Ascending(item => item.Request.State));

			// provider indexes
			this.ProviderPortfolios.EnsureIndex(IndexKeys<ProviderPortfolio>.GeoSpatial(item => item.Principal.Address.Location));

			// response indexes
			this.Responses.EnsureIndex(IndexKeys<Response>.Ascending(item => item.ConsumerPortfolioId));
			this.Responses.EnsureIndex(IndexKeys<Response>.Ascending(item => item.ProviderPortfolioId));
		}
		#endregion

		#region Administrator Methods

		#region Get Administrator Member
		public AdminMember GetAdministratorMember(string memberId)
		{
			// get the member
			var member = this.AdminMembers.FindAll().Where(item => item.Id == memberId).FirstOrDefault();

			return member;
		}
		#endregion

		#region Get Administrator Members
		public List<AdminMember> GetAdministratorMembers()
		{
			// get all the members
			return this.AdminMembers.FindAll().ToList();
		}
		#endregion

		#region Delete Administrator Member
		public void DeleteAdministratorMember(string memberId)
		{
			// remove this member
			this.AdminMembers.Remove(Query.EQ("_id", new ObjectId(memberId)));
		}
		#endregion

		#endregion

		#region Consumer Methods

		#region Get Consumer Member
		public ConsumerMember GetConsumerMember(string memberId)
		{
			// get the member
			var member = this.ConsumerMembers.FindAll().Where(item => item.Id == memberId).FirstOrDefault();

			return member;
		}
		#endregion

		#region Get Consumer Portfolio
		public ConsumerPortfolio GetConsumerPortfolio(string consumerPortfolioId)
		{
			// get the portfolio
			var portfolio = this.ConsumerPortfolios.FindAll().Where(item => item.Id == consumerPortfolioId).FirstOrDefault();

			return portfolio;
		}
		#endregion

		#region Get Consumer Member Portfolios
		public List<ConsumerPortfolio> GetConsumerMemberPortfolios(string consumerMemberId)
		{
			var portfolios = new List<ConsumerPortfolio>();

			// get the current member
			var member = this.ConsumerMembers.FindAll().Where(item => item.Id == consumerMemberId).FirstOrDefault();

			// find all consumer portfolios for this member
			var portfoliosAll = this.ConsumerPortfolios.FindAll().Where(item => member.PortfolioIds.Contains(item.Id)).ToList();

			if (portfoliosAll != null && portfoliosAll.Any())
			{
				// add the current one as the first
				portfolios.Add(portfoliosAll.FirstOrDefault(item => item.Id == member.PortfolioId));

				// add the rest
				portfolios.AddRange(portfoliosAll.Where(item => item.Id != member.PortfolioId));
			}

			return portfolios;
		}
		#endregion

		#region Get Consumer Portfolios
		public List<ConsumerPortfolio> GetConsumerPortfolios()
		{
			return this.ConsumerPortfolios.FindAll().ToList();;
		}
		#endregion

		#region Get Consumer Portfolio Members
		public List<ConsumerMember> GetConsumerPortfolioMembers(string consumerPortfolioId, string consumerMemberId = null)
		{
			// get all the members for this portfolio
			var members = this.ConsumerMembers.FindAll().Where(item => item.PortfolioIds.Contains(consumerPortfolioId)).ToList();

			// remove the consumer member (if specified)
			if (!string.IsNullOrEmpty(consumerMemberId))
				members.Remove(members.Where(item => item.Id == consumerMemberId).FirstOrDefault());

			return members;
		}
		#endregion

		#region Get Consumer Portfolio Creator
		public ConsumerMember GetConsumerPortfolioCreator(string consumerPortfolioId)
		{
			// get all the members for this portfolio
			var members = this.ConsumerMembers.FindAll().Where(item => item.PortfolioIds.Contains(consumerPortfolioId)).ToList();

			// return the creator
			return members.FirstOrDefault(item => item.AccountPrestige == Utility.AccountPrestige.Creator);
		}
		#endregion

		#region Delete Consumer Portfolio
		public List<ConsumerMember> DeleteConsumerPortfolio(string portfolioId)
		{
			// delete all entities related to this profile
			// 1) remove all responses 
			DeleteConsumerResponses(portfolioId);

			// 2) remove this portfolio
			this.ConsumerPortfolios.Remove(Query.EQ("_id", new ObjectId(portfolioId)));

			// 3) remove this portfolio from all consumer members and update their currently selected portfolio ID
			var members = GetConsumerPortfolioMembers(portfolioId);
			foreach (var member in members)
			{
				// remove this portfolio ID
				member.PortfolioIds.Remove(member.PortfolioIds.Where(item => item == portfolioId).FirstOrDefault());

				// select a new portfolio ID (for now, just select the most recent one by index)
				if (member.PortfolioId == portfolioId)
					member.PortfolioId = member.PortfolioIds.LastOrDefault();

				// save in the DB
				this.ConsumerMembers.Save(member);
			}

			return members;
		}
		#endregion

		#region Add Member Portfolio
		public ConsumerMember AddMemberPortfolio(string memberId, ConsumerPortfolio portfolio)
		{
			// get the member
			var member = GetConsumerMember(memberId);

			if (!member.PortfolioIds.Contains(portfolio.Id))
			{
				// add the portfolio
				member.PortfolioIds.Add(portfolio.Id);

				// set as the current portfolio
				member.PortfolioId = portfolio.Id;

				// update in the DB
				Update(member);
			}

			return member;
		}
		#endregion

		#endregion

		#region Provider Methods

		#region Get Provider Member
		public ProviderMember GetProviderMember(string memberId)
		{
			// get the member
			var member = this.ProviderMembers.FindAll().Where(item => item.Id == memberId).FirstOrDefault();

			return member;
		}
		#endregion

		#region Get Provider Portfolio
		public ProviderPortfolio GetProviderPortfolio(string providerPortfolioId)
		{
			// get the portfolio
			var portfolio = this.ProviderPortfolios.FindAll().Where(item => item.Id == providerPortfolioId).FirstOrDefault();

			return portfolio;
		}
		#endregion

		#region Get Provider Portfolios
		public List<ProviderPortfolio> GetProviderPortfolios(string providerMemberId)
		{
			// get the current member
			var member = this.ProviderMembers.FindAll().Where(item => item.Id == providerMemberId).FirstOrDefault();

			// find all portfolios for this member
			var portfolios = this.ProviderPortfolios.FindAll().Where(item => member.PortfolioIds.Contains(item.Id)).ToList();

			return portfolios;
		}
		#endregion

		#region Get Provider Portfolio Members
		public List<ProviderMember> GetProviderPortfolioMembers(string providerPortfolioId, string providerMemberId = null)
		{
			// get all the members for this portfolio
			var members = this.ProviderMembers.FindAll().Where(item => item.PortfolioIds.Contains(providerPortfolioId)).ToList();

			// remove the provider member (if specified)
			if (!string.IsNullOrEmpty(providerMemberId))
				members.Remove(members.Where(item => item.Id == providerMemberId).FirstOrDefault());

			return members;
		}
		#endregion

		#region Delete Provider Portfolio
		public List<ProviderMember> DeleteProviderPortfolio(string portfolioId)
		{
			// delete all entities related to this profile
			// 1) remove all responses 
			DeleteProviderResponses(portfolioId);

			// 2) remove this portfolio
			this.ProviderPortfolios.Remove(Query.EQ("_id", new ObjectId(portfolioId)));

			// 3) remove this portfolio from all consumer members and update their currently selected portfolio ID
			var members = GetProviderPortfolioMembers(portfolioId);
			foreach (var member in members)
			{
				// remove this portfolio ID
				member.PortfolioIds.Remove(member.PortfolioIds.Where(item => item == portfolioId).FirstOrDefault());

				// select a new portfolio ID (for now, just select the most recent one by index)
				if (member.PortfolioId == portfolioId)
					member.PortfolioId = member.PortfolioIds.LastOrDefault();

				// save in the DB
				this.ProviderMembers.Save(member);
			}

			return members;
		}
		#endregion

		#region Add Member Portfolio
		public ProviderMember AddMemberPortfolio(string memberId, ProviderPortfolio portfolio)
		{
			// get the member
			var member = GetProviderMember(memberId);

			if (!member.PortfolioIds.Contains(portfolio.Id)) 
			{
				// add the portfolio
				member.PortfolioIds.Add(portfolio.Id);

				// set as the current portfolio
				member.PortfolioId = portfolio.Id;

				// update in the DB
				Update(member);
			}

			return member;
		}
		#endregion

		#endregion

		#region Response Methods

		#region Get Response By Id
		public Response GetResponseById(string responseId)
		{
			return this.Responses.FindAll().Where(item => item.Id == responseId).FirstOrDefault();
		}
		#endregion

		#region Get Responses For Consumer Member
		public List<Response> GetResponsesForConsumerMember(string consumerMemberId)
		{
			// get the responses to the specified portfolio Ids for this member
			var portfolioIds = (GetConsumerMember(consumerMemberId)).PortfolioIds;
			var responses = this.Responses.FindAll().Where(item => portfolioIds.Contains(item.ConsumerPortfolioId)).ToList();
			
			return responses;
		}
		#endregion

		#region Get Response Schemes For Consumer Member
		public List<ConsumerResponseScheme> GetResponseSchemesForConsumerMember(string consumerMemberId)
		{
			// get the responses to the specified portfolio Ids for this member
			var schemes = new List<ConsumerResponseScheme>();
			var responses = GetResponsesForConsumerMember(consumerMemberId);
			var consumerPortfolios = GetConsumerMemberPortfolios(consumerMemberId);

			foreach (var response in responses)
			{
				var consumerPortfolio = consumerPortfolios.Where(item => item.Id == response.ConsumerPortfolioId).FirstOrDefault();
				var providerPortfolio = GetProviderPortfolio(response.ProviderPortfolioId);

				schemes.Add(new ConsumerResponseScheme(response, consumerPortfolio, providerPortfolio));
			}
			
			return schemes;
		}
		#endregion
		
		#region Get Responses For Consumer Portfolio
		public List<Response> GetResponsesForConsumerPortfolio(string consumerPortfolioId)
		{
			// get the responses to the specified portfolio
			var responses = this.Responses.FindAll().Where(item => item.ConsumerPortfolioId == consumerPortfolioId).ToList();

			return responses;
		}
		#endregion

		#region Get Responses For Provider Member
		public List<Response> GetResponsesForProviderMember(string providerMemberId)
		{
			// get the responses to the specified portfolio Ids for this member
			var portfolioIds = (GetProviderMember(providerMemberId)).PortfolioIds;
			var responses = this.Responses.FindAll().Where(item => portfolioIds.Contains(item.ProviderPortfolioId)).ToList();

			return responses;
		}
		#endregion

		#region Get Response Schemes For Provider Member
		public List<ProviderResponseScheme> GetResponseSchemesForProviderMember(string providerMemberId)
		{
			// get the responses to the specified portfolio Ids for this member
			var schemes = new List<ProviderResponseScheme>();
			var responses = GetResponsesForProviderMember(providerMemberId);
			var providerPortfolios = GetProviderPortfolios(providerMemberId);

			foreach (var response in responses)
			{
				var providerPortfolio = providerPortfolios.Where(item => item.Id == response.ProviderPortfolioId).FirstOrDefault();
				var consumerPortfolio = GetConsumerPortfolio(response.ConsumerPortfolioId);

				schemes.Add(new ProviderResponseScheme(response, providerPortfolio, consumerPortfolio));
			}

			return schemes;
		}
		#endregion

		#region Get Responses For Provider Portfolio
		public List<Response> GetResponsesForProviderPortfolio(string providerPortfolioId)
		{
			// get the responses to the specified portfolio
			var responses = this.Responses.FindAll().Where(item => item.ProviderPortfolioId == providerPortfolioId).ToList();

			return responses;
		}
		#endregion

		#region Generate Responses
		public List<Response> GenerateResponses(ConsumerPortfolio consumerPortfolio)
		{
			var responses = new List<Response>();

			// extract the longitude and latitude
			var longitude = consumerPortfolio.Preference.Proximity.Longitude.Value;
			var latitude = consumerPortfolio.Preference.Proximity.Latitude.Value;

			// find all providers with an active account status
			var query = Query.EQ("AccountStatus", AccountStatus.Active);

			// get all providers within the specified area and using the maximum distance as specified by the consumer
			var geoResults = this.ProviderPortfolios.GeoNear(query, longitude, latitude, UNLIMITED_RESULTS, CreateGeoNearOptions(consumerPortfolio.Preference.MaxDistance));
			
			// iterate over each geo result hit
			foreach (var hit in geoResults.Hits)
			{
				// get the provider portfolio
				var providerPortfolio = hit.Document;

				if (!ResponseExists(consumerPortfolio, providerPortfolio))
				{
					// generate a response for each consumer portfolio
					var response = new Response(providerPortfolio.Id, consumerPortfolio.Id, null);

					// set the calculated distance
					response.Distance = Math.Round(hit.Distance, 1);

					// save in the DB
					this.Responses.Save(response);

					// add this to the list of generated responses
					responses.Add(response);
				}
			}

			return responses;
		}

		public List<Response> GenerateResponses(ProviderPortfolio providerPortfolio)
		{
			var responses = new List<Response>();

			// extract the longitude and latitude
			var longitude = providerPortfolio.Principal.Address.Longitude.Value;
			var latitude = providerPortfolio.Principal.Address.Latitude.Value;

			// find all consumers with a pending request
			var query = Query.EQ("Request.State", RequestReceiptStates.Pending);

			// get all consumers within the specified area
			var geoResults = this.ConsumerPortfolios.GeoNear(query, longitude, latitude, UNLIMITED_RESULTS, CreateGeoNearOptions(MI_MAX_DISTANCE));

			// iterate over each geo result hit
			foreach (var hit in geoResults.Hits)
			{
				// get the consumer portfolio
				var consumerPortfolio = hit.Document;

				if (!ResponseExists(consumerPortfolio, providerPortfolio) && hit.Distance <= consumerPortfolio.Preference.MaxDistance)
				{
					// generate a response for each consumer portfolio
					var response = new Response(providerPortfolio.Id, consumerPortfolio.Id, null);

					// set the calculated distance
					response.Distance = Math.Round(hit.Distance, 1);

					// save in the DB
					this.Responses.Save(response);

					// add this to the list of generated responses
					responses.Add(response);
				}
			}

			return responses;
		}
		#endregion

		#region Get Pending Responses
		public List<Response> GetPendingResponses(ConsumerPortfolio consumerPortfolio)
		{
			var responses = GetResponsesForConsumerPortfolio(consumerPortfolio.Id);

			return responses.Where(item => item.State == ResponseReceiptStates.Pending).ToList();
		}
		#endregion

		#region Delete Response
		public void DeleteResponse(Response response)
		{
			this.Responses.Remove(Query.EQ("_id", new ObjectId(response.Id)));
		}
		#endregion

		#region Delete Responses
		public void DeleteConsumerResponses(string portfolioId)
		{
			var responses = GetResponsesForConsumerPortfolio(portfolioId);

			foreach (var response in responses)
				DeleteResponse(response);
		}

		public void DeleteProviderResponses(string portfolioId)
		{
			var responses = GetResponsesForProviderPortfolio(portfolioId);

			foreach (var response in responses)
				DeleteResponse(response);
		}
		#endregion

		#region Response Exists
		private bool ResponseExists(ConsumerPortfolio consumerPortfolio, ProviderPortfolio providerPortfolio)
		{
			return this.Responses.FindAll().Any(item => (item.ConsumerPortfolioId == consumerPortfolio.Id && item.ProviderPortfolioId == providerPortfolio.Id));
		}
		#endregion

		#endregion

		#region Update

		public void Update(ConsumerMember member)
		{
			this.ConsumerMembers.Save(member);
		}

		public void Update(ConsumerPortfolio portfolio)
		{
			this.ConsumerPortfolios.Save(portfolio);
		}

		public void Update(ProviderMember member)
		{
			this.ProviderMembers.Save(member);
		}

		public void Update(ProviderPortfolio portfolio)
		{
			this.ProviderPortfolios.Save(portfolio);
		}

		public void Update(Response response)
		{
			this.Responses.Save(response);
		}

		#endregion

		#region IDisposable
		public void Dispose()
		{

		}
		#endregion

		#region Create Geo Near Options
		private GeoNearOptionsBuilder CreateGeoNearOptions(double maxDistance)
		{
			var options = new GeoNearOptionsBuilder();

			// set the multiplier as miles
			options.SetDistanceMultiplier(MI_MULTIPLIER);

			// set the max distance
			options.SetMaxDistance(maxDistance / MI_MULTIPLIER);

			return options;
		}
		#endregion
	}
}

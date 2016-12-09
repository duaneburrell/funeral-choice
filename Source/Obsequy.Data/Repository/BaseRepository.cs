using MongoDB.Driver;
using Obsequy.Communication;
using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System;
using System.Linq;

namespace Obsequy.Data
{
	public class BaseRepository<T> : MongoRepository.MongoRepository<T> where T : MongoRepository.IEntity
	{
		#region Account Session
		public AccountSession AccountSession { get; set; }
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

		#region Switchboard
		public Switchboard Switchboard 
		{ 
			get; 
			set; 
		}
		#endregion

		#region Time Stamp
		private DateTime? _timeStamp;
		public DateTime TimeStamp
		{
			get 
			{
				if (_timeStamp == null)
					_timeStamp = DateTimeHelper.Now;
				return _timeStamp.Value;
			}
			set 
			{
				_timeStamp = value;
			}
		}
		#endregion

		#region Constructors

		public BaseRepository() : base() 
		{ 
		}
		
		public BaseRepository(MongoUrl url) : base(url) 
		{ 
		}
		
		public BaseRepository(string connectionString) 
			: base(connectionString) 
		{ 
		}

		#endregion
	}
}


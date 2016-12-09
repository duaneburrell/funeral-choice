using Obsequy.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;

namespace Obsequy.Model
{
	public class ChangeReceipt
	{
		#region Data Properties

		[BsonRepresentation(BsonType.ObjectId)]
		public string By { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime? On { get; set; }

		#endregion

		#region Properties

		#region Empty
		[BsonIgnore]
		public static ChangeReceipt Empty
		{
			get
			{
				return new ChangeReceipt()
				{
					By = string.Empty,
					On = DateTimeHelper.Empty
				};
			}
		}
		#endregion

		#endregion

        #region Computed Properties

        #region Is Empty
        [BsonIgnore]
        public bool IsEmpty
        {
            get
            {
                return (string.IsNullOrEmpty(this.By) && this.On == DateTimeHelper.Empty);
            }
        }
        #endregion

        #endregion

		#region Constructors

		public ChangeReceipt()
		{
            this.By = null;
            this.On = DateTimeHelper.Empty;
		}

		public ChangeReceipt(AccountSession accountSession)
		{
			this.By = (accountSession != null ? accountSession.MemberId : null);
			this.On = DateTimeHelper.Now;
		}
	
		#endregion

		#region Methods

		#region Clone
		public ChangeReceipt Clone()
		{
			return new ChangeReceipt()
			{
				By = this.By,
				On = this.On
			};
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0}: {1}", By, On);
		}
		#endregion

		#endregion
	}
}

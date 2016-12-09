using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Obsequy.Model
{
	[CollectionName(Definitions.Database.ResponseDocuments)]
	public class Response : IEntity
	{
		#region Data Properties

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string ProviderPortfolioId { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string ConsumerPortfolioId { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double? Distance { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double? Quote { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double? DepositDue { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double? DepositPaid { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double? BalanceDue { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double? BalancePaid { get; set; }

		public ResponseAgreement Agreement { get; set; }
		public ResponseAlternate Alternate { get; set; }
				
		public List<ResponseReceipt> Receipts { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public ResponseReceiptStates State { get; set; }

		#endregion

		#region Computed Properties

		#region Can Become Accepted
		[BsonIgnore]
		public bool CanBecomeAccepted
		{
			get
			{
				if (this.Current.IsPending)
					return true;
				return false;
			}
		}
		#endregion

		#region Can Become Available
		[BsonIgnore]
		public bool CanBecomeAvailable
		{
			get
			{
				if (this.Current.IsDismissed || this.Current.IsPending)
					return true;
				return false;
			}
		}
		#endregion

		#region Can Become Dismissed
		[BsonIgnore]
		public bool CanBecomeDismissed
		{
			get
			{
				if (this.Current.IsAvailable)
					return true;
				return false;
			}
		}
		#endregion

		#region Can Become Pending
		[BsonIgnore]
		public bool CanBecomePending
		{
			get
			{
				if (this.Current.IsAvailable)
					return true;
				return false;
			}
		}
		#endregion

		#region Can Become Rejected
		[BsonIgnore]
		public bool CanBecomeRejected
		{
			get
			{
				if (this.Current.IsPending)
					return true;
				return false;
			}
		}
		#endregion

		#region Created
		[BsonIgnore]
		public ResponseReceipt Created
		{
			get
			{
				// find the first receipt created
				if (this.Receipts.Any())
					return this.Receipts.OrderBy(item => item.On).ElementAt(0);

				return null;
			}
		}
		#endregion
		
		#region Current
		[BsonIgnore]
		public ResponseReceipt Current
		{
			get
			{
				// find the receipt created before the current one
				if (this.Receipts.Any())
					return this.Receipts.OrderByDescending(item => item.On).ElementAt(0);

				return null;
			}
		}
		#endregion

		#region Pending
		[BsonIgnore]
		public ResponseReceipt Pending
		{
			get
			{
				// find the most recent receipt with a pending state
				var receipts = this.Receipts.Where(r => r.State == ResponseReceiptStates.Pending).ToList();
				if (receipts.Any())
					return receipts.OrderByDescending(r => r.On).FirstOrDefault();

				return null;
			}
		}
		#endregion

		#endregion

		#region Constructors

		public Response()
			: base()
		{
			this.Agreement = new ResponseAgreement();
			this.Alternate = new ResponseAlternate();
			this.Receipts = new List<ResponseReceipt>();
		}

		public Response(string providerPortfolioId, string consumerPortfolioId, AccountSession accountSession)
			: base()
		{
			this.ProviderPortfolioId = providerPortfolioId;
			this.ConsumerPortfolioId = consumerPortfolioId;

			this.Agreement = new ResponseAgreement();
			this.Alternate = new ResponseAlternate();

			this.Distance = null;

			// create the initial receipt and the overall state of this response
			var receipt = new ResponseReceipt(accountSession);

			this.Receipts = new List<ResponseReceipt>();
			this.Receipts.Add(receipt);

			this.State = receipt.State;
		}

		#endregion

		#region Methods

		#region Clear
		public void Clear()
		{
			this.Quote = null;
			this.DepositDue = null;
			this.DepositPaid = null;
			this.BalanceDue = null;
			this.BalancePaid = null;

			this.Agreement.Clear();
			this.Alternate.Clear();
		}
		#endregion

		#region Set Receipt State
		public void SetReceiptState(ResponseReceiptStates state, AccountSession accountSession, double? value = null)
		{
			// push a new receipt onto the list
			this.Receipts.Add(new ResponseReceipt()
			{
				State = state,
				On = DateTimeHelper.Now,
				By = accountSession.MemberId,
				Value = QuoteHelper.NormalizeValue(value)
			});

			// set the current state
			this.State = state;

			if (state == ResponseReceiptStates.Pending)
			{
				// initialize the quote details based on the value submitted by the provider
				this.Quote = QuoteHelper.NormalizeValue(value);
				this.DepositDue = QuoteHelper.ComputeDepositeDue(value);
				this.DepositPaid = 0;
				this.BalanceDue = QuoteHelper.ComputeBalanceDue(value);
				this.BalancePaid = 0;
			}

			if (state == ResponseReceiptStates.Accepted)
			{
				// update the deposit paid based on the value paid by the consumer
				this.DepositDue = 0;
				this.DepositPaid = QuoteHelper.NormalizeValue(value);
				this.BalanceDue = QuoteHelper.ComputeBalanceDue(this.Quote, value);
				this.BalancePaid = 0;
			}
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0}: {1}", State, Distance);
		}
		#endregion

		#region Update
		public void Update(ProviderResponseScheme response, AccountSession accountSession)
		{
			if (this.Agreement.HasChanged(response.Agreement)) 
			{
				this.Agreement.Update(response.Agreement);
				this.Agreement.Receipt = new ChangeReceipt(accountSession);
			}

			if (this.Alternate.HasChanged(response.Alternate))
			{
				this.Alternate.Update(response.Alternate);
				this.Alternate.Receipt = new ChangeReceipt(accountSession);
			}
		}
		#endregion

		#region Was Submitted Before Checkpoint
		public bool WasSubmittedBeforeCheckpoint(DateTime? checkpoint)
		{
			if (this.Current.IsPending)
			{
				// was my bid my submtted date before the checkpoint?
				return (this.Current.On < checkpoint);
			}

			return false;
		}
		#endregion

		#endregion
	}
}
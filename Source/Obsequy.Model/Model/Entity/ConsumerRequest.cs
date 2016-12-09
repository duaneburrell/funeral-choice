using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class ConsumerRequest
	{
		#region Data Properties

		public List<RequestReceipt> Receipts { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public RequestReceiptStates State { get; set; }

		#endregion

		#region Properties

		#region Current
		[BsonIgnore]
		public RequestReceipt Current
		{
			get
			{
				// find the receipt created before the current one
				if (this.Receipts.Count() >= 1)
					return this.Receipts.OrderByDescending(r => r.On).ElementAt(0);

				return null;
			}
		}
		#endregion

		#region Is Accept Allowed
		[BsonIgnore]
		[JsonIgnore]
		public bool IsAcceptAllowed
		{
			get 
			{ 
				if (this.Current != null)
					return this.Current.IsPending;
				return false;
			}
		}
		#endregion

		#region Is Reject Allowed
		[BsonIgnore]
		[JsonIgnore]
		public bool IsRejectAllowed
		{
			get 
			{ 
				if (this.Current != null)
					return this.Current.IsPending;
				return false;
			}
		}
		#endregion

		#region Mnemonic
		[BsonIgnore]
		public string Mnemonic
		{
			get { return this.State.ToString(); }
		}
		#endregion

		#endregion

		#region Constructors

		public ConsumerRequest()
			: base()
		{
			this.Receipts = new List<RequestReceipt>();
		}

		public ConsumerRequest(AccountSession accountSession)
			: base()
		{
			this.Receipts = new List<RequestReceipt>();

			SetReceiptState(RequestReceiptStates.Draft, accountSession);
		}

		#endregion

		#region Methods

		#region Find Auto Reject Response Candidates
		public List<Response> FindAutoRejectResponseCandidates(object providerGroupId)
		{
			return null;
			//return this.Responses.Where(r => (r.Current.IsPending || r.Current.IsReConfirm) && r.ProviderPortfolioId != providerGroupId.ToString()).ToList();
		}
		#endregion
		
		#region Find Provider Response
		public Response FindProviderResponse(ProviderPortfolio providerGroup)
		{
			return FindProviderResponse(providerGroup.Id);
		}

		public Response FindProviderResponse(object providerGroupId)
		{
			return null;
			//return this.Responses.Where(r => r.ProviderPortfolioId == providerGroupId.ToString()).FirstOrDefault();
		}
		#endregion

		#region Is History Update Needed
		public bool IsHistoryUpdateNeeded(DateTime? checkpoint)
		{
			if (this.Current.IsPending)
			{
				// have any bids been submitted since the last change?
				var responses = ResponsesSubmittedBeforeCheckpoint(checkpoint);

				return responses.Any();
			}

			return false;
		}
		#endregion

		#region Responses Submitted Before Checkpoint
		public List<Response> ResponsesSubmittedBeforeCheckpoint(DateTime? checkpoint)
		{
			return null;
			//return this.Responses.Where(r => r.WasSubmittedBeforeCheckpoint(checkpoint)).ToList();
		}
		#endregion

		#region Set Receipt State
		public void SetReceiptState(RequestReceiptStates state, AccountSession accountSession)
		{
			// push a new receipt onto the list
			this.Receipts.Add(new RequestReceipt()
			{
				State = state,
				On = DateTimeHelper.Now,
				By = accountSession.MemberId
			});

			// set the overall state of this request
			this.State = state;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0}", State);
		}
		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ConsumerRequestValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}
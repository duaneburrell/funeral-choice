using Obsequy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Braintree;
using Obsequy.Utility;

namespace Obsequy.Security
{
	public class PaymentProcessor
	{
		#region Properties

		#region Account Session
		public AccountSession AccountSession
		{
			get;
			set;
		}
		#endregion

		#region Gateway
		private BraintreeGateway _gateway;
		public BraintreeGateway Gateway
		{
			get
			{
				if (_gateway == null)
				{
					// create a new Braintree gateway
					_gateway = new BraintreeGateway()
					{
						Environment = BraintreeConfiguration.Environment,
						MerchantId  = BraintreeConfiguration.MerchantKey,
						PublicKey = BraintreeConfiguration.PublicKey,
						PrivateKey = BraintreeConfiguration.PrivateKey
					};
				}

				return _gateway;
			}
		}
		#endregion

		#region Logger
		public NLog.Logger Logger
		{
			get;
			set;
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

		#region Construtor
		public PaymentProcessor()
		{
		}
		#endregion

		#region Process Payment Async
		public Task<Braintree.Result<Transaction>> ProcessPaymentAsync(Response response, Payment payment)
		{
			return Task<Braintree.Result<Transaction>>.Factory.StartNew(() =>
			{
				// get the consumer member
				var consumerMember = this.Mongo.GetConsumerMember(this.AccountSession.MemberId);

				// get the provider portfolio
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// create a transaction request
				var request = new TransactionRequest
				{
					Amount = Convert.ToDecimal(response.DepositDue),
					CreditCard = new TransactionCreditCardRequest
					{
						CardholderName = payment.CardholderName,
						Number = payment.CardNumber,
						CVV = payment.SecurityCode,
						ExpirationMonth = payment.ExpirationMonth,
						ExpirationYear = payment.ExpirationYear
					},
					BillingAddress = new AddressRequest
					{
						PostalCode = payment.PostalCode,
					},
					CustomFields = new Dictionary<string, string>
					{
						{ "rid", response.Id },
						{ "cmid", consumerMember.Id },
						{ "cmn", consumerMember.FullName },
						{ "cpid", response.ConsumerPortfolioId },
						{ "ppid", response.ProviderPortfolioId },
						{ "ppn", providerPortfolio.Principal.Name }
					},
					Options = new TransactionOptionsRequest
					{
						SubmitForSettlement = true
					}
				};

				// process payment
				var result = this.Gateway.Transaction.Sale(request);

				// return the result
				return result;
			});
		}
		#endregion

		#region Hold Payment Async
		public Task<Braintree.Result<Transaction>> HoldPaymentAsync(Response response, Payment payment)
		{
			return Task<Braintree.Result<Transaction>>.Factory.StartNew(() =>
			{
				// get the consumer member
				var consumerMember = this.Mongo.GetConsumerMember(this.AccountSession.MemberId);

				// get the provider portfolio
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// create a transaction request
				var request = new TransactionRequest
				{
					Amount = Convert.ToDecimal(response.DepositDue),
					CreditCard = new TransactionCreditCardRequest
					{
						CardholderName = payment.CardholderName,
						Number = payment.CardNumber,
						CVV = payment.SecurityCode,
						ExpirationMonth = payment.ExpirationMonth,
						ExpirationYear = payment.ExpirationYear
					},
					BillingAddress = new AddressRequest
					{
						PostalCode = payment.PostalCode,
					},
					CustomFields = new Dictionary<string, string>
					{
						{ "rid", response.Id },
						{ "cmid", consumerMember.Id },
						{ "cmn", consumerMember.FullName },
						{ "cpid", response.ConsumerPortfolioId },
						{ "ppid", response.ProviderPortfolioId },
						{ "ppn", providerPortfolio.Principal.Name }
					},
					Options = new TransactionOptionsRequest
					{
						SubmitForSettlement = true
					}
				};

				// process payment
				var result = this.Gateway.Transaction.Sale(request);

				// return the result
				return result;
			});
		}
		#endregion

		#region Charge Payment Async
		public Task<Braintree.Result<Transaction>> ChargePaymentAsync(Response response, Payment payment)
		{
			return Task<Braintree.Result<Transaction>>.Factory.StartNew(() =>
			{
				// get the consumer member
				var consumerMember = this.Mongo.GetConsumerMember(this.AccountSession.MemberId);

				// get the provider portfolio
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// create a transaction request
				var request = new TransactionRequest
				{
					Amount = Convert.ToDecimal(response.DepositDue),
					CreditCard = new TransactionCreditCardRequest
					{
						CardholderName = payment.CardholderName,
						Number = payment.CardNumber,
						CVV = payment.SecurityCode,
						ExpirationMonth = payment.ExpirationMonth,
						ExpirationYear = payment.ExpirationYear
					},
					BillingAddress = new AddressRequest
					{
						PostalCode = payment.PostalCode,
					},
					CustomFields = new Dictionary<string, string>
					{
						{ "rid", response.Id },
						{ "cmid", consumerMember.Id },
						{ "cmn", consumerMember.FullName },
						{ "cpid", response.ConsumerPortfolioId },
						{ "ppid", response.ProviderPortfolioId },
						{ "ppn", providerPortfolio.Principal.Name }
					},
					Options = new TransactionOptionsRequest
					{
						SubmitForSettlement = true
					}
				};

				// process payment
				var result = this.Gateway.Transaction.Sale(request);

				// return the result
				return result;
			});
		}
		#endregion

		#region Cancel Payment Async
		public Task<Braintree.Result<Transaction>> CancelPaymentAsync(Response response, Payment payment)
		{
			return Task<Braintree.Result<Transaction>>.Factory.StartNew(() =>
			{
				// get the consumer member
				var consumerMember = this.Mongo.GetConsumerMember(this.AccountSession.MemberId);

				// get the provider portfolio
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// create a transaction request
				var request = new TransactionRequest
				{
					Amount = Convert.ToDecimal(response.DepositDue),
					CreditCard = new TransactionCreditCardRequest
					{
						CardholderName = payment.CardholderName,
						Number = payment.CardNumber,
						CVV = payment.SecurityCode,
						ExpirationMonth = payment.ExpirationMonth,
						ExpirationYear = payment.ExpirationYear
					},
					BillingAddress = new AddressRequest
					{
						PostalCode = payment.PostalCode,
					},
					CustomFields = new Dictionary<string, string>
					{
						{ "rid", response.Id },
						{ "cmid", consumerMember.Id },
						{ "cmn", consumerMember.FullName },
						{ "cpid", response.ConsumerPortfolioId },
						{ "ppid", response.ProviderPortfolioId },
						{ "ppn", providerPortfolio.Principal.Name }
					},
					Options = new TransactionOptionsRequest
					{
						SubmitForSettlement = true
					}
				};

				// process payment
				var result = this.Gateway.Transaction.Sale(request);

				// return the result
				return result;
			});
		}
		#endregion

		#region Create Payment Transaction
		public Payment CreatePaymentTransaction(Braintree.Transaction transaction)
		{
			var payment = new Payment()
			{
				CardholderName = transaction.CreditCard.CardholderName,
				CardLastFour = transaction.CreditCard.LastFour,
				CardType = Convert.ToString(transaction.CreditCard.CardType),
				ExpirationMonth = transaction.CreditCard.ExpirationMonth,
				ExpirationYear = transaction.CreditCard.ExpirationYear,
				PostalCode = transaction.CreditCard.BillingAddress.PostalCode,
				Amount = Convert.ToDouble(QuoteHelper.NormalizeValue(transaction.Amount)),
				TransactionId = transaction.Id,
				MerchantAccountId = transaction.MerchantAccountId,
				ProcessorAuthorizationCode = transaction.ProcessorAuthorizationCode
			};

			return payment;
		}
		#endregion

		#region Process Test Payment Async
		public Task<Braintree.Result<Transaction>> ProcessTestPaymentAsync(Payment payment)
		{
			return Task<Braintree.Result<Transaction>>.Factory.StartNew(() =>
			{
				// make sure the amount is less than 10 cents
				var amount = Convert.ToDecimal(payment.Amount >= 0.0 && payment.Amount <= 0.10 ? payment.Amount : 0.01);

				// get the administrator account member
				var member = this.Mongo.GetAdministratorMember(this.AccountSession.MemberId);

				// create a transaction request
				var request = new TransactionRequest
				{
					Amount = amount,
					CreditCard = new TransactionCreditCardRequest
					{
						CardholderName = payment.CardholderName,
						Number = payment.CardNumber,
						CVV = payment.SecurityCode,
						ExpirationMonth = payment.ExpirationMonth,
						ExpirationYear = payment.ExpirationYear
					},
					BillingAddress = new AddressRequest
					{
						PostalCode = payment.PostalCode,
					},
					CustomFields = new Dictionary<string, string>
					{
						{ "rid", "test-rid" },
						{ "cmid", member.Id },
						{ "cmn",  member.FullName },
						{ "cpid", "test-cpid" },
						{ "ppid", "test-ppid" },
						{ "ppn", "test-ppn" }
					},
					Options = new TransactionOptionsRequest
					{
						SubmitForSettlement = true
					}
				};

				// process payment
				var result = this.Gateway.Transaction.Sale(request);

				// return the result
				return result;
			});
		}
		#endregion
	}
}

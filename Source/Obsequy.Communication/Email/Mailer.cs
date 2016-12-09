using Obsequy.Communication.Email.Templates;
using Obsequy.Model;
using Obsequy.Utility;
using SendGrid;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Obsequy.Communication
{
    public class Mailer
    {
        #region Properties

		#region Domain
		public string Domain
		{
			get
			{
				var url = HttpContext.Current.Request.Url;
				return string.Format("{0}{1}{2}", url.Scheme, System.Uri.SchemeDelimiter, Host);
			}
		}
		#endregion

        #region From
        public MailAddress From
        {
            get 
            {
                var host = HttpContext.Current.Request.Url.Host.ToLower();
                var isDev = host.Equals("localhost");
                var isDemo = host.Contains("-demo");

                var from = "Funeral-Choice";
                if (isDev)
                    from = string.Format("{0}-Dev", from);
                if (isDemo)
                    from = string.Format("{0}-Demo", from);

                return new MailAddress(string.Format("noreply@{0}", this.Host), from); 
            }
        }
        #endregion

        #region Host
        public string Host
		{
			get
			{
				var url = HttpContext.Current.Request.Url;
				var host = url.Host;
				var port = (url.IsDefaultPort || host.Equals("localhost") ? string.Empty : string.Format(":{0}", url.Port));
				return string.Format("{0}{1}", host, port);
			}
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

        #region Construction / Initialization

        public Mailer()
        {
        }

        #endregion

        #region Consumer Methods

        public void ConsumerMemberCreated(ConsumerMember member)
        {
        }

        public void ConsumerPortfolioReminded(ConsumerPortfolio consumerPortfolio)
        {
            // get consumer members for this portfolio
            var members = this.Mongo.GetConsumerPortfolioMembers(consumerPortfolio.Id);

            // get the raw HTML template
            var html = MergeTemplates("Layout", "Consumer/Reminder");

            foreach (var member in members)
            {
                // replace content
                html = html.Replace("[Member]", member.FullName);
                html = html.Replace("[Domain]", Domain);

                // deliver the message
                DeliverMessage(member.Email, "Ready to Submit?", html);
            }
        }

        public void ConsumerRequestSetToPending(ConsumerPortfolio consumerPortfolio)
        {
			// get administrator members for consumer registrations
			var members = this.Mongo.GetAdministratorMembers().Where(item => item.IsNotifiedOnConsumerRegistrations);

			// get the raw HTML template
			var html = MergeTemplates("Layout", "Administrator/ConsumerRegistration");

			// replace content
			html = html.Replace("[City]", consumerPortfolio.Preference.Proximity.City);
			html = html.Replace("[State]", consumerPortfolio.Preference.Proximity.State);
			html = html.Replace("[Id]", consumerPortfolio.Id);
			html = html.Replace("[Modified]", DateTimeHelper.ToLocalTime(consumerPortfolio.Modified.On).ToString());

			foreach (var member in members)
			{
				// replace content
				html = html.Replace("[Member]", member.FullName);
                html = html.Replace("[Domain]", Domain);

				// deliver the message
				DeliverMessage(member.Email, "Consumer Account Registered", html);
			}
        }

        #endregion

        #region Provider Methods

        public void ProviderMemberCreated(ProviderMember member)
        {
        }

        public void ProviderPortfolioCreated(ProviderPortfolio providerPortfolio)
        {
        }

        public void ProviderPortfolioAccountStatusChanged(ProviderPortfolio providerPortfolio)
        {
			if (providerPortfolio.AccountStatus == AccountStatus.Pending)
			{
				// get administrator members for provider registrations
				var members = this.Mongo.GetAdministratorMembers().Where(item => item.IsNotifiedOnProviderRegistrations);

				// get the raw HTML template
				var html = MergeTemplates("Layout", "Administrator/ProviderRegistration");

				// replace content
				html = html.Replace("[Name]", providerPortfolio.Principal.Name);
				html = html.Replace("[Phone]", providerPortfolio.Principal.Phone);
				html = html.Replace("[Email]", providerPortfolio.Principal.Email);
				html = html.Replace("[Id]", providerPortfolio.Id);
				html = html.Replace("[Modified]", DateTimeHelper.ToLocalTime(providerPortfolio.Modified.On).ToString());

				foreach (var member in members)
				{
					// replace content
					html = html.Replace("[Member]", member.FullName);

					// deliver the message
					DeliverMessage(member.Email, "Provider Account Registered", html);
				}
			}

			if (providerPortfolio.AccountStatus == AccountStatus.Active)
			{
				// get provider members 
				var members = this.Mongo.GetProviderPortfolioMembers(providerPortfolio.Id);

				// get the raw HTML template
				var html = MergeTemplates("Layout", "Provider/ProviderAccountActivated");

				// replace content
				html = html.Replace("[ProviderName]", providerPortfolio.Principal.Name);

				foreach (var member in members)
				{
					// replace content
					html = html.Replace("[Member]", member.FullName);

					// deliver the message
					DeliverMessage(member.Email, "Account Activated", html);
				}
			}

			if (providerPortfolio.AccountStatus == AccountStatus.Rejected)
			{
				// get provider members 
				var members = this.Mongo.GetProviderPortfolioMembers(providerPortfolio.Id);

				// get the raw HTML template
				var html = MergeTemplates("Layout", "Provider/ProviderAccountRejected");

				// replace content
				html = html.Replace("[ProviderName]", providerPortfolio.Principal.Name);

				foreach (var member in members)
				{
					// replace content
					html = html.Replace("[Member]", member.FullName);

					// deliver the message
					DeliverMessage(member.Email, "Account Rejected", html);
				}
			}
		}
        #endregion

        #region Response Methods

        #region Response Updated As Available
        public void ResponseUpdatedAsAvailable(Response response, ConsumerPortfolio consumerPortfolio, ProviderPortfolio providerPortfolio)
        {
            // get the recipients
            var members = this.Mongo.GetProviderPortfolioMembers(providerPortfolio.Id);

            // get the raw HTML template
            var html = MergeTemplates("Layout", "Provider/ResponseAvailable");

            foreach (var member in members)
            {
                // replace content
                html = html.Replace("[Name]", providerPortfolio.Principal.Name);
                html = html.Replace("[Domain]", Domain);

                // deliver the message
                DeliverMessage(member.Email, "New Available Opportunity", html);
            }
        }
        #endregion

        #region Response Updated As Dismissed
        public void ResponseUpdatedAsDismissed(Response response)
        {
        }
        #endregion

        #region Response Updated As Expired
        public void ResponseUpdatedAsExpired(Response response)
        {
        }
        #endregion

        #region Response Updated As Pending
        public void ResponseUpdatedAsPending(Response response, ConsumerPortfolio consumerPortfolio, ProviderPortfolio providerPortfolio)
        {
            var uri = HttpContext.Current.Request.Url;
            // get the recipients
            var members = this.Mongo.GetConsumerPortfolioMembers(consumerPortfolio.Id);

            // get the raw HTML template
            var html = MergeTemplates("Layout", "Consumer/ResponsePending");

            foreach (var member in members)
            {
                // replace content
                html = html.Replace("[FirstName]", member.FirstName);
                html = html.Replace("[Domain]", Domain);

                // deliver the message
                DeliverMessage(member.Email, "Quote for Funeral Services", html);
            }
        }
        #endregion

        #region Response Updated As User Accepted
        public void ResponseUpdatedAsUserAccepted(Response response, ConsumerPortfolio consumerPortfolio, ProviderPortfolio providerPortfolio)
        {
            // get the recipients
            var members = this.Mongo.GetProviderPortfolioMembers(providerPortfolio.Id);

            // get the raw HTML template
            var html = MergeTemplates("Layout", "Provider/ResponseAccepted");

            foreach (var member in members)
            {
                // replace content
                html = html.Replace("[Name]", providerPortfolio.Principal.Name);
                html = html.Replace("[Domain]", Domain);

                // deliver the message
                DeliverMessage(member.Email, "Quote Accepted", html);
            }



			// get administrator members for accepted responses
			var admins = this.Mongo.GetAdministratorMembers().Where(item => item.IsNotifiedOnAcceptedResponses);

			// get the raw HTML template
			html = MergeTemplates("Layout", "Administrator/AcceptedResponse");

			// replace content
			html = html.Replace("[City]", consumerPortfolio.Preference.Proximity.City);
			html = html.Replace("[State]", consumerPortfolio.Preference.Proximity.State);
			html = html.Replace("[Quote]", Convert.ToString(response.Quote));
			html = html.Replace("[BalancePaid]", Convert.ToString(response.BalancePaid));
			html = html.Replace("[BalanceDue]", Convert.ToString(response.BalanceDue));
			html = html.Replace("[Id]", response.Id);
			html = html.Replace("[Modified]", DateTimeHelper.ToLocalTime(response.Current.On).ToString());

			foreach (var member in admins)
			{
				// replace content
				html = html.Replace("[Member]", member.FullName);

				// deliver the message
				DeliverMessage(member.Email, "Provider Response Accepted", html);
			}
        }
        #endregion

        #region Response Updated As Auto Rejected
        public void ResponseUpdatedAsAutoRejected(Response response, ConsumerPortfolio consumerPortfolio, ProviderPortfolio providerPortfolio)
        {
            // get the recipients
            var members = this.Mongo.GetProviderPortfolioMembers(providerPortfolio.Id);

            // get the raw HTML template
            var html = MergeTemplates("Layout", "Provider/ResponseAutoRejected");

            foreach (var member in members)
            {
                // replace content
                html = html.Replace("[Name]", providerPortfolio.Principal.Name);

                // deliver the message
                DeliverMessage(member.Email, "Services Quote Rejected", html);
            }
        }
        #endregion

        #region Response Updated As User Rejected
        public void ResponseUpdatedAsUserRejected(Response response, ConsumerPortfolio consumerPortfolio, ProviderPortfolio providerPortfolio)
        {
            // get the recipients
            var members = this.Mongo.GetProviderPortfolioMembers(providerPortfolio.Id);

            // get the raw HTML template
            var html = MergeTemplates("Layout", "Provider/ResponseUserRejected");

            foreach (var member in members)
            {
                // replace content
                html = html.Replace("[Name]", providerPortfolio.Principal.Name);

                // deliver the message
                DeliverMessage(member.Email, "Services Quote Rejected", html);
            }
        }
        #endregion

        #endregion

        #region Mail Methods

        public void DeliverMessage(string email, string subject, string content)
        {
            // create the message object to send
            var message = new MailMessage();

            message.To.Add(email);
            message.From = this.From;
            message.Subject = subject;
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html));

            // create the SMTP object
            var smtp = new SmtpClient("smtp.sendgrid.net", 587)
            {
                Credentials = new NetworkCredential("azure_5e96253f1a59d8ca1913e28d003302f2@azure.com", "jakqsxfr")
            };

			smtp.Send(message);
        }

        #region Layout Methods

        private string MergeTemplates(string layoutName, string templateName)
        {
            var type = templateName.Split('/')[0];
            var layout = TemplateResources.Layout;
            var template = string.Format("<b>Could not find template: {0}</b>", templateName);

			if (type == "Administrator")
				template = FindTemplate(templateName.Split('/')[1]);
            if (type == "Consumer")
                template = FindTemplate(templateName.Split('/')[1]);
            if (type == "Provider")
                template = FindTemplate(templateName.Split('/')[1]);

            return layout.Replace("[MESSAGE_CONTENT]", template);
        }

        private string FindTemplate(string templateName)
        {
            switch (templateName)
            {
				// administrator templates
				case "ProviderRegistration": return TemplateResources.ProviderRegistration;
				case "ConsumerRegistration": return TemplateResources.ConsumerRegistration;
				case "AcceptedResponse": return TemplateResources.AcceptedResponse;

				// consumer templates
                case "ResponsePending": return TemplateResources.ResponsePending;
                case "Reminder": return TemplateResources.Reminder;

				// provider templates
				case "ProviderAccountActivated": return TemplateResources.ProviderAccountActivated;
				case "ProviderAccountRejected": return TemplateResources.ProviderAccountRejected;
				case "ResponseAccepted": return TemplateResources.ResponseAccepted;
                case "ResponseAutoRejected": return TemplateResources.ResponseAutoRejected;
                case "ResponseAvailable": return TemplateResources.ResponseAvailable;
                case "ResponseUserRejected": return TemplateResources.ResponseUserRejected;
            }

            throw new KeyNotFoundException(string.Format("Email Exception: Could not find resource for consumer template {0}. Make sure the template name is correct and it is in the ConsumerResources.resx file", templateName));
        }
        #endregion

        #endregion
	}
}

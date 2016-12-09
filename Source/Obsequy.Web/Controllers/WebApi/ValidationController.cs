using Obsequy.Data.Contracts;
using Obsequy.Model;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Obsequy.Web
{
    public class ValidationController : BaseApiController
    {
		#region Constuction
		public ValidationController(IObsequyUow uow)
			: base(uow)
		{
		}
		#endregion

		#region Password
		[AllowAnonymous]
		[HttpPut]
		[ActionName("password")]
		public HttpResponseMessage ValidatePassword(PropertyDto dto)
		{
			var propertyValidation = dto.ValidatePassword(this.AccountSession, dto.ValidationMode);

			if (propertyValidation.IsValid)
			{
				return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(propertyValidation);
		}
		#endregion

		#region Email
		[AllowAnonymous]
		[HttpPut]
		[ActionName("email")]
		public HttpResponseMessage ValidateEmail(PropertyDto dto)
		{
			var propertyValidation = dto.ValidateEmail(this.AccountSession, dto.ValidationMode);

			if (propertyValidation.IsValid)
			{
				return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(propertyValidation);
		}
		#endregion
	}
}

using System.Collections.Generic;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ConsumerPortfolioComparer : IComparer<ConsumerPortfolio>
	{
		public int Compare(ConsumerPortfolio left, ConsumerPortfolio right)
		{
			if (left.Request.State == right.Request.State)
				return 0;

			if (left.Request.State == RequestReceiptStates.Draft)
				return -1;

			if (left.Request.State == RequestReceiptStates.Pending)
				return (right.Request.State == RequestReceiptStates.Draft ? 1 : -1);

			if (left.Request.State == RequestReceiptStates.Completed)
				return 1;

			return 0;
		}
	}
}

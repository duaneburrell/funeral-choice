using System;

namespace Obsequy.Utility
{
    public static class QuoteHelper
    {
		private static int MIN_DEPOSIT_PENNIES = 30000; // $300.00
		private static int MAX_DEPOSIT_PENNIES = 50000; // $500.00

		public static double? NormalizeValue(decimal? value)
		{
			return NormalizeValue((double?)value);
		}

		public static double? NormalizeValue(double? value)
		{
			if (value != null)
			{
				var intValue = Convert.ToInt32((int)(value * 100.0));
				return Convert.ToDouble((double)intValue / 100.0);
			}
			return null;
		}

		public static double? ComputeDepositeDue(double? quote)
		{
			if (quote != null)
			{
				var intQuote = Convert.ToInt32((int)(quote * 100.0));
				var intPercent = Convert.ToInt32(intQuote / 10);
				var intDeposit = Math.Max(Math.Min(MAX_DEPOSIT_PENNIES, intPercent), MIN_DEPOSIT_PENNIES);
				
				return Convert.ToDouble((double)intDeposit / 100.0);
			}
			return 0.0;
		}

		public static double? ComputeBalanceDue(double? quote)
		{
			if (quote != null)
			{
				var intQuote = Convert.ToInt32((int)(quote * 100.0));
				var intPercent = Convert.ToInt32(intQuote / 10);
				var intDeposit = Math.Max(Math.Min(MAX_DEPOSIT_PENNIES, intPercent), MIN_DEPOSIT_PENNIES);
				var intBalance = (intQuote - intDeposit);
				
				return Convert.ToDouble((double)intBalance / 100.0); 
			}
			return 0.0;
		}

		public static double? ComputeBalanceDue(double? quote, double? paid)
		{
			if (quote != null && paid != null)
			{
				var intQuote = Convert.ToInt32((int)(quote * 100.0));
				var intPaid = Convert.ToInt32((int)(paid * 100.0));
				var intBalance = (intQuote - intPaid);

				return Convert.ToDouble((double)intBalance / 100.0);
			}
			return 0.0;
		}
    }
}

using System;
using Obsequy.Utility;

namespace Obsequy.Security
{
	public class BraintreeConfiguration
	{
		public static ApplicationEnvironments ApplicationEnvironment
		{
			get
			{
				//return ApplicationEnvironments.Demonstration;
				return ApplicationEnvironments.Development;
				//return ApplicationEnvironments.Production; 
			}
		}

		public static Braintree.Environment Environment
		{
			get
			{
				if (ApplicationEnvironment == ApplicationEnvironments.Production)
					return Braintree.Environment.PRODUCTION;

				return Braintree.Environment.SANDBOX;
			}
		}

		public static string MerchantKey
		{
			get
			{
				if (ApplicationEnvironment == ApplicationEnvironments.Demonstration)
					return "XXXXXX";

				if (ApplicationEnvironment == ApplicationEnvironments.Development)
                    return "hdxfnt624p4zzc3g";

				if (ApplicationEnvironment == ApplicationEnvironments.Production)
                    return "c3zskhhyzdxpsyvc";

				return string.Empty;
			}
		}

		public static string PublicKey
		{
			get
			{
				if (ApplicationEnvironment == ApplicationEnvironments.Demonstration)
					return "XXXXXX";

				if (ApplicationEnvironment == ApplicationEnvironments.Development)
					return "y5y7pq8m2gzfwcwt";

				if (ApplicationEnvironment == ApplicationEnvironments.Production)
                    return "4d569pwf62c84tbp";

				return string.Empty;
			}
		}

		public static string PrivateKey
		{
			get
			{
				if (ApplicationEnvironment == ApplicationEnvironments.Demonstration)
					return "XXXXXX";

				if (ApplicationEnvironment == ApplicationEnvironments.Development)
					return "6eb2f25914e7fc9720313f0a07cd6332";

				if (ApplicationEnvironment == ApplicationEnvironments.Production)
                    return "44d030558ca68369c8bf13a36f8084ef";

				return string.Empty;
			}
		}

		public static string CseKey
		{
			get
			{
				if (ApplicationEnvironment == ApplicationEnvironments.Demonstration)
					return "XXXXXX";

				if (ApplicationEnvironment == ApplicationEnvironments.Development)
					return "MIIBCgKCAQEAz1/azMRZS18li0hcYmEIo59wVYE7NKJ0ydl1PNo0rgQayaqfGIJv9J1j8mxh52w32YbcTS5TU0gns30OUnFp5GHED/ZYD43tqR2zXGeYLjpp3m92N4Ax9bGtnEc1hMJnVpqhDrHBdCIVsfC46PWf2tClqLknU2wBljecd07YcoHz5yZ/8GMWLbvV+WkpiBnLQ9EoibSA+CBbtUE978FJ2LyMWKnh5rVR4Ugjj7PZyPgc1+HS0Eo4zDU07sShDMw8FWZE47FJsB9JmnFP4Mk+IrCjdVL/kFqdjhA4Iw1ugx5+TfsosQNt7mh5QfSeLuTtxWFKvmHjbmlLe5GHlV68AwIDAQAB";

				if (ApplicationEnvironment == ApplicationEnvironments.Production)
                    return "MIIBCgKCAQEAwKm3vpUI0DipAtLOBlNedBw3SZjEEbstQNEv66UAeRLVNiT01DPFkTxIFZJ9nrXg4iGqFX4kRlkwFyBuZ+6Q+CK3zIU7nHDlGfwDWraCC0LK3gW/b3Loo/sjVd7AIdxLnhrdLNf2/zRCu+CHUgDpx1mGDEHfWaSjRNHKEdLHwr1wjNbCdzZhrWaYGDwWt5BTkKLRZJTB15C5kI32Bor62JS9yr13xHO4T6MbM35REXCk2OZiECi9BreUCZIOEhfGESXkp00USoZ65I20N5e8HnO0oO7IuwubFOupmp+cfsO+zZRGW9NBPal0LG1N0504o65KSNlU2U/Jq/dNCXATVwIDAQAB";

				return string.Empty;
			}
		}
	}
}

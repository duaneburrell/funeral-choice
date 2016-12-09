
namespace Obsequy.Utility
{
    public static class Definitions
    {
		public const int USER_SPECIFIED_VALUE = 1000;

		public static class Database
		{
			public const string AdminMemberDocuments = "AdminMembers";

			public const string ConsumerMemberDocuments = "ConsumerMembers";
			public const string ConsumerPortfolioDocuments = "ConsumerPortfolios";

			public const string ProviderMemberDocuments = "ProviderMembers";
			public const string ProviderPortfolioDocuments = "ProviderPortfolios";

			public const string ResponseDocuments = "Responses";
		}

        public static class Account
        {
            public static class Roles
            {
				public const string Administrator = "Administrator";
				public const string Consumer = "Consumer";
				public const string Provider = "Provider";

                public const string Elevated = "Elevated";
                public const string Standard = "Standard";
            }
        }

		public static class Logger
		{
			public const string Name = "Obsequy";
			public const string FileName = "Server.Log";
			public const string FileExt = ".txt";
		}

		public static class Controllers
		{
			public static class Home
			{
				public static class Route
				{
					public const string Server = "Home";
				}

				public static class Views
				{
					public const string Index = "Index";
				}
			}

			public static class Administrator
			{
				public static class Route
				{
					public const string Client = "/a/";
					public const string Server = "a";
				}

				public static class Views
				{
					public const string Index = "Index";
				}
			}

			public static class Consumer
			{
				public static class Route
				{
					public const string Client = "/c/";
					public const string Server = "c";
				}

				public static class Views
				{
					public const string Index = "Index";
				}
			}

			public static class Provider
			{
				public static class Route
				{
					public const string Client = "/p/";
					public const string Server = "p";
				}

				public static class Views
				{
					public const string Index = "Index";
				}
			}

			public static class Security
			{
				public static class Route
				{
					public const string Client = "/s/";
					public const string Server = "s";
				}

				public static class Views
				{
					public const string Upgrade = "u";
				}
			}
		}
    }
}

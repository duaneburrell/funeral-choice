using System;

namespace Obsequy.Utility
{
    public static class MathHelper
    {
		public static double RoundMaxDistance(double maxDistance)
		{
			if (maxDistance <= 1.0)
				return 0.0;
			if (maxDistance <= 5.0)
				return 5.0;
			if (maxDistance <= 10.0)
				return 10.0;
			if (maxDistance <= 15.0)
				return 15.0;
			if (maxDistance <= 20.0)
				return 20.0;
			if (maxDistance <= 25.0)
				return 25.0;
			if (maxDistance <= 50.0)
				return 50.0;
			if (maxDistance <= 75.0)
				return 75.0;

			return 100.0;
		}
    }
}

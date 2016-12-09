
namespace Obsequy.Utility
{
	public static class LoggerHelper
	{
		private static NLog.Logger _logger; 
		public static NLog.Logger Logger 
		{
			get 
			{
				if (_logger == null)
					_logger = NLog.LogManager.GetLogger(Definitions.Logger.Name);

				return _logger;
			}
		}
	}
}

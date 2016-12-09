
namespace Obsequy.Utility
{
	public interface IGeoCodeable
	{
		#region Properties

		bool CanGeoCode { get; }
		string GeocodingCoordinates { get; }
		GoogleMapsApi.Entities.Geocoding.Response.Status? GeocodingStatus { get; set; }
		string GeocodingQuery { get; }
		bool HasLocation { get; }

		#endregion
	}
}

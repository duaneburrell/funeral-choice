using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using System;
using System.Linq;

namespace Obsequy.Utility
{
    public static class GeoCoder
	{
		public class GeoCodeResult
		{
			public enum GeoCodeAction
			{
				None,
				Clear,
				Set
			};

			public GeoCodeAction Action { get; set; }
			public GoogleMapsApi.Entities.Geocoding.Response.Status? GeocodingStatus { get; set; }
			public string Latitude { get; set; }
			public string Longitude { get; set; }

			public GeoCodeResult()
			{
				this.Action = GeoCodeAction.None;
			}
		}

		#region Look-Up Geo-Location

		public static GeoCodeResult LookUpGeoLocation(IGeoCodeable entity)
		{
			var result = new GeoCodeResult();

			if (!entity.CanGeoCode)
				return result;

			try
			{
				// configure the geocode to get our latitude and longitude
				var geocodeRequest = new GeocodingRequest()
				{
					Address = entity.GeocodingQuery
				};

				// log the look up
			 	LoggerHelper.Logger.Trace(string.Format("Requesting location via Google Maps: {0}", geocodeRequest.Address));

				// execute the query (this is blocking)
				var geocodeResponse = GoogleMaps.Geocode.Query(geocodeRequest);

				if (geocodeResponse.Status == GoogleMapsApi.Entities.Geocoding.Response.Status.OK)
				{
					var haveLocation = geocodeResponse.Results.Any();

					// take the first result
					if (haveLocation)
					{
						var responseResult = geocodeResponse.Results.First();
						var addressComponents = responseResult.AddressComponents;

						// obtain the location
						var location = responseResult.Geometry.Location;

						// log the response
						LoggerHelper.Logger.Debug(string.Format("Response received from Google Maps. Results: ({0}, {1})", location.Latitude, location.Longitude));

						// set latitude, longitude, and location
						// note: the location is now longitude first, latitude second. 
						// see http://blogs.msdn.com/b/isaac/archive/2008/03/05/the-upcoming-geography-coordinate-order-swap-a-faq.aspx
						result.Action = GeoCodeResult.GeoCodeAction.Set;
						result.GeocodingStatus = geocodeResponse.Status;
						result.Latitude = location.Latitude.ToString();
						result.Longitude = location.Longitude.ToString();
					}
					else
					{
						// log the invalid response
						LoggerHelper.Logger.Debug(string.Format("Response received from Google Maps. Results are empty, clearing location for query {0}", entity.GeocodingQuery));

						// clear the location results
						result.Action = GeoCodeResult.GeoCodeAction.Clear;
						result.GeocodingStatus = geocodeResponse.Status;
					}
				}
				else
				{
					// log the return value
					LoggerHelper.Logger.Debug(string.Format("Response received from Google Maps: {0}, clearing location for query {1}", geocodeResponse.Status, entity.GeocodingQuery));

					// clear the location results
					result.Action = GeoCodeResult.GeoCodeAction.Clear;
					result.GeocodingStatus = geocodeResponse.Status;
				}
			}
			catch (Exception ex)
			{
				// log the exception
				LoggerHelper.Logger.Error(string.Format("Exception occured geocoding from Google Maps for query {0}", entity.GeocodingQuery), ex);

				// clear the location results
				result.Action = GeoCodeResult.GeoCodeAction.Clear;
			}

			return result;
		}
		#endregion
    }
}

using System;

namespace Obsequy.Model
{
	public static class DatabaseHelper
	{
		#region Properties

		#region Connection String
		private static string SqlConnectionString
		{
			get { return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; }
		}
		#endregion

		#endregion

		#region User Exists
		public static bool UserExists(string email)
		{
			var userExists = false;

			using (var sqlConnection = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
			{
				// open a connection to our SQL DB
				sqlConnection.Open();

				// find the Mongo Id for this email
				var query = new System.Data.SqlClient.SqlCommand(string.Format("SELECT MongoUserId FROM MEMBERSHIP WHERE Email = '{0}'", email), sqlConnection);
				var reader = query.ExecuteReader();

				// get our value
				reader.Read();

				// this user exists if the reader has rows
				userExists = reader.HasRows;

				// close the connection
				sqlConnection.Close();
			}

			return userExists;
		}
		#endregion

		#region Is Member Email
		public static bool IsMemberEmail(string email, string memberId)
		{
			var isMemberEmail = false;

			using (var sqlConnection = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
			{
				// open a connection to our SQL DB
				sqlConnection.Open();

				// find the Mongo Id for this email
				var query = new System.Data.SqlClient.SqlCommand(string.Format("SELECT MongoUserId FROM MEMBERSHIP WHERE Email = '{0}'", email), sqlConnection);
				var reader = query.ExecuteReader();

				// get our value
				reader.Read();

				// this user exists if the reader has rows
				if (reader.HasRows)
				{
					// get the mongo ID from SQL table
					var mongoUserId = reader.GetString(reader.GetOrdinal("MongoUserId"));

					// does this match the current user's member ID?
					isMemberEmail = (mongoUserId == memberId);
				}

				// close the connection
				sqlConnection.Close();
			}

			return isMemberEmail;
		}
		#endregion

		#region Update Member Email
		public static bool UpdateMemberEmail(string email, string memberId)
		{
			var isUpdated = false;

			using (var sqlConnection = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
			{
				// open a connection to our SQL DB
				sqlConnection.Open();

				// find the Mongo Id for this email
				var query = new System.Data.SqlClient.SqlCommand(string.Format("UPDATE MEMBERSHIP SET Email = '{0}' WHERE MongoUserId = '{1}' ", email, memberId), sqlConnection);
				var rowsAffected = query.ExecuteNonQuery();

				// close the connection
				sqlConnection.Close();

				if (rowsAffected == 1)
				{
					// verify the change was made
					isUpdated = IsMemberEmail(email, memberId);
				}
			}

			return isUpdated;
		}
		#endregion
    }
}

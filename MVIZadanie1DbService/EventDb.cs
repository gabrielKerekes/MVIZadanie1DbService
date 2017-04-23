using System.Text;
using MySql.Data.MySqlClient;

namespace MVIZadanie1DbService
{
    public class EventDb
    {
        // todo: refactor
        public static string GetAllEventsFromDb()
        {
            var server = "mysql57.websupport.sk";
            var database = "mechatronika1";
            var uid = "mechatronika1";
            var password = "mechatronika_mvi";
            
            var connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";PORT=3311;" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            var connection = new MySqlConnection(connectionString);
            connection.Open();
            
            var selectPostsCommand = new MySqlCommand("SELECT * FROM novinymvi_posts WHERE post_type='tribe_events' AND post_status='publish'", connection);
            var postsReader = selectPostsCommand.ExecuteReader();

            var sb = new StringBuilder();
            while (postsReader.Read())
            {
                var id = postsReader["ID"];
                var eventName = postsReader["post_title"];
                var eventDescription = postsReader["post_content"];

                var connection2 = new MySqlConnection(connectionString);
                connection2.Open();
                var selectMetaDataCommand = new MySqlCommand($"SELECT * FROM novinymvi_postmeta WHERE post_id={id}", connection2);
                var metaDataReader = selectMetaDataCommand.ExecuteReader();
                
                string dateStartString = "";
                string dateEndString = "";
                int venueId = -1;
                while (metaDataReader.Read())
                {
                    var meta_key = (string) metaDataReader["meta_key"];

                    if (meta_key == "_EventStartDate")
                        dateStartString = (string) metaDataReader["meta_value"];
                    else if (meta_key == "_EventEndDate")
                        dateEndString = (string) metaDataReader["meta_value"];
                    else if (meta_key == "_EventVenueID")
                        venueId = int.Parse((string)metaDataReader["meta_value"]);
                }

                if (venueId == -1)
                    continue;

                var connection3 = new MySqlConnection(connectionString);
                connection3.Open();
                var selectVenueCommand = new MySqlCommand($"SELECT * FROM novinymvi_posts WHERE ID={venueId}", connection3);
                var venueReader = selectVenueCommand.ExecuteReader();

                string place = "";
                while (venueReader.Read())
                {
                    place = (string) venueReader["post_title"];
                }

                sb.Append($"eventId$$${id}$$$eventName$$${eventName}$$$eventDescription$$${eventDescription}$$$start$$${dateStartString}$$$end$$${dateEndString}$$$venueId$$${venueId}$$$place$$${place};");
            }

            return sb.ToString();
        }
    }
}
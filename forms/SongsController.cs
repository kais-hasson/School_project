using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using School_project.Classes;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace School_project.forms
{
    public class SongsController : ApiController
    {

        [HttpGet]
        [Route("api/Storis/download")]
        public HttpResponseMessage GetSongs(string file_name)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var file_path = HttpContext.Current.Server.MapPath($"~/App_Data/Song/{file_name}");
            var file_bytes = File.ReadAllBytes(file_path);
            var file_Mem_Stream = new MemoryStream(file_bytes);
            result.Content = new StreamContent(file_Mem_Stream);
            var headers = result.Content.Headers;
            headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            headers.ContentDisposition.FileName = file_name;
            headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            headers.ContentLength = file_Mem_Stream.Length;
            return result;


        }
        // GET api/Songs
        public IEnumerable<string> Get()
          {
            string dol;
            dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
            SqlConnection to_data = new SqlConnection();
            to_data.ConnectionString = dol;
            string sql;
            SqlCommand my_select = new SqlCommand();
            my_select.Connection = to_data;
            sql = "SELECT * FROM SONG ";
            my_select.CommandText = sql;
            SqlDataReader DReader;
            to_data.Open();
            DReader = my_select.ExecuteReader();
            List<Song> SongList = new List<Song>();
            Song song = new Song();
            while (DReader.Read())
            {

                //x = DReader.GetOrdinal("VIDEO_NAME");
                song.Name = DReader["SONG_NAME"].ToString();
                song.Path = DReader["SONG_PATH"].ToString();
                song.Size = Convert.ToInt32(DReader["SONG_SIZE"]);
                song.Id = Convert.ToInt32(DReader["SONG_Id"]);
                yield return (song.Name + "," + song.Path + "," + song.Size+ ","+song.Id);
                SongList.Add(song);
            }
          }
        [HttpPost]
        public string Song_Edit([FromBody] Song song,int id)
        {
            try
            {
                string dol;
                dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
                SqlConnection to_data = new SqlConnection();
                to_data.ConnectionString = dol;
                SqlCommand my_select = new SqlCommand();
                to_data.Open();
                string sql;
                sql = "UPDATE[SONG] SET[SONG_NAME] = @song_name, [SONG_PATH]= @song_path, [SONG_SIZE] = song_size WHERE [SONG_Id]=@song_id";
                SqlCommand update = new SqlCommand(sql, to_data);
                update.Parameters.Add("@song_name", song.Name);
                update.Parameters.Add("@song_path", song.Path);
                update.Parameters.Add("@song_size", song.Size);
                update.Parameters.Add("@video_id", id);
                update.ExecuteNonQuery();
                to_data.Close();
               
            }
            catch (Exception e)
            {
                return $"Error:{e.Message}";
            }
            return ("the song is updated");
        }
        [HttpDelete]
        public string Song_Delete(string name)
        {
            try
            {
                string dol;
                dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
                SqlConnection to_data = new SqlConnection();
                to_data.ConnectionString = dol;
                SqlCommand my_select = new SqlCommand();
                to_data.Open();
                string sql;
                sql = "Delete from [SONG] WHERE [SONG_NAME] = @song_name";
                SqlCommand delete = new SqlCommand(sql, to_data);
                delete.Parameters.Add("@song_name", name);
                delete.ExecuteNonQuery();
                to_data.Close();
                var file_path = HttpContext.Current.Server.MapPath($"~/App_Data/Song/{name}");
                File.Delete(file_path);
            }
            catch(Exception e)
            {
                return $"Error:{e.Message}";
            }
            return ("the song is deleted");
        }
        [HttpPost]
        [Route("api/Songs/Song_upload")]
        public async Task<string> Song_upload()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/App_Data/Song");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.FileData)
                {

                    var name = file.Headers.ContentDisposition.FileName;
                    //remove double quotes from string
                    name = name.Trim('"');
                    var localFileName = file.LocalFileName;
                    var filepath = Path.Combine(root, name);
                    File.Move(localFileName, filepath);
                    byte[] file_Bytes;
                    using (var fs = new FileStream(filepath,
                        FileMode.Open, FileAccess.Read))
                    {
                        file_Bytes = new byte[fs.Length];
                        //fs.Read(file_Bytes, 0, Convert.ToInt32(fs.Length));
                    }
                    string dol;
                    dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
                    SqlConnection to_data = new SqlConnection();
                    to_data.ConnectionString = dol;
                    SqlCommand my_select = new SqlCommand();
                    to_data.Open();
                    string sql;
                    sql = "INSERT INTO [SONG] ([SONG_NAME],[SONG_PATH],[SONG_SIZE]) VALUES(@song_name,@song_path,@song_size)";
                    SqlCommand insert = new SqlCommand(sql, to_data);
                    insert.Parameters.AddWithValue("@song_name", name);
                    insert.Parameters.AddWithValue("@song_path", filepath);
                    insert.Parameters.AddWithValue("@song_size", Convert.ToInt32(file_Bytes.Length) );
                    insert.ExecuteNonQuery();
                    to_data.Close();
                }
            }
            catch (Exception e)
            {
                return $"Error:{e.Message}";
            }

            return "song uploading";
        }
    }
}
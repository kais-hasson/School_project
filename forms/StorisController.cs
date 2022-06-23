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
    public class StorisController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        [Route("api/Storis/download/{file_name}")]
        public HttpResponseMessage Getstoris(string file_name)
        {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
            var file_path = HttpContext.Current.Server.MapPath($"~/App_Data/Story/{file_name}");
            var file_bytes = File.ReadAllBytes(file_path+".pdf");
            var file_Mem_Stream = new MemoryStream(file_bytes);
            result.Content = new StreamContent(file_Mem_Stream);
            var headers = result.Content.Headers;
            headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            headers.ContentDisposition.FileName = file_name;
            headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            headers.ContentLength = file_Mem_Stream.Length;
            return result;
            
          
        }
        public IEnumerable<string> Get()
        {
            string dol;
            dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
            SqlConnection to_data = new SqlConnection();
            to_data.ConnectionString = dol;
            string sql;
            SqlCommand my_select = new SqlCommand();
            my_select.Connection = to_data;
            sql = "SELECT * FROM STORY ";
            my_select.CommandText = sql;
            SqlDataReader DReader;
            to_data.Open();
            DReader = my_select.ExecuteReader();
            List<Story> StoryList = new List<Story>();
            Story story = new Story();
            while (DReader.Read())
            {

                //x = DReader.GetOrdinal("VIDEO_NAME");
                story.Name = DReader["STORY_NAME"].ToString();
                story.Path = DReader["STORY_PATH"].ToString();
                story.Size = Convert.ToInt32( DReader["STORY_SIZE"]);
                story.Id = Convert.ToInt32(DReader["STORY_Id"]);
                yield return (story.Name + "," + story.Path + "," + story.Size + ","+story.Id);
                StoryList.Add(story);
            }
        }
        [HttpPost]
        public string Story_Edit([FromBody] Story story,int id)
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
                sql = "UPDATE[STORY] SET[STORY_NAME] = @story_name, [STORY_PATH]= @story_path, [STORY_SIZE] = story_size WHERE [STORY_Id]=@story_id";
                SqlCommand update = new SqlCommand(sql, to_data);
                update.Parameters.Add("@story_name", story.Name);
                update.Parameters.Add("@story_path", story.Path);
                update.Parameters.Add("@story_size", story.Size);
                update.Parameters.Add("@story_id", id);
                update.ExecuteNonQuery();
                to_data.Close();
                //var file_path = HttpContext.Current.Server.MapPath($"~/App_Data/Story/{story.Name}");
                //File.CreateText(file_path);
            }
            catch(Exception e)
            {
                return $"Error:{e.Message}";
            }
            return ("the story is updated");
        }
        [HttpDelete]
        public string Story_Delete(string name)
        {
            try { 
            string dol;
            dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
            SqlConnection to_data = new SqlConnection();
            to_data.ConnectionString = dol;
            SqlCommand my_select = new SqlCommand();
            to_data.Open();
            string sql;
            sql = "Delete from [STORY] WHERE [STORY_NAME] = @story_name";
            SqlCommand delete = new SqlCommand(sql, to_data);
            delete.Parameters.Add("@story_name", name);
            delete.ExecuteNonQuery();
            to_data.Close();
                var file_path = HttpContext.Current.Server.MapPath($"~/App_Data/Story/{name}");
                File.Delete(file_path);
            }
            catch (Exception e)
            {
                return $"Error:{e.Message}";
            }
            
            return ("the story is deleted");
            

        }
        [HttpPost]
        [Route("api/Storis/Story_upload")]
        public async Task<string> Story_upload()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/App_Data/Story");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
              await Request.Content.ReadAsMultipartAsync(provider);
                foreach(var file in provider.FileData)
                {

                    var name = file.Headers.ContentDisposition.FileName;
                    //remove double quotes from string
                    name = name.Trim('"');
                    var localFileName = file.LocalFileName;
                    var filepath = Path.Combine(root, name);
                    File.Move(localFileName, filepath);
                    byte[] file_Bytes;
                    using(var fs =new FileStream(filepath,
                        FileMode.Open, FileAccess.Read))
                    {
                        file_Bytes = new byte[fs.Length];
                        fs.Read(file_Bytes, 0, Convert.ToInt32(fs.Length));
                    }
                    string dol;
                    dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
                    SqlConnection to_data = new SqlConnection();
                    to_data.ConnectionString = dol;
                    SqlCommand my_select = new SqlCommand();
                    to_data.Open();
                    string sql;
                    sql = "INSERT INTO [STORY] ([STORY_NAME],[STORY_PATH],[STORY_SIZE]) VALUES(@story_name,@story_path,@story_size)";
                    SqlCommand insert = new SqlCommand(sql, to_data);
                    insert.Parameters.AddWithValue("@story_name",name);
                    insert.Parameters.AddWithValue("@story_path",filepath);
                    insert.Parameters.AddWithValue("@story_size",file_Bytes.Length);
                    insert.ExecuteNonQuery();
                    to_data.Close();
                }

            }
            catch (Exception e)
            {
                return $"Error:{e.Message}";
            }
            
            return "story uploading";
        }
        
    }
}
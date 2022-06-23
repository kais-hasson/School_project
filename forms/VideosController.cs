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
using System.Data.Linq.Mapping;
using System.Web.ModelBinding;

namespace School_project.forms
{
    public class VideosController : ApiController
    {
        [HttpGet]
        [Route("api/Storis/download")]
        public HttpResponseMessage Getvideos(string file_name)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var file_path = HttpContext.Current.Server.MapPath($"~/App_Data/Video/{file_name}");
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
        public IEnumerable<string> Get()
        {
                string dol;
                dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
                SqlConnection to_data = new SqlConnection();
                to_data.ConnectionString = dol;
                string sql;
                SqlCommand my_select = new SqlCommand();
                my_select.Connection = to_data;
                sql = "SELECT * FROM VIDEO  ";
                my_select.CommandText = sql;
                SqlDataReader DReader;
                to_data.Open();
                DReader = my_select.ExecuteReader();
                List<Video> VideosList = new List<Video>();
                Video video = new Video();
                while (DReader.Read())
                {

                //x = DReader.GetOrdinal("VIDEO_NAME");
                video.Name = DReader["VIDEO_NAME"].ToString();
                video.Path = DReader["VIDEO_PATH"].ToString();
                video.Size = Convert.ToInt32(DReader["VIDEO_SIZE"]);
                video.Id = Convert.ToInt32(DReader["VIDEO_Id"]);
                yield return (video.Name + "," + video.Path + "," + video.Size + "," + video.Id);
                    VideosList.Add(video);
                }
                to_data.Close();
            

        }
        [HttpPost]
        public string Video_Edit([FromBody] Video video,int id)
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
                sql = "UPDATE[VIDEO]  SET[VIDEO_NAME] = @video_name, [VIDEO_PATH]= @video_path, [VIDEO_SIZE] = video_size WHERE [VIDEO_Id]=@video_id";
                SqlCommand update = new SqlCommand(sql, to_data);
                update.Parameters.Add("@video_name", video.Name);
                update.Parameters.Add("@video_path", video.Path);
                update.Parameters.Add("@video_title", video.Size);
                update.Parameters.Add("@video_id", id);
                update.ExecuteNonQuery();
                to_data.Close();
            }
            catch (Exception e)
            {
                return $"Error:{e.Message}";
            }
            return ("the video is updated");
        }
        [HttpDelete]
        public string Video_Delete(string name)
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
                sql = "Delete from [VIDEO] WHERE [VIDEO_NAME] = @video_name";
                SqlCommand delete = new SqlCommand(sql, to_data);
                delete.Parameters.Add("@video_name", name);
                delete.ExecuteNonQuery();
                to_data.Close();
                var file_path = HttpContext.Current.Server.MapPath($"~/App_Data/Story/{name}");
                File.Delete(file_path);
            }
            catch (Exception e)
            {
                return $"Error:{e.Message}";
            }
            return ("the video is deleted");
        }
        [HttpPost]
        [Route("api/Videos/Video_upload")]
        public async Task<string> Video_upload() 
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/App_Data/Video");
            var provider = new MultipartFormDataStreamProvider(root);
            //if (video_file != null) {
                try {
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
                            fs.Read(file_Bytes, 0, Convert.ToInt32(fs.Length));
                        }
                        string dol;
                        dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
                        SqlConnection to_data = new SqlConnection();
                        to_data.ConnectionString = dol;
                        SqlCommand my_select = new SqlCommand();
                        to_data.Open();
                        string sql;
                        sql = "INSERT INTO [VIDEO] ([VIDEO_NAME],[VIDEO_PATH],[VIDEO_SIZE]) VALUES(@video_name,@video_path,@video_size)";
                        SqlCommand insert = new SqlCommand(sql, to_data);
                        insert.Parameters.AddWithValue("@video_name", name);
                        insert.Parameters.AddWithValue("@video_path", filepath);
                        insert.Parameters.AddWithValue("@video_size", file_Bytes.Length);
                        insert.ExecuteNonQuery();
                        to_data.Close();
                    }
                }
                catch (Exception e)
                {
                    return $"Error:{e.Message}";
                }
            //}

            return "video uploading";
        }
    }
}
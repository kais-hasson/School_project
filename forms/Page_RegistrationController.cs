using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using School_project.Classes;
using System.Data;

namespace School_project.Controllers
{ 
    
    public class Page_RegistrationController : ApiController
    {
        [HttpPost]
        public void Poststudent([FromBody]Students stud)
        {
            string dol;
            dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
            SqlConnection to_data = new SqlConnection();
            to_data.ConnectionString = dol;
            SqlCommand my_select = new SqlCommand();
            to_data.Open();
            string sql;
            sql = "INSERT INTO [STUDENT] ([FULL_NAME],[EMAIL],[FATHER_NAME],[MOTHER_NAME],[SUPERVISOR],[BIRTH_DATE],[GENDER],[LANGUAGE],[LANGUAGE_ABILITY],[LANGUAGE_STUDY],[LANGUAGE_LEVEL],[PHONE_NUMBER],[COUNTRY],[CITY],[ADDRESS],[MESSAGE],[PASSWORD]) VALUES(@FULL_NAME,@EMAIL,@FATHER_NAME,@MOTHER_NAME,@SUPERVISOR,@BIRTH_DATE,@GENDER,@LANGUAGE,@LANGUAGE_ABILITY,@LANGUAGE_STUDY,@LANGUAGE_LEVEL,@PHONE_NUMBER,@COUNTRY,@CITY,@ADDRESS,@MESSAGE,@PASSWORD)";
            SqlCommand new_student = new SqlCommand(sql,to_data);
                new_student.Parameters.AddWithValue("@FULL_NAME", stud.Full_name);
                new_student.Parameters.AddWithValue("@EMAIL", stud.Email);
                new_student.Parameters.AddWithValue("@FATHER_NAME", stud.Father_name);
                new_student.Parameters.AddWithValue("@MOTHER_NAME", stud.Mother_name);
                new_student.Parameters.AddWithValue("@SUPERVISOR", stud.Supervisor);
                new_student.Parameters.AddWithValue("@BIRTH_DATE", stud.Birth_date);
                new_student.Parameters.AddWithValue("@GENDER", stud.Gender);
                new_student.Parameters.AddWithValue("@LANGUAGE", stud.Language);
                new_student.Parameters.AddWithValue("@LANGUAGE_ABILITY", stud.Language_ability);
                new_student.Parameters.AddWithValue("@LANGUAGE_STUDY", stud.Language_study);
                new_student.Parameters.AddWithValue("@LANGUAGE_LEVEL", stud.Language_level);
                new_student.Parameters.AddWithValue("@PHONE_NUMBER", stud.Phone_number);
                new_student.Parameters.AddWithValue("@COUNTRY", stud.Country);
                new_student.Parameters.AddWithValue("@CITY", stud.City);
                new_student.Parameters.AddWithValue("@ADDRESS", stud.Address);
                new_student.Parameters.AddWithValue("@MESSAGE", stud.Message);
                new_student.Parameters.AddWithValue("@PASSWORD", stud.Phone_number);
            new_student.ExecuteNonQuery();
            to_data.Close();
            ResponseMessage: string v = ( " تم اضافة الطالب بنجاح");
        }
    }
}


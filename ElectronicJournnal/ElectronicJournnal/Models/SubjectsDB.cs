using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class SubjectsDB
    {
        public static List<Subject> GetSubjects()
        {
            List<Subject> subjects = new List<Subject>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "SELECT * FROM Subjects";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string subjectName = reader.GetString(1);

                        subjects.Add(new Subject(id, subjectName));
                    }
                }
                reader.Close();
            }

            return subjects;
        }

        public static List<Subject> GetPupilSubjects(int id)
        {
            List<Subject> subjects = new List<Subject>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetPupilSubject]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@Pupil_ID",
                    Value = id
                };

                command.Parameters.Add(pupilIDParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int subjectID = reader.GetInt32(0);
                        string subjectName = reader.GetString(1);

                        subjects.Add(new Subject(subjectID, subjectName));
                    }
                }
                reader.Close();
            }

            return subjects;
        }

        public static List<KeyValuePair<string, int>> GetLessonAttendence()
        {
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_LessonsAttendance]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        string subjectName = reader.GetString(0);
                        int n = reader.GetInt32(1);

                        result.Add(new KeyValuePair<string, int>(subjectName, n));
                    }
                }

                reader.Close();
            }
            return result;
        }

        public static int GetSubjectTeacher(int classID, string subjectName)
        {
            int result = 0;

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetSubjectTeacher]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter classDParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = classID
                };

                SqlParameter subjectNameParam = new SqlParameter
                {
                    ParameterName = "@Subject_Name",
                    Value = subjectName
                };

                command.Parameters.Add(classDParam);
                command.Parameters.Add(subjectNameParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        result = reader.GetInt32(0);
                    }
                }

                reader.Close();
            }

            return result;
        }
    }
}
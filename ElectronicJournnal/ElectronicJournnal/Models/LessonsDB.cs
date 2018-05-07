using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class LessonsDB
    {
        public static List<LessonFullInf> GetLessonsInDate(DateTime date)
        {
            List<LessonFullInf> lessons = new List<LessonFullInf>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetLessonsInDate]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@Lesson_Date",
                    Value = date
                };

                command.Parameters.Add(nameParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int lessonId = reader.GetInt32(0);
                        int classID = reader.GetInt32(1);
                        string className = reader.GetString(2);
                        int teacherID = reader.GetInt32(3);
                        string teacherName = reader.GetString(4);
                        string teacherSurname = reader.GetString(5);
                        string teacherPatronymic = reader.GetString(6);
                        int subjectID = reader.GetInt32(7);
                        string subjectName = reader.GetString(8);
                        DateTime lessonDate = reader.GetDateTime(9);

                        lessons.Add(new LessonFullInf(lessonId, classID, className, teacherID, teacherName, teacherSurname, teacherPatronymic, subjectID, subjectName, lessonDate));
                    }
                }

                reader.Close();
            }

            return lessons;
        }

        public static List<LessonFullInf> GetLessonsInDateAndClass(DateTime date, int class_ID)
        {
            List<LessonFullInf> lessons = new List<LessonFullInf>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetLessonsInDateAndClass]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter dateParam = new SqlParameter
                {
                    ParameterName = "@Lesson_Date",
                    Value = date
                };

                SqlParameter classIdParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = class_ID
                };

                command.Parameters.Add(dateParam);
                command.Parameters.Add(classIdParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int lessonId = reader.GetInt32(0);
                        int classID = reader.GetInt32(1);
                        string className = reader.GetString(2);
                        int teacherID = reader.GetInt32(3);
                        string teacherName = reader.GetString(4);
                        string teacherSurname = reader.GetString(5);
                        string teacherPatronymic = reader.GetString(6);
                        int subjectID = reader.GetInt32(7);
                        string subjectName = reader.GetString(8);
                        DateTime lessonDate = reader.GetDateTime(9);

                        lessons.Add(new LessonFullInf(lessonId, classID, className, teacherID, teacherName, teacherSurname, teacherPatronymic, subjectID, subjectName, lessonDate));
                    }
                }

                reader.Close();
            }

            return lessons;
        }

        public static List<DateTime> GetLessonsDates()
        {
            List<DateTime> dates = new List<DateTime>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetLessonsDate]";

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
                        DateTime lessonDate = reader.GetDateTime(0);

                        dates.Add(lessonDate);
                    }
                }
                reader.Close();
            }

            return dates;
        }

        public static List<DateTime> GetFilterDates(string expr)
        {
            List<DateTime> dates = new List<DateTime>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = expr;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        DateTime lessonDate = reader.GetDateTime(0);

                        dates.Add(lessonDate);
                    }
                }
                reader.Close();
            }

            return dates;
        }

        public static int GetLessonsCount()
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetLessonsCount]";
            int count = 0;

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
                        count = reader.GetInt32(0);
                    }
                }

                reader.Close();
            }
            return count;
        }

        public static void DeleteLesson(int id)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_DeleteLesson]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@Lesson_ID",
                    Value = id
                };

                command.Parameters.Add(pupilIDParam);

                command.ExecuteNonQuery();
            }
        }

        public static LessonFullInf GetLessonFullInf(int id)
        {
            LessonFullInf lesson = new LessonFullInf();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetLessonFullInf]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@Lesson_ID",
                    Value = id
                };

                command.Parameters.Add(nameParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int lessonId = reader.GetInt32(0);
                        int classID = reader.GetInt32(1);
                        string className = reader.GetString(2);
                        int teacherID = reader.GetInt32(3);
                        string teacherName = reader.GetString(4);
                        string teacherSurname = reader.GetString(5);
                        string teacherPatronymic = reader.GetString(6);
                        int subjectID = reader.GetInt32(7);
                        string subjectName = reader.GetString(8);
                        DateTime lessonDate = reader.GetDateTime(9);

                        lesson = new LessonFullInf(lessonId, classID, className, teacherID, teacherName, teacherSurname, teacherPatronymic, subjectID, subjectName, lessonDate);
                    }
                }

                reader.Close();
            }

            return lesson;
        }

        public static void UpdateLessonInf(int lessonID, int classID, int teacherID, int subjectID, DateTime date)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_UpdateLessonInf]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter lessonIDParam = new SqlParameter
                {
                    ParameterName = "@Lesson_ID",
                    Value = lessonID
                };

                SqlParameter classIDParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = classID
                };

                SqlParameter teacherIDParam = new SqlParameter
                {
                    ParameterName = "@Teacher_ID",
                    Value = teacherID
                };

                SqlParameter subjectIDParam = new SqlParameter
                {
                    ParameterName = "@Subject_ID",
                    Value = subjectID
                };

                SqlParameter dateParam = new SqlParameter
                {
                    ParameterName = "@Date",
                    Value = date
                };

                command.Parameters.Add(lessonIDParam);
                command.Parameters.Add(classIDParam);
                command.Parameters.Add(teacherIDParam);
                command.Parameters.Add(subjectIDParam);
                command.Parameters.Add(dateParam);

                command.ExecuteNonQuery();
            }
        }

        public static void AddLesson(int classID, int teacherID, int subjectID, DateTime date)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_InsertLessonInf]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter classIDParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = classID
                };

                SqlParameter teacherIDParam = new SqlParameter
                {
                    ParameterName = "@Teacher_ID",
                    Value = teacherID
                };

                SqlParameter subjectIDParam = new SqlParameter
                {
                    ParameterName = "@Subject_ID",
                    Value = subjectID
                };

                SqlParameter dateParam = new SqlParameter
                {
                    ParameterName = "@Date",
                    Value = date
                };

                command.Parameters.Add(classIDParam);
                command.Parameters.Add(teacherIDParam);
                command.Parameters.Add(subjectIDParam);
                command.Parameters.Add(dateParam);

                command.ExecuteNonQuery();
            }
        }

        public static List<int> GetAbsentPupils(int lessonID)
        {
            List<int> pupilsID = new List<int>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetAbsentPupils]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter lessonIDParam = new SqlParameter
                {
                    ParameterName = "@Lesson_ID",
                    Value = lessonID
                };

                command.Parameters.Add(lessonIDParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int id = reader.GetInt32(0);

                        pupilsID.Add(id);
                    }
                }

                reader.Close();
            }

            return pupilsID;
        }

        public static DateTime GetClassFirstLesson(int classID)
        {
            DateTime date = new DateTime();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetFirstLesson]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter classIDParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = classID
                };

                command.Parameters.Add(classIDParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        date = reader.GetDateTime(0);
                    }
                }

                reader.Close();
            }

            return date;
        }
    }
}
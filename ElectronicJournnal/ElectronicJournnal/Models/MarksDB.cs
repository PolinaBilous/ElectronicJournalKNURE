using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class MarksDB
    {
        public static List<Mark> GetPupilMarks(int id)
        {
            List<Mark> marks = new List<Mark>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetPupilMarks]";

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
                        int? typeOfWorkId = null;
                        int? mark = null;
                        int markId = reader.GetInt32(0);
                        int pupilId = reader.GetInt32(1);
                        int lessonId = reader.GetInt32(2);
                        try
                        {
                            typeOfWorkId = reader.GetInt32(3);
                        }
                        catch
                        {
                            typeOfWorkId = null;
                        }
                        try
                        {
                            mark = reader.GetInt32(4);
                        }
                        catch
                        {
                            mark = null;
                        }
                        marks.Add(new Mark(markId, pupilId, lessonId, typeOfWorkId, mark));
                    }
                }
                reader.Close();
            }

            return marks;
        }

        public static List<Mark> GetPupilSubjectMark(int pupilId, int subjectID)
        {
            List<Mark> marks = new List<Mark>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetPupilSubjectMarks]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@Pupil_ID",
                    Value = pupilId
                };

                SqlParameter subjectIDParam = new SqlParameter
                {
                    ParameterName = "@Subject_ID",
                    Value = subjectID
                };

                command.Parameters.Add(pupilIDParam);
                command.Parameters.Add(subjectIDParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int? typeOfWorkId = null;
                        int? mark = null;
                        int markId = reader.GetInt32(0);
                        int pupilID = reader.GetInt32(1);
                        int lessonId = reader.GetInt32(2);
                        try
                        {
                            typeOfWorkId = reader.GetInt32(3);
                        }
                        catch
                        {
                            typeOfWorkId = null;
                        }
                        try
                        {
                            mark = reader.GetInt32(4);
                        }
                        catch
                        {
                            mark = null;
                        }
                        marks.Add(new Mark(markId, pupilID, lessonId, typeOfWorkId, mark));
                    }
                }
                reader.Close();
            }

            return marks;
        }

        public static MarkFullInf GetMarkFullInf(int id)
        {
            MarkFullInf mark = new MarkFullInf();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetMarkFullInf]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@Mark_ID",
                    Value = id
                };

                command.Parameters.Add(pupilIDParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        string pName = reader.GetString(1);
                        string pSurname = reader.GetString(2);
                        string pPatonymic = reader.GetString(3);
                        string tName = reader.GetString(4);
                        string tSurname = reader.GetString(5);
                        string tPatonymic = reader.GetString(6);
                        string subjectName = reader.GetString(7);
                        string typeOfWorkName = reader.GetString(8);
                        DateTime date = reader.GetDateTime(9);
                        int? m = null;
                        try
                        {
                            m = reader.GetInt32(10);
                        }
                        catch
                        {
                            m = null;
                        }

                        mark = new MarkFullInf(id, pName, pSurname, pPatonymic, tName, tSurname, tPatonymic, subjectName, typeOfWorkName, date, m);
                    }
                }
                reader.Close();
            }
            return mark;
        }

        public static MarkAbsentFullInf GetMarkAbsentFullInf(int id)
        {
            MarkAbsentFullInf mark = new MarkAbsentFullInf();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetMarkAbsentFullInf]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@Mark_ID",
                    Value = id
                };

                command.Parameters.Add(pupilIDParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        string pName = reader.GetString(1);
                        string pSurname = reader.GetString(2);
                        string pPatonymic = reader.GetString(3);
                        string tName = reader.GetString(4);
                        string tSurname = reader.GetString(5);
                        string tPatonymic = reader.GetString(6);
                        string subjectName = reader.GetString(7);
                        DateTime date = reader.GetDateTime(8);
                        int? m = null;
                        try
                        {
                            m = reader.GetInt32(9);
                        }
                        catch
                        {
                            m = null;
                        }

                        mark = new MarkAbsentFullInf(id, pName, pSurname, pPatonymic, tName, tSurname, tPatonymic, subjectName, date, m);
                    }
                }
                reader.Close();
            }
            return mark;
        }

        public static void AddMark(int pupilID, int lessonID, int typeID, int mark)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_InsertMark]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@Pupil_Id",
                    Value = pupilID
                };

                SqlParameter lessonIDParam = new SqlParameter
                {
                    ParameterName = "@Lesson_ID",
                    Value = lessonID
                };

                SqlParameter typeIDParam = new SqlParameter
                {
                    ParameterName = "@Type_ID",
                    Value = typeID
                };

                SqlParameter markParam = new SqlParameter
                {
                    ParameterName = "@Mark",
                    Value = mark
                };

                command.Parameters.Add(pupilIDParam);
                command.Parameters.Add(lessonIDParam);
                command.Parameters.Add(typeIDParam);
                command.Parameters.Add(markParam);

                command.ExecuteNonQuery();
            }
        }

        public static void AddAbsentMark(int pupilID, int lessonID)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_InsertAbsentMark]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@Pupil_Id",
                    Value = pupilID
                };

                SqlParameter lessonIDParam = new SqlParameter
                {
                    ParameterName = "@Lesson_ID",
                    Value = lessonID
                };

                command.Parameters.Add(pupilIDParam);
                command.Parameters.Add(lessonIDParam);

                command.ExecuteNonQuery();
            }
        }

        public static void DeleteAbsentPupilsMark(int lessonID)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_DeleteAbsentPupilsMarks]";

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

                command.ExecuteNonQuery();
            }
        }

        public static void DeleteMark(int id)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_DeleteMark]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter markIDParam = new SqlParameter
                {
                    ParameterName = "@Mark_ID",
                    Value = id
                };

                command.Parameters.Add(markIDParam);

                command.ExecuteNonQuery();
            }
        }

        public static List<KeyValuePair<int, int>> GetSchoolMarksStatistic()
        {
            List<KeyValuePair<int, int>> result = new List<KeyValuePair<int, int>>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_SchoolMarksStatictic]";

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
                        int rangeHightBorder = reader.GetInt32(0);
                        int n = reader.GetInt32(1);

                        result.Add(new KeyValuePair<int, int>(rangeHightBorder, n));
                    }
                }

                reader.Close();
            }
            return result;
        }
    }
}
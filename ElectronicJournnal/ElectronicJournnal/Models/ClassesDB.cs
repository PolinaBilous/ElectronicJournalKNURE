using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class ClassesDB
    {
        public static List<Class> GetClassesWithoutCurr(string className)
        {
            List<Class> classes = new List<Class>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = $"SELECT * FROM Classes WHERE Class_Name != '{className}'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        DateTime fd;
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        fd = reader.GetDateTime(2);

                        classes.Add(new Class(id, name, fd));
                    }
                }

                reader.Close();
            }

            return classes;
        }

        public static int GetClassId(string className)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "sp_GetClassID";
            int id = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@Class_Name",
                    Value = className
                };

                command.Parameters.Add(nameParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        id = reader.GetInt32(0);
                    }
                }

                reader.Close();
            }
            return id;
        }

        public static List<Class> GetClasses()
        {
            List<Class> classes = new List<Class>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = $"SELECT * FROM Classes ORDER BY LEN(Class_Name), Class_Name";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read()) 
                    {
                        DateTime fd;
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        fd = reader.GetDateTime(2);

                        classes.Add(new Class(id, name, fd));
                    }
                }

                reader.Close();
            }

            return classes;
        }

        public static void AddClass(string name, DateTime formationDate)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_InsertClass]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter classNameParam = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = name
                };

                SqlParameter classFormationDateParam = new SqlParameter
                {
                    ParameterName = "@FormationDate",
                    Value = formationDate
                };

                command.Parameters.Add(classNameParam);
                command.Parameters.Add(classFormationDateParam);

                command.ExecuteNonQuery();
            }
        }

        public static void DeleteClass(int id)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_DeleteClass]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = id
                };

                command.Parameters.Add(pupilIDParam);

                command.ExecuteNonQuery();
            }
        }

        public static ClassWithManagment GetClassFullInf(int id)
        {
            ClassWithManagment Class = new ClassWithManagment();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetClassFullInf]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = id
                };

                command.Parameters.Add(nameParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int classID = reader.GetInt32(0);
                        string ClassName = reader.GetString(1);
                        DateTime fd;
                        fd = reader.GetDateTime(2);
                        int ClassManagmentId = reader.GetInt32(3);
                        int TeacherID = reader.GetInt32(4);
                        string name = reader.GetString(5);
                        string surname = reader.GetString(6);
                        string patronymic = reader.GetString(7);

                        Class = new ClassWithManagment(classID, ClassName, fd, ClassManagmentId, TeacherID, name, surname, patronymic);
                    }
                }

                reader.Close();
            }

            return Class;
        }

        public static void UpdateClass(int classID, DateTime formationDate, string className)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_UpdateClassInf]";

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

                SqlParameter formationDateParam = new SqlParameter
                {
                    ParameterName = "@Formation_Date",
                    Value = formationDate
                };

                SqlParameter classNameParam = new SqlParameter
                {
                    ParameterName = "@Class_Name",
                    Value = className
                };

                command.Parameters.Add(classIDParam);
                command.Parameters.Add(formationDateParam);
                command.Parameters.Add(classNameParam);

                command.ExecuteNonQuery();
            }
        }

        public static void FinishClassManagment(int classManagmentID)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_FinisClassManagment]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter classManagmentIDParam = new SqlParameter
                {
                    ParameterName = "@CM_ID",
                    Value = classManagmentID
                };

                command.Parameters.Add(classManagmentIDParam);

                command.ExecuteNonQuery();
            }
        }

        public static void AddClassManagment(int classID, int TeacherID)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_InsertClassManagment]";

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
                    Value = TeacherID
                };

                command.Parameters.Add(classIDParam);
                command.Parameters.Add(teacherIDParam);

                command.ExecuteNonQuery();
            }
        }

        public static List<PupilWithAvgMark> GetClassRating(int id)
        {
            List<PupilWithAvgMark> pupils = new List<PupilWithAvgMark>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_PupilsInClassRating]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = id
                };

                command.Parameters.Add(nameParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int pupilId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        int classId = reader.GetInt32(4);
                        double avgMark = Convert.ToDouble(reader.GetValue(5));

                        pupils.Add(new PupilWithAvgMark(pupilId, name, surname, patronymic, classId, avgMark));
                    }
                }

                reader.Close();
            }
            return pupils;
        }

        public static List<ClassForRating> GetRatingOfClasses()
        {
            List<ClassForRating> classes = new List<ClassForRating>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetClassesRating]";

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
                        int classId = reader.GetInt32(0);
                        string className = reader.GetString(1);
                        double avgMark = Convert.ToDouble(reader.GetValue(2));

                        classes.Add(new ClassForRating(classId, className, avgMark));
                    }
                }

                reader.Close();
            }
            return classes;
        }

        public static List<ClassesFillability> GetClassesFillability()
        {
            List<ClassesFillability> classes = new List<ClassesFillability>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_NumberOfPupilsInClasses]";

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
                        string className = reader.GetString(0);
                        int n = reader.GetInt32(1);

                        classes.Add(new ClassesFillability(className, n));
                    }
                }

                reader.Close();
            }
            return classes;
        }
    }
}
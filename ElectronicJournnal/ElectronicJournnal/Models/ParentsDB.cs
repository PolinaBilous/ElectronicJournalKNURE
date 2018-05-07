using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class ParentsDB
    {
        public static void AddParent(string name, string surname, string patronymic, string email)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_InsertIntoParents]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilNameParam = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = name
                };

                SqlParameter pupilSurnameParam = new SqlParameter
                {
                    ParameterName = "@Surname",
                    Value = surname
                };

                SqlParameter pupilPatronymicParam = new SqlParameter
                {
                    ParameterName = "@Patronymic",
                    Value = patronymic
                };

                SqlParameter emailParam = new SqlParameter
                {
                    ParameterName = "@Email",
                    Value = email
                };

                command.Parameters.Add(pupilNameParam);
                command.Parameters.Add(pupilSurnameParam);
                command.Parameters.Add(pupilPatronymicParam);
                command.Parameters.Add(emailParam);

                command.ExecuteNonQuery();
            }
        }

        public static int GetParentsLastID()
        {
            int id = 0;
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetParentLastID]";

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
                        id = reader.GetInt32(0);
                    }
                }

                reader.Close();
            }

            return id;
        }

        public static void AddParenthood(int parentID, int pupilID)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_InsertInParenthood]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter parentIDParam = new SqlParameter
                {
                    ParameterName = "@Parent_ID",
                    Value = parentID
                };

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@Pupil_ID",
                    Value = pupilID
                };

                command.Parameters.Add(parentIDParam);
                command.Parameters.Add(pupilIDParam);

                command.ExecuteNonQuery();
            }
        }

        public static List<Parenthood> GetParenthoods()
        {
            List<Parenthood> parenthoods = new List<Parenthood>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "SELECT * FROM Parenthood";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int parenthoodID = reader.GetInt32(0);
                        int parentID = reader.GetInt32(1);
                        int pupilID = reader.GetInt32(2);

                        parenthoods.Add(new Parenthood(parenthoodID, parentID, pupilID));
                    }
                }

                reader.Close();
            }

            return parenthoods;
        }
        
        public static Parent GetParentInfo(int id)
        {
           Parent parent = new Parent();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = $"SELECT * FROM Parents WHERE Parent_ID = {id}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int parentId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string email = reader.GetString(4);

                        parent = new Parent(parentId, name, surname, patronymic, email);
                    }
                }

                reader.Close();
            }

            return parent;
        }

        public static List<Parenthood> GetPupilParenthoods(int pupilID)
        {
            List<Parenthood> parenthoods = new List<Parenthood>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetPupilParenthoods]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilParam = new SqlParameter
                {
                    ParameterName = "@Pupil_ID",
                    Value = pupilID
                };

                command.Parameters.Add(pupilParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int parenthoodId = reader.GetInt32(0);
                        int parentID = reader.GetInt32(1);
                        int pupilId = reader.GetInt32(2);

                        parenthoods.Add(new Parenthood(parenthoodId, parentID, pupilId));
                    }
                }

                reader.Close();
            }
            return parenthoods;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class TypesOfWorkDB
    {
        public static List<TypeOfWork> GetTypesOfWork()
        {
            List<TypeOfWork> types = new List<TypeOfWork>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "SELECT * FROM Types_Of_Work";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int coeff = reader.GetInt32(2);

                        types.Add(new TypeOfWork(id, name, coeff));
                    }
                }
                reader.Close();
            }

            return types;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class PupilsDB
    {
        // Получаем список учеников с коассами из БД.
        public static List<PupilWithClass> GetPupilsWithClass()
        {
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "SELECT Pupil_ID, Name, Surname, Patronymic, Class_Name FROM Pupils INNER JOIN Classes ON Pupils.Class_ID = Classes.Class_ID";

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
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string pupilClass = reader.GetString(4);

                        pupils.Add(new PupilWithClass(id, name, surname, patronymic, pupilClass));
                    }
                }

                reader.Close();
            }

            return pupils;
        }

        public static List<PupilWithClass> GetPupilsWithClassOrderBySurname()
        {
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "SELECT Pupil_ID, Name, Surname, Patronymic, Class_Name FROM Pupils INNER JOIN Classes ON Pupils.Class_ID = Classes.Class_ID ORDER BY Surname, Name, Patronymic";

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
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string pupilClass = reader.GetString(4);

                        pupils.Add(new PupilWithClass(id, name, surname, patronymic, pupilClass));
                    }
                }

                reader.Close();
            }

            return pupils;
        }

        public static List<PupilWithClass> GetPupilsWithClassOrderByClass()
        {
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "SELECT Pupil_ID, Name, Surname, Patronymic, Class_Name FROM Pupils INNER JOIN Classes ON Pupils.Class_ID = Classes.Class_ID ORDER BY LEN(Class_Name), Class_Name";

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
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string pupilClass = reader.GetString(4);

                        pupils.Add(new PupilWithClass(id, name, surname, patronymic, pupilClass));
                    }
                }

                reader.Close();
            }
            return pupils;
        }

        // Получаем полную информацыю об ученике из БД.
        public static PupilFullInformation GetPupilsFullInformation(int ID)
        {
            PupilFullInformation pupil = new PupilFullInformation();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "sp_GetFullInformation";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@Pupil_ID",
                    Value = ID
                };

                command.Parameters.Add(nameParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string pupilClass = reader.GetString(4);
                        DateTime date_Of_Birth = reader.GetDateTime(5);
                        int age = reader.GetInt32(6);
                        string pupil_Phone = reader.GetString(7);
                        string parent_Phone = reader.GetString(8);
                        string address = reader.GetString(9);

                        pupil = new PupilFullInformation(id, name, surname, patronymic, pupilClass, date_Of_Birth, age, pupil_Phone, parent_Phone, address);
                    }
                }

                reader.Close();
            }

            return pupil;
        }

        // Обновляем информацию об ученике в БД (класс изменяется).
        public static void UpdatePupilWithClass(int pupilId, string name, string surname, string patronymic, int classID)
        {
            PupilFullInformation pupil = new PupilFullInformation();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_UpdatePupilInf]";

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

                SqlParameter pupilClassIDParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = classID
                };

                command.Parameters.Add(pupilIDParam);
                command.Parameters.Add(pupilNameParam);
                command.Parameters.Add(pupilSurnameParam);
                command.Parameters.Add(pupilPatronymicParam);
                command.Parameters.Add(pupilClassIDParam);

                command.ExecuteNonQuery();
            }
        }

        // Обновляем информацыю об ученике в БД (класс не изменяется).
        public static void UpdatePupilWithotClass(int pupilId, string name, string surname, string patronymic)
        {

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_UpdatePupilInfWithoutClass]";

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

                command.Parameters.Add(pupilIDParam);
                command.Parameters.Add(pupilNameParam);
                command.Parameters.Add(pupilSurnameParam);
                command.Parameters.Add(pupilPatronymicParam);

                command.ExecuteNonQuery();
            }
        }

        // Обновить анкету ученика.
        public static void UpdatePupilFullInfo(int pupilId, DateTime dateOfBirth, string parentPhoneNumber, string pupilPhoneNumber, string address)
        {

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_UpdateInfInPupilForm]";

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

                SqlParameter DateOfBirthParam = new SqlParameter
                {
                    ParameterName = "@Date_Of_Birth",
                    Value = dateOfBirth
                };

                SqlParameter ParentPhoneNumberParam = new SqlParameter
                {
                    ParameterName = "@Parent_Phone_Number",
                    Value = parentPhoneNumber
                };

                SqlParameter PupilPhoneNumberParam = new SqlParameter
                {
                    ParameterName = "@Pupil_Phone_Number",
                    Value = pupilPhoneNumber
                };

                SqlParameter AddressParam = new SqlParameter
                {
                    ParameterName = "@Address",
                    Value = address
                };

                command.Parameters.Add(pupilIDParam);
                command.Parameters.Add(DateOfBirthParam);
                command.Parameters.Add(ParentPhoneNumberParam);
                command.Parameters.Add(PupilPhoneNumberParam);
                command.Parameters.Add(AddressParam);

                command.ExecuteNonQuery();
            }
        }

        // Удаление ученика.
        public static void DeletePupil(int pupilId)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_DeletePupil]";

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

                command.Parameters.Add(pupilIDParam);

                command.ExecuteNonQuery();
            }
        }

        // Добавление ученика.
        public static void AddPupil(string name, string surname, string patronymic, int classID)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_AddPupil]";

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

                SqlParameter pupilClassIDParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = classID
                };

                command.Parameters.Add(pupilNameParam);
                command.Parameters.Add(pupilSurnameParam);
                command.Parameters.Add(pupilPatronymicParam);
                command.Parameters.Add(pupilClassIDParam);

                command.ExecuteNonQuery();
            }
        }

        // Добавление ученика.
        public static void AddPupilForm(int pupilId, DateTime dateOfBirth, string parentPhoneNumber, string pupilPhoneNumber, string address)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_InsertInfInPupilForm]";

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

                SqlParameter DateOfBirthParam = new SqlParameter
                {
                    ParameterName = "@Date_Of_Birth",
                    Value = dateOfBirth
                };

                SqlParameter ParentPhoneNumberParam = new SqlParameter
                {
                    ParameterName = "@Parent_Phone_Number",
                    Value = parentPhoneNumber
                };

                SqlParameter PupilPhoneNumberParam = new SqlParameter
                {
                    ParameterName = "@Pupil_Phone_Number",
                    Value = pupilPhoneNumber
                };

                SqlParameter AddressParam = new SqlParameter
                {
                    ParameterName = "@Address",
                    Value = address
                };

                command.Parameters.Add(pupilIDParam);
                command.Parameters.Add(DateOfBirthParam);
                command.Parameters.Add(ParentPhoneNumberParam);
                command.Parameters.Add(PupilPhoneNumberParam);
                command.Parameters.Add(AddressParam);

                command.ExecuteNonQuery();
            }
        }

        // Получение ID последнего добавленного ученика.
        public static int GetPupilsLastID()
        {
            int id = 0;
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetPupilsLastID]";

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

        // Получение учеников в классе.
        public static List<Pupil> GetPupilsInClass(int id)
        {
            List<Pupil> pupils = new List<Pupil>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetPupilsInClass]";

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

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        int pupilId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        int classID = reader.GetInt32(4);

                        pupils.Add(new Pupil(pupilId, name, surname, patronymic, classID));
                    }
                }

                reader.Close();
            }

            return pupils;
        }

        // Получить ученика по оценке.
        public static int GetPupilIdByMark(int id)
        {
            int result = 0;

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetPupilByMark]";

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

        // Поиск учеников по ФИ.
        public static List<PupilWithClass> SearchPupilByTwoParameters(string part1, string part2)
        {
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_SearchPupilByTwoParameter]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter firstParam = new SqlParameter
                {
                    ParameterName = "@First",
                    Value = part1
                };

                SqlParameter secondParam = new SqlParameter
                {
                    ParameterName = "@Second",
                    Value = part2
                };

                command.Parameters.Add(firstParam);
                command.Parameters.Add(secondParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string pupilClass = reader.GetString(4);

                        pupils.Add(new PupilWithClass(id, name, surname, patronymic, pupilClass));
                    }
                }

                reader.Close();
            }

            return pupils;
        }

        // Поиск учеников по ФИО.
        public static List<PupilWithClass> SearchPupilByThreeParameters(string part1, string part2, string part3)
        {
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_SearchPupilByThreeParameter]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter firstParam = new SqlParameter
                {
                    ParameterName = "@First",
                    Value = part1
                };

                SqlParameter secondParam = new SqlParameter
                {
                    ParameterName = "@Second",
                    Value = part2
                };

                SqlParameter thirdParam = new SqlParameter
                {
                    ParameterName = "@Third",
                    Value = part3
                };

                command.Parameters.Add(firstParam);
                command.Parameters.Add(secondParam);
                command.Parameters.Add(thirdParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string pupilClass = reader.GetString(4);

                        pupils.Add(new PupilWithClass(id, name, surname, patronymic, pupilClass));
                    }
                }

                reader.Close();
            }

            return pupils;
        }

        // Поиск учеников по ФИ в классе.
        public static List<PupilWithClass> SearchPupilByTwoParametersAndClass(string part1, string part2, int classID)
        {
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_SearchPupilByTwoParameterAndClass]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter firstParam = new SqlParameter
                {
                    ParameterName = "@First",
                    Value = part1
                };

                SqlParameter secondParam = new SqlParameter
                {
                    ParameterName = "@Second",
                    Value = part2
                };

                SqlParameter classParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = classID
                };

                command.Parameters.Add(firstParam);
                command.Parameters.Add(secondParam);
                command.Parameters.Add(classParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string pupilClass = reader.GetString(4);

                        pupils.Add(new PupilWithClass(id, name, surname, patronymic, pupilClass));
                    }
                }

                reader.Close();
            }

            return pupils;
        }

        // Поиск учеников по ФИО в классе.
        public static List<PupilWithClass> SearchPupilByThreeParametersAndClass(string part1, string part2, string part3, int classID)
        {
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_SearchPupilByThreeParameterAndClass]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter firstParam = new SqlParameter
                {
                    ParameterName = "@First",
                    Value = part1
                };

                SqlParameter secondParam = new SqlParameter
                {
                    ParameterName = "@Second",
                    Value = part2
                };

                SqlParameter thirdParam = new SqlParameter
                {
                    ParameterName = "@Third",
                    Value = part3
                };

                SqlParameter classParam = new SqlParameter
                {
                    ParameterName = "@Class_ID",
                    Value = classID
                };

                command.Parameters.Add(firstParam);
                command.Parameters.Add(secondParam);
                command.Parameters.Add(thirdParam);
                command.Parameters.Add(classParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string pupilClass = reader.GetString(4);

                        pupils.Add(new PupilWithClass(id, name, surname, patronymic, pupilClass));
                    }
                }

                reader.Close();
            }

            return pupils;
        }

        // Получить коичества отсутствующих.
        public static int GetPupilAbsentCount(int id)
        {
            int result = 0;

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetPupilAbsentCount]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter markIDParam = new SqlParameter
                {
                    ParameterName = "@Pupil_ID",
                    Value = id
                };

                command.Parameters.Add(markIDParam);

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

        // Слабые предметы ученика.
        public static List<KeyValuePair<string, double>> GetPupilWeakSubjects(int id)
        {
            List<KeyValuePair<string, double>> result = new List<KeyValuePair<string, double>>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_PupilWeakSubjects]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter markIDParam = new SqlParameter
                {
                    ParameterName = "@Pupil_ID",
                    Value = id
                };

                command.Parameters.Add(markIDParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        string subjectName = reader.GetString(0);
                        double avgMark = Convert.ToDouble(reader.GetValue(1));

                        result.Add(new KeyValuePair<string, double>(subjectName, avgMark));
                    }
                }
                reader.Close();
            }

            return result;
        }

        // Сильные предметы ученика.
        public static List<KeyValuePair<string, double>> GetPupilStrongSubjects(int id)
        {
            List<KeyValuePair<string, double>> result = new List<KeyValuePair<string, double>>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_PupilStrongSubjects]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter markIDParam = new SqlParameter
                {
                    ParameterName = "@Pupil_ID",
                    Value = id
                };

                command.Parameters.Add(markIDParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        string subjectName = reader.GetString(0);
                        double avgMark = Convert.ToDouble(reader.GetValue(1));

                        result.Add(new KeyValuePair<string, double>(subjectName, avgMark));
                    }
                }
                reader.Close();
            }

            return result;
        }

        // Фильтрация учеников.
        public static List<PupilWithClass> GetFilterPupils(string expression)
        {
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = expression;

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
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        string pupilClass = reader.GetString(4);

                        pupils.Add(new PupilWithClass(id, name, surname, patronymic, pupilClass));
                    }
                }

                reader.Close();
            }

            return pupils;
        }

        public static double GetPupilsAvgMarkInMonth(int id, DateTime beginDate, DateTime endDate) 
        {
            double result = 0;

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_PupilAvgMarkInMonth]";

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

                SqlParameter beginDateParam = new SqlParameter
                {
                    ParameterName = "@BeginDate",
                    Value = beginDate
                };

                SqlParameter endDateParam = new SqlParameter
                {
                    ParameterName = "@EndDate",
                    Value = endDate
                };

                command.Parameters.Add(pupilIDParam);
                command.Parameters.Add(beginDateParam);
                command.Parameters.Add(endDateParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        result = Convert.ToDouble(reader.GetValue(0));
                    }
                }
                reader.Close();
            }

            return result;
        }

        public static double GetPupilsSubjectAvgMark(int id, int subjectID)
        {
            double result = 0;

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_PupilSubjectAvgMark]";

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
                        result = Convert.ToDouble(reader.GetValue(0));
                    }
                }
                reader.Close();
            }

            return result;
        }
    }
}
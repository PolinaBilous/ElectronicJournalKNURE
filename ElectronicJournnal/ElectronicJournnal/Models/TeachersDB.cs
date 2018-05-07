using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicJournnal.Models
{
    public class TeachersDB
    {
        // Добавление учителя.
        public static void AddTeacher(string name, string surname, string patronymic)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_AddTeacher]";

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

                command.Parameters.Add(pupilNameParam);
                command.Parameters.Add(pupilSurnameParam);
                command.Parameters.Add(pupilPatronymicParam);

                command.ExecuteNonQuery();
            }
        }

        //Добавление анкеты учителя.
        public static void AddTeacherForm(int teacherID, DateTime date_Of_Birth, int experience, string address, string phone_Number, string email)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_InsertInfInTeacherForm]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter teacherIdParam = new SqlParameter
                {
                    ParameterName = "@Teacher_ID",
                    Value = teacherID
                };

                SqlParameter DateOfBirthParam = new SqlParameter
                {
                    ParameterName = "@Date_Of_Birth",
                    Value = date_Of_Birth
                };

                SqlParameter experienceParam = new SqlParameter
                {
                    ParameterName = "@Expirience",
                    Value = experience
                };

                SqlParameter AddressParam = new SqlParameter
                {
                    ParameterName = "@Address",
                    Value = address
                };

                SqlParameter PhoneNumberParam = new SqlParameter
                {
                    ParameterName = "@Phone_Number",
                    Value = phone_Number
                };

                SqlParameter EmailParam = new SqlParameter
                {
                    ParameterName = "@Email",
                    Value = email
                };

                command.Parameters.Add(teacherIdParam);
                command.Parameters.Add(DateOfBirthParam);
                command.Parameters.Add(AddressParam);
                command.Parameters.Add(experienceParam);
                command.Parameters.Add(PhoneNumberParam);
                command.Parameters.Add(EmailParam);

                command.ExecuteNonQuery();
            }
        }

        // Полученик ID последнего добавленного учителя.
        public static int GetTeachersLastIndex()
        {
            int id = 0;
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetTeachersLastID]";

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

        // Получение краткой информации про всех учителей.
        public static List<TeacherShortInf> GetTeachersShortInf()
        {
            List<TeacherShortInf> teachers = new List<TeacherShortInf>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetTeacherShortInf]";

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
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        int experience = reader.GetInt32(4);

                        teachers.Add(new TeacherShortInf(id, name, surname, patronymic, experience));
                    }
                }
                reader.Close();
            }
            return teachers;
        }

        // Получение полной информации про учителя.
        public static TeacherFullInformation GetTeacherFullInformation(int id)
        {
            TeacherFullInformation teacher = new TeacherFullInformation();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetTeacherFullInf]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@Teacher_ID",
                    Value = id
                };

                command.Parameters.Add(nameParam);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        int ID = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        DateTime date_Of_Birth = reader.GetDateTime(4);
                        int age = reader.GetInt32(5);
                        int experience = reader.GetInt32(6);
                        string address = reader.GetString(7);
                        string phone_Number = reader.GetString(8);
                        string email = reader.GetString(9);

                        teacher = new TeacherFullInformation(id, name, surname, patronymic, date_Of_Birth, age, experience, address, phone_Number, email);
                    }
                }

                reader.Close();
            }

            return teacher;
        }

        // Удаление учителя.
        public static void DeleteTeacher(int teacherId)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_DeleteTeacher]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter pupilIDParam = new SqlParameter
                {
                    ParameterName = "@TeacherId",
                    Value = teacherId
                };

                command.Parameters.Add(pupilIDParam);

                command.ExecuteNonQuery();
            }
        }

        // Обновление информации про учителя.
        public static void UpdateTeacher(int teacherId, string name, string surname, string patronymic)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_UpdateTeacherInf]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter teacherIDParam = new SqlParameter
                {
                    ParameterName = "@Teacher_ID",
                    Value = teacherId
                };

                SqlParameter teacherNameParam = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = name
                };

                SqlParameter teacherSurnameParam = new SqlParameter
                {
                    ParameterName = "@Surname",
                    Value = surname
                };

                SqlParameter teacherPatronymicParam = new SqlParameter
                {
                    ParameterName = "@Patronymic",
                    Value = patronymic
                };

                command.Parameters.Add(teacherIDParam);
                command.Parameters.Add(teacherNameParam);
                command.Parameters.Add(teacherSurnameParam);
                command.Parameters.Add(teacherPatronymicParam);

                command.ExecuteNonQuery();
            }
        }

        // Обновление анкеты учителя.
        public static void UpdateTeacherFullInf(int teacherId, DateTime dateOfBirth, int experience, string phoneNumber, string address, string email)
        {
            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_UpdateInfInTeacherForm]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter teacherIDParam = new SqlParameter
                {
                    ParameterName = "@TeacherID",
                    Value = teacherId
                };

                SqlParameter dateOfBirthParam = new SqlParameter
                {
                    ParameterName = "@Date_Of_Birth",
                    Value = dateOfBirth
                };

                SqlParameter teacherPhoneNumberParam = new SqlParameter
                {
                    ParameterName = "@Phone_Number",
                    Value = phoneNumber
                };

                SqlParameter teacherAddressParam = new SqlParameter
                {
                    ParameterName = "@Address",
                    Value = address
                };

                SqlParameter teacherExperienceParam = new SqlParameter
                {
                    ParameterName = "@Experience",
                    Value = experience
                };

                SqlParameter teacherEmailParam = new SqlParameter
                {
                    ParameterName = "@Email",
                    Value = email
                };

                command.Parameters.Add(teacherIDParam);
                command.Parameters.Add(dateOfBirthParam);
                command.Parameters.Add(teacherPhoneNumberParam);
                command.Parameters.Add(teacherAddressParam);
                command.Parameters.Add(teacherExperienceParam);
                command.Parameters.Add(teacherEmailParam);

                command.ExecuteNonQuery();
            }
        }

        // Сортировка учителей по ФИО.
        public static List<TeacherShortInf> GetTeacherShortInfOrderBySurname()
        {
            List<TeacherShortInf> teachers = new List<TeacherShortInf>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetTeacherShortInfOrderBySurname]";

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
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        int experience = reader.GetInt32(4);

                        teachers.Add(new TeacherShortInf(id, name, surname, patronymic, experience));
                    }
                }
                reader.Close();
            }
            return teachers;
        }

        // Сортировка учителей по стажу работы.
        public static List<TeacherShortInf> GetTeachersShortInfOrderByExperience()
        {
            List<TeacherShortInf> teachers = new List<TeacherShortInf>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_GetTeacherShortInfOrderByExperience]";

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
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string patronymic = reader.GetString(3);
                        int experience = reader.GetInt32(4);

                        teachers.Add(new TeacherShortInf(id, name, surname, patronymic, experience));
                    }
                }
                reader.Close();
            }
            return teachers;
        }

        // Поиск уцчителей по ФИО.
        public static List<TeacherShortInf> SearchTeacherByThreeParameters(string part1, string part2, string part3)
        {
            List<TeacherShortInf> teachers = new List<TeacherShortInf>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_SearchTeacherByThreeParameter]";

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
                        int experience = reader.GetInt32(4);

                        teachers.Add(new TeacherShortInf(id, name, surname, patronymic, experience));
                    }
                }
                reader.Close();
            }
            return teachers;
        }

        // Поиск учителей по ФИ.
        public static List<TeacherShortInf> SearchTeacherByTwoParameters(string part1, string part2)
        {
            List<TeacherShortInf> teachers = new List<TeacherShortInf>();

            string connectionString = @"Data Source=DESKTOP-81JSABO\SQLEXPRESS;Initial Catalog=ElectronicJournal;Integrated Security=True";
            string sqlExpression = "[sp_SearchTeacherByTwoParameter]";

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
                        int experience = reader.GetInt32(4);

                        teachers.Add(new TeacherShortInf(id, name, surname, patronymic, experience));
                    }
                }
                reader.Close();
            }
            return teachers;
        }
    }
}
using ElectronicJournnal.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ElectronicJournnal.Controllers
{
    public class TeacherController : Controller
    {
        public ActionResult TeacherIndex()
        {
            return View();
        }

        #region PupilsActions

        // Вывод вывод учеников.
        [HttpGet]
        public ActionResult ViewPupils()
        {
            List<PupilWithClass> p = PupilsDB.GetPupilsWithClass();
            List<Class> classes = ClassesDB.GetClasses();
            ViewBag.Classes = classes;
            return View(p);
        }

        // Переход на просмотр информации про ученика или удаление.
        [HttpPost]
        public ActionResult ViewPupils(int action = 0, int delete = 0, int marks = 0)
        {
            if (action != 0)
            {
                return RedirectToAction("PupilFullInf", "Teacher", new { id = action });
            }
            else if (delete != 0)
            {
                PupilsDB.DeletePupil(delete);
                return RedirectToAction("ViewPupils", "Teacher");
            }
            else
            {
                return RedirectToAction("ViewPupilMarks", "Teacher", new { pupilID = marks});
            }
        }

        public ActionResult ViewPupilMarks(int pupilID)
        {
            PupilFullInformation pupil = PupilsDB.GetPupilsFullInformation(pupilID);
            ViewBag.Pupil = pupil;
            ViewData["CurrentPupil"] = pupil.Pupil_ID;

            List<Subject> subjects = SubjectsDB.GetPupilSubjects(pupil.Pupil_ID);
            ViewBag.Subjects = subjects;

            List<List<Mark>> marks = new List<List<Mark>>();

            for (int i = 0; i < subjects.Count; i++)
            {
                marks.Add(MarksDB.GetPupilSubjectMark(pupil.Pupil_ID, subjects[i].Subject_ID));
            }

            ViewBag.Marks = marks;

            return View();
        }

        public ActionResult GetPupilMarksReport(int pupilID)
        {
            PupilFullInformation pupil = PupilsDB.GetPupilsFullInformation(pupilID);

            List<Subject> subjects = SubjectsDB.GetPupilSubjects(pupil.Pupil_ID);

            List<List<Mark>> marks = new List<List<Mark>>();

            for (int i = 0; i < subjects.Count; i++)
            {
                marks.Add(MarksDB.GetPupilSubjectMark(pupil.Pupil_ID, subjects[i].Subject_ID));
            }

            new ElectronicJournnal.Models.GeneratePupilMarksReport().CreatePackage(@"E:\Uni\Course Project\ElectronicJournnal\ElectronicJournnal\Files\CurrentPupilMarksReport.docx", pupil, subjects, marks);
            string file_path = Server.MapPath("~/Files/CurrentPupilMarksReport.docx"); ;
            string file_type = "application/docx";
            string file_name = "Оценки " + pupil.Surname + " " + pupil.Name + ".docx";
            return File(file_path, file_type, file_name);
        }

        // Просмотр полной информации про ученика.
        [HttpGet]
        public ActionResult PupilFullInf(int id)
        {
            PupilFullInformation p = PupilsDB.GetPupilsFullInformation(id);
            return View(p);
        }

        // Сохранение отредактированной информации про ученика.
        [HttpPost]
        public ActionResult SavePupilInformation(PupilFullInformation p, string cancel)
        {
            if (cancel != "cancel")
            {
                if (p.Class != null)
                {
                    PupilsDB.UpdatePupilWithClass(p.Pupil_ID, p.Name, p.Surname, p.Patronymic, Convert.ToInt32(p.Class));

                }
                else
                {
                    PupilsDB.UpdatePupilWithotClass(p.Pupil_ID, p.Name, p.Surname, p.Patronymic);
                }
                PupilsDB.UpdatePupilFullInfo(p.Pupil_ID, p.Date_Of_Birth, p.Parent_Phone_Number, p.Pupil_Phone_Number, p.Address);
                return RedirectToActionPermanent("ViewPupils", "Teacher");
            }
            else
            {
                return RedirectToActionPermanent("PupilFullInf", "Teacher", new { id = p.Pupil_ID });
            }
        }

        // Получение отчета "Анкета ученика"
        [HttpPost]
        public FileResult GetPupilReport(int action)
        {
            PupilFullInformation p = PupilsDB.GetPupilsFullInformation(action);
            string age = p.Age.ToString() + " лет";
            new Models.GeneratedCode.GenerateReport().CreatePackage(@"E:\Uni\Course Project\ElectronicJournnal\ElectronicJournnal\Files\CurrentPupilReport.docx", p.Name, p.Surname, p.Patronymic, p.Class, p.Date_Of_Birth.ToLongDateString(), age, p.Parent_Phone_Number, p.Pupil_Phone_Number, p.Address);
            string file_path = Server.MapPath("~/Files/CurrentPupilReport.docx"); ;
            string file_type = "application/docx";
            string name = "Анкета " + p.Surname + " " + p.Name + ".docx";
            string file_name = name;
            return File(file_path, file_type, file_name);
        }

        // Редактирование информации про ученика.
        public ActionResult EditPupilInformation(int action)
        {
            PupilFullInformation p = PupilsDB.GetPupilsFullInformation(action);
            List<Class> classesList = ClassesDB.GetClassesWithoutCurr(p.Class);
            SelectList classesSL = new SelectList(classesList, "Class_ID", "Class_Name");
            ViewBag.Classes = classesSL;
            return View(p);
        }

        // Добавление нового ученика.
        public ActionResult AddPupilForm()
        {
            List<Class> classesList = ClassesDB.GetClasses();
            SelectList classesSL = new SelectList(classesList, "Class_ID", "Class_Name");
            ViewBag.Classes = classesSL;
            return View();
        }

        // Сохранение нового ученика.
        public ActionResult AddNewPupil(PupilFullInformation p)
        {
            PupilsDB.AddPupil(p.Name, p.Surname, p.Patronymic, Convert.ToInt32(p.Class));
            int pupilId = PupilsDB.GetPupilsLastID();
            PupilsDB.AddPupilForm(pupilId, p.Date_Of_Birth, p.Parent_Phone_Number, p.Pupil_Phone_Number, p.Address);
            return RedirectToActionPermanent("ViewPupils", "Teacher");
        }

        // Сортировка учеников по ФИО.
        public ActionResult GetPupilsOrderBySurname()
        {
            List<PupilWithClass> pupils = PupilsDB.GetPupilsWithClassOrderBySurname();
            return PartialView(pupils);
        }

        // Сортировка учеников по классу.
        public ActionResult GetPupilsOrderByClass()
        {
            List<PupilWithClass> pupils = PupilsDB.GetPupilsWithClassOrderByClass();
            return PartialView(pupils);
        }

        // Поиск учеников по ФИО.
        public ActionResult SearchPupilsBySurname(string Person)
        {
            string[] parts = Person.Split(' ');
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            if (parts.Length == 3)
            {
                string part1 = parts[0];
                string part2 = parts[1];
                string part3 = parts[2];

                pupils = PupilsDB.SearchPupilByThreeParameters(part1, part2, part3);
            }
            else if (parts.Length == 2)
            {
                string part1 = parts[0];
                string part2 = parts[1];

                pupils = PupilsDB.SearchPupilByTwoParameters(part1, part2);
            }

            ViewBag.Pupils = PupilsDB.GetPupilsWithClass();
            if (pupils.Count() != 0)
                ViewBag.Pupil = pupils[0];

            return PartialView(pupils);
        }

        // "Ко всем".
        public ActionResult ToAllPupils()
        {
            return RedirectToActionPermanent("ViewPupils", "Teacher");
        }

        // Фильтрация учеников.
        public ActionResult FilterPupils(int Class_ID, int to = 0, int from = 0)
        {
            StringBuilder expr = new StringBuilder();
            expr.Append("SELECT Pupils.Pupil_ID, Pupils.Name, Pupils.Surname, Pupils.Patronymic, Classes.Class_Name FROM Pupils, Marks, Types_Of_Work, Classes WHERE Pupils.Pupil_ID = Marks.Pupil_ID AND Marks.TypeOfWork_ID = Types_Of_Work.TypeOfWork_ID AND Pupils.Class_ID = Classes.Class_ID AND Marks.Mark IS NOT NULL GROUP BY Pupils.Pupil_ID, Pupils.Name, Pupils.Surname, Pupils.Patronymic, Classes.Class_Name ");

            if (Class_ID == 0 && to == 0 && from == 0)
            {
                expr.Append("");
            }
            else if (Class_ID != 0 && to == 0 && from == 0)
            {
                string className = ClassesDB.GetClassFullInf(Class_ID).Class_Name;
                expr.Append($"HAVING Classes.Class_Name = '{className}'");
            }
            else if (Class_ID != 0 && from != 0 && to == 0)
            {
                string className = ClassesDB.GetClassFullInf(Class_ID).Class_Name;
                expr.Append($"HAVING (CAST(CAST(SUM (Marks.Mark * Coefficient) AS NUMERIC) / SUM(Coefficient) AS NUMERIC(10,2))) >= {from} AND Classes.Class_Name = '{className}' ");
            }
            else if (Class_ID != 0 && from == 0 && to != 0)
            {
                string className = ClassesDB.GetClassFullInf(Class_ID).Class_Name;
                expr.Append($"HAVING (CAST(CAST(SUM (Marks.Mark * Coefficient) AS NUMERIC) / SUM(Coefficient) AS NUMERIC(10,2))) <= {to} AND Classes.Class_Name = '{className}' ");
            }
            else if (Class_ID != 0 && from != 0 && to != 0)
            {
                string className = ClassesDB.GetClassFullInf(Class_ID).Class_Name;
                expr.Append($"HAVING (CAST(CAST(SUM (Marks.Mark * Coefficient) AS NUMERIC) / SUM(Coefficient) AS NUMERIC(10,2))) >= {from} AND (CAST(CAST(SUM(Marks.Mark * Coefficient) AS NUMERIC) / SUM(Coefficient) AS NUMERIC(10, 2))) <= {to} AND Classes.Class_Name = '{className}' ");
            }
            else if (Class_ID == 0 && from != 0 && to == 0)
            {
                expr.Append($"HAVING (CAST(CAST(SUM (Marks.Mark * Coefficient) AS NUMERIC) / SUM(Coefficient) AS NUMERIC(10,2))) >= {from}");
            }
            else if (Class_ID == 0 && from == 0 && to != 0)
            {
                expr.Append($"HAVING (CAST(CAST(SUM (Marks.Mark * Coefficient) AS NUMERIC) / SUM(Coefficient) AS NUMERIC(10,2))) <= {to}");
            }
            else if (Class_ID == 0 && from != 0 && to != 0)
            {
                expr.Append($"HAVING (CAST(CAST(SUM (Marks.Mark * Coefficient) AS NUMERIC) / SUM(Coefficient) AS NUMERIC(10,2))) >= {from} AND(CAST(CAST(SUM(Marks.Mark * Coefficient) AS NUMERIC) / SUM(Coefficient) AS NUMERIC(10, 2))) <= {to}");
            }

            List<PupilWithClass> pupils = PupilsDB.GetFilterPupils(expr.ToString());

            return PartialView(pupils);
        }

        #endregion 

        #region TeacherActions

        public ActionResult ViewTeachers()
        {
            List<TeacherShortInf> t = TeachersDB.GetTeachersShortInf();
            return View(t);
        }

        [HttpPost]
        public ActionResult ViewTeachers(int action = 0, int delete = 0)
        {
            if (action != 0)
            {
                return RedirectToAction("TeacherFullInf", "Teacher", new { id = action });
            }
            else
            {
                TeachersDB.DeleteTeacher(delete);
                return RedirectToAction("ViewTeachers", "Teacher");
            }
        }

        [HttpPost]
        public ActionResult TeacherInfo(int action = 0, int delete = 0)
        {
            if (action != 0)
            {
                return RedirectToAction("TeacherFullInf", "Teacher", new { id = action });
            }
            else
            {
                TeachersDB.DeleteTeacher(delete);
                return RedirectToAction("ViewTeachers", "Teacher");
            }
        }

        public ActionResult TeacherFullInf(int id)
        {
            TeacherFullInformation t = TeachersDB.GetTeacherFullInformation(id);
            return View(t);
        }

        public ActionResult EditTeacherInformation(int action)
        {
            TeacherFullInformation t = TeachersDB.GetTeacherFullInformation(action);
            return View(t);
        }

        public ActionResult SaveTeacherFullInformation(TeacherFullInformation t, string cancel)
        {
            if (cancel != "cancel")
            {
                TeachersDB.UpdateTeacher(t.Teacher_ID, t.Name, t.Surname, t.Patronymic);
                TeachersDB.UpdateTeacherFullInf(t.Teacher_ID, t.Date_Of_Birth, t.Experience, t.Phone_Number, t.Address, t.Email);
                return RedirectToActionPermanent("ViewTeachers", "Teacher");
            }
            else
            {
                return RedirectToActionPermanent("TeacherFullInf", "Teacher", new { id = t.Teacher_ID});
            }
        }

        public ActionResult AddTeacherForm()
        {
            return View();
        }

        public ActionResult AddNewTeacher(TeacherFullInformation t)
        {
            TeachersDB.AddTeacher(t.Name, t.Surname, t.Patronymic);
            int teacherID = TeachersDB.GetTeachersLastIndex();
            TeachersDB.AddTeacherForm(teacherID, t.Date_Of_Birth, t.Experience, t.Address, t.Phone_Number, t.Email);
            return RedirectToActionPermanent("ViewTeachers", "Teacher");
        }

        public ActionResult GetTeachersOrderBySurname()
        {
            List<TeacherShortInf> teachers = TeachersDB.GetTeacherShortInfOrderBySurname();
            return PartialView(teachers);
        }

        public ActionResult GetTeachersOrderByExperience()
        {
            List<TeacherShortInf> teachers = TeachersDB.GetTeachersShortInfOrderByExperience();
            return PartialView(teachers);
        }

        public ActionResult SearchTeachersBySurname(string Person)
        {
            string[] parts = Person.Split(' ');
            List<TeacherShortInf> teachers = new List<TeacherShortInf>();

            if (parts.Length == 3)
            {
                string part1 = parts[0];
                string part2 = parts[1];
                string part3 = parts[2];

                teachers = TeachersDB.SearchTeacherByThreeParameters(part1, part2, part3);
            }
            else if (parts.Length == 2)
            {
                string part1 = parts[0];
                string part2 = parts[1];

                teachers = TeachersDB.SearchTeacherByTwoParameters(part1, part2);
            }

            ViewBag.Teachers = TeachersDB.GetTeachersShortInf();

            if (teachers.Count != 0)
            {
                ViewBag.Teacher = teachers[0];
            }

            return PartialView(teachers);
        }

        public ActionResult ToAllTeachers()
        {
            return RedirectToActionPermanent("ViewTeachers", "Teacher");
        }
        
        #endregion

        #region ClassActions

        public ActionResult ViewClasses()
        {
            List<Class> classes = ClassesDB.GetClasses(); 
            return View(classes);
        }

        [HttpPost]
        public ActionResult ViewClasses(int action = 0, int delete = 0)
        {
            if (action != 0)
            {
                return RedirectToAction("ClassFullInf", "Teacher", new { id = action });
            }
            else
            {
                ClassesDB.DeleteClass(delete);
                return RedirectToAction("ViewClasses", "Teacher");
            }
        }

        public ActionResult ClassFullInf(int id)
        {
            ClassWithManagment Class = ClassesDB.GetClassFullInf(id);
            List<Pupil> pupils = PupilsDB.GetPupilsInClass(id);
            ViewBag.Pupils = pupils;
            ViewBag.ClassId = id;
            return View(Class);
        }

        [HttpPost]
        public ActionResult ClassFullInf(int ClassID, int action = 0, int delete = 0)
        {
            if (action != 0)
            {
                return RedirectToAction("PupilFullInf", "Teacher", new { id = action });
            }
            else
            {
                PupilsDB.DeletePupil(delete);
                return RedirectToAction("ClassFullInf", "Teacher", new { id = ClassID});
            }
        }

        public ActionResult EditClassInformation(int action)
        {
            ClassWithManagment Class = ClassesDB.GetClassFullInf(action);
            List<TeacherShortInf> teachers = TeachersDB.GetTeachersShortInf();
            ViewBag.Teachers = teachers;
            return View(Class);
        }

        public ActionResult SaveClassInformation(ClassWithManagment c, int action = 0, int cancel = 0)
        {
            if (action == 0)
                return RedirectToActionPermanent("ClassFullInf", "Teacher", new { id = c.Class_ID });
            else
            {
                ClassesDB.UpdateClass(c.Class_ID, c.Formation_Date, c.Class_Name);

                ClassWithManagment cl = ClassesDB.GetClassFullInf(c.Class_ID);
                if (cl.Teacher_ID != c.Teacher_ID)
                {
                    ClassesDB.FinishClassManagment(c.ClassManagment_ID);
                    ClassesDB.AddClassManagment(c.Class_ID, c.Teacher_ID);
                }

                return RedirectToActionPermanent("ClassFullInf", "Teacher", new { id = c.Class_ID });
            }
        }

        public ActionResult AddClassForm()
        {
            List<TeacherShortInf> teachers = TeachersDB.GetTeachersShortInf();
            ViewBag.Teachers = teachers;
            return View();
        }

        public ActionResult AddNewClass(ClassWithManagment c)
        {
            ClassesDB.AddClass(c.Class_Name, DateTime.Now);
            ClassesDB.AddClassManagment(ClassesDB.GetClassId(c.Class_Name), c.Teacher_ID);
            return RedirectToActionPermanent("ViewClasses", "Teacher");
        }

        #endregion

        #region LessonActions

        public ActionResult ViewLessons()
        {
            List<DateTime> dates = LessonsDB.GetLessonsDates();
            List<List<LessonFullInf>> lessons = new List<List<LessonFullInf>>();
            int lessonsCount = LessonsDB.GetLessonsCount();

            for (int i = 0; i < dates.Count(); i++)
            {
                lessons.Add(LessonsDB.GetLessonsInDate(dates[i]));
            }

            List<Class> classes = ClassesDB.GetClasses();

            List<Subject> subjects = SubjectsDB.GetSubjects();

            ViewBag.Subjects = subjects;
            ViewBag.Classes = classes;
            ViewBag.Dates = dates;
            ViewBag.Lessons = lessons;

            return View();
        }

        [HttpPost]
        public ActionResult ViewLessons(int action = 0, int delete = 0, int addMark = 0, int absent = 0)
        {
            if (delete != 0)
            {
                LessonsDB.DeleteLesson(delete);
                return RedirectToAction("ViewLessons", "Teacher");
            }
            else if (action != 0)
            {
                return RedirectToAction("EditLesson", "Teacher", new { lessonID = action });
            }
            else if (addMark != 0)
            {
                return RedirectToAction("AddMarkForm", "Teacher", new { lessonID = addMark });
            }
            else
            {
                return RedirectToAction("SelectAbsentPupils", "Teacher", new { lessonID = absent });
            }
        }

        public ActionResult EditLesson(int lessonID)
        {
            LessonFullInf lesson = LessonsDB.GetLessonFullInf(lessonID);
            List<Class> classes = ClassesDB.GetClasses();
            ViewBag.Classes = classes;
            List<TeacherShortInf> teachers = TeachersDB.GetTeachersShortInf();
            ViewBag.Teachers = teachers;
            List<Subject> subjects = SubjectsDB.GetSubjects();
            ViewBag.Subjects = subjects;
            return View(lesson);
        }

        [HttpPost]
        public ActionResult SaveLessonInformation(LessonFullInf lesson, int action = 0, int cancel = 0)
        {
            if (action == 0)
            {
                return RedirectToAction("ViewLessons", "Teacher");
            }
            else
            {
                LessonsDB.UpdateLessonInf(lesson.Lesson_ID, lesson.Class_ID, lesson.Teacher_ID, lesson.Subject_ID, lesson.Date);
                return RedirectToAction("ViewLessons", "Teacher");
            }
        }

        public ActionResult AddLessonForm()
        {
            List<Class> classes = ClassesDB.GetClasses();
            ViewBag.Classes = classes;
            List<TeacherShortInf> teachers = TeachersDB.GetTeachersShortInf();
            ViewBag.Teachers = teachers;
            List<Subject> subjects = SubjectsDB.GetSubjects();
            ViewBag.Subjects = subjects;
            return View();
        }

        [HttpPost]
        public ActionResult AddLessonForm(Lesson lesson)
        {
            LessonsDB.AddLesson(lesson.Class_ID, lesson.Teacher_ID, lesson.Subject_ID, lesson.Date);
            return RedirectToAction("ViewLessons", "Teacher");
        }

        public ActionResult SelectAbsentPupils(int lessonID)
        {
            LessonFullInf lesson = LessonsDB.GetLessonFullInf(lessonID);

            List<Pupil> pupils = PupilsDB.GetPupilsInClass(lesson.Class_ID);

            TeacherController.classID = lesson.Class_ID;

            ViewBag.Pupils = pupils;

            ViewData["Lesson"] = lesson.Lesson_ID;

            // список ID учеников, которые были ранее отмечены как отсутствующие на данном уроке
            List<int> previosAbsent = LessonsDB.GetAbsentPupils(lessonID);
            ViewBag.AbsentPupils = previosAbsent;

            // удаляем всех отсутствующих отмеченных ранее
            MarksDB.DeleteAbsentPupilsMark(lessonID);

            return View();
        }

        [HttpPost]
        public ActionResult SelectAbsentPupils(int lesson, List<int> Absent)
        {
            if (Absent != null)
            {
                for (int i = 0; i < Absent.Count; i++)
                {
                    MarksDB.AddAbsentMark(Absent[i], lesson);
                }
            }

            int classID = TeacherController.classID;

            return RedirectToAction("ViewMarks", "Teacher", new { id = classID });
        }

        public ActionResult ToAllLessons()
        {
            return RedirectToAction("ViewLessons", "Teacher");
        }

        public ActionResult FilterLessons(int Class_ID, string from, string to)
        {
            StringBuilder exprForDate = new StringBuilder();
            StringBuilder exprForLessons = new StringBuilder();

            exprForDate.Append("SELECT DISTINCT Lesson_Date FROM Lessons ");
            exprForLessons.Append("Lesson_ID, Lessons.Class_ID, Class_Name, Lessons.Teacher_ID, Name, Surname, Patronymic, Lessons.Subject_ID, Subject_Name, Lesson_Date ");
            exprForLessons.Append("FROM Lessons, Classes, Teachers, Subjects WHERE Classes.Class_ID = Lessons.Class_ID AND ");
            exprForLessons.Append("	  Teachers.Teacher_ID = Lessons.Teacher_ID AND Subjects.Subject_ID = Lessons.Subject_ID ");

            List<DateTime> dates = new List<DateTime>();
            List<List<LessonFullInf>> lessons = new List<List<LessonFullInf>>();

            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();

            try
            {
                fromDate = Convert.ToDateTime(from);
            }
            catch
            {
                from = "00.00.0001";
            }

            try
            {
                toDate = Convert.ToDateTime(to);
            }
            catch
            {
                to = "00.00.0001";
            }

            if (Class_ID !=0 && from != "01.01.0001" && to != "00.00.0001")
            {
                exprForDate.Append($"WHERE Lesson_Date >= '{fromDate.ToString("yyyy-MM-dd")}' AND Lesson_Date <= '{toDate.ToString("yyyy-MM-dd")}'");
                dates = LessonsDB.GetFilterDates(exprForDate.ToString());
                for (int i = 0; i < dates.Count(); i++)
                {
                    lessons.Add(LessonsDB.GetLessonsInDateAndClass(dates[i], Class_ID));
                }
                for (int i = 0; i < dates.Count(); i++)
                {
                    if (lessons[i].Count() == 0)
                    {
                        dates.RemoveAt(i);
                        lessons.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (Class_ID != 0 && from == "01.01.0001" && to != "00.00.0001")
            {
                exprForDate.Append($"WHERE Lesson_Date <= '{toDate.ToString("yyyy-MM-dd")}'");
                dates = LessonsDB.GetFilterDates(exprForDate.ToString());
                for (int i = 0; i < dates.Count(); i++)
                {
                    lessons.Add(LessonsDB.GetLessonsInDateAndClass(dates[i], Class_ID));
                }
                for (int i = 0; i < dates.Count(); i++)
                {
                    if (lessons[i].Count() == 0)
                    {
                        dates.RemoveAt(i);
                        lessons.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (Class_ID != 0 && from != "01.01.0001" && to == "00.00.0001")
            {
                exprForDate.Append($"WHERE Lesson_Date >= '{fromDate.ToString("yyyy-MM-dd")}' ");
                dates = LessonsDB.GetFilterDates(exprForDate.ToString());
                for (int i = 0; i < dates.Count(); i++)
                {
                    lessons.Add(LessonsDB.GetLessonsInDateAndClass(dates[i], Class_ID));
                }
                for (int i = 0; i < dates.Count(); i++)
                {
                    if (lessons[i].Count() == 0)
                    {
                        dates.RemoveAt(i);
                        lessons.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (Class_ID != 0 && from == "01.01.0001" && to == "00.00.0001")
            {
                dates = LessonsDB.GetFilterDates(exprForDate.ToString());
                for (int i = 0; i < dates.Count(); i++)
                {
                    lessons.Add(LessonsDB.GetLessonsInDateAndClass(dates[i], Class_ID));
                }
                for (int i = 0; i < dates.Count(); i++)
                {
                    if (lessons[i].Count() == 0)
                    {
                        dates.RemoveAt(i);
                        lessons.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (Class_ID == 0 && from != "01.01.0001" && to != "00.00.0001")
            {
                exprForDate.Append($"WHERE Lesson_Date >= '{fromDate.ToString("yyyy-MM-dd")}' AND Lesson_Date <= '{toDate.ToString("yyyy-MM-dd")}'");
                dates = LessonsDB.GetFilterDates(exprForDate.ToString());
                for (int i = 0; i < dates.Count(); i++)
                {
                    lessons.Add(LessonsDB.GetLessonsInDate(dates[i]));
                }
            }
            if (Class_ID == 0 && from == "01.01.0001" && to != "00.00.0001")
            {
                exprForDate.Append($"WHERE Lesson_Date <= '{toDate.ToString("yyyy-MM-dd")}'");
                dates = LessonsDB.GetFilterDates(exprForDate.ToString());
                for (int i = 0; i < dates.Count(); i++)
                {
                    lessons.Add(LessonsDB.GetLessonsInDate(dates[i]));
                }
            }
            if (Class_ID == 0 && from != "01.01.0001" && to == "00.00.0001")
            {
                exprForDate.Append($"WHERE Lesson_Date >= '{fromDate.ToString("yyyy-MM-dd")}' ");
                dates = LessonsDB.GetFilterDates(exprForDate.ToString());
                for (int i = 0; i < dates.Count(); i++)
                {
                    lessons.Add(LessonsDB.GetLessonsInDate(dates[i]));
                }
            }

            ViewBag.Dates = dates;
            ViewBag.Lessons = lessons;

            return PartialView();
        }

        #endregion

        #region MarkActions

        public ActionResult ChooseClass()
        {
            List<Class> classes = ClassesDB.GetClasses();
            return View(classes);
        }

        static int classID { get; set; }

        public ActionResult ViewMarks(int id)
        {
            classID = id;

            List<Pupil> pupils = PupilsDB.GetPupilsInClass(id);

            // Список учеников.
            ViewBag.Pupils = pupils;

            List<List<Subject>> subjects = new List<List<Subject>>();

            for (int i = 0; i < pupils.Count; i++)
            {
                subjects.Add(SubjectsDB.GetPupilSubjects(pupils[i].Pupil_ID));
            }

            // Предметы по которым у ученика есть оценки.
            ViewBag.Subjects = subjects;

            List<List<List<Mark>>> marks = new List<List<List<Mark>>>();

            for (int i = 0; i < pupils.Count; i++)
            {
                marks.Add(new List<List<Mark>>(subjects[i].Count));
                for (int j = 0; j < subjects[i].Count; j++)
                {
                    marks[i].Add(MarksDB.GetPupilSubjectMark(pupils[i].Pupil_ID, subjects[i][j].Subject_ID));
                }
            }

            ViewBag.Marks = marks;

            return View();
        }

        public ActionResult ViewMarkInfo(int markID)
        {
            MarkFullInf m = MarksDB.GetMarkFullInf(markID);
            return View(m);
        }

        [HttpPost]
        public ActionResult ViewMarkInfo(int delete, int a = 0)
        {
            int pupilId = PupilsDB.GetPupilIdByMark(delete);
            PupilFullInformation p = PupilsDB.GetPupilsFullInformation(pupilId);
            int classID = ClassesDB.GetClassId(p.Class);
            MarksDB.DeleteMark(delete);
            return RedirectToAction("ViewMarks", "Teacher", new { id = classID });
        }

        public ActionResult ViewMarkAbsentInfo(int markID)
        {
            MarkAbsentFullInf mark = MarksDB.GetMarkAbsentFullInf(markID);
            return View(mark);
        }

        public ActionResult AddMarkForm(int lessonID)
        {
            LessonFullInf lesson = LessonsDB.GetLessonFullInf(lessonID);
            int classID = lesson.Class_ID;

            List<Pupil> pupils = PupilsDB.GetPupilsInClass(classID);
            ViewBag.Pupils = pupils;

            List<TypeOfWork> types = TypesOfWorkDB.GetTypesOfWork();
            ViewBag.Types = types;

            ViewData["LessonID"] = lessonID;

            return View();
        }

        public ActionResult AddNewMark(Mark m, int res = 0)
        {
            MarksDB.AddMark(m.Pupil_ID, res, (int)m.TypeOfWork_ID, (int)m.PupilMark);

            PupilFullInformation pupil = PupilsDB.GetPupilsFullInformation(m.Pupil_ID);

            int classID = ClassesDB.GetClassId(pupil.Class);

            return RedirectToAction("ViewMarks", "Teacher", new { id = classID});
        }

        public ActionResult SearchMarksBySurname(string Person)
        {
            string[] parts = Person.Split(' ');
            List<PupilWithClass> pupils = new List<PupilWithClass>();

            if (parts.Length == 3)
            {
                string part1 = parts[0];
                string part2 = parts[1];
                string part3 = parts[2];

                pupils = PupilsDB.SearchPupilByThreeParametersAndClass(part1, part2, part3, TeacherController.classID);
            }
            else if (parts.Length == 2)
            {
                string part1 = parts[0];
                string part2 = parts[1];

                pupils = PupilsDB.SearchPupilByTwoParametersAndClass(part1, part2, TeacherController.classID);
            }

            if (pupils.Count != 0)
            {
                ViewBag.Pupil = pupils[0];
            }

            List<Pupil> pupilsInClass = PupilsDB.GetPupilsInClass(TeacherController.classID);

            ViewBag.Pupils = PupilsDB.GetPupilsInClass(TeacherController.classID);

            List<List<Subject>> subjects = new List<List<Subject>>();

            for (int i = 0; i < pupilsInClass.Count; i++)
            {
                subjects.Add(SubjectsDB.GetPupilSubjects(pupilsInClass[i].Pupil_ID));
            }

            // Предметы по которым у ученика есть оценки.
            ViewBag.Subjects = subjects;

            List<List<List<Mark>>> marks = new List<List<List<Mark>>>();

            for (int i = 0; i < pupilsInClass.Count; i++)
            {
                marks.Add(new List<List<Mark>>(subjects[i].Count));
                for (int j = 0; j < subjects[i].Count; j++)
                {
                    marks[i].Add(MarksDB.GetPupilSubjectMark(pupilsInClass[i].Pupil_ID, subjects[i][j].Subject_ID));
                }
            }

            ViewBag.Marks = marks;

            return PartialView();
        }

        public ActionResult ToAllMarks()
        {
            return RedirectToAction("ViewMarks", "Teacher", new { id = TeacherController.classID });
        }

        #endregion

        #region StatisticsAction

        public ActionResult ClassRating()
        {
            List<Class> classes = ClassesDB.GetClasses();
            ViewBag.Classes = classes;
            return View();
        }

        public ActionResult GetRating(int id)
        {
            List<PupilWithAvgMark> pupils = ClassesDB.GetClassRating(id);
            ViewData["ClassID"] = id;
            return PartialView(pupils);
        }

        public JsonResult RationgInClass(int classID)
        {
            List<PupilWithAvgMark> pupils = ClassesDB.GetClassRating(classID);

            return Json(new { Results = pupils }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRating(int action, int classID)
        {
            List<PupilWithAvgMark> pupils = ClassesDB.GetClassRating(classID);
            new ElectronicJournnal.Models.GenerateClassRatingReport().CreatePackage(@"E:\Uni\Course Project\ElectronicJournnal\ElectronicJournnal\Files\CurrentClassRatingReport.docx", pupils);
            string file_path = Server.MapPath("~/Files/CurrentClassRatingReport.docx"); ;
            string file_type = "application/docx";
            string file_name = "Рейтинг " + ClassesDB.GetClassFullInf(classID).Class_Name + " класса.docx";
            return File(file_path, file_type, file_name);
        }

        public ActionResult RatingOfClasses()
        {
            List<ClassForRating> classes = ClassesDB.GetRatingOfClasses();
            return View(classes);
        }

        public JsonResult GetRatingOfClasses()
        {
            List<ClassForRating> classes = ClassesDB.GetRatingOfClasses();

            return Json(new { Results = classes}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClassesFillability()
        {
            List<ClassesFillability> classes = ClassesDB.GetClassesFillability();
            return View(classes);
        }

        public ActionResult SchoolMarksStatistic()
        {
            List<KeyValuePair<int, int>> statistic = MarksDB.GetSchoolMarksStatistic();

            ViewBag.Statistic = statistic;
            return View();
        }

        public ActionResult LessonAttendence()
        {
            List<KeyValuePair<string, int>> statistic = SubjectsDB.GetLessonAttendence();

            ViewBag.Statistic = statistic;
            return View();
        }

        public JsonResult ClassesFillability()
        {
            List<ClassesFillability> classes = ClassesDB.GetClassesFillability();

            return Json(new { Results = classes }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSchoolMarksStatistic()
        {
            List<KeyValuePair<int, int>> statistic = MarksDB.GetSchoolMarksStatistic();

            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("От 0 до 4:", statistic[0].Value),
                new KeyValuePair<string, int>("От 4 до 7:", statistic[1].Value),
                new KeyValuePair<string, int>("От 7 до 9:", statistic[2].Value),
                new KeyValuePair<string, int>("От 9 до 12:", statistic[3].Value),
            };

            return Json(new { Results = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLessonAttendance()
        {
            List<KeyValuePair<string, int>> statistic = SubjectsDB.GetLessonAttendence();

            return Json(new { Lessons = statistic }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GeneralActions

        [HttpPost]
        public ActionResult AddInformation(string addPupil, string AddTeacherб, string addClass, string addLesson)
        {
            if (addPupil == "1")
                return RedirectToActionPermanent("AddPupilForm", "Teacher");
            else if (addClass == "1")
                return RedirectToActionPermanent("AddClassForm", "Teacher");
            else if (addLesson == "1")
                return RedirectToActionPermanent("AddLessonForm", "Teacher");

            return RedirectToActionPermanent("AddTeacherForm", "Teacher");
        }

        public ActionResult ManageInformation()
        {
            return View();
        }

        public ActionResult GetStatistics()
        {
            List<Class> classes = ClassesDB.GetClasses();
            return View(classes);
        }

        public ActionResult MailMerge()
        {
            List<Class> classes = ClassesDB.GetClasses();
            ViewBag.Classes = classes;
            return View();
        }

        [HttpPost]
        public ActionResult MailMerge(string send, List<int> Mail)
        {

            StringBuilder result = new StringBuilder();

            try
            {
                List<Parenthood> parenthoods = new List<Parenthood>();
                if (Mail == null)
                {
                    parenthoods = ParentsDB.GetParenthoods();
                }
                else
                {
                    List<Pupil> pupils = new List<Pupil>();

                    foreach (int c in Mail)
                    {
                        foreach (Pupil p in PupilsDB.GetPupilsInClass(c))
                            pupils.Add(p);
                    }

                    foreach (Pupil p in pupils)
                    {
                        foreach (Parenthood parenthood in ParentsDB.GetPupilParenthoods(p.Pupil_ID))
                            parenthoods.Add(parenthood);
                    }
                }
                

                foreach (Parenthood p in parenthoods)
                {
                    StringBuilder message = new StringBuilder();

                    PupilFullInformation pupil = PupilsDB.GetPupilsFullInformation(p.Pupil_ID);
                    Parent parent = ParentsDB.GetParentInfo(p.Parent_ID);
                    ClassWithManagment c = ClassesDB.GetClassFullInf(ClassesDB.GetClassId(pupil.Class));
                    TeacherFullInformation classManager = TeachersDB.GetTeacherFullInformation(c.Teacher_ID);

                    List<Subject> subjects = SubjectsDB.GetPupilSubjects(p.Pupil_ID);

                    List<List<Mark>> marks = new List<List<Mark>>();

                    for (int i = 0; i < subjects.Count; i++)
                    {
                        marks.Add(MarksDB.GetPupilSubjectMark(pupil.Pupil_ID, subjects[i].Subject_ID));
                    }

                    if (DateTime.Now.Hour < 10)
                        message.AppendLine($"<p>Доброе утро, {parent.Name} {parent.Patronymic}!</p>");
                    else if (DateTime.Now.Hour >= 10 && DateTime.Now.Hour <= 17)
                        message.AppendLine($"<p>Добрый день, {parent.Name} {parent.Patronymic}!</p>");
                    else
                        message.AppendLine($"<p>Добрый вечер, {parent.Name} {parent.Patronymic}!</p>");

                    if (marks.Count != 0)
                    {
                        message.AppendLine($"<p>Высылаем вам отчет об успеваемости, так как вы подписаны на рассылку.</p>");
                        message.AppendLine("<p>.</p>");

                        message.AppendLine($"<p><b>Ученик(ица)</b>: {pupil.Surname} {pupil.Name} {pupil.Patronymic}.</p>");
                        message.AppendLine($"<p><b>Класс</b>: {pupil.Class}.</p>");

                        DateTime from = LessonsDB.GetClassFirstLesson(ClassesDB.GetClassId(pupil.Class));
                        message.AppendLine($"<p><b>Оценки с {from.ToLongDateString()} по {DateTime.Now.ToLongDateString()}: </b></p>");

                        for (int i = 0; i < subjects.Count; i++)
                        {
                            message.Append("<p>" + subjects[i].Subject_Name + ": ");
                            foreach (Mark mark in marks[i])
                            {
                                if (mark.PupilMark != null)
                                    message.Append($"{mark.PupilMark.ToString()} ");
                                else
                                    message.Append("н ");
                            }
                            message.Append("</p>");
                            message.AppendLine();
                        }

                        message.AppendLine($"<p><b>Количество пропусков</b>: {PupilsDB.GetPupilAbsentCount(pupil.Pupil_ID)}.</p>");

                        List<KeyValuePair<string, double>> weakSubjects = PupilsDB.GetPupilWeakSubjects(pupil.Pupil_ID);
                        List<KeyValuePair<string, double>> strongSubjects = PupilsDB.GetPupilStrongSubjects(pupil.Pupil_ID);

                        if (weakSubjects.Count != 0)
                        {
                            message.AppendLine("<p><b>Слабые предметы</b>: </p>");
                            foreach (var subject in weakSubjects)
                            {
                                message.AppendLine($"<p>{subject.Key} (средний балл - {subject.Value.ToString()})</p>");
                            }
                            message.AppendLine("Если вы хотите связаться с учителями по проблемным предметам ученика(ицы), то вы можете написать им по следующим <b>электронным адрессам</b>:");
                            foreach (var subject in weakSubjects)
                            {
                                int teacherID = SubjectsDB.GetSubjectTeacher(ClassesDB.GetClassId(pupil.Class), subject.Key);
                                TeacherFullInformation teacher = TeachersDB.GetTeacherFullInformation(teacherID);

                                message.AppendLine($"<p>{subject.Key}: {teacher.Surname} {teacher.Name} {teacher.Patronymic }. Почта: {teacher.Email}</p>");
                            }
                        }
                        else
                        {
                            message.AppendLine("У данного ученика(ицы) нет слабых предметов. Успеваемость на хорошем уровне.");
                        }

                        if (strongSubjects.Count != 0)
                        {
                            message.AppendLine("<p><b>Сильные предметы</b>: </p>");
                            foreach (var subject in strongSubjects)
                            {
                                message.AppendLine($"<p>{subject.Key} (средний балл - {subject.Value.ToString()})</p>");
                            }
                            message.AppendLine("<p>(По данным предметам рекомендуется посетить олимпиады или турниры. Для более подробной информации необходимо связаться с классным руководителем.)</p>");
                            message.AppendLine($"<p><b>Классный руководитель: </b>{classManager.Surname} {classManager.Name} {classManager.Patronymic }. Почта: {classManager.Email}</p>");
                        }
                        else
                        {
                            message.AppendLine("<p>У данного ученика нет сильных предметов.</p>");
                        }

                        DateTime currentDate = DateTime.Now;
                        int currentMonth = currentDate.Month;
                        int Year = currentDate.Year;
                        int allDayCurrentMonth = DateTime.DaysInMonth(Year, currentMonth);
                        DateTime currentBegin = new DateTime(Year, currentMonth, 1);
                        DateTime currentEnd = currentBegin.AddMonths(1).AddDays(-1);

                        int previousMonth = currentMonth - 1;
                        int allDayPreviousMonth = DateTime.DaysInMonth(Year, previousMonth);
                        DateTime previousBegin = new DateTime(Year, previousMonth, 1);
                        DateTime previousEnd = previousBegin.AddMonths(1).AddDays(-1);

                        double previousMonthAvgMark = PupilsDB.GetPupilsAvgMarkInMonth(pupil.Pupil_ID, previousBegin, previousEnd);
                        double currentMonthAvgMark = PupilsDB.GetPupilsAvgMarkInMonth(pupil.Pupil_ID, currentBegin, currentEnd);

                        int change = Convert.ToInt32(((currentMonthAvgMark - previousMonthAvgMark) / previousMonthAvgMark) * 100);

                        message.AppendLine("<p><b>Анализ успеваемости:</b></p>");
                        if (change >= 0)
                        {
                            message.AppendLine($"<p>По сравнению с прошлым месяцем успеваесмость выросла на {change}%. Продолжайте в том же духе!</p>");
                        }
                        else
                        {
                            message.AppendLine($"<p>По сравнению с прошлым месяцем успеваесмость упала на {change}%. Стоит больше внимания уделить учебе!</p>");
                        }

                        for (int i = 0; i < subjects.Count; i++)
                        {
                            message.Append("<p> Предположительная оценка в семестре по " + subjects[i].Subject_Name + ": " + Convert.ToUInt32(PupilsDB.GetPupilsSubjectAvgMark(pupil.Pupil_ID, subjects[i].Subject_ID)) + " ");
                            message.Append("</p>");
                            message.AppendLine();
                        }

                    }
                    else
                    {
                        message.AppendLine("<p>.</p>");
                        message.AppendLine("<p>У данного ученика(ицы) нет оценок, поэтому мы не можем сформировать отчет. Возможно, Вам стоит связаться с классным руководителем по электронной почте.</p>");
                        message.AppendLine($"<p><b>Классный руководитель: </b>{classManager.Surname} {classManager.Name} {classManager.Patronymic }. Почта: {classManager.Email}</p>");
                    }
                    message.AppendLine("<p>.</p>");
                    message.AppendLine("<p>С уважением, администрация школы!</p>");

                    EmailService emailService = new EmailService();
                    emailService.SendEmailAsync(parent.Email, $"Отчет об успеваемости {pupil.Surname} {pupil.Name[0]}. {pupil.Patronymic[0]}.", message.ToString());
                }

                result.AppendLine("Рассылка прошла успешно!");
            }
            catch
            {
                result.AppendLine("<h3> К сожалению, при рассылке проиошла ошибка. Возможно, отсутствует подключение к интернету. Попробуйте позжу! </h3>");
            }

            ViewBag.Message = result.ToString();

            return View("~/Views/Some/Result.cshtml");
        }

        #endregion
    }
}
using ElectronicJournnal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ElectronicJournnal.Controllers
{
    public class ParentController : Controller
    {
        // GET: Parent
        public ActionResult ParentIndex()
        {
            return View();
        }

        #region SudsriptionAction

        [HttpGet]
        public ActionResult Subscription()
        {
            List<PupilWithClass> pupils = PupilsDB.GetPupilsWithClassOrderByClass();
            ViewBag.Pupils = pupils;
            return View();
        }

        public ActionResult Subscription(List<int> Subscribe, string subscr, Parent parent)
        {
            if (Subscribe.Count != 0)
            {
                ParentsDB.AddParent(parent.Name, parent.Surname, parent.Patronymic, parent.Email);
                int parentID = ParentsDB.GetParentsLastID();

                for (int i = 0; i < Subscribe.Count; i++)
                {
                    ParentsDB.AddParenthood(parentID, Subscribe[i]);
                }
            }
            return RedirectToRoute(new { controller = "Parent", action = "ParentIndex" });
        }

        #endregion

        #region StatisticsAction

        public ActionResult GetStatistics()
        {
            List<Class> classes = ClassesDB.GetClasses();
            return View(classes);
        }

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

        #endregion

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
                return RedirectToAction("PupilFullInf", "Parent", new { id = action });
            }
            else if (delete != 0)
            {
                PupilsDB.DeletePupil(delete);
                return RedirectToAction("ViewPupils", "Parent");
            }
            else
            {
                return RedirectToAction("ViewPupilMarks", "Parent", new { pupilID = marks });
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
                return RedirectToActionPermanent("ViewPupils", "Parent");
            }
            else
            {
                return RedirectToActionPermanent("PupilFullInf", "Parent", new { id = p.Pupil_ID });
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

        public ActionResult GetPupilsOrderBySurname()
        {
            List<PupilWithClass> pupils = PupilsDB.GetPupilsWithClassOrderBySurname();
            return PartialView(pupils);
        }

        public ActionResult GetPupilsOrderByClass()
        {
            List<PupilWithClass> pupils = PupilsDB.GetPupilsWithClassOrderByClass();
            return PartialView(pupils);
        }

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

        public ActionResult ToAllPupils()
        {
            return RedirectToActionPermanent("ViewPupils", "Parent");
        }

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

        public ActionResult ViewMarkAbsentInfo(int markID)
        {
            MarkAbsentFullInf mark = MarksDB.GetMarkAbsentFullInf(markID);
            return View(mark);
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

                pupils = PupilsDB.SearchPupilByThreeParametersAndClass(part1, part2, part3, ParentController.classID);
            }
            else if (parts.Length == 2)
            {
                string part1 = parts[0];
                string part2 = parts[1];

                pupils = PupilsDB.SearchPupilByTwoParametersAndClass(part1, part2, ParentController.classID);
            }

            if (pupils.Count != 0)
            {
                ViewBag.Pupil = pupils[0];
            }

            List<Pupil> pupilsInClass = PupilsDB.GetPupilsInClass(ParentController.classID);

            ViewBag.Pupils = PupilsDB.GetPupilsInClass(ParentController.classID);

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
            return RedirectToAction("ViewMarks", "Parent", new { id = ParentController.classID });
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
                return RedirectToAction("TeacherFullInf", "Parent", new { id = action });
            }
            else
            {
                TeachersDB.DeleteTeacher(delete);
                return RedirectToAction("ViewTeachers", "Parent");
            }
        }

        public ActionResult TeacherFullInf(int id)
        {
            TeacherFullInformation t = TeachersDB.GetTeacherFullInformation(id);
            return View(t);
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
            return RedirectToActionPermanent("ViewTeachers", "Parent");
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
                return RedirectToAction("ClassFullInf", "Parent", new { id = action });
            }
            else
            {
                ClassesDB.DeleteClass(delete);
                return RedirectToAction("ViewClasses", "Parent");
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
                return RedirectToAction("PupilFullInf", "Parent", new { id = action });
            }
            else
            {
                PupilsDB.DeletePupil(delete);
                return RedirectToAction("ClassFullInf", "Parent", new { id = ClassID });
            }
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
                return RedirectToAction("ViewLessons", "Parent");
            }
            return RedirectToAction("ViewLessons", "Parent");
        }

        public ActionResult ToAllLessons()
        {
            return RedirectToAction("ViewLessons", "Parent");
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

            if (Class_ID != 0 && from != "01.01.0001" && to != "00.00.0001")
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

        #region GeneralActions

        public ActionResult ManageInformation()
        {
            return View();
        }

        #endregion
    }
}

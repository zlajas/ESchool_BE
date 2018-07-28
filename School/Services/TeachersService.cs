using School.Models;
using School.Models.DTOs;
using School.Models.DTOs.TeacherView;
using School.Models.Enum;
using School.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace School.Services
{
    public class TeachersService : ITeachersService
    {
        private IUnitOfWork db;

        public TeachersService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Teacher> GetAllTeachers()
        {
            return db.TeacherRepository.Get(); 
        }

        public Teacher GetById(string id)
        {
            return db.TeacherRepository.GetByID(id);
        }

        public TeacherViewDTO GetAllForTeacher(string id)
        {
            Teacher teacher = db.TeacherRepository.GetByID(id);

            TeacherViewDTO teacherView = new TeacherViewDTO();
            EGradeYear sGrade;
            string stuId;
            string stuName;
            string sName;
            int? sId;
            IEnumerable<Mark> markValues;
      
            foreach (var subj in teacher.TeacherTeachesSubject)
            {
                sId = subj.Subject.SubjectId;
                sName = subj.Subject.SubjectName;
                sGrade = subj.Subject.Grade;

                SubjectsOfTeacherDTO teachersSubjects = new SubjectsOfTeacherDTO(sId, sName, sGrade);

                foreach (var stu in subj.StudentsAttendSubjects)
                {
                    stuId = stu.Student.Id;
                    stuName = stu.Student.FirstName + ' ' + stu.Student.LastName;
                    markValues = stu.Marks;

                    StudentsAndMarksDTO studentsAndMarks = new StudentsAndMarksDTO(stuId, stuName, markValues);
                    teachersSubjects.StudentsWithMarks?.Add(studentsAndMarks);
                }
                teacherView.TeachersAndSubjects.Add(teachersSubjects);
            }
            return teacherView;
        }

        public IEnumerable<Mark> GetMarksForStudentSubjectTeacher(string studentId, int? subjectId, string teacherId)
        {
            Student student = db.StudentRepository.GetByID(studentId);
            Subject subject = db.SubjectRepository.GetByID(subjectId);
            Teacher teacher = db.TeacherRepository.GetByID(teacherId);

            return teacher.TeacherTeachesSubject.FirstOrDefault(x => x.Teacher == teacher && x.Subject == subject).StudentsAttendSubjects.FirstOrDefault(y => y.Subject == subject && y.Student == student).Marks;
        }


        public IEnumerable<Subject> GetTeachersSubjects(string id)
        {
            Teacher teacher = db.TeacherRepository.GetByID(id);

            return teacher.TeacherTeachesSubject.Select(x => x.Subject);
        }


        public IEnumerable<Subject> GetAllRemainingSubjects(string id)
        {
            Teacher teacher = db.TeacherRepository.GetByID(id);

            return db.SubjectRepository.Get().Except(teacher.TeacherTeachesSubject.Select(x => x.Subject));

        }

        public Teacher InsertTeacher(Teacher newTeacher)
        {
            db.TeacherRepository.Insert(newTeacher);
            db.Save();
            return newTeacher;
        }

        public Teacher UpdateTeacher(string id, Teacher updatedTeacher)
        {
            Teacher teacher = db.TeacherRepository.GetByID(id);

            if (teacher != null)
            {
                teacher.FirstName = updatedTeacher.FirstName;
                teacher.LastName = updatedTeacher.LastName;
                teacher.UserName = updatedTeacher.UserName;
                teacher.EmailAddress = updatedTeacher.EmailAddress;

                db.TeacherRepository.Update(teacher);
                db.Save();
            }

            return teacher;
        }
        public Mark AddMarkToStudent (string studentId, int subjectId, string teacherId, int markValue)
        {
            TeacherToSubject subjectToTeacher = new TeacherToSubject();
            StudentToSubject studentToSubject = new StudentToSubject();

            Teacher teacher = db.TeacherRepository.GetByID(teacherId);
            Student student = db.StudentRepository.GetByID(studentId);
            Subject subject = db.SubjectRepository.GetByID(subjectId);
            Mark newMark = new Mark();
            newMark.MarkDate = DateTime.Now;
            newMark.MarkValue = markValue;
            newMark.SemesterEndMark = false;
            db.MarkRepository.Insert(newMark);


            teacher.TeacherTeachesSubject.FirstOrDefault(x => x.Teacher == teacher && x.Subject == subject).StudentsAttendSubjects.FirstOrDefault(y => y.Subject == subject && y.Student == student).Marks.Add(newMark);

            db.Save();

            SendMail(student, subject, teacher, newMark);

            return newMark; 
        }

        public Teacher AddSubjectToTeacher(string teacherId, int subjectId)
        {
            Teacher teacher = db.TeacherRepository.GetByID(teacherId);
            Subject subject = db.SubjectRepository.GetByID(subjectId);

            TeacherToSubject subjectToTeacher = new TeacherToSubject();

            subjectToTeacher.Teacher = teacher;
            subjectToTeacher.Subject = subject;

            teacher.TeacherTeachesSubject.Add(subjectToTeacher);
            db.Save();
            return teacher;
        }
        public TeacherToSubject RemoveSubjectTeacherPair(string teacherId, int subjectId)
        {
            Teacher teacher = db.TeacherRepository.GetByID(teacherId);
            Subject subject = db.SubjectRepository.GetByID(subjectId);

            TeacherToSubject teacherToSubject = teacher.TeacherTeachesSubject.FirstOrDefault(x => x.Teacher == teacher && x.Subject == subject);

            db.TeacherSubjectRepository.Delete(teacherToSubject);

            db.Save();
            return teacherToSubject;
        }

        public Teacher DeleteTeacher(string id)
        {
            Teacher teacher = db.TeacherRepository.GetByID(id);
            if (teacher == null)
            {
                return null;
            }
            db.TeacherRepository.Delete(teacher);
            db.Save();

            return teacher;
        }

        public void SendMail(Student student, Subject ssubject, Teacher teacher, Mark newMark)
        {
            string subject = "New mark notification";
            string FromMail = ConfigurationManager.AppSettings["from"];
            string emailTo;
            try
            {
                foreach (var parent in student.Parents)
                {
                    emailTo = parent.EmailAddress;

                    if (emailTo == null)
                    {
                        throw new Exception(string.Format("Parent {0} {1} dos not have an email address", parent.FirstName, parent.LastName));

                    }

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"]);
                    mail.From = new MailAddress(FromMail);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    string htmlBody;
                    htmlBody = string.Format(@"<table border=""1"" style=""undefined;table-layout: fixed; width: 525px>
                    <colgroup>
                    <col style=""width: 150px"">
                    <col style=""width: 75px"">
                    <col style=""width: 75px"">
                    <col style=""width: 75px"">
                    <col style=""width: 150px"">

                    </colgroup>
                      <tr>
                        <th><span style=""font-weight:bold"">Student</span><br></th>
                        <th><span style=""font-weight:bold"">Subject<br></th>
                        <th><span style=""font-weight:bold"">Mark</th>
                        <th><span style=""font-weight:bold"">Date</th>
                        <th><span style=""font-weight:bold"">Teacher</th>
                      </tr>
                      <tr>
                        <td>{0} {1}</td>
                        <td>{2}</td>
                        <td>{3}</td>
                        <td>{4}</td>
                        <td>{5} {6}</td>
                      </tr>
                    </table>", student.FirstName, student.LastName, ssubject.SubjectName, newMark.MarkValue, newMark.MarkDate,
                    teacher.FirstName, teacher.LastName);
                    mail.Body = htmlBody;
                    SmtpServer.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["from"], ConfigurationManager.AppSettings["password"]);
                    SmtpServer.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["smtpSsl"]);
                    SmtpServer.Send(mail);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
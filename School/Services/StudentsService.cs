using School.Models;
using School.Repositories;
using System;
using System.Collections.Generic;
using School.Models.Enum;
using System.Linq;
using System.Web;
using School.Models.DTOs;

namespace School.Services
{
    public class StudentsService : IStudentsService
    {
        private IUnitOfWork db;

        public StudentsService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return db.StudentRepository.Get();
        }

        public Student GetById(string id)
        {
            return db.StudentRepository.GetByID(id);
        }

        public IEnumerable<Subject> GetAllStudentsSubjects(string id)
        {
            Student student = db.StudentRepository.GetByID(id);

            return student.StudentAttendsSubject.Select(x => x.Subject);
        }


        public IEnumerable<Subject> GetAllRemainingSubjects(string id)
        {
            Student student = db.StudentRepository.GetByID(id);

            return db.SubjectRepository.Get().Except(student.StudentAttendsSubject.Select(x => x.Subject));

        }
        public IEnumerable<StudentViewDTO> GetAllMarks(string id)
        {
           Student student = db.StudentRepository.GetByID(id);

           List<StudentViewDTO> studentView = new List<StudentViewDTO>();

            string sName;
            EGradeYear grade;
            IEnumerable<Mark> markValues;
            string teacher;
            IEnumerable<DateTime> date;


            foreach (var subj in student.StudentAttendsSubject)
            {
                sName = subj.Subject.SubjectName;
                grade = subj.Subject.Grade;
                markValues = subj.Marks;
                teacher = subj.TacherTeachesSubject?.Teacher.FirstName + ' ' + subj.TacherTeachesSubject?.Teacher.LastName;
                date = subj.Marks.Select(x => x.MarkDate);
                studentView.Add(new StudentViewDTO(sName, grade, markValues, teacher, date));
            }
            return studentView;
        }

        public Student InsertStudent(Student newStudent)
        {
            db.StudentRepository.Insert(newStudent);
            db.Save();
            return newStudent;
        }

        public Student UpdateStudent(string id, Student updatedStudent)
        {
            Student student = db.StudentRepository.GetByID(id);

            if (student != null)
            {
                student.FirstName = updatedStudent.FirstName;
                student.LastName = updatedStudent.LastName;
                student.UserName = updatedStudent.UserName;
                student.DateOfBirth = updatedStudent.DateOfBirth;

                db.StudentRepository.Update(student);
                db.Save();
            }

            return student;
        }

        public Student AddSubjectToStudent(string studentId, int subjectId)
        {
            Student student = db.StudentRepository.GetByID(studentId);
            Subject subject = db.SubjectRepository.GetByID(subjectId);

            StudentToSubject subjectToStudent = new StudentToSubject();

            if (student.StudentAttendsSubject.Select(x => x.Subject).Contains(subject))
            {
                
            }

            subjectToStudent.Student = student;
            subjectToStudent.Subject = subject;
           
            student.StudentAttendsSubject.Add(subjectToStudent);
           
            db.Save();
            return student;
        }

        public Student AddTeacherToStudentSubject(string studentId, int subjectId, string teacherId)
        {
            Student student = db.StudentRepository.GetByID(studentId);
            Subject subject = db.SubjectRepository.GetByID(subjectId);
            Teacher teacher = db.TeacherRepository.GetByID(teacherId);

            StudentToSubject studentSubject = student.StudentAttendsSubject.FirstOrDefault(x => x.Student == student && x.Subject == subject);
            TeacherToSubject teacherSubject = teacher.TeacherTeachesSubject.FirstOrDefault(x => x.Teacher == teacher && x.Subject == subject);

            studentSubject.TacherTeachesSubject = teacherSubject;

            db.StudentSubjectRepository.Update(studentSubject);
            db.Save();
            return student;
        }


        public void SpecialAddSubjectToStudent()
        {
            foreach(Student student in db.StudentRepository.Get())
            {
                foreach(Subject subject in db.SubjectRepository.Get())
                {
                    if(student.DateOfBirth>= new DateTime(2011, 1, 1) && student.DateOfBirth<= new DateTime(2011, 12, 31) && subject.Grade == EGradeYear.FIRST)
                    {
                        StudentToSubject studentSubject = new StudentToSubject();
                        studentSubject.Student = student;
                        studentSubject.Subject = subject;
                        student.StudentAttendsSubject.Add(studentSubject);
                        db.Save();                      
                    }
                }
            }

            foreach (Student student in db.StudentRepository.Get())
            {
                foreach (Subject subject in db.SubjectRepository.Get())
                {
                    if (student.DateOfBirth >= new DateTime(2010, 1, 1) && student.DateOfBirth <= new DateTime(2010, 12, 31) && subject.Grade == EGradeYear.SECOND)
                    {
                        StudentToSubject studentSubject = new StudentToSubject();
                        studentSubject.Student = student;
                        studentSubject.Subject = subject;
                        student.StudentAttendsSubject.Add(studentSubject);
                        db.Save();
                    }
                }
            }

            foreach (Student student in db.StudentRepository.Get())
            {
                foreach (Subject subject in db.SubjectRepository.Get())
                {
                    if (student.DateOfBirth >= new DateTime(2009, 1, 1) && student.DateOfBirth <= new DateTime(2009, 12, 31) && subject.Grade == EGradeYear.THIRD)
                    {
                        StudentToSubject studentSubject = new StudentToSubject();
                        studentSubject.Student = student;
                        studentSubject.Subject = subject;
                        student.StudentAttendsSubject.Add(studentSubject);
                        db.Save();
                    }
                }
            }

            foreach (Student student in db.StudentRepository.Get())
            {
                foreach (Subject subject in db.SubjectRepository.Get())
                {
                    if (student.DateOfBirth >= new DateTime(2008, 1, 1) && student.DateOfBirth <= new DateTime(2008, 12, 31) && subject.Grade == EGradeYear.FOURTH)
                    {
                        StudentToSubject studentSubject = new StudentToSubject();
                        studentSubject.Student = student;
                        studentSubject.Subject = subject;
                        student.StudentAttendsSubject.Add(studentSubject);
                        db.Save();
                    }
                }
            }

            foreach (Student student in db.StudentRepository.Get())
            {
                foreach (Subject subject in db.SubjectRepository.Get())
                {
                    if (student.DateOfBirth >= new DateTime(2007, 1, 1) && student.DateOfBirth <= new DateTime(2007, 12, 31) && subject.Grade == EGradeYear.FIFTH)
                    {
                        StudentToSubject studentSubject = new StudentToSubject();
                        studentSubject.Student = student;
                        studentSubject.Subject = subject;
                        student.StudentAttendsSubject.Add(studentSubject);
                        db.Save();
                    }
                }
            }

            foreach (Student student in db.StudentRepository.Get())
            {
                foreach (Subject subject in db.SubjectRepository.Get())
                {
                    if (student.DateOfBirth >= new DateTime(2006, 1, 1) && student.DateOfBirth <= new DateTime(2006, 12, 31) && subject.Grade == EGradeYear.SIXTH)
                    {
                        StudentToSubject studentSubject = new StudentToSubject();
                        studentSubject.Student = student;
                        studentSubject.Subject = subject;
                        student.StudentAttendsSubject.Add(studentSubject);
                        db.Save();
                    }
                }
            }

            foreach (Student student in db.StudentRepository.Get())
            {
                foreach (Subject subject in db.SubjectRepository.Get())
                {
                    if (student.DateOfBirth >= new DateTime(2005, 1, 1) && student.DateOfBirth <= new DateTime(2005, 12, 31) && subject.Grade == EGradeYear.SEVENTH)
                    {
                        StudentToSubject studentSubject = new StudentToSubject();
                        studentSubject.Student = student;
                        studentSubject.Subject = subject;
                        student.StudentAttendsSubject.Add(studentSubject);
                        db.Save();
                    }
                }
            }

            foreach (Student student in db.StudentRepository.Get())
            {
                foreach (Subject subject in db.SubjectRepository.Get())
                {
                    if (student.DateOfBirth >= new DateTime(2004, 1, 1) && student.DateOfBirth <= new DateTime(2004, 12, 31) && subject.Grade == EGradeYear.EIGHTH)
                    {
                        StudentToSubject studentSubject = new StudentToSubject();
                        studentSubject.Student = student;
                        studentSubject.Subject = subject;
                        student.StudentAttendsSubject.Add(studentSubject);
                        db.Save();
                    }
                }
            }
        }

        public StudentToSubject RemoveSubjectStudentPair(string studentId, int subjectId)
        {
            Student student = db.StudentRepository.GetByID(studentId);
            Subject subject = db.SubjectRepository.GetByID(subjectId);

            StudentToSubject studentSubject = student.StudentAttendsSubject.FirstOrDefault(x => x.Student == student && x.Subject == subject);

            db.StudentSubjectRepository.Delete(studentSubject);

            db.Save();
            return studentSubject;
        }

        public Student DeleteStudent(string id)
        {
            Student student = db.StudentRepository.GetByID(id);
            if (student == null)
            {
                return null;
            }
            db.StudentRepository.Delete(student);
            db.Save();

            return student;
        }
    }
}
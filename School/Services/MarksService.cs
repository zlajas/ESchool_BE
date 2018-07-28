using School.Models;
using School.Models.Enum;
using School.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Services
{
    public class MarksService : IMarksService
    {
        private IUnitOfWork db;

        public MarksService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Mark> GetAllGrades()
        {
            return db.MarkRepository.Get();
        }

        public Mark GetById(int id)
        {
            return db.MarkRepository.GetByID(id);
        }

        public IEnumerable<Mark> GetMarksPerSubject(string studentId, int subjectId)
        {
            Student student = db.StudentRepository.GetByID(studentId);
            if (student.StudentAttendsSubject.Select(s => s.Subject.SubjectId).Contains(subjectId))
            {
                return db.StudentRepository.GetByID(studentId).StudentAttendsSubject.FirstOrDefault(x => x.Subject.SubjectId == subjectId).Marks;
            }
            return null;
        }

        public Mark InsertGrade(Mark newGrade)
        {
            db.MarkRepository.Insert(newGrade);
            db.Save();
            return newGrade;
        }

        public Mark AddMarkAsAdmin(string studentId, int subjectId, int markValue)
        {
            Mark newMark = new Mark();

            newMark.MarkDate = DateTime.Now;
            newMark.MarkValue = markValue;
            newMark.SemesterEndMark = false;
            db.MarkRepository.Insert(newMark);
            db.Save();
            db.MarkRepository.Update(newMark);
            db.StudentRepository.GetByID(studentId).StudentAttendsSubject.FirstOrDefault(x => x.Subject.SubjectId == subjectId).Marks.Add(newMark);
            db.Save();

            return newMark;
        }

        public Mark UpdateMark(int id, Mark updatedMark)
        {
            Mark mark = db.MarkRepository.GetByID(id);

            if (mark != null)
            {
                mark.MarkValue = updatedMark.MarkValue;
                mark.SemesterEndMark = updatedMark.SemesterEndMark;
                mark.MarkDate = updatedMark.MarkDate;

                db.MarkRepository.Update(mark);
                db.Save();
            }

            return mark;
        }
        public Mark DeleteGrade(int id)
        {
            Mark grade = db.MarkRepository.GetByID(id);
            if (grade == null)
            {
                return null;
            }
            db.MarkRepository.Delete(grade);
            db.Save();

            return grade;
        }
    }
}
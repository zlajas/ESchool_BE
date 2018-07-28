using School.Models;
using School.Models.Enum;
using School.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Services
{
    public class SubjectsService : ISubjectsService
    {
        private IUnitOfWork db;

        public SubjectsService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Subject> GetAllSubjects()
        {
            return db.SubjectRepository.Get();
        }

        public Subject GetById(int id)
        {
            return db.SubjectRepository.GetByID(id);
        }

        public Subject InsertSubject(Subject newSubject)
        {
            db.SubjectRepository.Insert(newSubject);
            db.Save();
            return newSubject;
        }

        public IEnumerable<Teacher> GetTeachersTeachingSubject(int id)
        {

            return db.SubjectRepository.GetByID(id).TeacherTeachesSubjects.Select(x => x.Teacher);
        }

        public Subject UpdateSubject(int id, Subject updatedSubject)
        {
            Subject subject = db.SubjectRepository.GetByID(id);

            if (subject != null)
            {
                subject.SubjectName = updatedSubject.SubjectName;
                subject.WeeklyFund = updatedSubject.WeeklyFund;
                subject.Grade = updatedSubject.Grade;
                subject.Semester = updatedSubject.Semester;
                

                db.SubjectRepository.Update(subject);
                db.Save();
            }

            return subject;
        }
        public Subject DeleteSubject(int id)
        {
            Subject subject = db.SubjectRepository.GetByID(id);
            if (subject == null)
            {
                return null;
            }
            db.SubjectRepository.Delete(subject);
            db.Save();

            return subject;
        }
    }
}
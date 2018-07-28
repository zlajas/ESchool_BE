using School.Models;
using School.Models.DTOs;
using School.Models.DTOs.ParentView;
using School.Models.Enum;
using School.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Services
{
    public class ParentsService : IParentsService
    {
        private IUnitOfWork db;

        public ParentsService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Parent> GetAllParents()
        {
            return db.ParentRepository.Get();
        }

        public Parent GetById(string id)
        {
            return db.ParentRepository.GetByID(id);
        }

        public IEnumerable<Student> GetAllParentsKids(string id)
        {
            Parent parent = db.ParentRepository.GetByID(id);

            return parent.Children;
        }


        public IEnumerable<Student> GetAllRemainingStudents(string id)
        {
            Parent parent = db.ParentRepository.GetByID(id);

            return db.StudentRepository.Get().Except(parent.Children);

        }

        public ParentViewDTO GetAllChildrenMarks(string id)
        {
            Parent parent = db.ParentRepository.GetByID(id);

            ParentViewDTO parentView = new ParentViewDTO();

            string teacher;
            string stuId;
            string stuFirstName;
            string stuLastName;
            string subjectName;
            EGradeYear subjectGrade;
            int? subjectId;
            IEnumerable<Mark> markValues;


            foreach (var child in parent.Children)
            {
                stuId = child.Id;
                stuFirstName = child.FirstName;
                stuLastName = child.LastName;
                
                ParentsChildrenDTO children = new ParentsChildrenDTO(stuId, stuFirstName, stuLastName);

                foreach (var subject in child.StudentAttendsSubject)
                {
                    subjectId = subject.Subject.SubjectId;
                    subjectName = subject.Subject.SubjectName;
                    subjectGrade = subject.Subject.Grade;
                    markValues = subject.Marks;
                    teacher = subject.TacherTeachesSubject?.Teacher.FirstName + ' ' + subject.TacherTeachesSubject?.Teacher.LastName;

                    SubjectsAndMarksDTO subjects = new SubjectsAndMarksDTO(subjectId, subjectName, subjectGrade, teacher, markValues);

                    children.SubjectsAndMarks?.Add(subjects);
                }
                parentView.Children.Add(children);
            }
            return parentView;
        }


        public Parent InsertParent(Parent newParent)
        {
            db.ParentRepository.Insert(newParent);
            db.Save();
            return newParent;
        }

        public Student AddStudentToParent(string parentId, string studentId)
        {
            Student student = db.StudentRepository.GetByID(studentId);
            Parent parent = db.ParentRepository.GetByID(parentId);

            parent.Children.Add(student);

            db.Save();
            return student;
        }

        public Student RemoveStudentFromParent(string parentId, string studentId)
        {
            Student student = db.StudentRepository.GetByID(studentId);
            Parent parent = db.ParentRepository.GetByID(parentId);

            parent.Children.Remove(student);

            db.Save();
            return student;
        }

        public Parent UpdateParent(string id, Parent updatedParent)
        {
            Parent parent = db.ParentRepository.GetByID(id);

            if (parent != null)
            {
                parent.FirstName = updatedParent.FirstName;
                parent.LastName = updatedParent.LastName;
                parent.UserName = updatedParent.UserName;
                parent.EmailAddress = updatedParent.EmailAddress;

                db.ParentRepository.Update(parent);
                db.Save();
            }

            return parent;
        }
        public Parent DeleteParent(string id)
        {
            Parent parent = db.ParentRepository.GetByID(id);
            if (parent == null)
            {
                return null;
            }
            db.ParentRepository.Delete(parent);
            db.Save();

            return parent;
        }
    }
}
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Parent> ParentRepository { get; }
        IGenericRepository<Teacher> TeacherRepository { get; }
        IGenericRepository<Student> StudentRepository { get; }
        IGenericRepository<Mark> MarkRepository { get; }
        IGenericRepository<Subject> SubjectRepository { get; }
        IGenericRepository<Admin> AdminRepository { get; }
        IGenericRepository<ApplicationUser> UserRepository { get; }
        IGenericRepository<StudentToSubject> StudentSubjectRepository { get; }
        IGenericRepository<TeacherToSubject> TeacherSubjectRepository { get; }
        IAuthRepository AuthRepository { get; }

        void Save();
    }
}
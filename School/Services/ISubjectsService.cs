using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Services
{
    public interface ISubjectsService
    {
        IEnumerable<Subject> GetAllSubjects();
        Subject GetById(int id);
        IEnumerable<Teacher> GetTeachersTeachingSubject(int id);
        Subject InsertSubject(Subject newSubject);
        Subject UpdateSubject(int id, Subject updatedSubject);
        Subject DeleteSubject(int id);
    }
}

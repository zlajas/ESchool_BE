using School.Models;
using School.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IParentsService
    {
        IEnumerable<Parent> GetAllParents();
        Parent GetById(string id);
        IEnumerable<Student> GetAllParentsKids(string id);
        IEnumerable<Student> GetAllRemainingStudents(string id);
        ParentViewDTO GetAllChildrenMarks(string id);
        Parent InsertParent(Parent newParent);
        Student AddStudentToParent(string parentId, string studentId);
        Student RemoveStudentFromParent(string parentId, string studentId);
        Parent UpdateParent(string id, Parent updatedParent);
        Parent DeleteParent(string id);

    }
}

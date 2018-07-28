using School.Models;
using School.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IMarksService
    {
        IEnumerable<Mark> GetAllGrades();
        Mark GetById(int id);
        IEnumerable<Mark> GetMarksPerSubject(string studentId, int subjectId);
        Mark InsertGrade(Mark newGrade);
        Mark AddMarkAsAdmin(string studentId, int subjectId, int markValue);
        Mark UpdateMark(int id, Mark updatedMark);
        Mark DeleteGrade(int id);

    }
}

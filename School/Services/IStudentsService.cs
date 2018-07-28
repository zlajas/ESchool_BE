using School.Models;
using School.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IStudentsService
    {
        IEnumerable<Student> GetAllStudents();
        Student GetById(string id);
        IEnumerable<Subject> GetAllStudentsSubjects(string id);
        IEnumerable<StudentViewDTO> GetAllMarks(string id);
        IEnumerable<Subject> GetAllRemainingSubjects(string id);
        Student InsertStudent(Student newStudent);
        Student UpdateStudent(string id, Student updatedStudent);
        Student AddSubjectToStudent(string studentId, int subjectId);
        Student AddTeacherToStudentSubject(string studentId, int subjectId, string teacherId);
        StudentToSubject RemoveSubjectStudentPair(string studentId, int subjectId);
        Student DeleteStudent(string id);
        void SpecialAddSubjectToStudent();

    }
}

using School.Models;
using School.Models.DTOs;
using School.Models.DTOs.TeacherView;
using School.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Services
{
    public interface ITeachersService
    {
        IEnumerable<Teacher> GetAllTeachers();
        Teacher GetById(string id);
        //TeacherViewDTO singleSubjectStudentMarks(string id);
        //IEnumerable<TeacherViewDTO> singleSubjectStudentMarks(string id, int subjectId);
        TeacherViewDTO GetAllForTeacher(string id);
       // IEnumerable<TeacherViewDTO> GetAllTeacherSubjects(string id);
        IEnumerable<Subject> GetTeachersSubjects(string id);
        IEnumerable<Subject> GetAllRemainingSubjects(string id);
        Teacher InsertTeacher(Teacher newTeacher);
        Teacher UpdateTeacher(string id, Teacher updatedTeacher);
        Teacher AddSubjectToTeacher(string teacherId, int subjectId);
        Mark AddMarkToStudent(string studentId, int subjectId, string teacherId, int markValue);
        TeacherToSubject RemoveSubjectTeacherPair(string teacherId, int subjectId);
        Teacher DeleteTeacher(string id);
    }
}
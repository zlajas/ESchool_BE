using School.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models.DTOs.TeacherView
{
    public class SubjectsOfTeacherDTO
    {
        public int? SubjectId;
        public string SubjectName { get; set; }
        public EGradeYear Grade { get; set; }
        public ICollection<StudentsAndMarksDTO> StudentsWithMarks { get; set; }


        public SubjectsOfTeacherDTO()
        {
            StudentsWithMarks = new List<StudentsAndMarksDTO>();
        }

        public SubjectsOfTeacherDTO(int? subjectId, string subjectName, EGradeYear grade)
        {
            SubjectId = subjectId;
            SubjectName = subjectName;
            Grade = grade;
            StudentsWithMarks = new List<StudentsAndMarksDTO>();

        }

}
    
}
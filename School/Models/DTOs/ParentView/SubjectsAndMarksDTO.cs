using School.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models.DTOs.ParentView
{
    public class SubjectsAndMarksDTO
    {
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }
        public EGradeYear SubjectGrade { get; set; }
        public IEnumerable<Mark> Marks { get; set; }
        public string Teacher { get; set; }

        public SubjectsAndMarksDTO(int? subjectId, string subjectName, EGradeYear subjectGrade, string teacher, IEnumerable<Mark> marks)
        {
            SubjectId = subjectId;
            SubjectName = subjectName;
            SubjectGrade = subjectGrade;
            Teacher = teacher;
            Marks = marks;
            
        }

    }

}
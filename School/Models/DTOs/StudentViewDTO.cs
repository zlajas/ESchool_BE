using School.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models.DTOs
{
    public class StudentViewDTO
    {
        public string SubjectName { get; set; }
        public EGradeYear Grade { get; set; }
        public IEnumerable<Mark> Marks { get; set; }
        public string Teacher { get; set; }
        public IEnumerable<DateTime> Date { get; set; }

        public StudentViewDTO(string subjectName, EGradeYear grade, IEnumerable<Mark> marks, string teacher, IEnumerable<DateTime> date)
        {
            SubjectName = subjectName;
            Grade = grade;
            Marks = marks;
            Teacher = teacher;
            Date = date;
        }

    }
}
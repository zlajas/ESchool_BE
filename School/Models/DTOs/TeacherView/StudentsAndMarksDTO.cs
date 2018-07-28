using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models.DTOs.TeacherView
{
    public class StudentsAndMarksDTO
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public IEnumerable<Mark> Marks { get; set; }

        public StudentsAndMarksDTO (string studentId, string studentName, IEnumerable<Mark> marks)
        {
            StudentId = studentId;
            StudentName = studentName;
            Marks = marks;
        }
    }
}
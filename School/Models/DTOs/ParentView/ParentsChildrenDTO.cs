using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models.DTOs.ParentView
{
    public class ParentsChildrenDTO
    {
        public string StudentId { get; set; }
        public string StudentsFirstName { get; set; }
        public string StudentsLastName { get; set; }
        public ICollection<SubjectsAndMarksDTO> SubjectsAndMarks { get; set; }

        public ParentsChildrenDTO()
        {
            SubjectsAndMarks = new List<SubjectsAndMarksDTO>();
        }

        public ParentsChildrenDTO(string studentId, string studentsFirstName, string studentsLastName)
        {
            StudentId = studentId;
            StudentsFirstName = studentsFirstName;
            StudentsLastName = studentsLastName;
            SubjectsAndMarks = new List<SubjectsAndMarksDTO>();
        }

    }
}
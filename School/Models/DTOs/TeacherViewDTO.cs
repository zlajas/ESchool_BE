using School.Models.Enum;
using System;
using System.Collections.Generic;
using School.Models.DTOs.TeacherView;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace School.Models.DTOs
{
    public class TeacherViewDTO
    {
        public ICollection<SubjectsOfTeacherDTO> TeachersAndSubjects { get; set;}

        public TeacherViewDTO()
        {
            TeachersAndSubjects = new List<SubjectsOfTeacherDTO>();
        }

    }
}
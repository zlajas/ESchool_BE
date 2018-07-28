using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace School.Models
{
    public class TeacherToSubject
    {
        [Key]
        public int TeacherTeachesSubjectID { get; set; }
        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }
        [ForeignKey("Subject")]
        public int SubjectID { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        [JsonIgnore]
        public virtual ICollection<StudentToSubject> StudentsAttendSubjects { get; set; }

        public TeacherToSubject()
        {
            StudentsAttendSubjects = new List<StudentToSubject>();
        }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using School.Models.Enum;

namespace School.Models
{
    public class Mark
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = ("Mark must have value between 1 and 5"))]
        public int MarkValue { get; set; }

        public bool SemesterEndMark { get; set; }

        public DateTime MarkDate { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public int? StudentAttendsSubjectID { get; set; }
        [JsonIgnore]
        public virtual StudentToSubject StudentAttendsSubject { get; set; }

        public Mark()
        { }

    }
}
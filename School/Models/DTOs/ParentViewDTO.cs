using School.Models.DTOs.ParentView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models.DTOs
{
    public class ParentViewDTO
    {
  
        public ICollection<ParentsChildrenDTO> Children { get; set; }

        public ParentViewDTO()
        {
            Children = new List<ParentsChildrenDTO>();
        }


    }
}
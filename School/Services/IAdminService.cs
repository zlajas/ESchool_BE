using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IAdminService
    {
        IEnumerable<Admin> GetAllAdmins();
        Admin GetById(string id);
        Admin UpdateAdmin(string id, Admin updatedAdmin);
        Admin DeleteAdmin(string id);
    }
}

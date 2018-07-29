using School.Models;
using School.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Services
{
    public class AdminService : IAdminService
    {
        private IUnitOfWork db;

        public AdminService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Admin> GetAllAdmins()
        {
            return db.AdminRepository.Get();
        }

        public Admin GetById(string id)
        {
            return db.AdminRepository.GetByID(id);
        }

        public Admin UpdateAdmin(string id, Admin updatedAdmin)
        {
            Admin admin = db.AdminRepository.GetByID(id);

            if (admin != null)
            {
                admin.FirstName = updatedAdmin.FirstName;
                admin.LastName = updatedAdmin.LastName;
                admin.UserName = updatedAdmin.UserName;
                db.AdminRepository.Update(admin);
                db.Save();
            }

            return admin;
        }

        public Admin DeleteAdmin(string id)
        {
            Admin admin = db.AdminRepository.GetByID(id);
            if (admin == null)
            {
                return null;
            }
            db.StudentRepository.Delete(admin);
            db.Save();

            return admin;
        }

    }
}
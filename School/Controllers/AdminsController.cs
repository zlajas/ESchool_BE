using NLog;
using School.Models;
using School.Models.DTOs;
using School.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace School.Controllers
{
    [RoutePrefix("project/admins")]
    public class AdminsController : ApiController
    {
        private IAdminService adminService;


        public AdminsController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Authorize(Roles = "admins")]
        [Route("")]
        public IEnumerable<Admin> GetAllAdmins()
        {
            logger.Info("Requesting admins");

            return adminService.GetAllAdmins().OrderBy(y => y.FirstName);
        }
        [Route("{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult GetAdminById(string id)
        {
            Admin admin = adminService.GetById(id);

            if (admin == null)
            {
                return NotFound();
            }

            logger.Info("Requesting admin by id");

            return Ok(admin);
        }

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(Admin))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult UpdateAdmin(string id, Admin admin)
        {
            Admin updatedAdmin = adminService.UpdateAdmin(id, admin);

            if (updatedAdmin == null)
            {
                return NotFound();
            }

            logger.Info("Updating admin");

            return Ok(updatedAdmin);
        }

        [Route("{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult DeleteAdmin(string id)
        {
            Admin admin = adminService.DeleteAdmin(id);

            if (admin == null)
            {
                return NotFound();
            }

            logger.Info("Deleting admin");

            return Ok(admin);
        }
    }
}
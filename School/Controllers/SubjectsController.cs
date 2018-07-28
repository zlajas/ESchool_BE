using NLog;
using School.Models;
using School.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace School.Controllers
{
    [RoutePrefix("project/subjects")]
    public class SubjectsController : ApiController
    {
        private ISubjectsService subjectsService;

        public SubjectsController(ISubjectsService subjectsService)
        {
            this.subjectsService = subjectsService;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Route("")]
        [Authorize(Roles = "admins")]
        public IEnumerable<Subject> GetAllSubjects()
        {
            logger.Info("Requesting subjects");

            return subjectsService.GetAllSubjects();
        }
        [Route("{id}")]
        [ResponseType(typeof(Subject))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetSubjectById(int id)
        {            
            Subject subject = subjectsService.GetById(id);

            if (subject == null)
            {
                return NotFound();
            }

            logger.Info("Requesting subject by id");

            return Ok(subject);
        }

        [Route("teachersTeachingSubject/{id}")]
        [ResponseType(typeof(Teacher))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetTeachersTeachingSubject(int id)
        {         
            Subject subject = subjectsService.GetById(id);

            if (subject == null)
            {
                return NotFound();
            }

            logger.Info("Requesting teachers for a subject");

            return Ok(subjectsService.GetTeachersTeachingSubject(id));
        }

        [Route("", Name = "PostSubject")]
        [ResponseType(typeof(Subject))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult PostSubject(Subject subject)
        { 
            Subject createdSubject = subjectsService.InsertSubject(subject);

            logger.Info("Adding new subject");

            return CreatedAtRoute("PostSubject", new { id = subject.SubjectId }, createdSubject);
        }

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(Subject))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult UpdateSubject(int id, Subject subject)
        {           
            Subject updatedSubject = subjectsService.UpdateSubject(id, subject);

            if (updatedSubject == null)
            {
                return NotFound();
            }

            logger.Info("update subject");

            return Ok(updatedSubject);
        }

        [Route("{id}")]
        [ResponseType(typeof(Subject))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult DeleteSubject(int id)
        {          
            Subject subject = subjectsService.DeleteSubject(id);

            if (subject == null)
            {
                return NotFound();
            }

            logger.Info("delete subject");

            return Ok(subject);
        }


    }
}

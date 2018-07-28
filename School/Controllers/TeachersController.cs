using NLog;
using School.Models;
using School.Models.DTOs;
using School.Models.DTOs.TeacherView;
using School.Models.Enum;
using School.Repositories;
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
    [RoutePrefix("project/teachers")]
    public class TeachersController : ApiController
    {
        private ITeachersService teachersService;
        private IStudentsService studentsService;
        private ISubjectsService subjectsService;

        public TeachersController(ITeachersService teachersService, IStudentsService studentsService, ISubjectsService subjectsService)
        {
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.subjectsService = subjectsService;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Route("")]
        [Authorize(Roles = "admins")]
        public IEnumerable<Teacher> GetAllTeachers()
        {
            logger.Info("Requesting teachers");

            return teachersService.GetAllTeachers().OrderBy(y => y.FirstName);
        }
        [Route("{id}")]
        [ResponseType(typeof(Teacher))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetTeacherById(string id)
        {           
            Teacher teacher = teachersService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            logger.Info("Requesting teacher by id");

            return Ok(teacher);
        }

        [Route("subjects/{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Subject))]
        public IHttpActionResult GetTeacherSubjects(string id)
        {               
            Teacher teacher = teachersService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            logger.Info("Requesting teachers subjects");

            return Ok(teachersService.GetTeachersSubjects(id));
        }

        [Route("remaining-subjects/{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Subject))]
        public IHttpActionResult GetAllRemainingSubjects(string id)
        {          
            Teacher teacher = teachersService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            logger.Info("Requesting remaining subjects");

            return Ok(teachersService.GetAllRemainingSubjects(id));

        }

        [Route("teachersView/{id}")]
        [Authorize(Roles = "admins, teachers")]
        [ResponseType(typeof(TeacherViewDTO))]
        [HttpGet]
        public IHttpActionResult GetAllForTeacher(string id)
        {           
            Teacher teacher = teachersService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            logger.Info("Requesting teachers view");

            return Ok(teachersService.GetAllForTeacher(id));
        }


        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(Teacher))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult UpdateTeacher(string id, Teacher teacher)
        {           
            Teacher updatedTeacher = teachersService.UpdateTeacher(id, teacher);

            if (updatedTeacher == null)
            {
                return NotFound();
            }

            logger.Info("Updating teacher");

            return Ok(updatedTeacher);
        }

        [Route("{teacherId}/addSubjectToTeacher/{subjectId}")]
        [ResponseType(typeof(Teacher))]
        [HttpPut]
        [Authorize(Roles = "admins")]
        public IHttpActionResult AddSubjectToTeacher(string teacherId, int subjectId)
        {           
            Teacher teacher = teachersService.GetById(teacherId);

            if (teacher == null || subjectsService.GetById(subjectId) == null)
            {
                return NotFound();
            }

            try
            {
                if (teacher.TeacherTeachesSubject.Select(x => x.Subject.SubjectId).Contains(subjectId))
                {
                    throw new Exception("Teacher already teaches this subject!");

                }
                else
                {
                    logger.Info("Adding subject to teacher");

                    teachersService.AddSubjectToTeacher(teacherId, subjectId);
                    return Ok(teacher.TeacherTeachesSubject.Select(s=>s.Subject));
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Adding subject to teacher");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("{studentId}/addMarkToStudent/{subjectId}/{teacherId}/{markValue}")]
        [ResponseType(typeof(Mark))]
        [HttpPost]
        [Authorize(Roles = "admins, teachers")]
        public IHttpActionResult AddMarkToStudent(string studentId, int subjectId, string teacherId, int markValue)
        {           
            Teacher teacher = teachersService.GetById(teacherId);
            Student student = studentsService.GetById(studentId);

            if (teacher == null || student == null ||subjectsService.GetById(subjectId) == null)
            {
                return NotFound();
            }

            try
            {
                if (!teacher.TeacherTeachesSubject.Select(x => x.Subject.SubjectId).Contains(subjectId))
                {
                    throw new NullReferenceException("Teacher doesn't teach this subject!");
                }
                else if (!student.StudentAttendsSubject.Select(x => x.TacherTeachesSubject?.Teacher).Contains(teacher))
                {
                    throw new NullReferenceException("Teacher doesn't teach this subject to provided student!");
                }
                else
                {
                    logger.Info("Adding mark to student");

                    Mark mark = teachersService.AddMarkToStudent(studentId, subjectId, teacherId, markValue);
                    return Ok(mark);
                }
                
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message, "Adding mark to student");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
            }

        }

        [Route("{teacherId}/deleteSubjectFromTeacher/{subjectId}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(TeacherToSubject))]
        [HttpPut]
        public IHttpActionResult RemoveSubjectFromTeacher(string teacherId, int subjectId)
        {
            Teacher teacher = teachersService.GetById(teacherId);

            if (teacher == null || subjectsService.GetById(subjectId) == null)
            {
                return NotFound();
            }

            try
            {
                if (!teacher.TeacherTeachesSubject.Select(x => x.Subject.SubjectId).Contains(subjectId))
                {
                    throw new NullReferenceException("Teacher doesn't teach this subject!");
                }
                
                else
                {
                    logger.Info("Removing subject from teacher");

                    TeacherToSubject teacherSubject = teachersService.RemoveSubjectTeacherPair(teacherId, subjectId);
                    return Ok(teacherSubject);
                }

            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message, "Removing subject from teacher");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
            }

        }

        [Route("{id}")]
        [ResponseType(typeof(Teacher))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult DeleteTeacher(string id)
        {           
            Teacher teacher = teachersService.DeleteTeacher(id);

            if (teacher == null)
            {
                return NotFound();
            }

            logger.Info("Deleting teacher");

            return Ok(teacher);
        }




    }
}

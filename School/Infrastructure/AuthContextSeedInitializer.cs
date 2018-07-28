using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using School.Infrastructure;
using School.Models.DTOs;
using School.Models.Enum;
using School.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace School.Models
{
    public enum FirstNames { Milan, Nenad, Marko, Nikola, Veselin, Zlatko, Zarko, Jovan, Marija, Ivana, Vesna, Petar, Vukasin, Aleksandar, Mitar, Slobodan, Ivanka, Mladen, Natasa, Doris, Nikolina, Zdenka, Danijela, Goran, Nemanja, Jovana }
    public enum LastNames { Miljanic, Stojakovic, Djordjevic, Grujic, Markovic, Petrovic, Nikolic, Jovanovic, Nenadic, Stankovic, Stojkovic, Mitrovic, Bogdanovic, Danilovic, Pesic, Obradovic, Ivkovic, Maljkovic, Jokic, Marjanovic, Milinkovic, Tadic, Stefanovic, Radulovic, Dacic, Kostic }
    public enum SubjectsFirstToFourthGrade {Srpski_jezik, Matematika, Likovna_kultura, Muzicka_kultura, Fizicko_vaspitanje, Engleski_jezik, Svet_oko_nas }
    public enum SubjectsFifthGrade {Srpski_jezik, Matematika, Likovna_kultura, Muzicka_kultura, Fizicko_vaspitanje, Engleski_jezik, Istorija, Geografija, Biologija, Tehnicko_i_informaticko_obrazovanje }
    public enum SubjectsSixthGrade { Srpski_jezik, Matematika, Likovna_kultura, Muzicka_kultura, Fizicko_vaspitanje, Engleski_jezik, Istorija, Geografija, Fizika, Biologija, Tehnicko_i_informaticko_obrazovanje }
    public enum SubjectsSeventhToEighthGrade { Srpski_jezik, Matematika, Likovna_kultura, Muzicka_kultura, Fizicko_vaspitanje, Engleski_jezik, Istorija, Geografija, Fizika, Biologija, Hemija, Tehnicko_i_informaticko_obrazovanje }
  

    public class AuthContextSeedInitializer : DropCreateDatabaseIfModelChanges<AuthContext>
    {

        public Random rnd = new Random();
        private int studentCount = 81;
        private int parentCount = 67;
        private int teacherCount = 21;
        private int subjectCount = 73;

        private Random gen = new Random();
        DateTime RandomDay()
        {
            DateTime start = new DateTime(2003, 1, 1);
            DateTime end = new DateTime(2011, 12, 31);
            int range = (end - start).Days;
            return start.AddDays(gen.Next(range));
        }

        protected override void Seed(AuthContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            roleManager.Create(new IdentityRole("students"));
            roleManager.Create(new IdentityRole("parents"));
            roleManager.Create(new IdentityRole("teachers"));
            roleManager.Create(new IdentityRole("admins"));

            // Add students
            Student[] students = new Student[studentCount];
            for (int i = 1; i < studentCount; i++)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                Student student = new Student();

                student.FirstName = System.Enum.GetName(typeof(FirstNames), rnd.Next(System.Enum.GetValues(typeof(FirstNames)).Length));
                student.LastName = System.Enum.GetName(typeof(LastNames), rnd.Next(System.Enum.GetValues(typeof(LastNames)).Length));
                student.UserName = string.Format("{0}{1}{2}", student.FirstName.ToLower(), student.LastName, rnd.Next(0, 100));
                string password = string.Format("{0}{1}", student.FirstName.ToLower(), rnd.Next(0, 100)); //password
                student.DateOfBirth = RandomDay();
                students[i] = student;
                userManager.Create(student, password); //ovde se prosledi password
                userManager.AddToRole(student.Id, "students"); //dodeljivanje role
            }
           
            context.SaveChanges();
            
            // Add students
            Parent[] parents = new Parent[parentCount];
            for (int g = 1; g <parentCount; g++)
            {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    Parent parent = new Parent();

                    parent.FirstName = System.Enum.GetName(typeof(FirstNames), rnd.Next(System.Enum.GetValues(typeof(FirstNames)).Length));
                    parent.LastName = System.Enum.GetName(typeof(LastNames), rnd.Next(System.Enum.GetValues(typeof(LastNames)).Length));
                    parent.UserName = string.Format("{0}{1}{2}", parent.FirstName.ToLower(), parent.LastName.ToLower(), rnd.Next(0, 100));
                    string password = string.Format("{0}{1}", parent.FirstName.ToLower(), rnd.Next(0, 100));
                    parent.EmailAddress = string.Format("{0}@example.com", parent.UserName);
                    parents[g] = parent;
                    userManager.Create(parent, password);
                    userManager.AddToRole(parent.Id, "parents");
            }

            context.SaveChanges();

            // Add students
            Teacher[] teachers = new Teacher[teacherCount];
            for (int z = 1; z < teacherCount; z++)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                Teacher teacher = new Teacher();

                teacher.FirstName = System.Enum.GetName(typeof(FirstNames), rnd.Next(System.Enum.GetValues(typeof(FirstNames)).Length));
                teacher.LastName = System.Enum.GetName(typeof(LastNames), rnd.Next(System.Enum.GetValues(typeof(LastNames)).Length));
                teacher.UserName = string.Format("{0}{1}{2}", teacher.FirstName.ToLower(), teacher.LastName.ToLower(), rnd.Next(0, 100));
                string password = string.Format("{0}{1}", teacher.FirstName.ToLower(), 777);
                teacher.EmailAddress = string.Format("{0}@example.com", teacher.UserName);
                teachers[z] = teacher;
                userManager.Create(teacher, password);
                userManager.AddToRole(teacher.Id, "teachers");
            }
            context.SaveChanges();

            // Add students
            Subject[] subjects = new Subject[subjectCount];
            int subjectName = (int)SubjectsFirstToFourthGrade.Srpski_jezik;
            for (int i = 0; i < 7; i++)
            {
                Subject subject = new Subject();

                subject.SubjectName = System.Enum.GetName(typeof(SubjectsFirstToFourthGrade), subjectName);
                if (subject.SubjectName == "Matematika" || subject.SubjectName == "Srpski_jezik")
                {
                    subject.WeeklyFund = 5;
                }
                else if (subject.SubjectName == "Fizicko_vaspitanje")
                {
                    subject.WeeklyFund = 3;
                }
                else
                {
                    subject.WeeklyFund = 2;
                }

                subject.Semester = ESemester.SECOND;
                subject.Grade = EGradeYear.FIRST;
                context.Subjects.Add(subject);
                subjects[i] = subject;
                subjectName++;
                if (subjectName > (int)SubjectsFirstToFourthGrade.Svet_oko_nas)
                {
                    subjectName = (int)SubjectsFirstToFourthGrade.Srpski_jezik;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                Subject subject = new Subject();
                subject.SubjectName = System.Enum.GetName(typeof(SubjectsFirstToFourthGrade), subjectName);
                if (subject.SubjectName == "Matematika" || subject.SubjectName == "Srpski_jezik")
                {
                    subject.WeeklyFund = 5;
                }
                else if (subject.SubjectName == "Fizicko_vaspitanje")
                {
                    subject.WeeklyFund = 3;
                }
                else
                {
                    subject.WeeklyFund = 2;
                }

                subject.Semester = ESemester.SECOND;
                subject.Grade = EGradeYear.SECOND;
                context.Subjects.Add(subject);
                subjects[i] = subject;
                subjectName++;
                if (subjectName > (int)SubjectsFirstToFourthGrade.Svet_oko_nas)
                {
                    subjectName = (int)SubjectsFirstToFourthGrade.Srpski_jezik;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                Subject subject = new Subject();
                subject.SubjectName = System.Enum.GetName(typeof(SubjectsFirstToFourthGrade), subjectName);
                if (subject.SubjectName == "Matematika" || subject.SubjectName == "Srpski_jezik")
                {
                    subject.WeeklyFund = 5;
                }
                else if (subject.SubjectName == "Fizicko_vaspitanje")
                {
                    subject.WeeklyFund = 3;
                }
                else
                {
                    subject.WeeklyFund = 2;
                }

                subject.Semester = ESemester.SECOND;
                subject.Grade = EGradeYear.THIRD;
                context.Subjects.Add(subject);
                subjects[i] = subject;
                subjectName++;
                if (subjectName > (int)SubjectsFirstToFourthGrade.Svet_oko_nas)
                {
                    subjectName = (int)SubjectsFirstToFourthGrade.Srpski_jezik;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                Subject subject = new Subject();
                subject.SubjectName = System.Enum.GetName(typeof(SubjectsFirstToFourthGrade), subjectName);
                if (subject.SubjectName == "Matematika" || subject.SubjectName == "Srpski_jezik")
                {
                    subject.WeeklyFund = 5;
                }
                else if (subject.SubjectName == "Fizicko_vaspitanje")
                {
                    subject.WeeklyFund = 3;
                }
                else
                {
                    subject.WeeklyFund = 2;
                }

                subject.Semester = ESemester.SECOND;
                subject.Grade = EGradeYear.FOURTH;
                context.Subjects.Add(subject);
                subjects[i] = subject;
                subjectName++;
                if (subjectName > (int)SubjectsFirstToFourthGrade.Svet_oko_nas)
                {
                    subjectName = (int)SubjectsFirstToFourthGrade.Srpski_jezik;
                }
            }

            subjectName = (int)SubjectsFifthGrade.Srpski_jezik;
            for (int i = 0; i < 10; i++)
            {
                Subject subject = new Subject();
                subject.SubjectName = System.Enum.GetName(typeof(SubjectsFifthGrade), subjectName);
                if (subject.SubjectName == "Matematika" || subject.SubjectName == "Srpski_jezik")
                {
                    subject.WeeklyFund = 4;
                }
                else if (subject.SubjectName == "Istorija" || subject.SubjectName == "Geografija")
                {
                    subject.WeeklyFund = 1;
                }
                else
                {
                    subject.WeeklyFund = 2;
                }

                subject.Semester = ESemester.SECOND;
                subject.Grade = EGradeYear.FIFTH;
                context.Subjects.Add(subject);
                subjects[i] = subject;
                subjectName++;
                if (subjectName > (int)SubjectsFifthGrade.Tehnicko_i_informaticko_obrazovanje)
                {
                    subjectName = (int)SubjectsFirstToFourthGrade.Srpski_jezik;
                }
            }

            subjectName = (int)SubjectsSixthGrade.Srpski_jezik;
            for (int i = 0; i < 11; i++)
            {
                Subject subject = new Subject();
                subject.SubjectName = System.Enum.GetName(typeof(SubjectsSixthGrade), subjectName);
                if (subject.SubjectName == "Matematika" || subject.SubjectName == "Srpski_jezik")
                {
                    subject.WeeklyFund = 4;
                }
                else if (subject.SubjectName == "Istorija" || subject.SubjectName == "Geografija")
                {
                    subject.WeeklyFund = 1;
                }
                else
                {
                    subject.WeeklyFund = 2;
                }

                subject.Semester = ESemester.SECOND;
                subject.Grade = EGradeYear.SIXTH;
                context.Subjects.Add(subject);
                subjects[i] = subject;
                subjectName++;
                if (subjectName > (int)SubjectsSixthGrade.Tehnicko_i_informaticko_obrazovanje)
                {
                    subjectName = (int)SubjectsSixthGrade.Srpski_jezik;
                }
            }

            subjectName = (int)SubjectsSeventhToEighthGrade.Srpski_jezik;
            for (int i = 0; i < 12; i++)
            {
                Subject subject = new Subject();
                subject.SubjectName = System.Enum.GetName(typeof(SubjectsSeventhToEighthGrade), subjectName);
                if (subject.SubjectName == "Matematika" || subject.SubjectName == "Srpski_jezik")
                {
                    subject.WeeklyFund = 4;
                }
                else if (subject.SubjectName == "Istorija" || subject.SubjectName == "Geografija")
                {
                    subject.WeeklyFund = 1;
                }
                else
                {
                    subject.WeeklyFund = 2;
                }

                subject.Semester = ESemester.SECOND;
                subject.Grade = EGradeYear.SEVENTH;
                context.Subjects.Add(subject);
                subjects[i] = subject;
                subjectName++;
                if (subjectName > (int)SubjectsSeventhToEighthGrade.Tehnicko_i_informaticko_obrazovanje)
                {
                    subjectName = (int)SubjectsSeventhToEighthGrade.Srpski_jezik;
                }
            }

            subjectName = (int)SubjectsSeventhToEighthGrade.Srpski_jezik;
            for (int i = 0; i < 12; i++)
            {
                Subject subject = new Subject();
                subject.SubjectName = System.Enum.GetName(typeof(SubjectsSeventhToEighthGrade), subjectName);
                if (subject.SubjectName == "Matematika" || subject.SubjectName == "Srpski_jezik")
                {
                    subject.WeeklyFund = 4;
                }
                else if (subject.SubjectName == "Istorija" || subject.SubjectName == "Geografija")
                {
                    subject.WeeklyFund = 1;
                }
                else
                {
                    subject.WeeklyFund = 2;
                }

                subject.Semester = ESemester.SECOND;
                subject.Grade = EGradeYear.EIGHTH;
                context.Subjects.Add(subject);
                subjects[i] = subject;
                subjectName++;
                if (subjectName > (int)SubjectsSeventhToEighthGrade.Tehnicko_i_informaticko_obrazovanje)
                {
                    subjectName = (int)SubjectsSeventhToEighthGrade.Srpski_jezik;
                }
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
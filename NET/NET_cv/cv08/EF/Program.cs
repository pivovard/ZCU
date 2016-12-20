using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer<Stag>(new StagInit());

            Stag stag = new Stag();
            stag.Database.Log = Console.WriteLine;

            Console.WriteLine(stag.Departments.Count());

            stag.Departments.Add(new Department() { });

            var errors = stag.GetValidationErrors();
            if (!errors.Any())
            {
                stag.SaveChanges();
            }
            else
            {
                foreach(var error in errors)
                {
                    foreach(var e in error.ValidationErrors)
                    {
                        Console.WriteLine($"{e.PropertyName} : {e.ErrorMessage}");
                    }
                }
            }

            

            Console.ReadKey();
        }
    }

    public class StagInit : DropCreateDatabaseIfModelChanges<Stag>
    {
        protected override void Seed(Stag context)
        {
            base.Seed(context);

            Department kiv = new Department() { Abbr = "KIV", Name = "Informatika" };
            kiv.Courses.Add(new Course() { Abbr = "NET", Department = kiv });

            context.Departments.Add(kiv);
            context.SaveChanges();
        }
    }

    public class Stag : DbContext
    {
        public DbSet<Department> Departments { get; set; }
    }

    public class Department
    {
        public int ID { get; set; }

        [Required]
        public string Abbr { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual List<Course> Courses { get; set; }
    }

    public class Course
    {
        public int ID { get; set; }
        [Required]
        public string Abbr { get; set; }
        public Department Department { get; set; }
    }
}

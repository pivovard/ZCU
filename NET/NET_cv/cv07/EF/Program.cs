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
            //Database.SetInitializer<Stag>(new StagInit());

            Stag stag = new Stag();

            Console.WriteLine(stag.Departments.Count());

            //stag.Departments.Add(new Department() { Abbr = "KIV", Name = "Informatika" });
            //stag.SaveChanges();
        }
    }

    public class StagInit : DropCreateDatabaseIfModelChanges<Stag>
    {
        protected override void Seed(Stag context)
        {
            base.Seed(context);
            context.Departments.Add(new Department() { Abbr = "KIV", Name = "Informatika" });
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
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    // PersonsDbContext => Represent the sql server Database Name
    public class ApplicationDbContext : DbContext
    {
        // database string pass through this constructor to the parent class  program.cs > Dbcontext > parent
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        // name of db set is pulare it is strongly recommand 
        public virtual DbSet<Country> Countries { get; set; } // DbSet<Country> => Represent the Sql server database table
        public virtual DbSet<Person> Persons { get; set; }

        // DbSet the bind the correspoind to the database table 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            // Seed to countries 
            string CountriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(CountriesJson);

            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }          

            // Seed to Person 
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> _persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person persons in _persons)
            {
                modelBuilder.Entity<Person>().HasData(persons);
            }

            // fluend api 
            modelBuilder.Entity<Person>().Property(temp => temp.PhoneNumber)
                .HasColumnName("Number")
                .HasColumnType("nvarchar(20)")
                .HasDefaultValue("+91");
                


        }
        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@PersonID",person.PersonID),
                new SqlParameter("@PersonName",person.PersonName),
                new SqlParameter("@Email",person.Email),
                new SqlParameter("@DateOfBirth",person.DateOfBirth),
                new SqlParameter("@Geneder",person.Geneder),
                new SqlParameter("@CountryID",person.CountryID),
                new SqlParameter("@Address",person.Address),
                new SqlParameter("@ReceivedNewsLetter",person.ReceivedNewsLetter)
            };

           return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson]@PersonID,@PersonName,@Email,@DateOfBirth,@Geneder,@CountryID,@Address,@ReceivedNewsLetter", parameter);


        }
    }
}

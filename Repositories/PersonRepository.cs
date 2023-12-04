using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System.Linq.Expressions;

namespace Repositories
{
    public class PersonRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;

        public PersonRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await  _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid personID)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(temp => temp.PersonID == personID));
            int rowDeleted = await _db.SaveChangesAsync();
            return rowDeleted > 0;
        }

        public async Task<List<Person>> GetAllPerson()
        {
          return await  _db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include("Country")
                .Where(predicate)
                .ToListAsync();

        }

        public async Task<Person?> GetPersonByPersonID(Guid personID)
        {
            return await _db.Persons.Include("Country")
                 .FirstOrDefaultAsync(temp => temp.PersonID == personID);
                 
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person matchingPerson = await _db.Persons.FirstAsync(temp => temp.PersonID == person.PersonID);

            if(matchingPerson == null)
                return person;

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Geneder = person.Geneder;
            matchingPerson.CountryID = person.CountryID;
            matchingPerson.Address = person.Address;
            matchingPerson.ReceivedNewsLetter = person.ReceivedNewsLetter;

            int countUpdated = await _db.SaveChangesAsync();
            return matchingPerson;
        }
    }
}

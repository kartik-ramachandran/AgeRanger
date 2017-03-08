using DataLayer.Domain;
using System.Collections.Generic;

namespace DataLayer.Repository
{
    public interface IPersonRepository
    {
        Person GetPerson(long id);
        long SavePerson(Person person);
        dynamic GetAllPerson();
        IEnumerable<dynamic> SearchPerson(string firstName, string lastName);
        dynamic GetAgeGroup();
    }
}
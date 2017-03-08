using System.IO;
using System.Linq;
using DataLayer.Domain;
using Dapper;
using System.Collections.Generic;

namespace DataLayer.Repository
{
    public class SqLitePersonRepository : SqLiteBaseRepository, IPersonRepository
    {
        public Person GetPerson(long id)
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                Person result = cnn.Query<Person>(
                    @"SELECT p.Id, FirstName, LastName, Age
                    FROM Person p
                    WHERE p.Id = @id", new { id }).FirstOrDefault();
                return result;
            }
        }

        public dynamic GetAllPerson()
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                var result = cnn.Query(
                    @"SELECT p.Id, FirstName, LastName, Age
                    FROM Person p");
                return result;
            }
        }

        public long SavePerson(Person person)
        {
            if (!File.Exists(DbFile))
            {
                CreateDatabase();
            }

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                person.Id = cnn.Query<long>(
                    @"INSERT INTO Person 
                    ( FirstName, LastName, Age ) VALUES 
                    ( @FirstName, @LastName, @Age );
                    select last_insert_rowid()", person).First();

                return person.Id;
            }
        }

        public IEnumerable<dynamic> SearchPerson(string firstName, string lastName)
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();

                var query = string.Empty;

                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    query = @"SELECT p.Id, FirstName, LastName, Age FROM Person p WHERE FirstName Like @firstName || '%' And LastName Like @lastName || '%'";
                }

                if (string.IsNullOrEmpty(firstName))
                {
                    query = @"SELECT p.Id, FirstName, LastName, Age FROM Person p WHERE LastName Like @lastName || '%'";
                }

                if (string.IsNullOrEmpty(lastName))
                {
                    query = @"SELECT p.Id, FirstName, LastName, Age FROM Person p WHERE FirstName Like @firstName || '%'";
                }

                var results = cnn.Query(query, new { firstName, lastName });

                return results;
            }
        }

        private static void CreateDatabase()
        {
            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @"create table Customer
                      (
                         Id                                  bigint primary key,
                         FirstName                           text not null,
                         LastName                            text not null,
                         Age                         bigint not null
                      )");
            }
        }

        public dynamic GetAgeGroup()
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                var result = cnn.Query(
                    @"SELECT Id, MinAge, MaxAge, Description
                    FROM AgeGroup");
                return result;
            }
        }
    }
}

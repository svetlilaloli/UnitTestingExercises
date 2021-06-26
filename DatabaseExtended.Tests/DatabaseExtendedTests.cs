using ExtendedDatabase;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DatabaseExtended.Tests
{
    [TestFixture]
    public class DatabaseExtendedTests
    {
        private string name = "George";
        private long id = 12345;
        [Test]
        public void ExtendedDatabaseConstructor_CreateDatabaseWithoutParams_CountShouldBeZero()
        {
            var db = new ExtendedDatabase.ExtendedDatabase();
            var actual = db.Count;
            Assert.AreEqual(0, actual);
        }
        [Test]
        public void ExtendedDatabaseConstructor_CreateDatabase_ShouldStoreDataInArray()
        {
            var db = new ExtendedDatabase.ExtendedDatabase();
            var wrappedDatabase = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(db);
            var dbData = wrappedDatabase.GetField("people");
            var actual = dbData.GetType().Name;
            Assert.AreEqual(typeof(Person[]).Name, actual);
        }
        [Test]
        public void ExtendedDatabaseConstructor_ArrayCapacityMustBe16()
        {
            var db = new ExtendedDatabase.ExtendedDatabase();
            var wrappedDatabase = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(db);
            var dbData = (Person[])wrappedDatabase.GetField("people");
            var actual = dbData.Length;
            Assert.AreEqual(16, actual);
        }
        [Test]
        public void Add_AddNullPerson_ShouldTrowArgumentNullException()
        {
            var db = new ExtendedDatabase.ExtendedDatabase();

            var ex = Assert.Throws<ArgumentNullException>(() => db.Add(null));
            StringAssert.Contains("Person cannot be null", ex.Message);
        }
        [Test]
        public void Add_AddMoreThan16People_ShouldThrowInvalidOperationException()
        {
            var db = new ExtendedDatabase.ExtendedDatabase();
            for (int i = 0; i < 16; i++)
            {
                db.Add(new FakePerson(id++, name += 'a'));
            }
            var ex = Assert.Throws<InvalidOperationException>(() => db.Add(new FakePerson(id++, name += 'z')));
            StringAssert.Contains("Array's capacity must be exactly 16", ex.Message);
        }
        [Test]
        public void Add_AddPersonWithSameUsername_ShouldTrowInvalidOperationException()
        {
            var db = new ExtendedDatabase.ExtendedDatabase();
            db.Add(new FakePerson(123, "John"));
            
            var ex = Assert.Throws<InvalidOperationException>(() => db.Add(new FakePerson(124, "John")));
            StringAssert.Contains("There is already user with this username", ex.Message);
        }
        [Test]
        public void Add_AddPersonWithSameId_ShouldTrowInvalidOperationException()
        {
            var db = new ExtendedDatabase.ExtendedDatabase();
            db.Add(new FakePerson(123, "John"));

            var ex = Assert.Throws<InvalidOperationException>(() => db.Add(new FakePerson(123, "Johnna")));
            StringAssert.Contains("There is already user with this Id", ex.Message);
        }
        [Test]
        public void Add_AddValidPerson_ShouldAddToTheDatabase()
        {
            var db = new ExtendedDatabase.ExtendedDatabase();
            db.Add(new FakePerson(123, "John"));
            var actual = db.Count;
            Assert.AreEqual(1, actual);
        }
        [Test]
        public void Remove_RemoveFromEmptyDatabase_ShouldThrowInvelidOperationException()
        {
            var db = new ExtendedDatabase.ExtendedDatabase();
            Assert.Throws<InvalidOperationException>(() => db.Remove());
        }
        [Test]
        public void Remove_RemoveElementFromNotEmptyDatabase_ShouldDecrementCount()
        {
            var db = new ExtendedDatabase.ExtendedDatabase(new FakePerson(123, "John"));
            db.Remove();

            var actual = db.Count;
            Assert.AreEqual(0, actual);
        }
        [Test]
        public void Remove_RemoveElementFromNotEmptyDatabase_ShouldRemoveLastElementAndSetDefaultValue()
        {
            var db = new ExtendedDatabase.ExtendedDatabase(new FakePerson(123, "John"));
            db.Remove();
            var actual = db.GetType().GetField("people", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
            var emptyDb = new ExtendedDatabase.ExtendedDatabase();
            var expected = emptyDb.GetType().GetField("people", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(emptyDb);
            Assert.AreEqual(expected, actual);
        }
        [TestCase("")]
        [TestCase(null)]
        public void FindByUsername_UsernameParameterIsNullOrEmpty_ShouldThrowArgumentNullException(string username)
        {
            var db = new ExtendedDatabase.ExtendedDatabase();

            var ex = Assert.Throws<ArgumentNullException>(() => db.FindByUsername(username));
            StringAssert.Contains("Username parameter is null", ex.Message);
        }
        [TestCase("Jane")]
        [TestCase("john")]
        public void FindByUsername_NoUserPresentByProvidedUsernameArgumentsAreCaseSensitive_ShouldThrowInvalidOperationException(string username)
        {
            var db = new ExtendedDatabase.ExtendedDatabase(new FakePerson(123, "John"));
            var ex = Assert.Throws<InvalidOperationException>(() => db.FindByUsername(username));
            StringAssert.Contains("No user is present by this username", ex.Message);
        }
        [Test]
        public void FindByUsername_PersonPresentByProvidedUsername_ShouldReturnThePerson()
        {
            var expected = new FakePerson(123, "John");
            var db = new ExtendedDatabase.ExtendedDatabase(expected);
            var actual = db.FindByUsername("John");
            Assert.AreSame(expected, actual);
        }
        [Test]
        public void FindById_NoUserPresentByProvidedId_ShouldThrowInvalidOperationException()
        {
            var db = new ExtendedDatabase.ExtendedDatabase(new FakePerson(123, "John"));
            var ex = Assert.Throws<InvalidOperationException>(() => db.FindById(33333));
            StringAssert.Contains("No user is present by this ID", ex.Message);
        }
        [Test]
        public void FindById_NegativeIdProvided_ShouldThrowArgumentOutOfRangeException()
        {
            var db = new ExtendedDatabase.ExtendedDatabase(new FakePerson(123, "John"));
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => db.FindById(-123));
            StringAssert.Contains("Id should be a positive number", ex.Message);
        }
        [Test]
        public void FindById_PersonPresentByProvidedId_ShouldReturnThePerson()
        {
            var expected = new FakePerson(123, "John");
            var db = new ExtendedDatabase.ExtendedDatabase(expected);
            var actual = db.FindById(123);
            Assert.AreSame(expected, actual);
        }
    }
}

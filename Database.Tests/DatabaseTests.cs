using Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class DatabaseTests
    {
        [Test]
        public void DatabaseConstructor_CreateDatabaseWithoutParams_CountShouldBeZero()
        {
            var db = new Database.Database();
            var actual = db.Count;
            Assert.AreEqual(0, actual);
        }
        [Test]
        public void DatabaseConstructor_CreateDatabaseWithIntegers_ShouldStoreDataInArray()
        {
            var db = new Database.Database(1, 2, 3);
            var wrappedDatabase = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(db);
            var dbData = wrappedDatabase.GetField("data");
            var actual = dbData.GetType().Name;
            Assert.AreEqual(typeof(Int32[]).Name, actual);
        }
        [Test]
        public void DatabaseConstructor_ArrayCapacityMustBe16()
        {
            var db = new Database.Database();
            var wrappedDatabase = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(db);
            int[] dbData = (int[])wrappedDatabase.GetField("data");
            var actual = dbData.Length;
            Assert.AreEqual(16, actual);
        }
        [Test]
        public void Add_AddMoreThan16Elements_ShouldThrowInvalidOperationException()
        {
            var db = new Database.Database(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            var ex = Assert.Throws<InvalidOperationException>(() => db.Add(17));
            StringAssert.Contains("Array's capacity must be exactly 16 integers", ex.Message);
        }
        [Test]
        public void Add_AddElement_ShouldAddElementAtTheNextFreeCell()
        {
            var db = new Database.Database(1);
            db.Add(2);
            var wrappedDatabase = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(db);
            int[] dbData = (int[])wrappedDatabase.GetField("data");
            var actual = dbData[1];

            Assert.AreEqual(2, actual);
        }
        [Test]
        public void Add_AddElement_ShouldAddElementToTheDatabase()
        {
            var db = new Database.Database();
            db.Add(1);
            var actual = db.Count;
            Assert.AreEqual(1, actual);
        }
        [Test]
        public void Remove_RemoveElementFromEmptyDatabase_ShouldThrowInvalidOperationException()
        {
            var db = new Database.Database();
            var ex = Assert.Throws<InvalidOperationException>(() => db.Remove());
            StringAssert.Contains("The collection is empty", ex.Message);
        }
        [Test]
        public void Remove_RemoveElementFromNotEmptyDatabase_ShouldRemoveLastElementSettingDefaultValue()
        {
            var db = new Database.Database(1, 2, 3);
            db.Remove();
            var wrappedDatabase = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(db);
            int[] dbData = (int[])wrappedDatabase.GetField("data");
            var actual = dbData[db.Count];
            Assert.AreEqual(0, actual);
        }
        [Test]
        public void Fetch_GetTheElementsFromTheDatabase_ShouldReturnElementsAsArray()
        {
            var db = new Database.Database(1);
            var actual = db.Fetch();
            Assert.AreEqual(typeof(Int32[]).Name, actual.GetType().Name);
        }
    }
}
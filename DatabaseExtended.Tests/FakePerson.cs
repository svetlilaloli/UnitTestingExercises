using ExtendedDatabase;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseExtended.Tests
{
    public class FakePerson : Person
    {
        public FakePerson(long id, string userName) : base(id, userName)
        {
        }
    }
}

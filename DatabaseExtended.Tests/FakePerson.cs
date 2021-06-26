using ExtendedDatabase;

namespace DatabaseExtended.Tests
{
    public class FakePerson : Person
    {
        public FakePerson(long id, string userName) : base(id, userName)
        {
        }
    }
}

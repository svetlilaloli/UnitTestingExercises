using FightingArena;
using NUnit.Framework;
using System;
using System.Linq;

namespace Tests
{
    public class ArenaTests
    {
        private Arena arena;
        private Warrior warrior;
        [SetUp]
        public void Setup()
        {
            arena = new Arena();
            warrior = new Warrior("Jack", 35, 100);
        }

        [Test]
        public void Enroll_AlreadyEnrolledWarrior_ShouldThrowInvalidOperationException()
        {
            arena.Enroll(warrior);
            var ex = Assert.Throws<InvalidOperationException>(() => arena.Enroll(warrior));
            StringAssert.Contains("Warrior is already enrolled for the fights", ex.Message);
        }
        [Test]
        public void Enroll_NotEnrolledWarrior_ShouldAddTheWarriorToTheArena()
        {
            arena.Enroll(warrior);
            var actual = arena.Warriors.Any(x => x.Name == warrior.Name);
            Assert.IsTrue(actual);
        }
        [Test]
        public void Enroll_NotEnrolledWarrior_ShouldUpdateCount()
        {
            arena.Enroll(warrior);
            var expected = 1;
            var actual = arena.Count;
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Enroll_AttackerWarriorNotEnrolled_ShouldThrowInvalidOperationException()
        {
            var attacker = new Warrior("James", 45, 90);
            var defender = warrior;
            arena.Enroll(defender);
            var ex = Assert.Throws<InvalidOperationException>(() => arena.Fight(attacker.Name, defender.Name));
            StringAssert.Contains($"There is no fighter with name {attacker.Name} enrolled for the fights", ex.Message);
        }
        [Test]
        public void Enroll_DefenderWarriorNotEnrolled_ShouldThrowInvalidOperationException()
        {
            var defender = new Warrior("James", 45, 90);
            var attacker = warrior;
            arena.Enroll(attacker);
            var ex = Assert.Throws<InvalidOperationException>(() => arena.Fight(attacker.Name, defender.Name));
            StringAssert.Contains($"There is no fighter with name {defender.Name} enrolled for the fights", ex.Message);
        }
    }
}

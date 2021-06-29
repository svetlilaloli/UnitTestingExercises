using FightingArena;
using NUnit.Framework;
using System;

namespace Tests
{
    public class WarriorTests
    {
        private const string warriorName = "Jack";
        private const int warriorDamage = 35;
        private const int warriorHp = 100;
        Warrior warrior;
        [SetUp]
        public void Setup()
        {
            warrior = new Warrior(warriorName, warriorDamage, warriorHp);
        }

        [Test]
        public void Constructor_WithValidParameters_ShouldSetTheFieldsCorrectly()
        {
            var name = warrior.Name;
            var damage = warrior.Damage;
            var hp = warrior.HP;
            var actual = false;
            if (name == warriorName && damage == warriorDamage && hp == warriorHp)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }
        [TestCase(null)]
        [TestCase(" ")]
        public void Constructor_WithNullOrWhitespaceName_ShouldThrowArgumentException(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Warrior(name, warriorDamage, warriorHp));
            StringAssert.Contains("Name should not be empty or whitespace", ex.Message);
        }
        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_WithZeroOrNegativeDamage_ShouldThrowArgumentException(int damage)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Warrior(warriorName, damage, warriorHp));
            StringAssert.Contains("Damage value should be positive", ex.Message);
        }
        [TestCase(-1)]
        public void Constructor_WithNegativeHp_ShouldThrowArgumentException(int hp)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Warrior(warriorName, warriorDamage, hp));
            StringAssert.Contains("HP should not be negative", ex.Message);
        }
        [TestCase(30)]
        [TestCase(29)]
        public void Attack_AttackingWarriorHasHpLessOrEqualToMinAttackHp_ShouldThrowInvalidOperationException(int hp)
        {
            var attackingWarrior = new Warrior(warriorName, warriorDamage, hp);
            var ex = Assert.Throws<InvalidOperationException>(() => attackingWarrior.Attack(warrior));
            StringAssert.Contains("Your HP is too low in order to attack other warriors", ex.Message);
        }
        [TestCase(30)]
        [TestCase(29)]
        public void Attack_AttackedWarriorHasHpLessOrEqualToMinAttackHp_ShouldThrowInvalidOperationException(int hp)
        {
            var attackedWarrior = new Warrior(warriorName, warriorDamage, hp);
            var ex = Assert.Throws<InvalidOperationException>(() => warrior.Attack(attackedWarrior));
            StringAssert.Contains("Enemy HP must be greater than ", ex.Message);
        }
        [Test]
        public void Attack_AttackedWarriorHasHpLessThanAttackedWarriorDamage_ShouldThrowInvalidOperationException()
        {
            var attackingWarrior = new Warrior(warriorName, warriorDamage, 31);
            var ex = Assert.Throws<InvalidOperationException>(() => attackingWarrior.Attack(warrior));
            StringAssert.Contains("You are trying to attack too strong enemy", ex.Message);
        }
        [Test]
        public void Attack_AttackingWarriorHasHpGreaterThanMinimumAttackHpAndAttackedWarriorDamage_ShouldUpdateCorrectlyAttackingWarriorHp()
        {
            var attackingWarrior = new Warrior(warriorName, warriorDamage, warriorHp);
            attackingWarrior.Attack(warrior);
            var expected = warriorHp - warriorDamage;
            var actual = attackingWarrior.HP;
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Attack_AttackingWarriorHasHpGreaterThanMinimumAttackHpAndEqualToAttackedWarriorDamage_ShouldUpdateCorrectlyAttackedWarriorHp()
        {
            var attackingWarrior = new Warrior(warriorName, 100, warriorHp);

            var expected = 0;
            attackingWarrior.Attack(warrior);
            var actual = warrior.HP;
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Attack_AttackingWarriorHasHpGreaterThanMinimumAttackHpAndAttackedWarriorDamage_ShouldUpdateCorrectlyAttackedWarriorHp()
        {
            var attackingWarrior = new Warrior(warriorName, 100, warriorHp);

            var expected = warrior.HP - attackingWarrior.Damage;
            attackingWarrior.Attack(warrior);
            var actual = warrior.HP;

            Assert.AreEqual(expected, actual);
        }
    }
}
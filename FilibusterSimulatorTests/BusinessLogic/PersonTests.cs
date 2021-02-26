using Microsoft.VisualStudio.TestTools.UnitTesting;
using FilibusterSimulator.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilibusterSimulator.BusinessLogic.Tests
{
	[TestClass()]
	public class PersonTests
	{
		Person person = new Person { Satiety = 100, Mood = 100, Stamina = 100, Gold = 100 };

		// Сытость
		[TestMethod()]
		public void SubstractSatiety_20()
		{
			Assert.IsTrue(person.SubstractSatiety(20));
		}

		[TestMethod()]
		public void SubstractSatiety_120()
		{
			Assert.IsFalse(person.SubstractSatiety(120));
		}

		[TestMethod()]
		public void SubstractSatiety_minus20()
		{
			Assert.IsFalse(person.SubstractSatiety(-20));
		}


		// Настроение
		[TestMethod()]
		public void SubstractMood_20()
		{
			Assert.IsTrue(person.SubstractMood(20));
		}

		[TestMethod()]
		public void SubstractMood_120()
		{
			Assert.IsFalse(person.SubstractMood(120));
		}

		[TestMethod()]
		public void SubstractMood_minus20()
		{
			Assert.IsFalse(person.SubstractMood(-20));
		}

		// Запас жизненных сил
		[TestMethod()]
		public void SubstractStamina_20()
		{
			Assert.IsTrue(person.SubstractStamina(20));
		}

		[TestMethod()]
		public void SubstractStamina_120()
		{
			Assert.IsFalse(person.SubstractStamina(120));
		}

		[TestMethod()]
		public void SubstractStamina_minus20()
		{
			Assert.IsFalse(person.SubstractStamina(-20));
		}

		// Золото
		[TestMethod()]
		public void SubstractGold_20()
		{
			Assert.IsTrue(person.SubstractGold(20));
		}

		[TestMethod()]
		public void SubstractGold_120()
		{
			Assert.IsFalse(person.SubstractGold(120));
		}

		[TestMethod()]
		public void SubstractGold_minus20()
		{
			Assert.IsFalse(person.SubstractGold(-20));
		}
	}
}
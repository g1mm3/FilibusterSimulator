using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilibusterSimulator.BusinessLogic
{
	public class Person
	{
		// Делегат и событие для отправки сообщений во внешнюю среду
		public delegate void NotificationHandler(string message);
		public event NotificationHandler Notify;

		//Основные характеристики игрока
		public int Satiety { get; set; }
		public int Mood { get; set; }
		public int Stamina { get; set; }
		public int Gold { get; set; }
		public Rank Rank { get; set; }

		public bool IsMoored { get; set; }
		public int RobbedShipsCount { get; set; }
		public TimeSpan CurrentTime { get; set; }

		private int eatingsCount;

		private static Random random;

		public Person()
		{
			eatingsCount = 0;
			random = new Random();
		}

		//Получить строку с текущим временем в формате: текущий день | часы:минуты
		public string GetTimeString()
		{
			// DayOfYear можно использовать для счета кол-ва возможных дней без учета кол-ва месяцев
			return $"{CurrentTime.Days} день | {CurrentTime.Hours}:{CurrentTime.Minutes}";
		}

		public void Eat()
		{
			if (SpendResourcesByHours(0.5))
			{
				string messageText = "Вы поели =) \n+50 к сытости \n+10 к настроению \n+20 к запасу жизненных сил \nПрошло 30 минут";

				// Каждый пятый прием пищи -250 золота
				if (eatingsCount % 5 == 0)
				{
					if (eatingsCount > 0)
					{
						if (SubstractGold(250))
							messageText += "\n-250 золота";
					}
				}

				AddSatiety(50);
				AddMood(10);
				AddStamina(20);
				CurrentTime = CurrentTime.Add(TimeSpan.FromMinutes(30));

				eatingsCount++;

				Notify?.Invoke(messageText);
			}
		}

		public void Sleep()
		{
			//if (SpendResourcesByHours(8))
			if (SubstractSatiety(3 * 8))
			{
				AddStamina(1000);
				AddMood(10);
				CurrentTime = CurrentTime.Add(TimeSpan.FromHours(8));
				Notify?.Invoke("Вы поспали =) \n+10 к настроению" +
					"\nЗапас жизненных сил пополен до 100 \nПрошло 8 часов");
			}
		}

		public void Dance()
		{
			if (SpendResourcesByHours(0.5))
			{
				if (SubstractStamina(10))
				{
					AddMood(25);
					Notify?.Invoke("Вы потанцевали =) \n+25 к настроению" +
					"\n-10 к запасу жизненных сил \nПрошло 30 минут");
				}
			}
		}

		public void Moor()
		{
			Notify?.Invoke("Вы пришвартовались \nТеперь вы можете сходить в бар");
			IsMoored = true;
		}

		public void Unmoor()
		{
			Notify?.Invoke("Вы отчалили \nТеперь вы не сможете сходить в бар");
			IsMoored = false;
		}

		public void Drink()
		{
			if (SpendResourcesByHours(1))
			{
				if (SubstractGold(150))
				{
					AddSatiety(25);
					AddMood(40);
					CurrentTime = CurrentTime.Add(TimeSpan.FromHours(1));

					Notify?.Invoke("Вы выпили рома в баре =) \n+25 к сытости" +
					"\n+40 к настроению \n-150 золота \nПрошел 1 час");
				}
			}
		}

		public void RobShip()
		{
			if (SpendResourcesByHours(1))
			{
				int randomNumber = random.Next(0, 100);
				int loot;


				if (randomNumber == 100)
					loot = random.Next(50000, 100000);
				else if (randomNumber % 50 == 0)
					loot = random.Next(30000, 50000);
				else if (randomNumber % 35 == 0)
					loot = random.Next(10000, 30000);
				else if (randomNumber % 10 == 0)
					loot = random.Next(5000, 10000);
				else if (randomNumber % 5 == 0)
					loot = random.Next(2500, 5000);
				else if (randomNumber % 2 == 0)
					loot = random.Next(1000, 2500);
				else
					loot = random.Next(500, 1000);

				Gold += loot;

				RobbedShipsCount++;

				Notify?.Invoke($"Вы ограбили корабль \nДобыча: {loot} \nПрошел 1 час");
			}
		}

		// Косвенные траты по-прохождению hours часов
		private bool SpendResourcesByHours(double hours)
		{
			return SubstractSatiety((int)(3 * hours)) && SubstractStamina((int)(10 * hours));
		}

		// Методы добавления и вычитания некоторых характеристик игрока
		private void AddSatiety(int satiety)
		{
			if (Satiety + satiety >= 100)
				Satiety = 100;
			else
				Satiety += satiety;
		}

		private void AddMood(int mood)
		{
			if (Mood + mood >= 100)
				Mood = 100;
			else
				Mood += mood;
		}

		private void AddStamina(int stamina)
		{
			if (Stamina + stamina >= 100)
				Stamina = 100;
			else
				Stamina += stamina;
		}

		// public только из-за юнит-тестов
		public bool SubstractSatiety(int satiety)
		{
			if (satiety < 0)
				return false;

			if (Satiety - satiety < 0)
			{
				Notify?.Invoke("Вы слишком голодны");
				return false;
			}
			else
			{
				Satiety -= satiety;
				return true;
			}
		}

		public bool SubstractMood(int mood)
		{
			if (mood < 0)
				return false;

			if (Mood - mood < 0)
			{
				Notify?.Invoke("У вас слишком плохое настроение");
				return false;
			}
			else
			{
				Mood -= mood;
				return true;
			}
		}

		public bool SubstractStamina(int stamina)
		{
			if (stamina < 0)
				return false;

			if (Stamina - stamina < 0)
			{
				Notify?.Invoke("Вы слишком устали");
				return false;
			}
			else
			{
				Stamina -= stamina;
				return true;
			}
		}

		public bool SubstractGold(int gold)
		{
			if (gold < 0)
				return false;

			if (Gold - gold < 0)
			{
				Notify?.Invoke("У вас слишком мало золота");
				return false;
			}
			else
			{
				Gold -= gold;
				return true;
			}
		}
	}
}

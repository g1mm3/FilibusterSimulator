using FilibusterSimulator.GameWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilibusterSimulator.BusinessLogic
{
	public class Game
	{
		// Сколько игровых часов проходит за одну реальную минуту
		public int GameHoursPerOneRealMinute { get; set; }

		// Текст на дисплее
		public string DisplayText { get; set; }

		private bool breakCurrentTime;

		private Person person;

		private HomeWindow homeWindow;

		public void SetPersonAndHomeWindow(HomeWindow homeWindow, Person person)
		{
			this.homeWindow = homeWindow;
			this.person = person;
		}

		// Метод, который прекращает течение текущего времени
		public void BreakCurrentTime()
		{
			breakCurrentTime = true;
		}

		// Течение игрового времени
		public async void ProcessTime()
		{

			homeWindow.tblTime.Text = person.GetTimeString();
			int i = 0;

			while (true)
			{
				if (homeWindow.IsActive)
				{
					if (breakCurrentTime)
					{
						breakCurrentTime = false;
						break;
					}

					person.CurrentTime = person.CurrentTime.Add(TimeSpan.FromMinutes(this.GameHoursPerOneRealMinute));
					homeWindow.tblTime.Text = person.GetTimeString();
					i++;

					if (i == (60 / this.GameHoursPerOneRealMinute))
					{
						if (person.Satiety < 3 || person.Stamina < 10)
						{
							// Поражение
							LostWindow lostWindow = new LostWindow(homeWindow);

							WPF_Misc.FocusWindow(lostWindow);
							WPF_Misc.OpenPauseWindow(homeWindow, lostWindow, false);
						}

						person.Satiety -= 3;
						person.Stamina -= 10;
						i = 0;
					}

					RefreshCharacteristics();
					RefreshRanks();
					RefreshSomeUIElements();

					await Task.Delay(1000);
				}
				else
				{
					await Task.Delay(1000);
				}
			}
		}

		// Обновление характеристик
		public void RefreshCharacteristics()
		{
			homeWindow.tblSatiety.Text = person.Satiety.ToString();

			homeWindow.tblMood.Text = person.Mood.ToString();
			homeWindow.imgMood.Source = WPF_Misc.ImageSourceFromBitmap(WPF_Misc.GetMoodImage(person.Mood));

			homeWindow.tblStamina.Text = person.Stamina.ToString();
			homeWindow.tblGold.Text = person.Gold.ToString();
			homeWindow.tblRank.Text = person.Rank.Name;

			homeWindow.tblTime.Text = person.GetTimeString();
		}

		// Обновление рангов
		public void RefreshRanks()
		{
			if (person.CurrentTime.Days == 2 && person.Rank.Id <= 1)
				person.Rank = new Rank(Rank.GetRankNameById(2));

			if (person.CurrentTime.Days == 4 && person.Rank.Id <= 2)
				person.Rank = new Rank(Rank.GetRankNameById(3));

			if (person.RobbedShipsCount >= 10 && person.RobbedShipsCount < 25)
				person.Rank = new Rank(Rank.GetRankNameById(4));

			if (person.RobbedShipsCount >= 25 && person.RobbedShipsCount < 50)
				person.Rank = new Rank(Rank.GetRankNameById(5));

			if (person.RobbedShipsCount >= 50)
			{
				person.Rank = new Rank(Rank.GetRankNameById(6));

				// Победа
				WinWindow winWindow = new WinWindow(homeWindow);

				WPF_Misc.FocusWindow(winWindow);
				WPF_Misc.OpenPauseWindow(homeWindow, winWindow, false);
			}
		}

		// Обновление некоторых элементов интерфейса, требующих этого
		public void RefreshSomeUIElements()
		{
			homeWindow.btnRobShip.IsEnabled = person.Rank.Id >= 3 ? true : false;
		}
	}
}

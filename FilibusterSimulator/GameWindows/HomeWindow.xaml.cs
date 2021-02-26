using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FilibusterSimulator.BusinessLogic;

namespace FilibusterSimulator.GameWindows
{
	/// <summary>
	/// Логика взаимодействия для HomeWindow.xaml
	/// </summary>
	public partial class HomeWindow : Window
	{
		public Game Game;
		public Person Person;

		// Нужно для последующего закрытия при выходе в главное меню
		public MainWindow MainWindow;

		// Вывести сообщение на экран
		void DisplayMessage(string message) => tblDisplay.Text = message;

		// Стандартный конструктор
		public HomeWindow(MainWindow mainWindow)
		{
			InitializeComponent();
			MainWindow = mainWindow;

			InitGame();
			InitPerson();
			InitImages();
			InitButtons();
		}

		// Конструктор, использующийся для загрузки сохранений
		public HomeWindow(Game newGame, Person newPerson, MainWindow mainWindow)
		{
			InitializeComponent();
			MainWindow = mainWindow;

			InitGame(newGame);
			InitPerson(newPerson);
			InitImages();
			InitButtons();
		}

		// Инициализация игры
		public void InitGame(Game game = null)
		{
			if (game == null)
			{
				Game = new Game
				{
					GameHoursPerOneRealMinute = 4,
					DisplayText = "Добро пожаловать в Симулятор Флибустьера!" +
					"\n\nВ правом верхнем углу вы видите такие характеристики, как: сытость, настроение, запас жизненных сил, золото и ранг." +
					"\n\nСлева от дисплея находятся действия, которые влияют на персонажа." +
					"\n\nСмысл игры - дойти до ранга `Капитан`." +
					"\nВсего рангов 6: Юнга, Пират, Боцман, Квартмейстер, Старпом, Капитан." +
					"\n\nДо Боцмана ранг поднимается каждые два дня. Потом у вас откроется возможность грабить корабли." +
					"\n\nКвартмейстер - 10 кораблей, Старпом - 25 кораблей, Капитан - 50 кораблей."
				};
			}
			else
			{
				Game = game;
			}

			DisplayMessage(Game.DisplayText);
		}

		// Инициализация игрока
		public void InitPerson(Person person = null)
		{
			if (person == null)
			{
				Person = new Person
				{
					Satiety = 100,
					Mood = 100,
					Stamina = 100,
					Gold = 1000,
					Rank = new Rank("Юнга"),

					IsMoored = false,
					RobbedShipsCount = 0,
					CurrentTime = new TimeSpan(0, 0, 0, 0)
				};
			}
			else
			{
				Person = person;
			}

			if (Person.RobbedShipsCount > 0)
				tblRobbedShipsCount.Text = "Ограблено кораблей: " + Person.RobbedShipsCount.ToString();
			else
				tblRobbedShipsCount.Visibility = Visibility.Hidden;

			btnMoorUnmoor.Content = Person.IsMoored ? "Отчалить от порта" : "Пришвартоваться к ближайшему порту";
			btnDrink.IsEnabled = Person.IsMoored ? true : false;

			// Person.Notify -> DisplayMessage
			Person.Notify += DisplayMessage;

			// Методы класса Game, вызов которых возможен только после инициализация игрока
			Game.SetPersonAndHomeWindow(this, Person);
			Game.RefreshCharacteristics();
			Game.RefreshSomeUIElements();
			Game.ProcessTime();
		}

		// Кнопка "Пауза"
		void BtnPauseGame_Click(object sender, RoutedEventArgs e)
		{
			PauseWindow pauseWindow = new PauseWindow(this);

			WPF_Misc.FocusWindow(pauseWindow);
			WPF_Misc.OpenPauseWindow(this, pauseWindow);
		}

		// Метод, отвечающий за выполнение других методов при нажатии на кнопки
		void BtnActions_Click(object sender, RoutedEventArgs e)
		{
			switch ((sender as Button).Name)
			{
				case "btnEat": Person.Eat(); break;
				case "btnSleep": Person.Sleep(); break;
				case "btnDance": Person.Dance(); break;

				case "btnMoorUnmoor":
					if (Person.IsMoored)
					{
						btnMoorUnmoor.Content = "Пришвартоваться к ближайшему порту";
						btnDrink.IsEnabled = false;
						Person.Unmoor();
					}
					else
					{
						btnMoorUnmoor.Content = "Отчалить от порта";
						btnDrink.IsEnabled = true;
						Person.Moor();
					}
					break;

				case "btnDrink": Person.Drink(); break;

				case "btnRobShip":
					Person.RobShip();
					tblRobbedShipsCount.Text = "Ограблено кораблей: " + Person.RobbedShipsCount.ToString();
					break;
			}

			Game.RefreshCharacteristics();
		}

		// Инициализация иконок характеристик
		void InitImages()
		{
			imgSatiety.Source = WPF_Misc.ImageSourceFromBitmap(Properties.Resources.Satiety);
			imgMood.Source = WPF_Misc.ImageSourceFromBitmap(WPF_Misc.GetMoodImage(Person.Mood));
			imgStamina.Source = WPF_Misc.ImageSourceFromBitmap(Properties.Resources.Stamina);
			imgGold.Source = WPF_Misc.ImageSourceFromBitmap(Properties.Resources.Gold);
			imgRank.Source = WPF_Misc.ImageSourceFromBitmap(Properties.Resources.Rank);
		}

		// Инициализация кнопок
		void InitButtons()
		{
			btnPauseGame.Click += BtnPauseGame_Click;
			btnEat.Click += BtnActions_Click;
			btnSleep.Click += BtnActions_Click;
			btnDance.Click += BtnActions_Click;
			btnMoorUnmoor.Click += BtnActions_Click;
			btnDrink.Click += BtnActions_Click;
			btnRobShip.Click += BtnActions_Click;
		}
	}
}

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
	/// Логика взаимодействия для LoadWindow.xaml
	/// </summary>
	public partial class LoadWindow : Window
	{
		HomeWindow homeWindow;
		MainWindow mainWindow;

		public LoadWindow(HomeWindow homeWindow, MainWindow mainWindow = null)
		{
			InitializeComponent();

			this.homeWindow = homeWindow;

			if (mainWindow != null)
				this.mainWindow = mainWindow;

			InitLoads();
		}

		void BtnLoad_Click(object sender, RoutedEventArgs e)
		{
			Button thisButton = sender as Button;

			if (thisButton.Content != null)
			{
				ConfirmWindow confirmWindow = new ConfirmWindow("Вы действительно хотите загрузить это сохранение?");

				if (confirmWindow.ShowDialog() == false)
				{
					return;
				}
			}

			LoadProcess(thisButton.Content.ToString());
		}

		void BtnReturn_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void LoadProcess(string buttonContent)
		{
			List<Save> saves = Save.GetAllSaves();

			for (int i = 0; i < saves.Count; i++)
			{
				if (saves[i].Name == buttonContent)
				{
					homeWindow.Game.BreakCurrentTime();

					if (mainWindow != null)
						WPF_Misc.OpenNewWindow(mainWindow, new HomeWindow(saves[i].Game, saves[i].Person, mainWindow));
					else
						WPF_Misc.OpenNewWindow(homeWindow, new HomeWindow(saves[i].Game, saves[i].Person, homeWindow.MainWindow));

					this.Close();
					break;
				}
			}
		}

		// Метод, отвечающий за заполнение ячеек сохранений
		private void InitLoads()
		{
			List<Save> saves = Save.GetAllSaves();
			if (saves.Count != 0)
			{
				foreach (var el in saves)
				{
					switch (el.Slot)
					{
						case 1: btnLoad1.Content = el.Name; break;
						case 2: btnLoad2.Content = el.Name; break;
						case 3: btnLoad3.Content = el.Name; break;
						case 4: btnLoad4.Content = el.Name; break;
						default: continue;
					}
				}
			}
		}
	}
}

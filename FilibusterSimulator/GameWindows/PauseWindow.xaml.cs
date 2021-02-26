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

namespace FilibusterSimulator.GameWindows
{
	/// <summary>
	/// Логика взаимодействия для PauseWindow.xaml
	/// </summary>
	public partial class PauseWindow : Window
	{
		HomeWindow homeWindow;

		public PauseWindow(HomeWindow homeWindow)
		{
			InitializeComponent();

			this.homeWindow = homeWindow;
		}

		private void BtnReturnToGame_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnSaveGame_Click(object sender, RoutedEventArgs e)
		{
			SaveWindow saveWindow = new SaveWindow(homeWindow);

			WPF_Misc.FocusWindow(saveWindow);
			WPF_Misc.OpenNewWindow(this, saveWindow, false);
		}

		private void BtnLoadGame_Click(object sender, RoutedEventArgs e)
		{
			LoadWindow loadWindow = new LoadWindow(homeWindow);

			WPF_Misc.FocusWindow(loadWindow);
			WPF_Misc.OpenNewWindow(this, loadWindow, false);
		}

		private void BtnEndGame_Click(object sender, RoutedEventArgs e)
		{
			WPF_Misc.OpenNewWindow(this, new MainWindow(), false, false);

			// Вместе с этим окном закроется и окно паузы, т.к. это окно-родитель
			homeWindow.MainWindow.Close();
			homeWindow.Close();

		}

		private void BtnRestartGame_Click(object sender, RoutedEventArgs e)
		{
			homeWindow.Game.BreakCurrentTime();
			homeWindow.InitGame();
			homeWindow.InitPerson();
			this.Close();
		}
	}
}

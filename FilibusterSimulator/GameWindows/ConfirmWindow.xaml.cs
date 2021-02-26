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
	/// Логика взаимодействия для ConfirmWindow.xaml
	/// </summary>
	public partial class ConfirmWindow : Window
	{
		public ConfirmWindow(string text)
		{
			InitializeComponent();

			// Настройки окна
			this.Top = 348.75;
			this.Left = 600;
			this.Width = 400;
			this.Height = 200;

			this.BorderThickness = new Thickness(2, 10, 2, 2);

			this.WindowStyle = WindowStyle.None;
			this.ResizeMode = ResizeMode.NoResize;

			tblText.Text = text;
		}

		void BtnYes_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
	}
}

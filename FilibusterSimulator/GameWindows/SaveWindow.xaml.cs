using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;

namespace FilibusterSimulator.GameWindows
{
	/// <summary>
	/// Логика взаимодействия для SaveWindow.xaml
	/// </summary>
	public partial class SaveWindow : Window
	{
		HomeWindow homeWindow;
		Button thisButton;
		TextBox textBox;
		string buttonName;

		public SaveWindow(HomeWindow homeWindow)
		{
			InitializeComponent();

			this.homeWindow = homeWindow;

			InitSaves();
		}

		void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			thisButton = sender as Button;

			if (textBox != null)
				return;

			if (thisButton.Content != null)
			{
				ConfirmWindow confirmWindow = new ConfirmWindow("Вы действительно хотите перезаписать это сохранение?");

				if (confirmWindow.ShowDialog() == false)
					return;
			}

			buttonName = thisButton.Name;
			SaveProcess();
		}

		void BtnReturn_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		void SaveProcess()
		{
			textBox = new TextBox();
			textBox.Text = "";
			textBox.TextAlignment = TextAlignment.Center;
			textBox.FontSize = 15;
			textBox.Width = thisButton.ActualWidth;
			textBox.Height = thisButton.ActualHeight / 2;
			textBox.Margin = thisButton.Margin;

			Button saveButton = new Button();
			saveButton.Content = "Сохранить игру";
			saveButton.FontSize = 15;
			saveButton.Width = textBox.Width;
			saveButton.Height = textBox.Height;
			saveButton.IsDefault = true;
			saveButton.Click += BtnTemp_Click;

			thisButton.Visibility = Visibility.Collapsed;

			StackPanel stackPanel;

			switch (buttonName)
			{
				case "btnSave1": stackPanel = save1StackPanel; break;
				case "btnSave2": stackPanel = save2StackPanel; break;
				case "btnSave3": stackPanel = save3StackPanel; break;
				case "btnSave4": stackPanel = save4StackPanel; break;
				default: return;
			}

			stackPanel.Children.Add(textBox);
			stackPanel.Children.Add(saveButton);
		}

		void BtnTemp_Click(object sender, RoutedEventArgs e)
		{
			if (textBox.Text == "")
				return;

			Save newSave = new Save
			{
				Name = textBox.Text,
				Slot = Convert.ToInt32(buttonName.Remove(0, 7)), // btnSave удаляем из названия кнопки
				Person = homeWindow.Person,
				Game = homeWindow.Game
			};

			List<Save> saves = Save.GetAllSaves();

			for (int i = 0; i < saves.Count; i++)
			{
				if (saves[i].Slot == newSave.Slot)
				{
					saves[i] = newSave;
				}
			}

			if (!saves.Contains(newSave))
			{
				saves.Add(newSave);
			}

			JsonSerializer serializer = new JsonSerializer();
			serializer.NullValueHandling = NullValueHandling.Ignore;

			using (StreamWriter sw = new StreamWriter("saves.json"))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				foreach (var el in saves)
				{
					serializer.Serialize(writer, el);
				}
			}

			StackPanel stackPanel;

			switch (buttonName)
			{
				case "btnSave1": stackPanel = save1StackPanel; break;
				case "btnSave2": stackPanel = save2StackPanel; break;
				case "btnSave3": stackPanel = save3StackPanel; break;
				case "btnSave4": stackPanel = save4StackPanel; break;
				default: return;
			}

			stackPanel.Children.Remove(textBox);
			stackPanel.Children.Remove(sender as Button);

			thisButton.Content = newSave.Name;
			thisButton.Visibility = Visibility.Visible;

			thisButton = null;
			textBox = null;
			buttonName = null;
		}

		// Метод, отвечающий за заполнение ячеек сохранений
		void InitSaves()
		{
			List<Save> saves = Save.GetAllSaves();
			if (saves.Count != 0)
			{
				foreach (var el in saves)
				{
					switch (el.Slot)
					{
						case 1: btnSave1.Content = el.Name; break;
						case 2: btnSave2.Content = el.Name; break;
						case 3: btnSave3.Content = el.Name; break;
						case 4: btnSave4.Content = el.Name; break;
						default: continue;
					}
				}
			}
		}
	}
}

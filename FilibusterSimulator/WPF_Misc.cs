using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FilibusterSimulator
{
	public class WPF_Misc
	{
		// Получение нужной иконки настроения в зависимости от его значения
		public static Bitmap GetMoodImage(int mood)
		{
			if (mood >= 75)
				return Properties.Resources.SmileMood;
			else if (mood > 25 && mood < 75)
				return Properties.Resources.NeutralMood;
			else
				return Properties.Resources.SadMood;
		}

		// Метод, отвечающий за открытие нового окна
		public static void OpenNewWindow(Window currentWindow, Window windowToOpen, bool isCloseCurrentWindow = true, bool isCopyPreviousParameters = true)
		{
			if (isCopyPreviousParameters)
			{
				windowToOpen.Top = currentWindow.Top;
				windowToOpen.Left = currentWindow.Left;
				windowToOpen.Height = currentWindow.ActualHeight;
				windowToOpen.Width = currentWindow.ActualWidth;

				windowToOpen.Title = currentWindow.Title;
				windowToOpen.BorderThickness = currentWindow.BorderThickness;

				windowToOpen.WindowStyle = currentWindow.WindowStyle;
				windowToOpen.ResizeMode = currentWindow.ResizeMode;
			}

			windowToOpen.WindowStartupLocation = WindowStartupLocation.CenterScreen;

			if (isCloseCurrentWindow)
				currentWindow.Close();

			windowToOpen.Show();
		}

		// Метод, отвечающий за открытие нового маленького окна ("пауза").
		public static void OpenPauseWindow(Window currentWindow, Window windowToOpen, bool isCloseCurrentWindow = false)
		{
			windowToOpen.Top = currentWindow.Top * 1.5;
			windowToOpen.Left = currentWindow.Left * 1.6;
			windowToOpen.Height = 400;
			windowToOpen.Width = 800;

			windowToOpen.Title = currentWindow.Title;

			windowToOpen.BorderThickness = new Thickness(2, 10, 2, 2);

			windowToOpen.WindowStyle = currentWindow.WindowStyle;
			windowToOpen.ResizeMode = currentWindow.ResizeMode;

			if (isCloseCurrentWindow)
				currentWindow.Close();

			windowToOpen.ShowDialog();
		}

		// Метод, отвечающий за перенос фокуса на указанное окно
		public static void FocusWindow(Window windowToFocus)
		{
			windowToFocus.Owner = Application.Current.MainWindow;
			windowToFocus.ShowInTaskbar = false;
		}

		// Нужные вещи для работы следующего метода
		[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject([In] IntPtr hObject);

		// Метод, отвечающий за получение ImageSource от Bitmap
		public static ImageSource ImageSourceFromBitmap(Bitmap bmp)
		{
			var handle = bmp.GetHbitmap();
			try
			{
				return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}
			finally { DeleteObject(handle); }
		}
	}
}

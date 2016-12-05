using Gtk;
namespace Osobni_Troškovnik
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Init();
			//Gtk.Settings.Default.SetLongProperty("gtk-button-images", 1, "");
			var win = new MainWindow();
			win.Show();



			Application.Run();
		}
	}
}

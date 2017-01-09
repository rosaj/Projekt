using Gtk;
namespace Osobni_Troškovnik
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Init();
			Gtk.Settings.Default.SetLongProperty("gtk-button-images", 1, "");
			/*	var splash = new Window("Učitavanje");
					splash.TypeHint = Gdk.WindowTypeHint.Splashscreen;

					splash.SetSizeRequest(300, 200);
					var progresBar = new ProgressBar();
					progresBar.Text = "Otvaranje osobnog troškovnika";
					progresBar.Fraction = 0;
					var vbox = new VBox();
					vbox.PackStart(new Label("Otvaranje aplikacije"), true, true, 0);
					vbox.PackStart(progresBar, false, true, 0);

					splash.Add(vbox);

					splash.ShowAll();

					GLib.Timeout.Add(50,delegate {
						if (progresBar.Fraction > 0.97) return false;
						progresBar.Fraction += 0.02;
						return true;
					});
					var win = new MainWindow();
					win.Hide();

					GLib.Timeout.Add(3000,delegate {
						

						win.Show();

						splash.Destroy();
						return false;
					});
*/


			var win = new MainWindow();

			win.Show();

			Application.Run();
		}
	}
}

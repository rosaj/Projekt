using System;
using Gtk;
namespace Osobni_Troškovnik
{


	public partial class NovaKategorijaWidow : Gtk.Window
	{
		public delegate void eventHandler(string ime);
		public event eventHandler resurs;
		public NovaKategorijaWidow(Window parent) : base(Gtk.WindowType.Toplevel)
		{
			this.TransientFor = parent;

			this.ParentWindow = parent.GdkWindow;
			this.Build();
			this.Icon = parent.Icon;
			this.Title = "Dodaj novu kategoriju";

		}

		public void spremiKategorijuClicked(object sender, EventArgs e)
		{
			if (resurs != null)	resurs(novaKategorija.Text);
			this.Destroy();

		}
		protected void KeyPress(object o, KeyReleaseEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;
			if (keyCode == 65307) this.Destroy();
			else if (keyCode == 65293) spremiKategorijuClicked(o, null);
		}

	}
}

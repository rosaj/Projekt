using System;
using Gtk;
namespace Osobni_Troškovnik
{


	public partial class NovaKategorijaWidow : Gtk.Window
	{
		public delegate void eventHandler(string ime);
		public event eventHandler resurs;
		public NovaKategorijaWidow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Dodaj novu kategoriju";
		}

		public void spremiKategorijuClicked(object sender, EventArgs e)
		{
			if (resurs != null)	resurs(novaKategorija.Text);
			this.Destroy();

		}


	}
}

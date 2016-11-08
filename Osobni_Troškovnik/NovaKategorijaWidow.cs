using System;
namespace Osobni_Troškovnik
{


	public partial class NovaKategorijaWidow : Gtk.Window
	{
		public delegate void eventHandler(string ime);
		public event eventHandler resurs;
		public NovaKategorijaWidow() :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
		}

		public void spremiKategorijuClicked(object sender, EventArgs e)
		{
			resurs(novaKategorija.Text);
			this.Destroy();

		}

	}
}

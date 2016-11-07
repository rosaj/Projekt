using System;
namespace Osobni_Troškovnik
{
	public partial class UnesiTrosakWindow : Gtk.Window
	{
		public UnesiTrosakWindow() :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Resizable = false;
		}

		protected void odustaniClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		protected void spremiAndNoviClicked(object sender, EventArgs e)
		{
		}

		protected void spremiClicked(object sender, EventArgs e)
		{
		}

		protected void novaKategorijaClicke(object sender, EventArgs e)
		{
		}
	}
}

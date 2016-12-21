using System;
using Gtk;
namespace Osobni_Troškovnik
{


	public partial class NovaKategorijaWidow : Gtk.Window
	{
		
		public KategorijaPresenter kategorijaPresenter;

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

				try
				{
				kategorijaPresenter.insertKategorija(novaKategorija.Text);
					MessageBox.Popout("Kategorija dodana", 1, TransientFor);
					this.Destroy();


				}
				catch (Exception ex)
				{
					MessageBox.Popout(ex.Message, 2, this);
				}



		}
		protected void KeyPress(object o, KeyReleaseEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;
			if (keyCode == 65307) this.Destroy();
			else if (keyCode == 65293) spremiKategorijuClicked(o, null);
		}

	}
}

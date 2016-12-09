using System;
using Gtk;
namespace Osobni_Troškovnik
{



	public partial class UnesiTrosakWindow : Gtk.Window
	{
		public delegate void eventHandler();
		public event eventHandler signaliziraj;
		private Gdk.Color bgColor = Props.bgColor;

		KategorijaPresenter kategorijaPresenter;
		public UnesiTrosakWindow(Window parent) :base(Gtk.WindowType.Toplevel)
		{
			this.TransientFor = parent;

			this.ParentWindow = parent.GdkWindow;
			this.Build();
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Novi trošak";
			eventboxHome.ModifyBg(StateType.Normal,bgColor);
			this.Resizable = false;
			kategorijaPresenter = new KategorijaPresenter(listaKategorija);
			cijena.Text = "";
		}


		protected void odustaniClicked(object sender, EventArgs e)
		{
			
			OnDeleteEvent(sender, null);

		}

		protected void spremiAndNoviClicked(object sender, EventArgs e)
		{
			if (spremi())
			{
				cijena.Text = "";
				opis.Buffer.Text = "";
				MessageBox.Popout("Trošak dodan", 1, this);
			}
		}

		protected void spremiClicked(object sender, EventArgs e)
		{
			if (spremi())
			{
				MessageBox.Popout("Trošak dodan", 1, TransientFor);
				OnDeleteEvent(sender, null);
			}
		}

		protected void novaKategorijaClicked(object sender, EventArgs e)
		{
			var nova = new NovaKategorijaWidow(this);
			nova.resurs += dodajKategoriju;
		}
		public void dodajKategoriju(string e)
		{
			e = e.Trim();

			if (e.Length > 0)
			{
				if (kategorijaPresenter.insertKategorija(e))
				{
					MessageBox.Popout("Kategorija dodana", 1, this);
				}
				else 
				{
					MessageBox.Popout("Kategorija već postoji", 2, this);
				}
			}
		}
		private bool spremi()
		{
			double broj;
			if (!double.TryParse(cijena.Text, out broj))
			{
				MessageBox.Show(this, Gtk.DialogFlags.Modal, Gtk.MessageType.Warning, Gtk.ButtonsType.Ok, "Cijena nesmije biti prazna");
				return false;
			}
			else if (broj.CompareTo(0) == 0)
			{
				MessageBox.Show(this, Gtk.DialogFlags.Modal, Gtk.MessageType.Warning, Gtk.ButtonsType.Ok, "Cijena nesmije biti nula");
				return false;
			}
		
			else if (opis.Buffer.Text.Trim() == "")
			{
				MessageBox.Show(this, Gtk.DialogFlags.Modal, Gtk.MessageType.Warning, Gtk.ButtonsType.Ok, "Opis nesmije biti prazan");
				return false;
			}
			else {
				
				Baza.getInstance.insertTrosak(listaKategorija.ActiveText, broj, kalendar.GetDate(),opis.Buffer.Text);
				return true;
			}
		}


		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			if (signaliziraj != null) signaliziraj();
			Destroy();

		}
		protected void KeyPress(object o, KeyReleaseEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;
			if (keyCode == 65307) OnDeleteEvent(o, null);
			
		}
	}
}

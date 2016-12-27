using System;
using Gtk;
namespace Osobni_Troškovnik
{



	public partial class UnesiTrosakWindow : Gtk.Window
	{
		
		TrosakNodeStore trosakPresenter;
		public UnesiTrosakWindow(Window parent,TrosakNodeStore trosakPesenter) :base(Gtk.WindowType.Toplevel)
		{
			this.TransientFor = parent;

			this.ParentWindow = parent.GdkWindow;
			this.Build();
			this.Icon = parent.Icon;
			this.Title = "Novi trošak";
			eventboxHome.ModifyBg(StateType.Normal,MainWindow.bgColor);

			this.trosakPresenter = trosakPesenter;
			KategorijaPresenter.generirajKategorije(listaKategorija);
			cijena.Text = "";

			foreach (Widget w in spremiButton)
			{
				if (w is Label)
				{
					w.ModifyFont(Pango.FontDescription.FromString("Bold"));
				}
			}
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

		}

		private bool spremi()
		{
			double broj;
			double.TryParse(cijena.Text, out broj);
				try
				{
					trosakPresenter.dodajNoviTrosak(listaKategorija.ActiveText, broj, kalendar.GetDate(), opis.Buffer.Text);
				return true;
				}
				catch (Exception e)
				{
					MessageBox.Popout(e.Message, 2, this);
				return false;
				}

		}


		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			KategorijaPresenter.otpustiComboBox(listaKategorija);
			Destroy();


		}
		protected void KeyPress(object o, KeyReleaseEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;
			if (keyCode == 65307) OnDeleteEvent(o, null);
			
		}
	}
}

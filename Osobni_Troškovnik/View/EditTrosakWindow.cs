using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public partial class EditTrosakWindow : Gtk.Window
	{
		private Trosak trosak;
		public EventHandler signal;
		public EventHandler brisiTrosak;
		public EditTrosakWindow(Trosak t) : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			trosak = t;
			cijena.Text = t.Cijena.ToString();
			opis.Buffer.Text = t.Opis;
			kategorijaLabel.LabelProp = t.Kategorija;
			var datum = DateTime.ParseExact(t.Datum, "dd.MM.yyyy", null);
			kalendar.SelectMonth((uint)datum.Month - 1, (uint)datum.Year);
			kalendar.SelectDay((uint)datum.Day);
		}

		protected void spremiClicked(object sender, EventArgs e)
		{
			trosak.Cijena = double.Parse(cijena.Text);
			trosak.Datum = kalendar.GetDate().ToString("dd.MM.yyyy");
			trosak.Opis = opis.Buffer.Text;
			Baza.getInstance.updateTrosak(trosak);
			signal(trosak, e);
			OnDeleteEvent(sender, new Gtk.DeleteEventArgs());
		}

		protected void odustaniClicked(object sender, EventArgs e)
		{
			OnDeleteEvent(sender, new Gtk.DeleteEventArgs());
		}

		protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs a)
		{
			Destroy();

		}

		protected void brisiClicked(object sender, EventArgs e)
		{
			Dialog d = new Gtk.MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.None, "Jeste li sigurni da želite izbrisati trošak?");

			d.AddButton("Da", Gtk.ResponseType.Yes);
			d.AddButton("Ne", Gtk.ResponseType.No);

			var odgovor = (Gtk.ResponseType)d.Run();


			if (odgovor == Gtk.ResponseType.Yes)
			{
				Baza.getInstance.brisiTrosak(trosak);
				brisiTrosak(trosak, e);
				d.Destroy();
				Destroy();
			}
			else d.Destroy();

		}
	}
}

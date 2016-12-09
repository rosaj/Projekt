using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public partial class EditTrosakWindow : Gtk.Window
	{
		private Trosak trosak;
		public EventHandler signal;
		public EventHandler brisiTrosak;
		private Gdk.Color bgColor = Props.bgColor;
		public EditTrosakWindow(Trosak t,Window parent) : base(Gtk.WindowType.Toplevel)
		{
	
			this.TransientFor = parent;

			this.ParentWindow = parent.GdkWindow;

			this.Build();
			eventBox.ModifyBg(StateType.Normal, bgColor);
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
			signal(trosak, e);
			MessageBox.Popout("Trošak spremljen", 1, TransientFor);
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
				brisiTrosak(trosak, e);
				d.Destroy();
				MessageBox.Popout("Trošak izbrisan", 1,TransientFor);
				Destroy();
			}
			else 
			{
				d.Destroy();
			}

		}

		protected void OnKeyPress(object o, KeyPressEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;
			if (keyCode == 65307) OnDeleteEvent(o, null);
		}
	}
}

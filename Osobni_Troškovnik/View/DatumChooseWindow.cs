using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public partial class DatumChooseWindow : Gtk.Window
	{

		public delegate void eventHandler(DateTime odDatum, DateTime doDatum);
		public event eventHandler signaliziraj;

		public DatumChooseWindow(DateTime p, DateTime k) : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Odaberi raspon";
			p = p.AddMonths(-1);
			k = k.AddMonths(-1);
			kalendarOd.SelectDay((uint)p.Day);
			kalendarOd.SelectMonth((uint)p.Month, (uint) p.Year);

			kalendarDo.SelectDay((uint)k.Day);
			kalendarDo.SelectMonth((uint)k.Month, (uint)k.Year);

			mjeseciCombo.Sensitive = false;
		}

		protected void filtrirajClicked(object sender, EventArgs e)
		{
			if (rangeRadio.Active)
			{
				if (kalendarOd.GetDate() > kalendarDo.GetDate())
					MessageBox.Show(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,
									"Datum Početak nemože biti veći od datuma Kraj");
				else {

					if (signaliziraj != null) signaliziraj(kalendarOd.GetDate(), kalendarDo.GetDate());
					Destroy();

				}
			}
			else if (mjeseciRadio.Active)
			{
				var mjesec = mjeseciCombo.Active+1;
				var odDatum = new DateTime(DateTime.Now.Year, mjesec, 1);
				DateTime doDatum;
				if (mjesec == 12)
				{
					doDatum = new DateTime(DateTime.Now.Year + 1, 1, 1);
				}
				else
				{
					 doDatum = new DateTime(DateTime.Now.Year, mjesec + 1, 1);
				}
				doDatum = doDatum.AddDays(-1);
				if (signaliziraj != null) signaliziraj(odDatum,doDatum);

				Destroy();
			}

		}

		protected void onDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
			Destroy();
		}
		protected void KeyPress(object o, KeyReleaseEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;
			if (keyCode == 65307) onDeleteEvent(o,null);
		}

		protected void radioMjeseciToggled(object sender, EventArgs e)
		{
			mjeseciCombo.Sensitive = true;
			kalendarDo.Sensitive = false;
			kalendarOd.Sensitive = false;
		}

		protected void radioRasponToggled(object sender, EventArgs e)
		{
			mjeseciCombo.Sensitive = false;
			kalendarDo.Sensitive = true;
			kalendarOd.Sensitive = true;
		}
	}
}

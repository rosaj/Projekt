using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public partial class DatumChooseWindow : Gtk.Window
	{

		public delegate void eventHandler(DateTime odDatum, DateTime doDatum);
		public event eventHandler signaliziraj;

		public DatumChooseWindow(DateTime p, DateTime k, Window parent,bool rangeEnabled, bool mjesecEnabled) : base(Gtk.WindowType.Toplevel)
		{
			this.TransientFor = parent;

			this.ParentWindow = parent.GdkWindow;
			this.Build();
			this.Icon = parent.Icon;
			this.Title = "Odaberi raspon";

			eventboxHome.ModifyBg(StateType.Normal, MainWindow.bgColor);
			var odMjesec = (uint)(p.Month - 1);
			var doMjesec = (uint)(k.Month -1);

			kalendarOd.SelectDay((uint)p.Day);
			kalendarOd.SelectMonth(odMjesec, (uint) p.Year);

			kalendarDo.SelectDay((uint)k.Day);
			kalendarDo.SelectMonth(doMjesec, (uint)k.Year);
			godinaSpinButton.Value = DateTime.Now.Year;

			if (!rangeEnabled) 
			{ 
				rangeRadio.Sensitive = false;
				kalendarDo.Sensitive = false;
				kalendarOd.Sensitive = false;
				mjeseciRadio.Active = true;
			}
			if (!mjesecEnabled)
			{
				mjeseciRadio.Sensitive = false;
				mjeseciCombo.Sensitive = false;
				godinaCheckButton.Sensitive = false;

			}
			if (!mjesecEnabled && !rangeEnabled) filtrirajButton.Sensitive = false;
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

				int year;
				if (godinaCheckButton.Active) year = Int32.Parse(godinaSpinButton.Text);
				else year = DateTime.Now.Year;

				var odDatum = new DateTime(year, mjesec, 1);
				DateTime doDatum;
				if (mjesec == 12)
				{
					doDatum = new DateTime(year+ 1, 1, 1);
				}
				else
				{
					 doDatum = new DateTime(year, mjesec + 1, 1);
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
			godinaCheckButton.Sensitive = true;
		}

		protected void radioRasponToggled(object sender, EventArgs e)
		{
			mjeseciCombo.Sensitive = false;
			kalendarDo.Sensitive = true;
			kalendarOd.Sensitive = true;
			if (godinaCheckButton.Active) godinaCheckButton.Activate();
			godinaCheckButton.Sensitive = false;

		}

		protected void godinaCheckToggled(object sender, EventArgs e)
		{
			if (godinaCheckButton.Active) godinaSpinButton.Sensitive = true;
			else if (!godinaCheckButton.Active) godinaSpinButton.Sensitive = false;
		}
	}
}

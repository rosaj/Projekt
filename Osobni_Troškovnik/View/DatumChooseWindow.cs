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


		}

		protected void filtrirajClicked(object sender, EventArgs e)
		{
			if (kalendarOd.GetDate() > kalendarDo.GetDate())
				MessageBox.Show(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,
								"Datum Početak nemože biti veći od datuma Kraj");
			else {

				if (signaliziraj != null) signaliziraj(kalendarOd.GetDate(),kalendarDo.GetDate());
			
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
	}
}

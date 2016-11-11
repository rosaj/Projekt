using System;
namespace Osobni_Troškovnik
{
	public partial class DatumChooseWindow : Gtk.Window
	{
		public delegate void eventHandler(DateTime odDatum, DateTime doDatum);
		public event eventHandler signaliziraj;
		public DatumChooseWindow() :base(Gtk.WindowType.Toplevel)
		{
			this.Build();
		}

		protected void filtrirajClicked(object sender, EventArgs e)
		{
			signaliziraj(kalendarOd.GetDate(), kalendarDo.GetDate());
			Destroy();
		}

		protected void onDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
			Destroy();
		}
	}
}

using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public partial class DatumChooseWindow : Gtk.Window
	{

		public delegate void eventHandler(DateTime odDatum, DateTime doDatum);
		public event eventHandler signaliziraj;
		public delegate void cancelHandler();
		public event cancelHandler cancelOdabiranje;

		public DatumChooseWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Odaberi raspon";
		}

		protected void filtrirajClicked(object sender, EventArgs e)
		{
			if(signaliziraj!=null)signaliziraj(kalendarOd.GetDate(), kalendarDo.GetDate());
			Destroy();
		}

		protected void onDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
			if(cancelOdabiranje!=null)	cancelOdabiranje();
			Destroy();
		}
	}
}

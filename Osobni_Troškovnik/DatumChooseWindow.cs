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
			var datum = DateTime.Now.AddMonths(-2);
			kalendarOd.SelectMonth((uint)datum.Month, (uint)datum.Year);


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
		protected void KeyPress(object o, KeyReleaseEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;
			if (keyCode == 65307) onDeleteEvent(o,null);
		}
	}
}

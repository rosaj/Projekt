using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public partial class BudgetWindow : Gtk.Window
	{
		public delegate void eventHandler(double budget);
		public event eventHandler resurs;
		public BudgetWindow(Window parent) :base(Gtk.WindowType.Toplevel)
		{
			this.TransientFor = parent;

			this.ParentWindow = parent.GdkWindow;

			this.Build();
			this.Icon = parent.Icon;

		}

		public void spremiBudgetClicked(object sender, EventArgs e)
		{
			if (resurs != null) resurs(double.Parse(spinbutton1.Text));
			this.Destroy();

		}

		protected void KeyPress(object o, KeyPressEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;
			if (keyCode == 65307) this.Destroy();
			else if (keyCode == 65293) spremiBudgetClicked(o, null);
		}
	}
}

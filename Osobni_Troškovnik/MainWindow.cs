using System;
using Gtk;

namespace Osobni_Troškovnik
{
	public partial class MainWindow : Gtk.Window
	{
		public	UnesiTrosakWindow uT;
		public MainWindow() :base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			notebook.CurrentPage = 0;
		}
		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}

		protected void noviTrosakClicked(object sender, EventArgs e)
		{
			if (uT == null)
			{
				uT = new UnesiTrosakWindow();
				uT.signaliziraj+= () => uT=null;
			}
		}
		protected void statistikaClicked(object sender, EventArgs e)
		{
			notebook.CurrentPage = 1;
			generirajKategorije();
		}

		protected void totalCostClicked(object sender, EventArgs e)
		{
		}

		protected void izvjesceClicked(object sender, EventArgs e)
		{
		}

		protected void izlazClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}
		private void generirajKategorije()
		{
			var lista = Baza.getInstance.getKategorije();

			var t = new VBox();


			notebook.Add(t);
			foreach (string s in lista)
			{
				var b = new Button();
				b.Label = s;
				t.Add(b);
	
			
			}
			t.Spacing = 10;

			t.Forall((widget) => widget.HeightRequest=40);


			notebook.ShowAll();
		}
	}
}

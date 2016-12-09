using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public class MessageBox
	{


		private MessageBox() { }
		public static void Show(Gtk.Window parent_window, DialogFlags flags, MessageType msgtype, ButtonsType btntype, string msg)
		{
			MessageDialog md = new MessageDialog(parent_window, flags, msgtype, btntype, msg);
			md.Run();
			md.Destroy();
		}

		public static void Show(string msg)
		{
			MessageDialog md = new MessageDialog(null, DialogFlags.Modal, MessageType.Other, ButtonsType.Ok, msg);
			md.Run();
			md.Destroy();
		}
		public static void Popout(string text, int sekundePrikaza,Window parent)
		{
			var popout = new Window(WindowType.Popup);

			popout.Add(new Label(text));
			popout.SetSizeRequest(200, 100);
			popout.TransientFor = parent;

			popout.ParentWindow = parent.GdkWindow;

			popout.SetPosition(WindowPosition.CenterOnParent);

			popout.ShowAll();

			var timer = new System.Timers.Timer(sekundePrikaza*1000);
			timer.Elapsed += (sender1, e1) =>
			{
				popout.Destroy();
				timer.Stop();
				timer.Dispose();
			};
			timer.Start();
		}

	}

}

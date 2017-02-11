using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public class ImageButton
	{
		private ImageButton()
		{
		}
		public static Button imageButton(string image, string label)
		{
			var box = new HBox(false, 0);
			box.BorderWidth = 2;
			var i = new Gtk.Image(image, IconSize.Button);

			var lookup = Stetic.Gui.w1.Lookup(image);
			if (lookup == null)
			{
				i = new Gtk.Image("r", IconSize.Button);
			}
			var labela = new Label(label);
			box.PackStart(i, false, false, 10);
			box.PackStart(labela, false, false, 5);
			var b = new Button(box);
			b.Name = label;
			return b;
		}
		public static Widget imageButton(string image)
		{
			return new Gtk.Image(image, IconSize.Button);

		}
		public static Button imageButton(string image, string label, uint imagePadding, uint labelPadding)
		{
			var box = new HBox(false, 0);
			box.BorderWidth = 2;
			var i = new Gtk.Image(image, IconSize.Button);

			var labela = new Label(label);
			box.PackStart(i, false, false, imagePadding);
			box.PackStart(labela, false, false, labelPadding);
			var b = new Button(box);
			b.Name = label;
			return b;
		}

	}
}

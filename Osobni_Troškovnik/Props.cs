using System;
using System.Collections.Generic;
using Gtk;
namespace Osobni_Troškovnik
{
	public abstract class Props
	{

		public static readonly List<string> defultLista = new List<string>(){ "Hrana", "Školovanje", "Gorivo","Automobil", "Namještaj", "Stanarina",
			"Računalna oprema", "Struja", "Voda", "Telefon", "Internet", "TV", "Odjeća", "Nakit", "Shopping", "Zdravlje"};



		public static Gdk.Color getColor(string color)
		{
			var c = new Gdk.Color();
			Gdk.Color.Parse(color, ref c);
			return c;
		}
		public static EventBox add2EventBox(Widget w, Gdk.Color boxColor)
		{
			var e = new EventBox();
			e.Add(w);
			e.ModifyBg(StateType.Normal, boxColor);
			return e;
		}

		public static EventBox add2EventBox(Widget w, Gdk.Color boxColor, Gdk.Color textColor)
		{

			var e = new EventBox();
			e.Add(w);
			e.ModifyBg(StateType.Normal, boxColor);
			w.ModifyFg(StateType.Normal, textColor);
			return e;
		}



		public static EventBox add2EventBox(Widget w, Gdk.Color boxColor, Gdk.Color textColor, string font)
		{

			var e = new EventBox();
			e.Add(w);
			e.ModifyBg(StateType.Normal, boxColor);
			w.ModifyFg(StateType.Normal, textColor);
			w.ModifyFont(Pango.FontDescription.FromString(font));
			return e;
		}
	}
}

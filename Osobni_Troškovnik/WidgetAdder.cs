using System;
using System.Collections.Generic;
using Gtk;
namespace Osobni_Troškovnik
{
	public class WidgetAdder

	{
		private WidgetAdder()
		{
		}

		public static HBox combineWidgetsInline<T>(List<T> lista,bool homogeno, int spacing) where T:Gtk.Widget
		{
			var h = new HBox(homogeno, spacing);
			lista.ForEach((T t) =>
			{
				h.PackStart(t, true, true, 0);
			});

			return h;
		}
	}
}

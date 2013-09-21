//
// Clip.cs: Ejemplo de rotacion de un cuadrado respecto a su origen.
//
// Autor:
//   Matias Muñoz Espinoza [Writkas]
//

using System;
using Cairo;
using Gtk;
using System.Timers;

public class GtkCairo
{
    static void Main ()
    {
        Application.Init ();
        Gtk.Window w = new Gtk.Window ("RotarCuadrado");

        DrawingArea da = new CairoGraphic ();
        Box box = new VBox (true, 0);
        box.Add (da);	

        w.Add (box);
        w.Resize (256, 256);
		
        w.DeleteEvent += closeWindow;
        w.ShowAll ();
 
        Application.Run ();
    }

    static void closeWindow (object obj, DeleteEventArgs args)
    {
        Application.Quit ();
    }
}
 
public class CairoGraphic : DrawingArea
{
	private double y = 0.0;
	private double r = 0.0;
	private int width = 50;
	private int height = 50;

	public CairoGraphic ()
	{
		GLib.Timeout.Add (10, new GLib.TimeoutHandler (siempre));
	}	

	private bool siempre ()
	{
		QueueDraw ();
		return true;
	}
	
    protected override bool OnExposeEvent (Gdk.EventExpose args)
    {
		using (Context g = Gdk.CairoHelper.Create (args.Window)) {
			r++;

			g.Translate (100.0, y + 100.0);
			g.Arc (50.0, 50.0, 100.0, 0.0, 0.0);

			// Rotar desde el centro
			g.Rotate (r * (System.Math.PI / 180));			
			g.Translate (-0.5 * width, -0.5 * height);

			g.Rectangle (0.0, 0.0, 50.0, 50.0);

			g.LineWidth = 10.0;
			g.Stroke ();
		}

		return true;
	}
}

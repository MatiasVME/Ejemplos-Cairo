//
// Clip.cs: Ejemplo del uso de Clip y Animacion
//
// Autor:
//   Matias Muñoz Espinoza [Writkas]

using System;
using Cairo;
using Gtk;
using System.Timers;
 
public class GtkCairo
{	
    static void Main ()
    {
        Application.Init ();
        Gtk.Window w = new Gtk.Window ("ClipAnimated");

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
	private static double grosorLinea;
	private static double transparencia;
	private static bool contraerLinea;
	private static bool expanderLinea;
	private static bool transparenciaPos;
	private static bool transparenciaNeg;

	public CairoGraphic ()
	{
		transparenciaPos = expanderLinea = true;
		transparenciaNeg = contraerLinea = false;
		GLib.Timeout.Add (100, new GLib.TimeoutHandler (siempre));
		grosorLinea = 20.0;
		transparencia = 0.20;
	}	

	private bool siempre ()
	{
		QueueDraw ();

		return true;
	}
	
    protected override bool OnExposeEvent (Gdk.EventExpose args)
    {
		// Expander y contraer lineas
		//

		if (expanderLinea == true) {
			grosorLinea += 0.5;

			if (grosorLinea > 24.0) {
				expanderLinea = false;
				contraerLinea = true;
			}
		}

		else if (contraerLinea == true) {
			grosorLinea -= 0.5;

			if (grosorLinea < 21.0) {
				contraerLinea = false;
				expanderLinea = true;
			}
		}

		// Aumentar y Reducir transparencia
		//

		if (transparenciaPos == true) {
			transparencia += 0.01;

			if (transparencia > 0.99) {
				transparenciaPos = false;
				transparenciaNeg = true;
			}
		}

		else if (transparenciaNeg == true) {
			transparencia -= 0.01;

			if (transparencia < 0.50) {
				transparenciaNeg = false;
				transparenciaPos = true;
			}
		}
		
		double PI = System.Math.PI;
		
		using (Context c = Gdk.CairoHelper.Create (args.Window)) {
			c.Arc (128.0, 128.0, 76.8, 0, 2 * PI);
			c.Clip ();
			c.NewPath ();
			c.Rectangle (0, 0, 256, 256);
			c.Fill (); // Transfiere el Color que pasa por la mascara
			
			// Dibuja la X			
			//

			c.SetSourceRGBA (0, 1, 0, transparencia); // Color Verde		
			c.MoveTo (0, 0);
			c.LineTo (256, 256);
			c.MoveTo (256, 0);
			c.LineTo (0, 256);

			c.LineWidth = grosorLinea;
			c.Stroke (); // Transfiere la X

		}

        return true;
    }
}

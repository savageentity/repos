#include <gst/gst.h>
#include <gst/video/video.h>
#include <cairo.h>
#include <cairo-gobject.h>
#include <glib.h>

 typedef struct {
   gboolean valid;
   GstVideoInfo vinfo;
   int width;
   int height;
 } CairoOverlayState;


 static void prepare_overlay (GstElement * overlay, GstCaps * caps, gpointer user_data)
 {
   CairoOverlayState *state = (CairoOverlayState *)user_data;
   state->valid = TRUE;
   /* state->valid = gst_video_info_from_caps (&state->vinfo, caps); */
 }

 static void draw_overlay (GstElement *overlay, cairo_t *cr, guint64 timestamp, guint64 duration, gpointer user_data)
 {
   CairoOverlayState *s = (CairoOverlayState *) user_data;
   double scale;
  // int width, height;

   if (!s->valid)
     return;

   //width = GST_VIDEO_INFO_WIDTH (&s->vinfo);
   //height = GST_VIDEO_INFO_HEIGHT (&s->vinfo);

   scale = 2*(((timestamp/(int)1e7) % 70)+30)/100.0;
   cairo_translate(cr, 640/2, (480/2)-30);
   cairo_scale (cr, scale, scale);

   cairo_move_to (cr, 0, 0);
   cairo_curve_to (cr, 0,-30, -50,-30, -50,0);
   cairo_curve_to (cr, -50,30, 0,35, 0,60 );
   cairo_curve_to (cr, 0,35, 50,30, 50,0 );
   cairo_curve_to (cr, 50,-30, 0,-30, 0,0 );
   cairo_set_source_rgba (cr, 0.9, 0.0, 0.1, 0.7);
   cairo_fill (cr);
 }

int main(int argc, char *argv[]) {
  GstElement *pipeline, *source, *convert, *format, *sink, *cairo_overlay, *adapter1, *adapter2;
  GstCaps *caps;
  GstBus *bus;
  GstMessage *msg;
  GstStateChangeReturn ret;
  CairoOverlayState *overlay_state;

  /* Initialize GStreamer */
  gst_init (&argc, &argv);

  /* Create the elements */
  /*  source = gst_element_factory_make ("videotestsrc", "source"); */
  source = gst_element_factory_make ("v4l2src", NULL);
  convert = gst_element_factory_make ("videoconvert", NULL);
  format = gst_element_factory_make ("capsfilter", NULL); 
  sink = gst_element_factory_make ("autovideosink", "sink");

  adapter1 = gst_element_factory_make ("videoconvert", "adapt1");
  cairo_overlay = gst_element_factory_make ("cairooverlay", "overlay");
  adapter2 = gst_element_factory_make ("videoconvert", "adapt2");
  
 g_signal_connect (cairo_overlay, "draw", G_CALLBACK (draw_overlay), overlay_state);
 g_signal_connect (cairo_overlay, "caps-changed", G_CALLBACK (prepare_overlay), overlay_state);

  caps = gst_caps_from_string ("video/x-raw, width=640, height=480");
  g_object_set (format, "caps", caps, NULL);

  /* Create the empty pipeline */
  pipeline = gst_pipeline_new ("test-pipeline");

  if (!pipeline || !source || !convert || !format || !adapter1 || !adapter2 || !cairo_overlay || !sink) {
    g_printerr ("Not all elements could be created.\n");
    return -1;
  }

  /* Build the pipeline */
  gst_bin_add_many (GST_BIN (pipeline), source, adapter1, cairo_overlay, adapter2, convert, format, sink, NULL);
  if (!gst_element_link_many (source, adapter1, cairo_overlay, adapter2, convert, format, sink, NULL)) {
    g_printerr ("Elements could not be linked.\n");
    gst_object_unref (pipeline);
    return -1;
  }

  /* Modify the source's properties */
  /* g_object_set (source, "device-name", "/dev/video0", NULL); */
  /* g_object_set (src, "num-buffers", gst_structure_n_fields (prog), "device", argv[1] ? argv[1] : "/dev/video0", NULL); */

  /* Start playing */
  ret = gst_element_set_state (pipeline, GST_STATE_PLAYING);
  if (ret == GST_STATE_CHANGE_FAILURE) {
    g_printerr ("Unable to set the pipeline to the playing state.\n");
    gst_object_unref (pipeline);
    return -1;
  }

  /* Wait until error or EOS */
  bus = gst_element_get_bus (pipeline);
  msg = gst_bus_timed_pop_filtered (bus, GST_CLOCK_TIME_NONE, GST_MESSAGE_ERROR | GST_MESSAGE_EOS);

  /* Parse message */
  if (msg != NULL) {
    GError *err;
    gchar *debug_info;

    switch (GST_MESSAGE_TYPE (msg)) {
      case GST_MESSAGE_ERROR:
        gst_message_parse_error (msg, &err, &debug_info);
        g_printerr ("Error received from element %s: %s\n", GST_OBJECT_NAME (msg->src), err->message);
        g_printerr ("Debugging information: %s\n", debug_info ? debug_info : "none");
        g_clear_error (&err);
        g_free (debug_info);
        break;
      case GST_MESSAGE_EOS:
        g_print ("End-Of-Stream reached.\n");
        break;
      default:
        /* We should not reach here because we only asked for ERRORs and EOS */
        g_printerr ("Unexpected message received.\n");
        break;
    }
    gst_message_unref (msg);
  }

  /* Free resources */
  gst_object_unref (bus);
  gst_element_set_state (pipeline, GST_STATE_NULL);
  gst_object_unref (pipeline);
  return 0;
}

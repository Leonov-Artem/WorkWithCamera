using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WorkWithCamera
{
    class SurfaceHolderCallback : Java.Lang.Object, ISurfaceHolderCallback
    {
        Camera camera;

        public SurfaceHolderCallback(Camera camera)
        {
            this.camera = camera;
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                camera.SetPreviewDisplay(holder);
                camera.StartPreview();
            }
            catch (Exception e)
            {
                _ = e.StackTrace;
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {

        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {

        }
    }       
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Media;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using Java.IO;
using Android.Icu.Text;
using Java.Util;

namespace WorkWithCamera
{
    class PictureCallback : Java.Lang.Object, Camera.IPictureCallback
    {
        public void OnPictureTaken(byte[] data, Camera camera)
        {
            File photoFile = GetOutputMediaFile();
            try
            {
                FileOutputStream fos = new FileOutputStream(photoFile);
                fos.Write(data);
                fos.Close();
            }
            catch (Exception e)
            {
                _ = e.StackTrace;
            }
        }

        private static File GetOutputMediaFile()
        {
            File pictures = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            var photoFile = new File(pictures, "myphoto.jpg");
            return photoFile;
        }
    }
}
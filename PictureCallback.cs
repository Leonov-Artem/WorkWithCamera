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
using Android.Net;
using System;
using Android.Util;

namespace WorkWithCamera
{
    class PictureCallback : Java.Lang.Object, Camera.IPictureCallback
    {
        public void OnPictureTaken(byte[] data, Camera camera)
        {
            File photoFile = GetOutputMediaFile();
            try
            { 
                var fos = new FileOutputStream(photoFile);
                fos.Write(data);
                fos.Close();
                Log.Info("MyCamera", "Фото сделано");
            }
            catch (Exception e)
            {
                _ = e.StackTrace;
            }
        }

        private File GetOutputMediaFile()
        {
            string timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date());
            string imageFileName = "JPEG_" + timeStamp + "_";
            File storageDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            File image = File.CreateTempFile(
                imageFileName,  /* prefix */
                ".jpg",         /* suffix */
                storageDir      /* directory */
            );

            return image;
        }
    }
}
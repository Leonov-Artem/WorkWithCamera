using Java.IO;
using Android.App;
using Android.Hardware;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content.PM;
using System;
using System.Collections.Generic;
using Android;
using Android.Graphics;
using static Android.Hardware.Camera;

namespace WorkWithCamera
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button button;
        Android.Hardware.Camera camera;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            SuccessfulPermissionCheck();

            button =  FindViewById<Button>(Resource.Id.btn1);
            button.Click += Button_Click;          
        }

        private void Button_Click(object sender, EventArgs e)
        {
            camera = Android.Hardware.Camera.Open();
            try
            {
                camera.SetPreviewTexture(new Android.Graphics.SurfaceTexture(10));
            }
            catch (IOException e1)
            {
                //Log.e(Version.APP_ID, e1.getMessage());
            }

            Parameters param = camera.GetParameters();
            param.SetPreviewSize(640, 480);
            param.FlashMode = (Parameters.FlashModeOff);
            param.PictureFormat = ImageFormat.Jpeg;
            camera.SetParameters(param);
            camera.StartPreview();
            camera.TakePicture(null, null, new PictureCallback());
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (camera != null)
                camera.Release();
            camera = null;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private bool SuccessfulPermissionCheck()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var permissionStillNeeded = GetNotGrantedPermissions();
                if (permissionStillNeeded.Length > 0)
                {
                    RequestPermissions(permissionStillNeeded, 5);
                    return false;
                }
            }

            return true;
        }

        private string[] GetNotGrantedPermissions()
        {
            var permissionsToCheck = new string[]
            {
                Android.Manifest.Permission.Camera,
                Android.Manifest.Permission.WriteExternalStorage
            };

            var permissionStillNeeded = new List<string>();
            for (int i = 0; i < permissionsToCheck.Length; i++)
            {
                if (Permission.Granted != CheckSelfPermission(permissionsToCheck[i]))
                    permissionStillNeeded.Add(permissionsToCheck[i]);
            }

            return permissionStillNeeded.ToArray();
        }
    }
}

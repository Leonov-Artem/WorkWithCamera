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
        Button takePhotoButton;
        Android.Hardware.Camera camera;
        string[] permissionsToCheck = new string[]
        {
            Android.Manifest.Permission.Camera,
            Android.Manifest.Permission.WriteExternalStorage
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            CallNotGrantedPermissions(permissionsToCheck);

            takePhotoButton =  FindViewById<Button>(Resource.Id.btn1);
            takePhotoButton.Click += TakePhotoButton_Click;          
        }

        private void TakePhotoButton_Click(object sender, EventArgs e)
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

            Parameters newParams = GetModifiedCameraParameters();
            camera.SetParameters(newParams);
            camera.StartPreview();
            camera.TakePicture(null, null, new PictureCallback());
        }

        private Parameters GetModifiedCameraParameters()
        {
            Parameters param = camera.GetParameters();
            param.SetPreviewSize(640, 480);

            var sizes = param.SupportedPictureSizes;
            var size = sizes[0];
            for (int i = 0; i < sizes.Count; i++)
            {
                if (sizes[i].Width > size.Width)
                    size = sizes[i];
            }

            param.SetPictureSize(size.Width, size.Height);
            param.FlashMode = (Parameters.FlashModeOff);
            param.FocusMode = Parameters.FocusModeContinuousPicture;
            param.SceneMode = Parameters.SceneModeAuto;
            param.WhiteBalance = Parameters.WhiteBalanceAuto;
            param.ExposureCompensation = 0;
            param.PictureFormat = ImageFormat.Jpeg;
            param.JpegQuality = 100;
            param.SetRotation(90);

            return param;
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

        private void CallNotGrantedPermissions(string[] permissionsToCheck)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var permissionStillNeeded = GetNotGrantedPermissions(permissionsToCheck);
                if (permissionStillNeeded.Length > 0)
                {
                    RequestPermissions(permissionStillNeeded, 5);
                }
            }
        }

        private string[] GetNotGrantedPermissions(string[] permissionsToCheck)
        {
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

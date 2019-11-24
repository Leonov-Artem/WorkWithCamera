using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Content.PM;
using Android.Hardware;
using System;
using System.Collections.Generic;
using Android.Hardware.Camera2;
using Android.Content;
using Android.Views;

namespace WorkWithCamera
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button _takePhotoButton;
        HiddenTakingPhotos _frontCamera;
        HiddenTakingPhotos _backCamera;
        string[] _permissionsToCheck = new string[]
        {
            Android.Manifest.Permission.Camera,
            Android.Manifest.Permission.WriteExternalStorage,
            Android.Manifest.Permission.ForegroundService
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            CallNotGrantedPermissions(_permissionsToCheck);

            var cameraManager = (CameraManager)GetSystemService(Context.CameraService);
            var cameraInfo = new CameraInfo(cameraManager, WindowManager);

            if (cameraInfo.NumberOfCameras() == 2)
                _frontCamera = new HiddenTakingPhotos(cameraInfo, CameraFacing.Front);
            _backCamera = new HiddenTakingPhotos(cameraInfo, CameraFacing.Back);
           
            _takePhotoButton = FindViewById<Button>(Resource.Id.btn1);
            _takePhotoButton.Click += TakePhotoButton_Click;
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (_frontCamera != null)
                _frontCamera.Stop();
            //_backCamera.Stop();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void TakePhotoButton_Click(object sender, EventArgs e)
        {
            if (_frontCamera != null)
                _frontCamera.TakePhoto();
           // _backCamera.TakePhoto();
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

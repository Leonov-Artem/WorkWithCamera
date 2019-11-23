using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Content.PM;
using Android.Hardware;
using System;
using System.Collections.Generic;

namespace WorkWithCamera
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button _takePhotoButton;
        HiddenTakingPhotos _hidden;
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

            _hidden = new HiddenTakingPhotos();
            _takePhotoButton =  FindViewById<Button>(Resource.Id.btn1);
            _takePhotoButton.Click += TakePhotoButton_Click;          
        }

        protected override void OnPause()
        {
            base.OnPause();
            _hidden.Stop();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void TakePhotoButton_Click(object sender, EventArgs e)
            => _hidden.TakePhoto();

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

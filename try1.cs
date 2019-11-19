
//using Java.IO;
//using Android.App;
//using Android.Hardware;
//using Android.Media;
//using Android.OS;
//using Android.Views;
//using Android.Support.V7.App;
//using Android.Runtime;
//using Android.Widget;
//using Android.Content.PM;
//using System;
//using System.Collections.Generic;

//namespace WorkWithCamera
//{
//    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
//    public class MainActivity : AppCompatActivity
//    {
//        SurfaceView surfaceView;
//        Button takePhoto;
//        Camera camera;
//        File photoFile;

//        protected override void OnCreate(Bundle savedInstanceState)
//        {
//            base.OnCreate(savedInstanceState);
//            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
//            SetContentView(Resource.Layout.activity_main);

//            File pictures = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
//            photoFile = new File(pictures, "myphoto.jpg");

//            surfaceView = FindViewById<SurfaceView>(Resource.Id.surfaceView);
//            var surfaceHolderCallback = new SurfaceHolderCallback(camera);
//            surfaceView.Holder.AddCallback(surfaceHolderCallback);

//            takePhoto = FindViewById<Button>(Resource.Id.button1);
//            takePhoto.Click += TakePhoto_Click;
//        }

//        protected override void OnResume()
//        {
//            base.OnResume();

//            if (SuccessfulPermissionCheck())
//            {
//                camera = Camera.Open(0);
//            }
//        }

//        protected override void OnPause()
//        {
//            base.OnPause();
//            if (camera != null)
//                camera.Release();
//            camera = null;
//        }

//        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
//        {
//            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

//            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
//        }

//        private void TakePhoto_Click(object sender, System.EventArgs e)
//        {
//            PictureCallback pictureCallback = new PictureCallback(photoFile);
//            camera.TakePicture(null, null, pictureCallback);
//        }

//        private bool SuccessfulPermissionCheck()
//        {
//            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
//            {
//                var permissionStillNeeded = GetNotGrantedPermissions();
//                if (permissionStillNeeded.Length > 0)
//                {
//                    RequestPermissions(permissionStillNeeded, 5);
//                    return false;
//                }
//            }

//            return true;
//        }

//        private string[] GetNotGrantedPermissions()
//        {
//            var permissionsToCheck = new string[]
//            {
//                   Android.Manifest.Permission.Camera,
//                   Android.Manifest.Permission.WriteExternalStorage
//            };

//            var permissionStillNeeded = new List<string>();
//            for (int i = 0; i < permissionsToCheck.Length; i++)
//            {
//                if (Permission.Granted != CheckSelfPermission(permissionsToCheck[i]))
//                    permissionStillNeeded.Add(permissionsToCheck[i]);
//            }

//            return permissionStillNeeded.ToArray();
//        }
//    }
//}
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
using System.Linq;
using System.Collections.Generic;
using Android;
using Android.Graphics;
using static Android.Hardware.Camera;
using Android.Content;

namespace WorkWithCamera
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity, ISensorEventListener
    {
        Button takePhotoButton;
        Android.Hardware.Camera camera;
        private SensorManager sensorManager;
        private Sensor mLight;
        string[] permissionsToCheck = new string[]
        {
            Android.Manifest.Permission.Camera,
            Android.Manifest.Permission.WriteExternalStorage,
            Android.Manifest.Permission.ForegroundService
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            CallNotGrantedPermissions(permissionsToCheck);

            sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            mLight = sensorManager.GetDefaultSensor(SensorType.Light);   

            takePhotoButton =  FindViewById<Button>(Resource.Id.btn1);
            takePhotoButton.Click += TakePhotoButton_Click;          
        }

        protected override void OnResume()
        {
            base.OnResume();
            sensorManager.RegisterListener(this, mLight, SensorDelay.Normal);
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (camera != null)
                camera.Release();
            camera = null;
            sensorManager.UnregisterListener(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnSensorChanged(SensorEvent e)
        {
            float lux = e.Values[0];
            
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
        }

        private void TakePhotoButton_Click(object sender, EventArgs e)
        {
            camera = Open();
            try
            {
                camera.SetPreviewTexture(new SurfaceTexture(10));
            }
            catch (IOException e1)
            {
                //Log.e(Version.APP_ID, e1.getMessage());
            }

            Parameters newParameters = GetModifiedCameraParameters();
            camera.SetParameters(newParameters);
            camera.StartPreview();
            camera.TakePicture(null, null, new PictureCallback());
        }

        private Parameters GetModifiedCameraParameters()
        {
            Parameters parameters = camera.GetParameters();
            Size size = FindMaxSize(parameters.SupportedPictureSizes);

            parameters.SetPreviewSize(640, 480);
            parameters.SetPictureSize(size.Width, size.Height);
            parameters.Set("contrast", "0");
            parameters.FlashMode = Parameters.FlashModeOff;
            parameters.FocusMode = Parameters.FocusModeAuto;
            parameters.SceneMode = Parameters.SceneModeAuto;
            parameters.AutoExposureLock = false;
            parameters.WhiteBalance = Parameters.WhiteBalanceAuto;
            parameters.ExposureCompensation = 12;
            parameters.PictureFormat = ImageFormat.Jpeg;
            parameters.JpegQuality = 100;
            parameters.SetRotation(90);    

            return parameters;
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

        private Size FindMaxSize(IList<Size> sizes)
        {
            Size[] orderByDescending = sizes
                                    .OrderByDescending(x => x.Width)
                                    .ToArray();
            return orderByDescending[0];
        }

        //private void SetMeasuringArea(ref Parameters parameters)
        //{
        //    // check that metering areas are supported
        //    if (parameters.MaxNumMeteringAreas > 0)
        //    {                
        //        var meteringAreas = new List<Area>();
        //        var areaRect1 = new Rect(-100, -100, 100, 100);     // specify an area in center of image
        //        meteringAreas.Add(new Area(areaRect1, 600));        // set weight to 60%

        //        var areaRect2 = new Rect(800, -1000, 1000, -800);   // specify an area in upper right of image
        //        meteringAreas.Add(new Area(areaRect2, 400));        // set weight to 40%

        //        parameters.MeteringAreas = meteringAreas;
        //    }
        //}
    }
}

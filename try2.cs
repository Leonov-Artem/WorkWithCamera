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
//using Android;
//using Android.Graphics;

//namespace WorkWithCamera
//{
//    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
//    public class MainActivity : Activity, TextureView.ISurfaceTextureListener
//    {
//        Camera _camera;
//        File photoFile;
//        Button button;
//        TextureView _textureView;

//        protected override void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);
           
//            _textureView = new TextureView(this);
//            _textureView.SurfaceTextureListener = this;

//            SetContentView(_textureView);
//            _textureView.Click += Click;

//            File pictures = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
//            photoFile = new File(pictures, "myphoto.jpg");
//        }

//        private void Click(object o, System.EventArgs eventArgs)
//        {
//            var pictureCallback = new WorkWithCamera.PictureCallback(photoFile);
//            new System.Threading.Thread(() => _camera.TakePicture(null, null, pictureCallback)).Start();
//            //_camera.TakePicture(null, null, pictureCallback);
//        }

//        private void TakePhoto()
//        {
//            _camera = Camera.Open();
//        }

//        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int w, int h)
//        {
//            _camera = Camera.Open();

//            _textureView.LayoutParameters = new FrameLayout.LayoutParams(w, h);

//            try
//            {
//                _camera.SetPreviewTexture(surface);
//                _camera.StartPreview();
//            }
//            catch (Java.IO.IOException ex)
//            {
//                System.Console.WriteLine(ex.Message);
//            }
//        }

//        public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
//        {
//            _camera.StopPreview();
//            _camera.Release();
            
//            return true;
//        }

//        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
//        {
//            // camera takes care of this
//        }

//        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
//        {

//        }
//    }
//}
using Android.Hardware;
using Android.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkWithCamera
{
    public class HiddenTakingPhotos
    {
        PictureCallback _pictureCallback;
        Camera.Parameters _newParameters;
        Camera _camera;

        public HiddenTakingPhotos()
        {
            _pictureCallback = new PictureCallback();
            Camera.Parameters oldParameters = GetOldParameters();
            _newParameters = GetModifiedParameters(oldParameters);
        }

        public void TakePhoto()
        {
            if (_camera == null)
                _camera =  GetCamera();

            _camera.SetPreviewTexture(new Android.Graphics.SurfaceTexture(10));
            _camera.SetParameters(_newParameters);
            _camera.StartPreview();
            _camera.TakePicture(null, null, _pictureCallback);
        }

        public void Stop()
        {
            if (_camera != null)
                _camera.Release();
            _camera = null;
        }

        private Camera GetCamera()
            => Camera.Open();

        private Camera.Parameters GetOldParameters()
        {
            var camera = GetCamera();
            Camera.Parameters oldParameters = camera.GetParameters();
            camera.Release();

            return oldParameters;
        }

        private Camera.Parameters GetModifiedParameters(Camera.Parameters oldParameters)
        {
            Camera.Parameters newParameters = oldParameters;
            Camera.Size size = FindMaxSize(newParameters.SupportedPictureSizes);

            newParameters.SetPreviewSize(640, 480);
            newParameters.SetPictureSize(size.Width, size.Height);
            newParameters.Set("contrast", "0");
            newParameters.FlashMode = Camera.Parameters.FlashModeOff;
            newParameters.FocusMode = Camera.Parameters.FocusModeAuto;
            newParameters.SceneMode = Camera.Parameters.SceneModeAuto;
            newParameters.AutoExposureLock = false;
            newParameters.WhiteBalance = Camera.Parameters.WhiteBalanceAuto;
            newParameters.ExposureCompensation = 12;
            newParameters.PictureFormat = Android.Graphics.ImageFormat.Jpeg;
            newParameters.JpegQuality = 100;
            newParameters.SetRotation(90);

            return newParameters;
        }

        private Camera.Size FindMaxSize(IList<Camera.Size> sizes)
        {
            Camera.Size[] orderByDescending = sizes
                                    .OrderByDescending(x => x.Width)
                                    .ToArray();
            return orderByDescending[0];
        }
    }
}
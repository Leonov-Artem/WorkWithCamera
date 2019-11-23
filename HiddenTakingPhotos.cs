using Android.Hardware;
using Android.Hardware.Camera2;
using Android.Util;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkWithCamera
{
    public class HiddenTakingPhotos
    {
        CameraInfo _cameraInfo;
        PictureCallback _pictureCallback;
        Camera.Parameters _newParameters;
        Camera _camera;
        CameraFacing _cameraFacing;
        readonly int CAMERA_ID = 0;
        Android.Views.IWindowManager windowManager;

        public HiddenTakingPhotos(CameraManager cameraManager, Android.Views.IWindowManager windowManager, CameraFacing cameraFacing)
        {
            this.windowManager = windowManager;
            _cameraInfo = new CameraInfo(cameraManager);
            _pictureCallback = new PictureCallback();
            _cameraFacing = cameraFacing;
        }

        public void TakePhoto()
        {
            if (_camera == null)
                _camera = GetCamera();

            Camera.Parameters oldParameters = _camera.GetParameters();
            _newParameters = GetModifiedParameters(oldParameters);

            _camera.SetPreviewTexture(new Android.Graphics.SurfaceTexture(10));
            _camera.SetParameters(_newParameters);
            _camera.StartPreview();
            _camera.TakePicture(null, null, _pictureCallback);
        }

        public void StopCamera()
        {
            if (_camera != null)
                _camera.Release();
            _camera = null;
        }

        private Camera GetCamera()
            => Camera.Open(CAMERA_ID);

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
            newParameters.SetRotation(setCameraDisplayOrientation(CAMERA_ID));

            return newParameters;
        }

        private Camera.Size FindMaxSize(IList<Camera.Size> sizes)
        {
            Camera.Size[] orderByDescending = sizes
                                    .OrderByDescending(x => x.Width)
                                    .ToArray();
            return orderByDescending[0];
        }

        int setCameraDisplayOrientation(int cameraId)
        {
            // определяем насколько повернут экран от нормального положения
            SurfaceOrientation rotation = windowManager.DefaultDisplay.Rotation;
            int degrees = 0;
            switch (rotation)
            {
                case SurfaceOrientation.Rotation0:
                    degrees = 0;
                    break;
                case SurfaceOrientation.Rotation90:
                    degrees = 90;
                    break;
                case SurfaceOrientation.Rotation180:
                    degrees = 180;
                    break;
                case SurfaceOrientation.Rotation270:
                    degrees = 270;
                    break;
            }

            int result = 0;

            // получаем инфо по камере cameraId
            var info = new Camera.CameraInfo();
            Camera.GetCameraInfo(CAMERA_ID, info);

            if (info.Facing == Camera.CameraInfo.CameraFacingFront)
            {
                result = (info.Orientation + degrees) % 360;
                result = (360 - result) % 360;  // compensate the mirror
            }
            else
            {  // back-facing
                result = (info.Orientation - degrees + 360) % 360;
            }

            return result;
        }
    }
}
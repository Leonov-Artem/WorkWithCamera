using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WorkWithCamera
{
    class CameraInfo
    {
        CameraManager _cameraManager;
        Camera.CameraInfo _cameraInfo;
        IWindowManager _windowManager;

        public CameraInfo(CameraManager cameraManager, IWindowManager windowManager=null)
        {
            _cameraManager = cameraManager;
            _cameraInfo = new Camera.CameraInfo();
        }

        public Dictionary<CameraFacing, int> DictionaryCameraFacing()
        {
            var keyValuePairs = new Dictionary<CameraFacing, int>();
            string[] cameraIDs =  _cameraManager.GetCameraIdList();

            foreach(var id in cameraIDs)
            {
                int cameraID = int.Parse(id);
                Camera.GetCameraInfo(cameraID, _cameraInfo);
                keyValuePairs[_cameraInfo.Facing] = cameraID;
            }

            return keyValuePairs;
        }

        public int NumberOfCameras()
            => _cameraManager.GetCameraIdList().Length;

        public int GetOrientation(int cameraID)
        {
            Camera.GetCameraInfo(cameraID, _cameraInfo);
            return _cameraInfo.Orientation;
        }

        public int GetCameraID(CameraFacing cameraFacing)
        {
            switch (cameraFacing)
            {
                case CameraFacing.Back: return 0;
                case CameraFacing.Front: return 1;
            }
            throw new ArgumentException();
        }

        void setCameraDisplayOrientation(int cameraId)
        {
            // определяем насколько повернут экран от нормального положения
            
            SurfaceOrientation rotation = _windowManager.DefaultDisplay.Rotation;
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
            Camera.GetCameraInfo(cameraId, info);

            // задняя камера
            if (info.Facing == Camera.CameraInfo.CameraFacingBack)
            {
                result = ((360 - degrees) + info.Orientation);
            }
            else
            // передняя камера
            if (info.Facing == Camera.CameraInfo.CameraFacingFront)
            {
                result = ((360 - degrees) - info.Orientation);
                result += 360;
            }
            result = result % 360;

            //camera.setDisplayOrientation(result);
        }
    }
}
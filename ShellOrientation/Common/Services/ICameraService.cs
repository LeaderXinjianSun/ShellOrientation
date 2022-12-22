

using HalconDotNet;

namespace ShellOrientation.Common.Services
{
    interface ICameraService
    {
        bool OpenCamera(string cameraName, string CameraInterface = "GigEVision");
        void CloseCamera();
        void GrabeImageStart();
        HImage GrabImage();
        HImage GrabeImageAsync();
    }
}

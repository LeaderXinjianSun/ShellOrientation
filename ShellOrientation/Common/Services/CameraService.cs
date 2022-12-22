

using HalconDotNet;
using System;

namespace ShellOrientation.Common.Services
{
    public class CameraService : ICameraService
    {
        HFramegrabber Framegrabber;
        HTuple AcqHandle;
        public bool OpenCamera(string cameraName, string CameraInterface = "GigEVision")
        {
            try
            {                
                HOperatorSet.OpenFramegrabber(CameraInterface, 1, 1, 0, 0, 0, 0, "default", 8, "rgb",-1, "false", "default", cameraName, 0, -1, out AcqHandle);
                Framegrabber = new HFramegrabber(AcqHandle);
                return true;
            }
            catch (HalconException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public void CloseCamera()
        {
            try
            {
                Framegrabber?.Dispose();
            }
            catch { }
        }
        public void GrabeImageStart()
        {
            Framegrabber.GrabImageStart(-1);
        }
        public HImage GrabImage()
        {
            try
            {
                return Framegrabber.GrabImage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public HImage GrabeImageAsync()
        {
            try
            {
                return Framegrabber.GrabImageAsync(-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

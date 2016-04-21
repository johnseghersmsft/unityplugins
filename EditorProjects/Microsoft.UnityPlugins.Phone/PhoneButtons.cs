using System;

namespace Microsoft.UnityPlugins
{
    public class ProxyCameraEventArgs : EventArgs
    {
    };

    public class ProxyBackPressedEventArgs : EventArgs
    {

        public bool Handled { get; set; }
    }

    public class PhoneButtons
    {
        public event EventHandler<ProxyBackPressedEventArgs> BackPressed;
        public event EventHandler<ProxyCameraEventArgs> CameraHalfPressed;
        public event EventHandler<ProxyCameraEventArgs> CameraPressed;
        public event EventHandler<ProxyCameraEventArgs> CameraReleased;

        public void EnableBackPressedEvent()
        {
        }

        public void DisableBackPressedEvent()
        {
        }

        public void EnableCameraHalfPressedEvent()
        {
        }

        public void DisableCameraHalfPressedEvent()
        {
        }

        public void EnableCameraPressedEvent()
        {
        }

        public void DisableCameraPressedEvent()
        {
        }

        public void EnableCameraReleasedEvent()
        {
        }

        public void DisableCameraReleasedEvent()
        {
        }

    }
}

using System;
using Windows.Phone.UI.Input;

namespace Microsoft.UnityPlugins
{
    public class ProxyCameraEventArgs
    {
    };

    public class ProxyBackPressedEventArgs
    {
        private readonly BackPressedEventArgs _baseEventArgs;

        public ProxyBackPressedEventArgs()
        {
            _baseEventArgs = null;
        }
        internal ProxyBackPressedEventArgs(BackPressedEventArgs args)
        {
            _baseEventArgs = args;
        }

        public bool Handled {
            get { return _baseEventArgs != null && _baseEventArgs.Handled; }
            set
            {
                if (_baseEventArgs != null)
                    _baseEventArgs.Handled = value;
            }
        }
    }

    public class PhoneButtons
    {
        public event EventHandler<ProxyBackPressedEventArgs> BackPressed;
        public event EventHandler<ProxyCameraEventArgs> CameraHalfPressed;
        public event EventHandler<ProxyCameraEventArgs> CameraPressed;
        public event EventHandler<ProxyCameraEventArgs> CameraReleased;

        public bool IsHardwareButtonsAPIPresent { get; private set; }

        public PhoneButtons()
        {
            IsHardwareButtonsAPIPresent = Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        }

        public void EnableBackPressedEvent()
        {
            if (!IsHardwareButtonsAPIPresent)
                return;

            HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;
        }

        public void DisableBackPressedEvent()
        {
            if (!IsHardwareButtonsAPIPresent)
                return;

            HardwareButtons.BackPressed -= HardwareButtonsOnBackPressed;
        }

        public void EnableCameraHalfPressedEvent()
        {
            if (!IsHardwareButtonsAPIPresent)
                return;

            HardwareButtons.CameraHalfPressed += HardwareButtonsOnCameraHalfPressed;
        }

        public void DisableCameraHalfPressedEvent()
        {
            if (!IsHardwareButtonsAPIPresent)
                return;

            HardwareButtons.CameraHalfPressed -= HardwareButtonsOnCameraHalfPressed;
        }

        public void EnableCameraPressedEvent()
        {
            if (!IsHardwareButtonsAPIPresent)
                return;

            HardwareButtons.CameraPressed += HardwareButtonsOnCameraPressed;
        }

        public void DisableCameraPressedEvent()
        {
            if (!IsHardwareButtonsAPIPresent)
                return;

            HardwareButtons.CameraPressed -= HardwareButtonsOnCameraPressed;
        }

        public void EnableCameraReleasedEvent()
        {
            if (!IsHardwareButtonsAPIPresent)
                return;

            HardwareButtons.CameraReleased += HardwareButtonsOnCameraReleased;
        }

        public void DisableCameraReleasedEvent()
        {
            if (!IsHardwareButtonsAPIPresent)
                return;

            HardwareButtons.CameraReleased -= HardwareButtonsOnCameraReleased;
        }

        private void HardwareButtonsOnBackPressed(object sender, BackPressedEventArgs backPressedEventArgs)
        {
            InvokeBackButtonEvent(BackPressed, sender, backPressedEventArgs);
        }

        private void HardwareButtonsOnCameraHalfPressed(object sender, CameraEventArgs cameraEventArgs)
        {
            InvokeCameraEvent(CameraHalfPressed, sender);
        }

        private void HardwareButtonsOnCameraPressed(object sender, CameraEventArgs cameraEventArgs)
        {
            InvokeCameraEvent(CameraPressed, sender);
        }

        private void HardwareButtonsOnCameraReleased(object sender, CameraEventArgs cameraEventArgs)
        {
            InvokeCameraEvent(CameraReleased, sender);
        }

        private static void InvokeBackButtonEvent(EventHandler<ProxyBackPressedEventArgs> eventHandler, object sender,
            BackPressedEventArgs eventArgs)
        {
            if (eventHandler != null)
            {
                Utils.RunOnUnityAppThread(() => eventHandler(sender, new ProxyBackPressedEventArgs(eventArgs)));
            }

        }
        private static void InvokeCameraEvent(EventHandler<ProxyCameraEventArgs> eventHandler, object sender)
        {
            if (eventHandler != null)
            {
                Utils.RunOnUnityAppThread(() => eventHandler(sender, new ProxyCameraEventArgs()));
            }
        }

    }
}

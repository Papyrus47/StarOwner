using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.Cameras
{
    public class MoveScreenCamera : ICameraModifier
    {
        public int timeLeft;
        public Vector2 screenCenter;
        public float amount = 0.8f;
        private string _uniqueIdentity;

        public MoveScreenCamera(int timeLeft, Vector2 screenCenter, string uniqueIdentity = null)
        {
            this.timeLeft = timeLeft;
            this.screenCenter = screenCenter;
            _uniqueIdentity = uniqueIdentity;
        }

        public string UniqueIdentity => _uniqueIdentity;
        public bool Finished => timeLeft <= 0; 

        public void Update(ref CameraInfo cameraPosition)
        {
            timeLeft--;
            cameraPosition.CameraPosition = Vector2.Lerp(screenCenter, cameraPosition.CameraPosition, amount);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class WalkState : AnimationState
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private ILocomotion3d _locomotion;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Start()
        {
            base.Start();

            _locomotion = (ILocomotion3d)_dependencyManager.Registry[typeof(ILocomotion3d)];
            if (_locomotion == null)
            {
                Debug.LogError("Locomotion not found on " + name);
            }
        }

        public override bool ShouldPlay()
        {
            if (_locomotion.HorizontalPlaneMovement.XZPlane().magnitude > float.Epsilon)
            {
                return true;
            }

            return false;
        }
    }
}

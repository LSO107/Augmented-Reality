using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal static class InputWrapper
    {
        private static bool UseTouchControls()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
			return true;
#else
            return false;
#endif
        }

        public static bool HasDoubleTapped()
        {
            if (UseTouchControls())
            {
                return Input.GetTouch(0).tapCount == 2;
            }
            else
            {
                return Input.GetMouseButtonDown(0);
            }
        }

        public static bool IsPinching()
        {
            if (UseTouchControls())
            {
                return Input.touchCount == 2;
            }
            else
            {
                return false;
            }
        }
    }
}

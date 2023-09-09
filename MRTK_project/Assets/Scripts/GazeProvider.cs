using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GazeProvider
    {
        // Log current gaze target
        public static void LogCurrentGazeTarget()
        {
            if (CoreServices.InputSystem.GazeProvider.GazeTarget)
                Debug.Log("User gaze is currently over game object: "
                    + CoreServices.InputSystem.GazeProvider.GazeTarget);
        }
    }
}
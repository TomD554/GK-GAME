using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.iOS;

public class ScreenSizeControllerOnLaunch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;

        Screen.orientation = ScreenOrientation.LandscapeLeft;

//        PlayerSettings.iOS.deferSystemGesturesMode = SystemGestureDeferMode.All;
    }

}

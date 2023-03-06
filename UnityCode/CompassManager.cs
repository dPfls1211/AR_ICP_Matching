using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CompassManager : MonoBehaviour
{
    public Text gyoOut;
    public static float magneticHeading;
    public static float _trueHeading;
    public static Vector3 _rawVector;

    private static WaitForSeconds seconds;
    private static bool compassStarted = false;

    private void Start()
    {
        Input.location.Start();
        Input.compass.enabled = true;
    }

    private void Update()
    {
        magneticHeading = Input.compass.magneticHeading;
        _trueHeading = Input.compass.trueHeading;
        _rawVector = Input.compass.rawVector;
    }

    //private void Awake()
    //{
    //    //Input.location.Start();  //위치 서비스 시작
    //    //Input.compass.enabled = true;  //나침반 활성화
    //    seconds = new WaitForSeconds(1.0f);
    //}

    //IEnumerator Start()
    //{
    //    Input.location.Start();
    //    Input.compass.enabled = true; //나침반 활성화
    //    int maxWait = 20;
    //    while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
    //    {
    //        yield return seconds;
    //        maxWait -= 1;
    //    }

    //    if (maxWait < 1)
    //    {
    //        Debug.Log("compass Timed out");
    //        yield break;
    //    }

    //    if (Input.location.status == LocationServiceStatus.Failed)
    //    {
    //        Debug.Log("Unable to detemine device loaction with Compass");
    //        yield break;
    //    }
    //    else
    //    {

    //        compassStarted = true;

    //        while (compassStarted)
    //        {
    //            //헤딩 값 가져오기
    //            //if (Input.compass.headingAccuracy == 0 || Input.compass.headingAccuracy > 0)
    //            //{
    //                magneticHeading = Input.compass.magneticHeading;
    //                _rawVector = Input.compass.rawVector;
    //                _trueHeading = Input.compass.trueHeading;
    //                gyoOut.text = "각도 : " + _rawVector.ToString();
    //                //Debug.Log(rawVecor.x);
    //            //}
    //            yield return seconds;
    //        }
    //    }
    //}

    //public static void StopGyro()
    //{
    //    if (Input.location.isEnabledByUser)
    //    {
    //        compassStarted = false;
    //        Input.location.Stop();
    //    }
    //}
}

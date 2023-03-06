using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GyroManager : MonoBehaviour
{
    public Text gyoOut;
    
    public static Vector3 GyroVector;
    public static float GyroW;

    private static WaitForSeconds seconds;
    private static bool GyroStarted = false;

    private void Awake()
    {
        //Input.location.Start();  //위치 서비스 시작
        //Input.compass.enabled = true;  //나침반 활성화
        seconds = new WaitForSeconds(1.0f);
    }

    IEnumerator Start()
    {
        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return seconds;
            maxWait -= 1;
        }

        if (maxWait < 1)
        {
            Debug.Log("compass Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to detemine device loaction with Compass");
            yield break;
        }
        else
        {
            Input.gyro.enabled = true; //자이로센서 활성화
            GyroStarted = true;

            while (GyroStarted)
            {
                GyroW=Input.gyro.attitude.w;
                GyroVector = new Vector3(Input.gyro.attitude.x, Input.gyro.attitude.y, Input.gyro.attitude.z);

                yield return seconds;
            }
        }
    }

    public static void StopGyro()
    {
        if (Input.location.isEnabledByUser)
        {
            GyroStarted = false;
            Input.location.Stop();
        }
    }
}

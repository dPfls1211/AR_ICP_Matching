using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;


public class CheckRoi : MonoBehaviour
{
    private static float _planeROI = 5.0f;
    private static float _obstacleROI = 3.0f;

    public static bool PlaneCheck(Vector3 point)
    {
        float size = Vector3.Distance(GetCameraPos._userPos, point);
        if (size > _planeROI) return false;
        else return true;
    }
    public static bool ObstacleCheck(Vector3 point)
    {
        float size = Vector3.Distance(GetCameraPos._userPos, point);
        if (size > _obstacleROI) return false;
        else return true;
    }

    public static bool ROI(Vector3 point)
    {
        bool isROI = true;
        Vector3 user = GetCameraPos._userPos;

        //�¿� 
        if (point.x - user.x >= 1 && point.x - user.x <= 0)
            isROI = false;
        //�յ�
        if (point.y - user.y >= 1 && point.y - user.y <= 0)
            isROI = false;
        //����
        if (point.z - user.z >= 1 && point.z - user.z <= 0)
            isROI = false;


        return isROI;
    }
}

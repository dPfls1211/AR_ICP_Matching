using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PointCloudVisualization : MonoBehaviour
{
    public static List<Vector3> _obstaclePoints = new List<Vector3>();

    private static WaitForSeconds seconds;

    // Start is called before the first frame update
    //private void Awake()
    //{
    //    //Input.location.Start();  //��ġ ���� ����
    //    //Input.compass.enabled = true;  //��ħ�� Ȱ��ȭ
    //    seconds = new WaitForSeconds(1.0f);
    //}

    //IEnumerator Start()
    //{
    //    var points = _obstaclePoints;
    //    points.Clear();
        


    //    yield return seconds;
    //}
}

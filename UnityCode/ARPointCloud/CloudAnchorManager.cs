using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using System;

public class CloudAnchorManager : MonoBehaviour
{
    public ARPointCloudManager ARPointCloudManager;
    public ARAnchorManager anchorManager;
    public ARRaycastManager raycastManager;
    //���� ��ų ������
    public GameObject anchorPrefab;
    //���� ��ü ���� (�����ϱ� ���� �뵵)
    private GameObject anchorGameObject;

    //���� ��Ŀ ���庯��
    private ARAnchor localAnchor;
    //zŬ���� ��Ŀ ���庯��
    private ARAnchor cloudlAnchor;
    //Ŭ���� ��Ŀ ID �����ϱ� ���� Ű
    private const string cloudAnchorkey = "CLOUD_ANCHOR_ID";
    //Ŭ���� ��Ŀ ID
    private string strCloudAnchorId;

    // Raycast Hit
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
       // ARPointCloudManager.Execute();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Hosting() //��ǥ �ִ��� Ȯ��
    {
        if (Input.touchCount < 1) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;


        // ���� ��Ŀ�� �����ϴ��� ���θ� Ȯ��
        if (localAnchor == null)
        {
            // Raycast �߻�
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                // ���� ��Ŀ�� ����
                localAnchor = anchorManager.AddAnchor(hits[0].pose);
                // ���� ��Ŀ ��ġ�� �罿 ������Ű�� ������ ����
                anchorGameObject = Instantiate(anchorPrefab, localAnchor.transform);
            }
        }

    }
}

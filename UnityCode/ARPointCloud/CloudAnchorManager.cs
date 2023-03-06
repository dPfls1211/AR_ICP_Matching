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
    //증강 시킬 프리팹
    public GameObject anchorPrefab;
    //저장 객체 변수 (삭제하기 위한 용도)
    private GameObject anchorGameObject;

    //로컬 앵커 저장변수
    private ARAnchor localAnchor;
    //z클라우드 앵커 저장변수
    private ARAnchor cloudlAnchor;
    //클라우드 앵커 ID 저장하깅 ㅟ한 키
    private const string cloudAnchorkey = "CLOUD_ANCHOR_ID";
    //클라우드 앵커 ID
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
    public void Hosting() //좌표 있는지 확인
    {
        if (Input.touchCount < 1) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;


        // 로컬 앵커가 존재하는지 여부를 확인
        if (localAnchor == null)
        {
            // Raycast 발사
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                // 로컬 앵커를 생성
                localAnchor = anchorManager.AddAnchor(hits[0].pose);
                // 로컬 앵커 위치에 사슴 증강시키고 변수에 저장
                anchorGameObject = Instantiate(anchorPrefab, localAnchor.transform);
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Runtime.InteropServices;
using System.IO;

using arF = UnityEngine.XR.ARFoundation.ARPointCloudParticleVisualizion;



public class DatabaseBehavior : MonoBehaviour
{
    int n = 0;
    [DllImport("dll_func_icp80.dll")]
    private static extern double pointSort(double[,] dbData, double[,] newData, int dbDataSize, int newDataSize);
    string icpPth = "Assets/ICPResult/";
    public InputField InputField;
    public InputField InputFielda;
   // public bool isBtnDown=true;
    public Text GPS_T;
    public Text Gyro_T;
    public Text compass_T;
    public Text point_T;
    public InputField InputPlace;

    private GameObject PointTag;
    private bool _chPoint;
    private double _pointFit;

    double[,] _newData_;
    int ID = 0;
    FirebaseFirestore db;

    static Vector3 __centerPositionCH;

    static List<Vector3> s_Vertices = new List<Vector3>();

    public List<string> listQuery = new List<string>();
    public List<List<string>> listPoint = new List<List<string>>();
    public List<List<string>> listPointO = new List<List<string>>();

    public GameObject ARNewObj;
    public GameObject ARUpdateObj;

    private Vector2 touchPosition;

    public Collider coll;
    public Camera _arCam;

    List<Vector3> arObjPos;
    void Start()
    {
        arObjPos = new List<Vector3>();
        coll = ARNewObj.GetComponent<Collider>();
        ID = 0;
        _chPoint = false;
        _pointFit = 0;
        db = FirebaseFirestore.DefaultInstance;
        Input.location.Start();
        Input.compass.enabled = true;
        Input.gyro.enabled = true;
        PointTag = GameObject.FindGameObjectWithTag("PointTag");
        if (PointTag != null)
            Debug.Log("Don't Find");
        //visualizer = PointTag.GetComponent<arF>();
        //s_Vertices.Add(new Vector3(1, 2, 3));
        //s_Vertices.Add(new Vector3(1, 2, 4));
        //s_Vertices.Add(new Vector3(1, 2, 5));
        //s_Vertices.Add(new Vector3(1, 2, 6));
        //After remove
        //string orignCollectss = "37x126a57";
        //db.Collection(orignCollectss).GetSnapshotAsync().ContinueWithOnMainThread(taskj =>
        //{
        //    QuerySnapshot orignQuerySnapshots = taskj.Result;
        //    foreach (DocumentSnapshot docd in orignQuerySnapshots.Documents)
        //    {
        //        string orignDoc = docd.Id.ToString();
        //        Query orignquery = db.Collection(orignCollectss).Document(orignDoc).Collection("pointCloud");
        //        orignquery.GetSnapshotAsync().ContinueWithOnMainThread(taskOa =>
        //        {
        //            QuerySnapshot documentSnapshotsff = taskOa.Result;

        //            foreach (DocumentSnapshot listSnapshots in documentSnapshotsff.Documents)
        //            {
        //                string docu = listSnapshots.Id.ToString();
        //                List<string> listpoints = new List<string>();
        //                foreach (KeyValuePair<string, object> pair in listSnapshots.ToDictionary())
        //                {
        //                    listpoints.Add(pair.Value.ToString());
        //                }
        //                Debug.Log("inOrign" + listpoints.Count);

        //                listPointO.Add(listpoints);
        //                Debug.Log("inOrignlistO" + listPointO.Count);
        //            }
        //        });
        //    }
        //});

        //StartCoroutine(startDBCorutine(37, 126, 0));
        //StartCoroutine(DBCenterObjUpdate()); //그.. 새 세션시 증강
        //StartCoroutine(DBObjUpdate());
        //CheckPosition();
    }
    private void Update()
    {
        //객체 위치 파악
        //CheckPosition();

       
        //GPS_T.text = (GpsManager.current_Lat).ToString() + "x" + (GpsManager.current_Long).ToString();
        //if can not Find gps
        if (Input.location.status == LocationServiceStatus.Failed)
            GPS_T.text = "GPS : Fail";
        Gyro_T.text = GyroManager.GyroW.ToString() + ", " + GyroManager.GyroVector.x + ", " + GyroManager.GyroVector.y +
            ", " + GyroManager.GyroVector.z;
        compass_T.text = CompassManager._trueHeading.ToString();
        //point_T.text = "change";
        //point_T.text = s_Vertices[0].ToString();
        //point_T.text = visualizer.m_NumParticles.ToString();
        //point_T.text = arF.m_PointCloud;

        //var p_oints = PointTag.GetComponent<arF.ARPointCloudParticleVisualizer>().m_PointCloud;
        //var p_oints = arF.m_Particles;
        //var p_oints = PointTag.GetComponent<arF.ARPointCloudParticleVisualizion>().m_Particles;

        //var p_oints = visualizer.m_Particles;
        //point_T.text = "change1";
        //var points = s_Vertices;
        //point_T.text = "change2";
        //points.Clear();
        //point_T.text = "change3";


        //point_T.text = s_Vertices[0].x.ToString() + "x" + p_oints[0].position.x.ToString();
        // display touch
        //if (Input.touchCount == 1) //
        //{
        //    if (Input.GetTouch(0).phase == TouchPhase.Ended)
        //    {
        //        point_T.text = "hit";
        //        StartCoroutine(CenterPosition());
        //        //클릭 위치
        //        float t_x, t_y, t_z;
        //        t_x = Input.GetTouch(0).position.x;
        //        t_y = Input.GetTouch(0).position.y;
        //        //z는 중심점 보다 2정도 위?
        //        Vector3 objPosition = new Vector3(__centerPositionCH.x, __centerPositionCH.y, __centerPositionCH.z);
        //        //Vector3 objPosition = new Vector3(t_x, t_y, __centerPositionCH.z);
        //        Instantiate(ARObj, objPosition, Quaternion.identity);

        //        //DB에 위치. 방향 저장
        //    }

        //}
        if (!Utility.TryGetInputPosition(out touchPosition)) return;
        //if (isBtnDown)
        {

            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("ARObj");
            //point_T.text = gameObjects.Length.ToString();
            if (gameObjects.Length <= 0)
            {
                if (Utility.Raycast(touchPosition, out Pose hitPose))
                {
                    if (_objPos(hitPose))
                    {
                        float _isize =(Vector3.Distance(FindCenterPoint(hitPose), _arCam.transform.position) * 0.5f) / 3;
                        transform.localScale = new Vector3(0.05f + _isize, 0.05f + _isize, 0.05f + _isize);
                        Vector3 centerVec = FindCenterPoint(hitPose);
                        StartCoroutine(CenterDBSave(hitPose, centerVec));


                        Instantiate(ARNewObj, centerVec, Quaternion.identity);
                        arObjPos.Add(centerVec);
                        //StartCoroutine(DBSave(hitPose));
                        //DBSave(hitPose);
                    }
                }
            }

            for (int gameID=0; gameID<gameObjects.Length;gameID++)
            {
                //Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y));
                //RaycastHit _cHit;
                if (!gameObjects[gameID].GetComponent<colliderRay>().Colls())
                {

                    if (Utility.Raycast(touchPosition, out Pose hitPose))
                    {
                        //GPS_T.text = Vector3.Distance(FindCenterPoint(hitPose), _arCam.transform.position).ToString();
                        if (Vector3.Distance(FindCenterPoint(hitPose), _arCam.transform.position) > 30.0f)
                            if (_objPos(hitPose))
                            {
                                float _isize = (Vector3.Distance(FindCenterPoint(hitPose), _arCam.transform.position) * 0.5f) / 3;
                                transform.localScale = new Vector3(0.05f + _isize, 0.05f + _isize, 0.05f + _isize);
                                Vector3 centerVec = FindCenterPoint(hitPose);
                                StartCoroutine(CenterDBSave(hitPose, centerVec));


                                Instantiate(ARNewObj, centerVec, Quaternion.identity);
                                arObjPos.Add(centerVec);
                                //StartCoroutine(DBSave(hitPose));
                                //DBSave(hitPose);
                            }
                    }
                    break;
                }
            }
            
        }
        
        //CheckPosition();
    }
    public bool _objPos(Pose _hitPose)
    {
        foreach (Vector3 gameObject in arObjPos)
        {
            //point_T.text = "list in";
            if (Vector3.Distance(gameObject, FindCenterPoint(_hitPose))>=300.0f)
                return false;
        }
        return true;
    }
    public void IcpTest2()
    {
        StartCoroutine(ICPCall2(InputField.text, InputFielda.text));

    }
    IEnumerator ICPCall2(string field, string afield)
    {
        var collection = field;
        //var com = int.Parse(afield);

        int n = 0;
        listPointO.Clear();
        CollectionReference firstRefd = db.Collection("Pointcloud").Document("1").Collection(field);
        
            listPoint.Clear();

            //주변 각도 찾기
            var collections = collection;
            //collections += (com).ToString();
            Debug.Log("path : " + collections.ToString());

            CollectionReference firstRef = db.Collection("Pointcloud").Document("1").Collection(afield);
            Query allfirstQuery = firstRef;
        db.Collection("Pointcloud").Document("1").Collection(field).GetSnapshotAsync().ContinueWithOnMainThread(taskn =>   //new
        {
            QuerySnapshot newQuerySnapshots = taskn.Result;
            allfirstQuery.GetSnapshotAsync().ContinueWithOnMainThread(tasks =>
            {
                Debug.Log(collections.ToString() + " // collectoin ch");
                QuerySnapshot allfirstQuerySnapshots = tasks.Result;

                foreach (DocumentSnapshot documentSnapshotnn in newQuerySnapshots.Documents)
                {
                    Debug.Log("//////");
                    string docn = documentSnapshotnn.Id.ToString();
                    foreach (DocumentSnapshot documentSnapshot in allfirstQuerySnapshots.Documents)
                    {
                        string doc = documentSnapshot.Id.ToString();
                        Debug.Log(doc + "//////");
                        //When documents is Once
                        Query newdataQuery = db.Collection("Pointcloud").Document("1").Collection(field).Document(docn).Collection("pointCloud");
                        Query listquery = db.Collection("Pointcloud").Document("1").Collection(afield).Document(doc).Collection("pointCloud");
                        newdataQuery.GetSnapshotAsync().ContinueWithOnMainThread(taskNN =>
                        {
                            QuerySnapshot NNdocumentSnapshot = taskNN.Result;
                            listquery.GetSnapshotAsync().ContinueWithOnMainThread(taskL =>
                            {
                                listPoint.Clear();
                                listPointO.Clear();
                                QuerySnapshot documentSnapshots = taskL.Result;

                                foreach (DocumentSnapshot listsw in NNdocumentSnapshot.Documents)
                                {

                                    string docns = listsw.Id.ToString();
                                    List<string> newPoint = new List<string>();
                                    foreach (KeyValuePair<string, object> pair in listsw.ToDictionary())
                                    {
                                        newPoint.Add(pair.Value.ToString());
                                    }
                                    listPointO.Add(newPoint);
                                }
                                //list->array
                                //db
                                _newData_ = new double[listPointO.Count, 3];
                                Debug.Log("newData length : " + _newData_.Length);
                                for (int _tNum = 0; _tNum < listPointO.Count; _tNum++)
                                {
                                    _newData_[_tNum, 0] = Double.Parse(listPointO[_tNum][0]);
                                    _newData_[_tNum, 1] = Double.Parse(listPointO[_tNum][1]);
                                    _newData_[_tNum, 2] = Double.Parse(listPointO[_tNum][2]);
                                }
                                Debug.Log("newData length : " + _newData_.Length);
                                Debug.Log("newDatalist count : " + listPointO.Count);
                                point_T.text = "Db->arr";

                                foreach (DocumentSnapshot listSnapshots in documentSnapshots.Documents)
                                {
                                    string docu = listSnapshots.Id.ToString();
                                    List<string> listpoints = new List<string>();
                                    foreach (KeyValuePair<string, object> pair in listSnapshots.ToDictionary())
                                    {
                                        listpoints.Add(pair.Value.ToString());
                                    }
                                    Debug.Log("iistPointsCount : " + listpoints.Count);
                                    listPoint.Add(listpoints);

                                }
                                //list->array
                                //db
                                double[,] _dbSaveData_ = new double[listPoint.Count, 3];
                                _dbSaveData_.Initialize();

                                Debug.Log("arrPoint Length : " + _dbSaveData_.Length);
                                Debug.Log("listPoint Count : " + listPoint.Count);
                               
                                for (int tint = 0; tint < listPoint.Count; tint++)
                                {
                                    for (int r = 0; r < 3; r++)
                                    {
                                        _dbSaveData_[tint, r] = Double.Parse(listPoint[tint][r]);

                                    }

                                }
                                Debug.Log("arrPoint2 Length" + _dbSaveData_.Length);
                                point_T.text = "Db->arr";
                                n = 0;
                                StartCoroutine(saveTxt(_newData_, _dbSaveData_, (field + "aft" + afield)));
                                
                                //icp호출 
                                if (listPoint.Count != 0)
                                    _pointFit = pointSort(_dbSaveData_, _newData_, _dbSaveData_.Length, _newData_.Length);

                                Debug.Log("ICPFit : " + _pointFit);
                                Debug.Log(point_T.text);
                                //fit에 따라서 chPoint=T로 ..
                                if (_pointFit < 0.03 && _pointFit != 0)
                                    _chPoint = true;
                                
                                point_T.text = "icpStart // " + _dbSaveData_.Length + "\n"
                                    + "call // check : " + _chPoint.ToString() + ", fitness : " + _pointFit.ToString();
                                

                                Debug.Log(point_T.text);
                                Debug.Log("out" + listPoint.Count);
                            });
                        });

                        Debug.Log("last : " + listPoint.Count);
                        listPoint.Clear();
                    }
                }
                
            });
        });


        yield break;
    }
    public void IcpTest()
    {
        //int n = 0;
        //do
        //{
        //    if (false == File.Exists(icpPth + InputField.text + InputFielda.text + "_" + n.ToString()+".txt"))
        //    {
        //        var file = File.CreateText(icpPth + InputField.text + InputFielda.text + "_" + n.ToString() + ".txt");
        //        file.Close();
        //        break;
        //    }
        //    else n++;
        //} while (true);
        //StreamWriter sw = new StreamWriter(icpPth + InputField.text+ "_" + n.ToString() + ".txt");
        //Debug.Log(icpPth + InputField.text + "_" + n.ToString());
        StartCoroutine(ICPCall( InputField.text, InputFielda.text));
        //sw.WriteLine("dat");
        //sw.WriteLine("dataaa");
        //sw.Flush();
        //sw.Close();
      
    }
    IEnumerator saveTxt(double[,] _newdata_, double[,] _dbSaveData_, string savetxt)
    {
        double[,] _newData = _newdata_;
        double[,] _DBData = _dbSaveData_;
        Debug.Log("savetxt : "+ savetxt);
        _pointFit = pointSort(_DBData, _newData, _DBData.Length, _newData.Length);
        Debug.Log("savetxt:" + _pointFit);
       
        do
        {
            if (false == File.Exists(icpPth+savetxt + "_" + n.ToString() + ".txt"))
            {
                Debug.Log("savetxtNum:" + n);
                var file = File.CreateText(icpPth+savetxt + "_" + n.ToString() + ".txt");
                file.Close();
                break;
            }
            else n++;
        } while (n<15);
        Debug.Log("savetxt: stream");
        StreamWriter sw = new StreamWriter(icpPth+savetxt + "_" + n.ToString() + ".txt");
        sw.WriteLine("new Data");
        for (int _tNum = 0; _tNum < _newData.Length/3; _tNum++)
        {
            sw.WriteLine(_newData[_tNum, 0] + " " + _newData[_tNum, 1] + " " + _newData[_tNum, 2]);
            //data += (_newData_[_tNum, 0] + " " + _newData_[_tNum, 1] + " " + _newData_[_tNum, 2] + "\n");
        }

        Debug.Log("savetxt: stream : new"+_newData.Length);
        sw.WriteLine("");
        sw.WriteLine("DB Data");
        for (int tint = 0; tint < _DBData.Length/3; tint++)
        {
            
            sw.WriteLine(_DBData[tint, 0] + " " + _DBData[tint, 1] + " " + _DBData[tint, 2]);

        }
        Debug.Log("savetxt: stream : db"+_DBData.Length);
        sw.WriteLine("");
        sw.WriteLine("ICP : "+_pointFit.ToString());

        sw.Flush();
        sw.Close();
        yield break;
    }
    
    IEnumerator ICPCall( string field, string afield)
    {
        var collection = field;
        var com = int.Parse(afield);
       
        int n = 0;
        listPointO.Clear();
        CollectionReference firstRefd = db.Collection("Pointcloud").Document("1").Collection(field+afield);
       
        for (int j = -10; j < 10; j++)
        {
            listPoint.Clear();

            //주변 각도 찾기
            var collections = collection;
            collections += (com + j).ToString();
            Debug.Log("path : "+collections.ToString());

            CollectionReference firstRef = db.Collection("Pointcloud").Document("1").Collection(collections);
            Query allfirstQuery = firstRef;
            db.Collection("Pointcloud").Document("1").Collection(field + afield).GetSnapshotAsync().ContinueWithOnMainThread(taskn =>   //new
            {
                QuerySnapshot newQuerySnapshots = taskn.Result;
                allfirstQuery.GetSnapshotAsync().ContinueWithOnMainThread(tasks =>
                {
                Debug.Log(collections.ToString() + " // collectoin ch");
                QuerySnapshot allfirstQuerySnapshots = tasks.Result;

                    foreach (DocumentSnapshot documentSnapshotnn in newQuerySnapshots.Documents)
                    {
                        Debug.Log( "//////");
                        string docn = documentSnapshotnn.Id.ToString();
                        foreach (DocumentSnapshot documentSnapshot in allfirstQuerySnapshots.Documents)
                        {
                            string doc = documentSnapshot.Id.ToString();
                            Debug.Log(doc+"//////");
                            //When documents is Once
                            Query newdataQuery = db.Collection("Pointcloud").Document("1").Collection(field + afield).Document(docn).Collection("pointCloud");
                            Query listquery = db.Collection("Pointcloud").Document("1").Collection(collections).Document(doc).Collection("pointCloud");
                            newdataQuery.GetSnapshotAsync().ContinueWithOnMainThread(taskNN =>
                            {
                                QuerySnapshot NNdocumentSnapshot = taskNN.Result;
                                listquery.GetSnapshotAsync().ContinueWithOnMainThread(taskL =>
                                {
                                    //StreamWriter sw = new StreamWriter(path);
                                    listPoint.Clear();
                                    listPointO.Clear();
                                    QuerySnapshot documentSnapshots = taskL.Result;

                                    foreach (DocumentSnapshot listsw in NNdocumentSnapshot.Documents)
                                    {

                                        string docns = listsw.Id.ToString();
                                        List<string> newPoint = new List<string>();
                                        foreach (KeyValuePair<string, object> pair in listsw.ToDictionary())
                                        {
                                            newPoint.Add(pair.Value.ToString());
                                            //Debug.Log("testPointcloud value : " + pair.Value.ToString());
                                        }
                                        listPointO.Add(newPoint);
                                    }
                                    //list->array
                                    //db
                                    _newData_ = new double[listPointO.Count, 3];
                                    Debug.Log("newData length : " + _newData_.Length);
                                    //int _newCount = 0;
                                    //sw.WriteLine("newData x y z");
                                    //data += "newData x y z\n";
                                    //sw.WriteLine("");
                                    //sw.WriteLine("new Point");
                                    for (int _tNum = 0; _tNum < listPointO.Count; _tNum++)
                                    {
                                        _newData_[_tNum, 0] = Double.Parse(listPointO[_tNum][0]);
                                        _newData_[_tNum, 1] = Double.Parse(listPointO[_tNum][1]);
                                        _newData_[_tNum, 2] = Double.Parse(listPointO[_tNum][2]);
                                        //sw.WriteLine(_newData_[_tNum, 0] + " " + _newData_[_tNum, 1] + " " + _newData_[_tNum, 2]);
                                        //data += (_newData_[_tNum, 0] + " " + _newData_[_tNum, 1] + " " + _newData_[_tNum, 2] + "\n");
                                    }
                                    Debug.Log("newData length : " + _newData_.Length);
                                    Debug.Log("newDatalist count : " + listPointO.Count);
                                    point_T.text = "Db->arr";

                                    foreach (DocumentSnapshot listSnapshots in documentSnapshots.Documents)
                                    {
                                        string docu = listSnapshots.Id.ToString();
                                        List<string> listpoints = new List<string>();
                                        foreach (KeyValuePair<string, object> pair in listSnapshots.ToDictionary())
                                        {
                                            listpoints.Add(pair.Value.ToString());
                                            //Debug.Log("testPointcloud value : " + pair.Value.ToString());
                                        }
                                        Debug.Log("iistPointsCount : " + listpoints.Count);
                                        listPoint.Add(listpoints);

                                    }
                                    //list->array
                                    //db
                                    double[,] _dbSaveData_ = new double[listPoint.Count, 3];
                                    _dbSaveData_.Initialize();

                                    Debug.Log("arrPoint Length : " + _dbSaveData_.Length);
                                    Debug.Log("listPoint Count : " + listPoint.Count);
                                    //int _saveCount = 0;
                                    //sw.WriteLine("");
                                    //sw.WriteLine("dbPoint");
                                    for (int tint = 0; tint < listPoint.Count; tint++)
                                    {
                                        //string[] test = listPoint[tint].ToArray();
                                        //int tnum = 0;
                                        for (int r = 0; r < 3; r++)
                                        {
                                            _dbSaveData_[tint, r] = Double.Parse(listPoint[tint][r]);
                                            //Debug.Log("ToArray test : " + _dbSaveData_[_saveCount, tnum]);

                                        }
                                        //sw.WriteLine(_dbSaveData_[tint, 0] + " " + _dbSaveData_[tint, 1] + " " + _dbSaveData_[tint, 2]);

                                    }
                                    Debug.Log("arrPoint2 Length" + _dbSaveData_.Length);
                                    point_T.text = "Db->arr";
                                    n = 0;
                                    StartCoroutine(saveTxt(_newData_, _dbSaveData_, (field + afield + "aft" + collections)));
                                    //foreach(var SData in listPoint)
                                    //{
                                    //    _dbSaveData_[_saveCount,0] = SData.
                                    //}
                                    //test
                                    //double[,] _newData_ = new double[listPoint.Count, 3];
                                    //_saveCount = 0;
                                    //for (int tint = 0; tint < listPoint.Count; tint++)
                                    //{
                                    //    string[] test = listPoint[tint].ToArray();
                                    //    int tnum = 0;
                                    //    foreach (var t in test)
                                    //    {
                                    //        _newData_[_saveCount, tnum] = Double.Parse(t);
                                    //        //Debug.Log("ToArray test : " + _dbSaveData_[_saveCount, tnum]);
                                    //        tnum++;
                                    //    }
                                    //    _saveCount++;
                                    //}

                                    ////new
                                    //double[,] _newData_ = new double[arF.t_numParticle, 3];
                                    ////int _newCount = 0;
                                    //for (int _tNum = 0; _tNum < arF.t_numParticle; _tNum++)
                                    //{
                                    //    _newData_[_tNum, 0] = arF.m_Particles[_tNum].position.x;
                                    //    _newData_[_tNum, 1] = arF.m_Particles[_tNum].position.y;
                                    //    _newData_[_tNum, 2] = arF.m_Particles[_tNum].position.z;
                                    //}


                                    point_T.text = "before ICP:" + _dbSaveData_.Length + "compass:" + (com + j).ToString();
                                    Debug.Log(point_T.text);
                                    //icp호출 
                                    if (listPoint.Count != 0)
                                        _pointFit = pointSort(_dbSaveData_, _newData_, _dbSaveData_.Length, _newData_.Length);
                                    // sw.WriteLine("");
                                    // sw.WriteLine("ICP : "+ _pointFit);

                                    Debug.Log("ICPFit : " + _pointFit);
                                    //point_T.text = "after ICP" + _pointFit;
                                    Debug.Log(point_T.text);
                                    //fit에 따라서 chPoint=T로 ..
                                    if (_pointFit < 0.03 && _pointFit != 0)
                                        _chPoint = true;
                                    //data update
                                    //StartCoroutine(updateCorutine(collections, doc, _chPoint, _pointFit));


                                    ///업데이트 코루틴
                                    //DocumentReference uPRef = db.Collection(collections).Document(doc);
                                    //db.RunTransactionAsync(transaction =>
                                    //{
                                    //    return transaction.GetSnapshotAsync(uPRef).ContinueWith((snapshotTask) =>
                                    //    {
                                    //        DocumentSnapshot snapshot_ = snapshotTask.Result;
                                    //        //bool _chPoint_ = snapshot_.GetValue<bool>("checkPoint");
                                    //        Dictionary<string, object> updates = new Dictionary<string, object>
                                    //         {
                                    //            {"checkPoint",_chPoint },
                                    //            {"pointFitness",_pointFit }
                                    //         };
                                    //        transaction.Update(uPRef, updates);
                                    //        Debug.Log("Update"+_pointFit);
                                    //    });
                                    //});
                                    point_T.text = "icpStart // " + _dbSaveData_.Length + "\n"
                                            + "call // check : " + _chPoint.ToString() + ", fitness : " + _pointFit.ToString();

                                    //sw.Flush();
                                    //sw.Close();

                                    Debug.Log(point_T.text);
                                    Debug.Log("out" + listPoint.Count);
                                });
                            });

                            Debug.Log("last : " + listPoint.Count);
                            listPoint.Clear();
                            //Debug.Log("inout" + listPoint.Count);
                        }
                    }
               
                //Debug.Log("date"+listQuery.Count);
            });
            });
            //Debug.Log("outout" + listPoint.Count);
            //Debug.Log("gps"+listQuery.Count);
        }

        
        
        //sw.WriteLine("dat");


        
        yield break;
    }
    public Vector3 FindCenterPoint(Pose pose) //현재 pointcloud의 중심
    {
        float cx, cy, cz;
        int j;
        __centerPositionCH.x = 0;
        __centerPositionCH.y = 0;
        __centerPositionCH.z = 0;
        float x = 0, y = 0, z = 0;
        for (int i = 0; i < arF.t_numParticle; i++)
        {
            cx = pose.position.x - arF.m_Particles[i].position.x;
            cy = pose.position.y - arF.m_Particles[i].position.y;
            cz = pose.position.z - arF.m_Particles[i].position.z;

            if (cx < 0) cx *= -1;
            if (cy < 0) cy *= -1;
            if (cz < 0) cz *= -1;

            if (cx > 3 || cy > 3 || cz > 3)
                continue;
            x += arF.m_Particles[i].position.x;
            y += arF.m_Particles[i].position.y;
            z += arF.m_Particles[i].position.z;
        }
        for (j =0; j < arF.t_numParticle / 2.8; j++)
        {
            x += pose.position.x;
            y += pose.position.y;
            z += pose.position.y;
        }

        __centerPositionCH.x = x / (arF.t_numParticle + j);
        __centerPositionCH.y = y / (arF.t_numParticle + j);
        __centerPositionCH.z = z / (arF.t_numParticle + j);

        return new Vector3(__centerPositionCH.x, __centerPositionCH.y, __centerPositionCH.z);
    }
    private void CheckPosition()
    {
        GameObject[] whales;
        GameObject[] Uwhales;
        whales = GameObject.FindGameObjectsWithTag("newObj");
        Uwhales = GameObject.FindGameObjectsWithTag("updateObj");
        foreach (GameObject w in whales)
        {
            //Debug.Log("find obj");
            w.transform.Find("positionW").GetComponent<TextMesh>().text = "(x = " + Math.Round(w.transform.position.x, 3) + ", y = " + Math.Round(w.transform.position.y, 3) + ", y = " + Math.Round(w.transform.position.z, 3) + ")";
        }
        foreach (GameObject w in Uwhales)
        {
            //Debug.Log("find obj");
            w.transform.Find("positionW").GetComponent<TextMesh>().text = "(x = " + Math.Round(w.transform.position.x, 3) + ", y = " + Math.Round(w.transform.position.y, 3) + ", y = " + Math.Round(w.transform.position.z, 3) + ")";
        }
    }
    IEnumerator DBSave(Pose pose)
    //public void DBSave(Pose pose)
    {
        //int ID = 0; //임의로 0해둠


        var lat = GpsManager.current_Lat;
        lat = Math.Truncate(lat);
        var lng = GpsManager.current_Long;
        lng = Math.Truncate(lng);
        var com = CompassManager.magneticHeading;
        com = Mathf.Round(com);
        string collection = lat.ToString() + "x" + lng.ToString() + "a" + com.ToString();
        Input.location.Start();
        LocationInfo location = Input.location.lastData;

        point_T.text = collection;
        //var p_oint = PointTag.GetComponent<arF.ARPointCloudParticleVisualizer>().m_Particles;
        //var p_oint = PointTag.GetComponent<arF.ARPointCloudParticleVisualizer>().m_PointCloud;
        //var p_oint = new List<Vector3>();
        //var p_oints = visualizer.m_Particles;
        //var points = s_Vertices;
        //points.Clear();


        ArData data = new ArData
        {
            UserGPS = (GpsManager.current_Lat).ToString() + "x" + (GpsManager.current_Long).ToString(),
            Compass = CompassManager._trueHeading.ToString(),

            checkPoint = _chPoint,
            pointFitness = _pointFit,

            Gyro_w = GyroManager.GyroW,
            Gyro_x = GyroManager.GyroVector.x,
            Gyro_y = GyroManager.GyroVector.y,
            Gyro_z = GyroManager.GyroVector.z,
            Latitude = Math.Round(lat, 5),
            Longitude = Math.Round(lng, 5),

            arUseCheck = true,
            //testPoint = s_Vertices[0].x,

            //UserGPS = (location.latitude).ToString() + "x" + (location.longitude).ToString(),
            //Compass = Input.compass.magneticHeading.ToString(),
            //Gyro_w = Input.gyro.attitude.w,
            //Gyro_x = Input.gyro.attitude.x,
            //Gyro_y = Input.gyro.attitude.y,
            //Gyro_z = Input.gyro.attitude.z,
            //Latitude = Math.Round(location.latitude, 5),
            //Longitude = Math.Round(location.longitude, 5),

        };

        ObjSetting objproperty = new ObjSetting
        {
            updateCheck = 1,

            position_obj_x = pose.position.x,
            position_obj_y = pose.position.y,
            position_obj_z = pose.position.z,

            rotation_obj_w = pose.rotation.w,
            rotation_obj_x = pose.rotation.x,
            rotation_obj_y = pose.rotation.y,
            rotation_obj_z = pose.rotation.z,
        };


        PointCloudData p_data;

        Debug.Log("저장");
        int _pointCount = 0;
        point_T.text = "save";
        string saveTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        point_T.text = "s_time";
        DocumentReference documentReference = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime);
        point_T.text = "change1";
        DocumentReference documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("pointCloud").Document(_pointCount.ToString());
        point_T.text = "change2";

        documentReference.SetAsync(data).ContinueWithOnMainThread(task =>
        {

            Debug.Log("first");
        });
        point_T.text = "change3";

        documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("OBJ").Document(ID.ToString());
        documentReferenceSecond.SetAsync(objproperty).ContinueWithOnMainThread(task => { Debug.Log("first"); }); //obj 좌표 저장
        point_T.text = "change4";
        for (int i = 0; i < arF.t_numParticle; i++)
        {
            point_T.text = "visualizer.t_numParticle";
            _pointCount = i;
            documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("pointCloud").Document(_pointCount.ToString());
            p_data = new PointCloudData
            {
                FPoints_x = arF.m_Particles[i].position.x,
                FPoints_y = arF.m_Particles[i].position.y,
                FPoints_z = arF.m_Particles[i].position.z,
            };
            point_T.text = "change5";
            documentReferenceSecond.SetAsync(p_data).ContinueWithOnMainThread(task =>
            {
                Debug.Log("second");
            });
        }
        point_T.text = "save check";
        _chPoint = false;
        _pointFit = 0;
        yield break;
    }
    public void savePointCloud()
    {
        //isBtnDown = false;
        StartCoroutine(pointcloudSave());
    }
    IEnumerator pointcloudSave()
    {
        var lat = GpsManager.current_Lat;
        lat = Math.Truncate(lat);
        var lng = GpsManager.current_Long;
        lng = Math.Truncate(lng);
        var com = CompassManager.magneticHeading;
        com = Mathf.Round(com);
        string collection = lat.ToString() + "x" + lng.ToString() + InputPlace.text+ "a" + com.ToString();
        Input.location.Start();
        LocationInfo location = Input.location.lastData;
        
        point_T.text = collection;
        
        ArData data = new ArData
        {
            UserGPS = (GpsManager.current_Lat).ToString() + "x" + (GpsManager.current_Long).ToString(),
            Compass = CompassManager._trueHeading.ToString(),

            checkPoint = _chPoint,
            pointFitness = _pointFit,

            Gyro_w = GyroManager.GyroW,
            Gyro_x = GyroManager.GyroVector.x,
            Gyro_y = GyroManager.GyroVector.y,
            Gyro_z = GyroManager.GyroVector.z,
            Latitude = Math.Round(lat, 5),
            Longitude = Math.Round(lng, 5),

            arUseCheck = false,
           

        };


        PointCloudData p_data;

        Debug.Log("저장");
        int _pointCount = 0;
        point_T.text = "ptsave";
        string saveTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        point_T.text = "pts_time";
        DocumentReference documentReference = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime);
        point_T.text = "ptchange1";
        DocumentReference documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("pointCloud").Document(_pointCount.ToString());
        point_T.text = "ptchange2";

        documentReference.SetAsync(data).ContinueWithOnMainThread(task =>
        {

            Debug.Log("first");
        });
        point_T.text = "ptchange3";

        point_T.text = "ptchange4";
        for (int i = 0; i < arF.t_numParticle; i++)
        {
            point_T.text = "ptvisualizer.t_numParticle";
            _pointCount = i;
            documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("pointCloud").Document(_pointCount.ToString());
            p_data = new PointCloudData
            {
                FPoints_x = arF.m_Particles[i].position.x,
                FPoints_y = arF.m_Particles[i].position.y,
                FPoints_z = arF.m_Particles[i].position.z,
            };
            point_T.text = "ptchange5";
            documentReferenceSecond.SetAsync(p_data).ContinueWithOnMainThread(task =>
            {
                Debug.Log("second");
            });
        }
        point_T.text = "ptsave check";
        _chPoint = false;
        _pointFit = 0;
        yield break;
    }
    IEnumerator CenterDBSave(Pose pose, Vector3 vec3)
    {
        //int ID = 0; //임의로 0해둠
        float cx, cy, cz;

        var lat = GpsManager.current_Lat;
        lat = Math.Truncate(lat);
        var lng = GpsManager.current_Long;
        lng = Math.Truncate(lng);
        var com = CompassManager.magneticHeading;
        com = Mathf.Round(com);
        string collection = lat.ToString() + "x" + lng.ToString() + "a" + com.ToString();
        Input.location.Start();
        LocationInfo location = Input.location.lastData;
        
        //collection += InputPlace.text;
        point_T.text = collection;
        //var p_oint = PointTag.GetComponent<arF.ARPointCloudParticleVisualizer>().m_Particles;
        //var p_oint = PointTag.GetComponent<arF.ARPointCloudParticleVisualizer>().m_PointCloud;
        //var p_oint = new List<Vector3>();
        //var p_oints = visualizer.m_Particles;
        //var points = s_Vertices;
        //points.Clear();
        centerPoints centerPoints = new centerPoints
        {
            centerPointX = vec3.x,
            centerPointY = vec3.y,
            centerPointZ = vec3.z,
        };

        ArData data = new ArData
        {
            UserGPS = (GpsManager.current_Lat).ToString() + "x" + (GpsManager.current_Long).ToString(),
            Compass = CompassManager._trueHeading.ToString(),

            checkPoint = _chPoint,
            pointFitness = _pointFit,

            Gyro_w = GyroManager.GyroW,
            Gyro_x = GyroManager.GyroVector.x,
            Gyro_y = GyroManager.GyroVector.y,
            Gyro_z = GyroManager.GyroVector.z,
            Latitude = Math.Round(lat, 5),
            Longitude = Math.Round(lng, 5),

            arUseCheck = true,
            //testPoint = s_Vertices[0].x,

            //UserGPS = (location.latitude).ToString() + "x" + (location.longitude).ToString(),
            //Compass = Input.compass.magneticHeading.ToString(),
            //Gyro_w = Input.gyro.attitude.w,
            //Gyro_x = Input.gyro.attitude.x,
            //Gyro_y = Input.gyro.attitude.y,
            //Gyro_z = Input.gyro.attitude.z,
            //Latitude = Math.Round(location.latitude, 5),
            //Longitude = Math.Round(location.longitude, 5),

        };

        ObjSetting objproperty = new ObjSetting
        {
            updateCheck = 1,

            position_obj_x = pose.position.x,
            position_obj_y = pose.position.y,
            position_obj_z = pose.position.z,

            rotation_obj_w = pose.rotation.w,
            rotation_obj_x = pose.rotation.x,
            rotation_obj_y = pose.rotation.y,
            rotation_obj_z = pose.rotation.z,
        };


        PointCloudData p_data;

        Debug.Log("저장");
        int _pointCount = 0;
        point_T.text = "save";
        string saveTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        point_T.text = "s_time";
        DocumentReference documentReference = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime);
        point_T.text = "change1";
        DocumentReference documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("pointCloud").Document(_pointCount.ToString());
        point_T.text = "change2";

        documentReference.SetAsync(data).ContinueWithOnMainThread(task =>
        {

            Debug.Log("first");
        });
        point_T.text = "change3";

        documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("CenterPoint").Document(ID.ToString());
        documentReferenceSecond.SetAsync(centerPoints).ContinueWithOnMainThread(taskc => { Debug.Log("center"); }); //중심점 좌표 저장

        documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("OBJ").Document(ID.ToString());
        documentReferenceSecond.SetAsync(objproperty).ContinueWithOnMainThread(task => { Debug.Log("first"); }); //obj 좌표 저장
        point_T.text = "change4";
        for (int i = 0; i < arF.t_numParticle; i++)
        {
            cx = pose.position.x - arF.m_Particles[i].position.x;
            cy = pose.position.y - arF.m_Particles[i].position.y;
            cz = pose.position.z - arF.m_Particles[i].position.z;

            if (cx < 0) cx *= -1;
            if (cy < 0) cy *= -1;
            if (cz < 0) cz *= -1;

            if (cx > 3 || cy > 3 || cz > 3)
                continue;

            point_T.text = "visualizer.t_numParticle";

            _pointCount = i;
            documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("pointCloud").Document(_pointCount.ToString());
            p_data = new PointCloudData
            {
                FPoints_x = arF.m_Particles[i].position.x,
                FPoints_y = arF.m_Particles[i].position.y,
                FPoints_z = arF.m_Particles[i].position.z,
            };
            point_T.text = "change5";
            documentReferenceSecond.SetAsync(p_data).ContinueWithOnMainThread(task =>
            {
                Debug.Log("second");
            });
        }
        point_T.text = "save check";
        _chPoint = false;
        _pointFit = 0;
        yield break;
    }
    public void startCO()
    {
        StartCoroutine(DBObjUpdate());
        
    }
    IEnumerator startDBCorutine(int GPSx,int GPSy, int values)
    {
        string collectionds= GPSx.ToString()+"x"+ GPSy.ToString();
        for(int i = 0; i <= 360; i++)
        {
            string collectiond = collectionds+"a"+i.ToString();

            CollectionReference firsRef = db.Collection("Pointcloud").Document("1").Collection(collectiond);

            Query firRef = firsRef;
            firRef.GetSnapshotAsync().ContinueWithOnMainThread(tak =>
            {
                QuerySnapshot allsecSnap = tak.Result;

                foreach (DocumentSnapshot document in allsecSnap.Documents)
                {
                    string docs = document.Id.ToString();
                   //Query querys = db.Collection(collectiond).Document(docs).Collection("OBJ");

                    DocumentReference uPRefs = db.Collection("Pointcloud").Document("1").Collection(collectiond).Document(docs).Collection("OBJ").Document("0");
                    db.RunTransactionAsync(transaction =>
                    {
                        point_T.text = "Start reset";
                        return transaction.GetSnapshotAsync(uPRefs).ContinueWith((snapshotTask) =>
                        {
                            DocumentSnapshot snapshot_ = snapshotTask.Result;
                            //bool _chPoint_ = snapshot_.GetValue<bool>("checkPoint");
                            Dictionary<string, object> updates = new Dictionary<string, object>
                                 {
                                    {"updateCheck", 0}
                                 };
                            transaction.Update(uPRefs, updates);
                            Debug.Log("UpdateCorutine : " + _pointFit);
                        });
                    });
                }
            });

        }

       // isBtnDown = true;
        yield break;
    }

    IEnumerator DBObjUpdate()
    {
        var lat = GpsManager.current_Lat;
        lat = Math.Truncate(lat);
        //lat = 37;
        var lng = GpsManager.current_Long;
        lng = Math.Truncate(lng);
        //lng = 126;
        var com = CompassManager.magneticHeading;
        com = Mathf.Round(com);
        //com = 200;
        string collection = lat.ToString() + "x" + lng.ToString();
    


        for (int j = -10; j <= 10; j++)
        {
            point_T.text = "jjj";
            listPoint.Clear();

            //주변 각도 찾기
            var collections = collection;
            float compasss = com + j;
            collections += "a" + (com + j).ToString();
            Debug.Log(collections.ToString());

            //Find pointcloud
            CollectionReference firstRef = db.Collection("Pointcloud").Document("1").Collection(collections);
            
            Query allsecondQuery = firstRef;
            allsecondQuery.GetSnapshotAsync().ContinueWithOnMainThread(tasks =>
            {
                QuerySnapshot allsecondQuerySnapshots = tasks.Result;
                point_T.text = "collection";

                foreach (DocumentSnapshot documentSnapshot in allsecondQuerySnapshots.Documents)
                {
                    string docs = documentSnapshot.Id.ToString();
                    Debug.Log(docs);
                    Query listquerys = db.Collection("Pointcloud").Document("1").Collection(collections).Document(docs).Collection("OBJ");
                    listquerys.GetSnapshotAsync().ContinueWithOnMainThread(taskL =>
                    {
                        Pose updatePose = new Pose();
                        point_T.text = "oBJ";
                        listPoint.Clear();
                        QuerySnapshot documentSnapshots = taskL.Result;
                        foreach (DocumentSnapshot listSnapshots in documentSnapshots.Documents)
                        {
                            ObjSetting objset = listSnapshots.ConvertTo<ObjSetting>();
                            point_T.text = "oBJ in";
                            Debug.Log("in" );
                            int types = 1;
                            string docu = listSnapshots.Id.ToString();
                            List<string> listpoints = new List<string>();
                            foreach (KeyValuePair<string, object> pair in listSnapshots.ToDictionary())
                            {
                                Debug.Log("indb"+ pair.Value.ToString());
                                listpoints.Add(pair.Value.ToString());
                                // if (pair.Key.CompareTo("position_obj_x")==1) //다른것도 고치기
                                //    updatePose.position.x = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.CompareTo("position_obj_y") == 1)
                                //    updatePose.position.y = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.CompareTo("position_obj_z") == 1)
                                //    updatePose.position.z = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.CompareTo("rotation_obj_x") == 1)
                                //    updatePose.rotation.x = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.CompareTo("rotation_obj_y") == 1)
                                //    updatePose.rotation.y = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.CompareTo("rotation_obj_z") == 1)
                                //    updatePose.rotation.z = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.CompareTo("rotation_obj_w") == 1)
                                //    updatePose.rotation.w = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.Equals("updateCheck"))
                                //    types = int.Parse(pair.Key);
                                //else
                                //    Debug.Log("none");
                                //if (pair.Key.Equals("position_obj_x"))
                                //    updatePose.position.x = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.Equals("position_obj_y"))
                                //    updatePose.position.y = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.Equals("position_obj_z"))
                                //    updatePose.position.z = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.Equals("rotation_obj_x"))
                                //    updatePose.rotation.x = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.Equals("rotation_obj_y"))
                                //    updatePose.rotation.y = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.Equals("rotation_obj_z"))
                                //    updatePose.rotation.z = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.Equals("rotation_obj_w"))
                                //    updatePose.rotation.w = float.Parse(pair.Key.ToString());
                                //else if (pair.Key.Equals("updateCheck"))
                                //    types = int.Parse(pair.Key);
                                //else
                                //    Debug.Log("none");
                                Debug.Log("indbss");

                            }
                            Debug.Log("outdb");
                            //point_T.text = "obj out before"+ listpoints[7];
                            Debug.Log("in" + listpoints.Count);
                            Debug.Log("in" + updatePose.position.x);
                            Debug.Log("in" + updatePose.position.y);
                            Debug.Log("in" + updatePose.position.z);
                            Debug.Log("in" + types);
                            if (objset.updateCheck==0)//(int.Parse(listpoints[7])==0)//(types==0) //변수 수정필요 z가 여기로 들어오는거같음.. 이말은.. 전체적으로제대로 좌표를 안넣기도 한다는거.ㅎ
                            {
                                point_T.text = "obj out";

                                updatePose.position.x = objset.position_obj_x;
                                updatePose.position.y = objset.position_obj_y;
                                updatePose.position.z = objset.position_obj_z;

                                updatePose.rotation.w = objset.rotation_obj_w;
                                updatePose.rotation.x = objset.rotation_obj_x;
                                updatePose.rotation.y = objset.rotation_obj_y;
                                updatePose.rotation.z = objset.rotation_obj_z;

                                //updatePose.position.x = float.Parse(listpoints[0]);
                                //updatePose.position.y = float.Parse(listpoints[1]);
                                //updatePose.position.z = float.Parse(listpoints[2]);

                                //updatePose.rotation.w = float.Parse(listpoints[3]);
                                //updatePose.rotation.x = float.Parse(listpoints[4]);
                                //updatePose.rotation.y = float.Parse(listpoints[5]);
                                //updatePose.rotation.z = float.Parse(listpoints[6]);
                                Instantiate(ARUpdateObj, updatePose.position, Quaternion.identity);


                                DocumentReference uPRefsd = db.Collection("Pointcloud").Document("1").Collection(collections).Document(docs).Collection("OBJ").Document("0");
                                db.RunTransactionAsync(transaction =>
                                {
                                    return transaction.GetSnapshotAsync(uPRefsd).ContinueWith((snapshotTask) =>
                                    {
                                        DocumentSnapshot snapshot_ = snapshotTask.Result;
                                        //bool _chPoint_ = snapshot_.GetValue<bool>("checkPoint");
                                        Dictionary<string, object> updates = new Dictionary<string, object>
                                 {
                                    {"updateCheck", 2 }
                                 };
                                        transaction.Update(uPRefsd, updates);
                                        Debug.Log("UpdateCorutine : " + _pointFit);
                                    });
                                });
                            }
                            
                        }
                    });

                }
            });
        }
        //true인것들 추가

        //오브젝트 위치 파악후증강
        point_T.text = "wait before";
        yield return new WaitForSeconds(10);
        StartCoroutine(DBObjUpdate());
    }

    IEnumerator DBCenterObjUpdate()
    {
        var lat = GpsManager.current_Lat;
        lat = Math.Truncate(lat);
        //lat = 37;
        var lng = GpsManager.current_Long;
        lng = Math.Truncate(lng);
        //lng = 126;
        var com = CompassManager.magneticHeading;
        com = Mathf.Round(com);
        //com = 200;
        string collection = lat.ToString() + "x" + lng.ToString();



        for (int j = -10; j <= 10; j++)
        {
            point_T.text = "jjj";
            listPoint.Clear();

            //주변 각도 찾기
            var collections = collection;
            float compasss = com + j;
            collections += "a" + (com + j).ToString();
            Debug.Log(collections.ToString());

            //Find pointcloud
            CollectionReference firstRef = db.Collection("Pointcloud").Document("1").Collection(collections);

            Query allsecondQuery = firstRef;
            allsecondQuery.GetSnapshotAsync().ContinueWithOnMainThread(tasks =>
            {
                QuerySnapshot allsecondQuerySnapshots = tasks.Result;
                point_T.text = "collection";

                foreach (DocumentSnapshot documentSnapshot in allsecondQuerySnapshots.Documents)
                {
                    string docs = documentSnapshot.Id.ToString();
                    Debug.Log(docs);
                    Query listquerys = db.Collection("Pointcloud").Document("1").Collection(collections).Document(docs).Collection("CenterPoint");
                    listquerys.GetSnapshotAsync().ContinueWithOnMainThread(taskL =>
                    {
                        Pose updatePose = new Pose();
                        point_T.text = "oBJ";
                        listPoint.Clear();
                        QuerySnapshot documentSnapshots = taskL.Result;
                        foreach (DocumentSnapshot listSnapshots in documentSnapshots.Documents)
                        {
                            ObjSetting objset = listSnapshots.ConvertTo<ObjSetting>();
                            centerPoints objcen = listSnapshots.ConvertTo<centerPoints>();
                            point_T.text = "oBJ in";
                            Debug.Log("in");
                            int types = 1;
                            string docu = listSnapshots.Id.ToString();
                            List<string> listpoints = new List<string>();

                            //(int.Parse(listpoints[7])==0)//(types==0) //변수 수정필요 z가 여기로 들어오는거같음.. 이말은.. 전체적으로제대로 좌표를 안넣기도 한다는거.ㅎ

                            point_T.text = "obj out";

                            Instantiate(ARUpdateObj, new Vector3(objcen.centerPointX, objcen.centerPointY, objcen.centerPointZ), Quaternion.identity);


                            DocumentReference uPRefsd = db.Collection("Pointcloud").Document("1").Collection(collections).Document(docs).Collection("OBJ").Document("0");
                            db.RunTransactionAsync(transaction =>
                            {
                                return transaction.GetSnapshotAsync(uPRefsd).ContinueWith((snapshotTask) =>
                                {
                                    DocumentSnapshot snapshot_ = snapshotTask.Result;
                                        //bool _chPoint_ = snapshot_.GetValue<bool>("checkPoint");
                                        Dictionary<string, object> updates = new Dictionary<string, object>
                             {
                                    {"updateCheck", 2 }
                             };
                                    transaction.Update(uPRefsd, updates);
                                    Debug.Log("UpdateCorutine : " + _pointFit);
                                });
                            });


                        }
                    });

                }
            });
        }
        //true인것들 추가

        //오브젝트 위치 파악후증강
        point_T.text = "wait before";
        yield return new WaitForSeconds(10);
        StartCoroutine(DBObjUpdate());
    }

    public void DBCenterObj_Update()
    {
        var lat = GpsManager.current_Lat;
        lat = Math.Truncate(lat);
        //lat = 37;
        var lng = GpsManager.current_Long;
        lng = Math.Truncate(lng);
        //lng = 126;
        var com = CompassManager.magneticHeading;
        com = Mathf.Round(com);
        //com = 200;
        string collection = lat.ToString() + "x" + lng.ToString();



        for (int j = -10; j <= 10; j++)
        {
            point_T.text = "jjj";
            listPoint.Clear();

            //주변 각도 찾기
            var collections = collection;
            float compasss = com + j;
            collections += "a" + (com + j).ToString();
            Debug.Log(collections.ToString());

            //Find pointcloud
            CollectionReference firstRef = db.Collection("Pointcloud").Document("1").Collection(collections);

            Query allsecondQuery = firstRef;
            allsecondQuery.GetSnapshotAsync().ContinueWithOnMainThread(tasks =>
            {
                QuerySnapshot allsecondQuerySnapshots = tasks.Result;
                point_T.text = "collection";

                foreach (DocumentSnapshot documentSnapshot in allsecondQuerySnapshots.Documents)
                {
                    string docs = documentSnapshot.Id.ToString();
                    Debug.Log(docs);
                    Query listquerys = db.Collection("Pointcloud").Document("1").Collection(collections).Document(docs).Collection("CenterPoint");
                    listquerys.GetSnapshotAsync().ContinueWithOnMainThread(taskL =>
                    {
                        Pose updatePose = new Pose();
                        point_T.text = "oBJ";
                        listPoint.Clear();
                        QuerySnapshot documentSnapshots = taskL.Result;
                        foreach (DocumentSnapshot listSnapshots in documentSnapshots.Documents)
                        {
                            ObjSetting objset = listSnapshots.ConvertTo<ObjSetting>();
                            centerPoints objcen = listSnapshots.ConvertTo<centerPoints>();
                            point_T.text = "oBJ in";
                            Debug.Log("in");
                            int types = 1;
                            string docu = listSnapshots.Id.ToString();
                            List<string> listpoints = new List<string>();

                            if (objset.updateCheck == 0)//(int.Parse(listpoints[7])==0)//(types==0) //변수 수정필요 z가 여기로 들어오는거같음.. 이말은.. 전체적으로제대로 좌표를 안넣기도 한다는거.ㅎ
                            {
                                point_T.text = "obj out";

                                Instantiate(ARUpdateObj, new Vector3(objcen.centerPointX, objcen.centerPointY, objcen.centerPointZ), Quaternion.identity);


                                DocumentReference uPRefsd = db.Collection("Pointcloud").Document("1").Collection(collections).Document(docs).Collection("OBJ").Document("0");
                                db.RunTransactionAsync(transaction =>
                                {
                                    return transaction.GetSnapshotAsync(uPRefsd).ContinueWith((snapshotTask) =>
                                    {
                                        DocumentSnapshot snapshot_ = snapshotTask.Result;
                                        //bool _chPoint_ = snapshot_.GetValue<bool>("checkPoint");
                                        Dictionary<string, object> updates = new Dictionary<string, object>
                                 {
                                    {"updateCheck", 2 }
                                 };
                                        transaction.Update(uPRefsd, updates);
                                        Debug.Log("UpdateCorutine : " + _pointFit);
                                    });
                                });
                            }

                        }
                    });

                }
            });
        }
        //true인것들 추가

        //오브젝트 위치 파악후증강
        point_T.text = "query wait before";
    }

    IEnumerator CenterPosition()
    {
        float x=0, y=0, z=0;
        for(int i=0; i<arF.t_numParticle; i++)
        {
            x += arF.m_Particles[i].position.x;
            y += arF.m_Particles[i].position.y;
            z += arF.m_Particles[i].position.z;
        }

        __centerPositionCH.x = x/ arF.t_numParticle;
        __centerPositionCH.y = y/ arF.t_numParticle;
        __centerPositionCH.z = z/ arF.t_numParticle;

        yield break;
    }
    IEnumerator CenterDBPosition() //1초마다 계산..? 그치 그래야 다시 왔을때의 증강시키는 거니까..
    {
        var check = false;
        var lat = GpsManager.current_Lat;
        lat = Math.Truncate(lat);
        lat = 37;
        var lng = GpsManager.current_Long;
        lng = Math.Truncate(lng);
        lng = 126;
        var com = CompassManager.magneticHeading;
        com = Mathf.Round(com);
        com = 57;
        string collection = lat.ToString() + "x" + lng.ToString();


        float x = 0, y = 0, z = 0;
        

        __centerPositionCH.x = x / 3;
        __centerPositionCH.y = y / 3;
        __centerPositionCH.z = z / 3;

        yield break;
    }
    IEnumerator saveDataCorutine()
    {
        yield break;
    }
    IEnumerator updateCorutine(string collections, string doc,bool _chPoint, double _pointFit)
    {
        DocumentReference uPRef = db.Collection("Pointcloud").Document("1").Collection(collections).Document(doc);
        db.RunTransactionAsync(transaction =>
        {
            return transaction.GetSnapshotAsync(uPRef).ContinueWith((snapshotTask) =>
            {
                DocumentSnapshot snapshot_ = snapshotTask.Result;
                //bool _chPoint_ = snapshot_.GetValue<bool>("checkPoint");
                Dictionary<string, object> updates = new Dictionary<string, object>
                                 {
                                    {"checkPoint",_chPoint },
                                    {"pointFitness",_pointFit }
                                 };
                transaction.Update(uPRef, updates);
                Debug.Log("UpdateCorutine : " + _pointFit);
            });
        });
        yield break;
    }
    public void SaveData()
    {
        var lat = GpsManager.current_Lat;
        lat = Math.Truncate(lat);
        var lng = GpsManager.current_Long;
        lng = Math.Truncate(lng);
        var com = CompassManager.magneticHeading;
        com = Mathf.Round(com);
        string collection = lat.ToString() + "x" + lng.ToString() + "a" + com.ToString();
        Input.location.Start();
        LocationInfo location = Input.location.lastData;
        //var p_oint = PointTag.GetComponent<arF.ARPointCloudParticleVisualizer>().m_Particles;
        //var p_oint = PointTag.GetComponent<arF.ARPointCloudParticleVisualizer>().m_PointCloud;
        //var p_oint = new List<Vector3>();
        //var p_oints = visualizer.m_Particles;
        //var points = s_Vertices;
        //points.Clear();


        ArData data = new ArData
        {
            UserGPS = (GpsManager.current_Lat).ToString() + "x" + (GpsManager.current_Long).ToString(),
            Compass = CompassManager._trueHeading.ToString(),

            checkPoint = _chPoint,
            pointFitness = _pointFit,

            Gyro_w = GyroManager.GyroW,
            Gyro_x = GyroManager.GyroVector.x,
            Gyro_y = GyroManager.GyroVector.y,
            Gyro_z = GyroManager.GyroVector.z,
            Latitude = Math.Round(lat, 5),
            Longitude = Math.Round(lng, 5),

            //testPoint = s_Vertices[0].x,

            //UserGPS = (location.latitude).ToString() + "x" + (location.longitude).ToString(),
            //Compass = Input.compass.magneticHeading.ToString(),
            //Gyro_w = Input.gyro.attitude.w,
            //Gyro_x = Input.gyro.attitude.x,
            //Gyro_y = Input.gyro.attitude.y,
            //Gyro_z = Input.gyro.attitude.z,
            //Latitude = Math.Round(location.latitude, 5),
            //Longitude = Math.Round(location.longitude, 5),

        };
       

        PointCloudData p_data;

        Debug.Log("저장");
        int _pointCount = 0;
        point_T.text = "save";
        string saveTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        DocumentReference documentReference = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime);
        point_T.text = "change1";
        DocumentReference documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(saveTime).Collection("pointCloud").Document(_pointCount.ToString());
        point_T.text = "change2";

        documentReference.SetAsync(data).ContinueWithOnMainThread(task =>
        {
            
            Debug.Log("first");
        });
        point_T.text = "change3";

        for (int i = 0; i < arF.t_numParticle; i++)
        {
            point_T.text = "visualizer.t_numParticle";
            _pointCount = i;
            documentReferenceSecond = db.Collection("Pointcloud").Document("1").Collection(collection).Document(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")).Collection("pointCloud").Document(_pointCount.ToString());
            p_data = new PointCloudData
            {
                FPoints_x = arF.m_Particles[i].position.x,
                FPoints_y = arF.m_Particles[i].position.y,
                FPoints_z = arF.m_Particles[i].position.z,
            };
            documentReferenceSecond.SetAsync(p_data).ContinueWithOnMainThread(task =>
            {
                Debug.Log("second");
            });
        }
        point_T.text = "save // check : "+_chPoint.ToString() + ", fitness : " + _pointFit.ToString();
        _chPoint = false;
        _pointFit = 0;
        //for (int i = 0; i < s_Vertices.Count; i++)
        //{
        //    point_T.text = "change4";
        //    _pointCount = i;
        //    documentReferenceSecond = db.Collection(collection).Document(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")).Collection("pointCloud").Document(_pointCount.ToString());
        //    p_data = new PointCloudData
        //    {
        //        FPoints_x = s_Vertices[i].x,
        //        FPoints_y = s_Vertices[i].y,
        //        FPoints_z = s_Vertices[i].z,
        //    };
        //    documentReferenceSecond.SetAsync(p_data).ContinueWithOnMainThread(task =>
        //    {
        //        Debug.Log("second");
        //    });
        //}


        //db.Collection(collection).AddAsync(data).ContinueWithOnMainThread(task =>
        //{
        //    //DocumentReference addedDocRef = task.Result;
        //    Debug.Log("success");
        //    for (int i = 0; i < s_Vertices.Count; i++)
        //    {
        //        p_data = new PointCloudData
        //        {
        //            FPoints_x = s_Vertices[i].x,
        //            FPoints_y = s_Vertices[i].y,
        //            FPoints_z = s_Vertices[i].z,
        //        };
        //        db.Collection("pointCloud").Document(i.ToString()).SetAsync(p_data).ContinueWithOnMainThread(tasks =>
        //        {
        //            DocumentReference addedDocRefs = db.Collection("pointCloud").Document(i.ToString());
        //            Debug.Log("success");
        //        });
        //    }
        //});

        //db.Collection(collection).AddAsync(data).ContinueWithOnMainThread(task =>
        //{
        //        //DocumentReference addedDocRef = task.Result;
        //        Debug.Log("success");
        //    for (int i = 0; i <s_Vertices.Count; i++)
        //    {
        //        p_data = new PointCloudData
        //        {
        //            FPoints_x = s_Vertices[i].x,
        //            FPoints_y =s_Vertices[i].y,
        //            FPoints_z = s_Vertices[i].z,
        //        };
        //        db.Collection("pointCloud").Document(i.ToString()).SetAsync(p_data).ContinueWithOnMainThread(tasks =>
        //        {
        //            DocumentReference addedDocRefs = db.Collection("pointCloud").Document(i.ToString());
        //            Debug.Log("success");
        //        });
        //    }
        //});

        //for (int i = 0; i < p_oint.Length; i++)
        //{
        //    p_data = new PointCloudData
        //    {
        //        FPoints_x = p_oint[i].position.x,
        //        FPoints_y = p_oint[i].position.y,
        //        FPoints_z = p_oint[i].position.z,
        //    };
        //    db.Collection(collection).AddAsync(data).Collection("pointCloud").Document(i.ToString()).SetAsync(p_data).ContinueWithOnMainThread(task =>
        //    {
        //        DocumentReference addedDocRef = task.Result;
        //        Debug.Log("success");
        //    });
        //}

    }

    public void QueryData()
    {
        listPoint.Clear();
        var check = false;
        var lat = GpsManager.current_Lat;
        lat = Math.Truncate(lat);
        //lat = 37;
        var lng = GpsManager.current_Long;
        lng = Math.Truncate(lng);
        //lng = 126;
        var com = CompassManager.magneticHeading;
        com = Mathf.Round(com);
        //com = 57;
        string collection = lat.ToString() + "x" + lng.ToString();// + "a" + com.ToString();


        ////After remove
        //string orignCollect = collection + "a" + com.ToString();
        //db.Collection(orignCollect).GetSnapshotAsync().ContinueWithOnMainThread(taskj =>
        //{
        //    QuerySnapshot orignQuerySnapshots = taskj.Result;
        //    foreach (DocumentSnapshot doc in orignQuerySnapshots.Documents)
        //    {
        //        string orignDoc = doc.Id.ToString();
        //        Query orignquery = db.Collection(orignCollect).Document(orignDoc).Collection("pointCloud");
        //        orignquery.GetSnapshotAsync().ContinueWithOnMainThread(taskOa =>
        //        {
        //            QuerySnapshot documentSnapshots = taskOa.Result;

        //            foreach (DocumentSnapshot listSnapshots in documentSnapshots.Documents)
        //            {
        //                string docu = listSnapshots.Id.ToString();
        //                List<string> listpoints = new List<string>();
        //                foreach (KeyValuePair<string, object> pair in listSnapshots.ToDictionary())
        //                {
        //                    listpoints.Add(pair.Value.ToString());
        //                }
        //                Debug.Log("inOrign" + listpoints.Count);
        //                listPointO.Add(listpoints);

        //            }
        //        });
        //    }
        //});

        //double[,] _newData_ = new double[listPointO.Count, 3];
        ////int _newCount = 0;
        //for (int _tNum = 0; _tNum < listPointO.Count; _tNum++)
        //{
        //    _newData_[_tNum, 0] = Double.Parse(listPointO[_tNum][0]);
        //    _newData_[_tNum, 1] = Double.Parse(listPointO[_tNum][1]);
        //    _newData_[_tNum, 2] = Double.Parse(listPointO[_tNum][2]);
        //}
        //Debug.Log("newData : " + _newData_.Length);
        //Debug.Log("newDatalist : " + listPointO.Count);
        
        for (int j = -10;j<10; j++)
        {
            listPoint.Clear();
            
            //주변 각도 찾기
            var collections = collection;
            float compasss = com + j;
            collections += "a" + (com + j).ToString();
            Debug.Log(collections.ToString());
            
            CollectionReference firstRef = db.Collection(collections);
            Query allfirstQuery = firstRef;
            allfirstQuery.GetSnapshotAsync().ContinueWithOnMainThread(tasks =>
            {
                Debug.Log(collections.ToString()+" // ch");
                QuerySnapshot allfirstQuerySnapshots = tasks.Result;
                
                
                foreach(DocumentSnapshot documentSnapshot in allfirstQuerySnapshots.Documents)
                {
                    string doc = documentSnapshot.Id.ToString();
                    Debug.Log(doc);
                    Query listquery = db.Collection(collections).Document(doc).Collection("pointCloud");
                    listquery.GetSnapshotAsync().ContinueWithOnMainThread(taskL =>
                    {
                        listPoint.Clear();
                        QuerySnapshot documentSnapshots = taskL.Result;
                        
                        foreach(DocumentSnapshot listSnapshots in documentSnapshots.Documents)
                        {
                            string docu = listSnapshots.Id.ToString();
                            List<string> listpoints = new List<string>();
                            foreach(KeyValuePair<string, object> pair in listSnapshots.ToDictionary())
                            {
                                listpoints.Add(pair.Value.ToString());
                                Debug.Log("test value"+pair.Value.ToString());
                            }
                            Debug.Log("in" + listpoints.Count);
                            listPoint.Add(listpoints);
                            
                        }
                        //list->array
                        //db
                        double[,] _dbSaveData_ = new double[listPoint.Count, 3] ;
                        _dbSaveData_.Initialize();
                
                        Debug.Log("arrPoint" + _dbSaveData_.Length);
                        Debug.Log("listPoint" + listPoint.Count);
                        //int _saveCount = 0;
                        for (int tint=0; tint<listPoint.Count; tint++)
                        {
                            //string[] test = listPoint[tint].ToArray();
                            //int tnum = 0;
                            for(int r=0;r<3;r++)
                            {
                                _dbSaveData_[tint, r] = Double.Parse(listPoint[tint][r]);
                                //Debug.Log("ToArray test : " + _dbSaveData_[_saveCount, tnum]);
                             
                            }
                            
                        }
                        //Debug.Log("arrPoint2" + _dbSaveData_.Length);
                        point_T.text = "Db->arr";
                        //foreach(var SData in listPoint)
                        //{
                        //    _dbSaveData_[_saveCount,0] = SData.
                        //}
                        //test
                        //double[,] _newData_ = new double[listPoint.Count, 3];
                        //_saveCount = 0;
                        //for (int tint = 0; tint < listPoint.Count; tint++)
                        //{
                        //    string[] test = listPoint[tint].ToArray();
                        //    int tnum = 0;
                        //    foreach (var t in test)
                        //    {
                        //        _newData_[_saveCount, tnum] = Double.Parse(t);
                        //        //Debug.Log("ToArray test : " + _dbSaveData_[_saveCount, tnum]);
                        //        tnum++;
                        //    }
                        //    _saveCount++;
                        //}

                        ////new
                        //double[,] _newData_ = new double[arF.t_numParticle, 3];
                        ////int _newCount = 0;
                        //for (int _tNum = 0; _tNum < arF.t_numParticle; _tNum++)
                        //{
                        //    _newData_[_tNum, 0] = arF.m_Particles[_tNum].position.x;
                        //    _newData_[_tNum, 1] = arF.m_Particles[_tNum].position.y;
                        //    _newData_[_tNum, 2] = arF.m_Particles[_tNum].position.z;
                        //}
                        _newData_ = new double[listPointO.Count, 3];
                        //int _newCount = 0;
                        for (int _tNum = 0; _tNum < listPointO.Count; _tNum++)
                        {
                            _newData_[_tNum, 0] = Double.Parse(listPointO[_tNum][0]);
                            _newData_[_tNum, 1] = Double.Parse(listPointO[_tNum][1]);
                            _newData_[_tNum, 2] = Double.Parse(listPointO[_tNum][2]);
                        }
                        Debug.Log("newData : " + _newData_.Length);
                        Debug.Log("newDatalist : " + listPointO.Count);

                        point_T.text = "before ICP:"+ _dbSaveData_.Length+"compass:"+(com+j).ToString();
                        Debug.Log(point_T.text);
                        //icp호출 
                        if(listPoint.Count!=0)
                            _pointFit = pointSort(_dbSaveData_, _newData_, _dbSaveData_.Length, _newData_.Length);

                        point_T.text = "after ICP" + _pointFit;
                        Debug.Log(point_T.text);
                        //fit에 따라서 chPoint=T로 ..
                        if (_pointFit < 0.03 && _pointFit != 0)
                            _chPoint = true;
                        //data update
                        StartCoroutine(updateCorutine(collections,doc,_chPoint,_pointFit));
                        ///업데이트 코루틴
                        //DocumentReference uPRef = db.Collection(collections).Document(doc);
                        //db.RunTransactionAsync(transaction =>
                        //{
                        //    return transaction.GetSnapshotAsync(uPRef).ContinueWith((snapshotTask) =>
                        //    {
                        //        DocumentSnapshot snapshot_ = snapshotTask.Result;
                        //        //bool _chPoint_ = snapshot_.GetValue<bool>("checkPoint");
                        //        Dictionary<string, object> updates = new Dictionary<string, object>
                        //         {
                        //            {"checkPoint",_chPoint },
                        //            {"pointFitness",_pointFit }
                        //         };
                        //        transaction.Update(uPRef, updates);
                        //        Debug.Log("Update"+_pointFit);
                        //    });
                        //});
                        point_T.text = "icpStart // "+ _dbSaveData_.Length + "\n"
                        + "call // check : " + _chPoint.ToString() + ", fitness : " + _pointFit.ToString();
                        
                        
                        Debug.Log(point_T.text);
                        Debug.Log("out" + listPoint.Count);
                    });
                    listPoint.Clear();
                    //Debug.Log("inout" + listPoint.Count);
                }
                //Debug.Log("date"+listQuery.Count);
            });
            //Debug.Log("outout" + listPoint.Count);
            //Debug.Log("gps"+listQuery.Count);
        }
        
        //Debug.Log("outout" + listPoint.Count);
        //listQuery.Add("1");
        Debug.Log("new");
        //for (int i = 0; i <(listQuery.Count/2+1);i--)
        for (int i = 0; i <2;i--)
        {
            i++;

            i++;
            Debug.Log("yqp");
            //Debug.Log(listQuery.Count);
            //Debug.Log(listQuery[1]);
            //Query secondPointQuery = db.Collection(listQuery[i++]).Document(listQuery[i++]).Collection("pointCloud");
            //secondPointQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            //{
            //    Debug.Log(secondPointQuery.ToString() + " // da");
            //    QuerySnapshot allsecondQuerySnapshots = task.Result;

            //    foreach (DocumentSnapshot documentSnapshot in allsecondQuerySnapshots.Documents)
            //    {
            //        string doc = documentSnapshot.Id.ToString();
            //        //pointcloud list구조체.->vertex
                
            //        Debug.Log(String.Format("pointcloud data for {0} document:", documentSnapshot.Id));

            //        Dictionary<string, object> pointDict = documentSnapshot.ToDictionary();
            //        foreach(KeyValuePair<string,object> pair in pointDict)
            //        {
            //            //Debug.Log(String.Format("x : {0}, y: {1}, z: {2}", ))
            //            Debug.Log(String.Format("{0} // {1}: {2}", documentSnapshot.Id, pair.Key, pair.Value));
            //        }
            //        //Debug.Log("ss");
            //        // Debug.Log(listQuery.Count);
            //        //Debug.Log(doc);
            //    }

            //});
        }
        //Query query = gpsRef.WhereEqualTo("obGPS", collection); //obGPS = collection
        //query.GetSnapshotAsync().ContinueWithOnMainThread((QuerySnapshotTask) =>
        //{
        //    foreach (DocumentSnapshot doc in QuerySnapshotTask.Result.Documents)
        //    {
        //        Debug.Log("Find ");
        //        check = true;
        //    }
        //});
        //check = true;
        Debug.Log("check : "+ check.ToString());
        //GPS_T.text = check.ToString() + "// query check";
        //ex..?
        //var check = false;
        //var lat = GpsManager.current_Lat;
        //var lng = GpsManager.current_Long;
        //string collection = lat.ToString() + "x" + lng.ToString();

        //CollectionReference gpsRef = db.Collection(collection);
        //Query query = gpsRef.WhereEqualTo("obGPS", collection); //obGPS = collection
        //query.GetSnapshotAsync().ContinueWithOnMainThread((QuerySnapshotTask) =>
        //{
        //    foreach (DocumentSnapshot doc in QuerySnapshotTask.Result.Documents)
        //    {
        //        Debug.Log("Find ");
        //        check = true;
        //    }
        //});
        //return check;

    }

    //private Vector3 GetObstacleUnityPosition()
    //{
    //    //중심좌표
    //    var obstaclePointList = PointCloudVisualization._obstaclePoints;
    //    var count = obstaclePointList.Count;
    //    //double Obstacle_x = 0;
    //    //double Obstacle_z = 0;
    //    float Obstacle_x = 0;
    //    float Obstacle_z = 0;
    //    foreach (var point in obstaclePointList)
    //    {
    //        Obstacle_x += point.x;
    //        Obstacle_z += point.z;
    //    }

    //    return new Vector3(Obstacle_x,count, Obstacle_z);
    //}

    //private string GetObstacleGPS(double lat, double lng,float x,float z)
    //{
    //    return lat.ToString();
    //}

    //private float GetObstacleDistance(float x,float z)
    //{
    //    return 1;
    //}
}

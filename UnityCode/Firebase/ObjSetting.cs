using UnityEngine;
using Firebase.Firestore;

[FirestoreData]

public struct ObjSetting
{
    [FirestoreProperty]
    public int updateCheck { get; set; }

    [FirestoreProperty]
    public float position_obj_x { get; set; }
    [FirestoreProperty]
    public float position_obj_y { get; set; }
    [FirestoreProperty]
    public float position_obj_z { get; set; }

    [FirestoreProperty]
    public float rotation_obj_w { get; set; }
    [FirestoreProperty]
    public float rotation_obj_x { get; set; }
    [FirestoreProperty]
    public float rotation_obj_y { get; set; }
    [FirestoreProperty]
    public float rotation_obj_z { get; set; }
}

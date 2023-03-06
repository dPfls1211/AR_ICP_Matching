using UnityEngine;
using Firebase.Firestore;

[FirestoreData]

public struct PointCloudData
{
    [FirestoreProperty]
    public double FPoints_x { get; set; }

    [FirestoreProperty]
    public double FPoints_y { get; set; }

    [FirestoreProperty]
    public double FPoints_z { get; set; }

}


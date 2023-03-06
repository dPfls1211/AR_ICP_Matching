using UnityEngine;
using Firebase.Firestore;

[FirestoreData]

public struct ObstacleData
{
    [FirestoreProperty]
    public double Latitude { get; set; }

    [FirestoreProperty]
    public double Longitude { get; set; }

    [FirestoreProperty]
    public double position_x { get; set; }

    [FirestoreProperty]
    public double position_z { get; set; }

    [FirestoreProperty]
    public string compass { get; set; }

    [FirestoreProperty]
    public double obstacleDis { get; set; }

    [FirestoreProperty]
    public string obGPS { get; set; }
}


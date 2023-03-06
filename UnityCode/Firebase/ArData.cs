using UnityEngine;
using Firebase.Firestore;

[FirestoreData]

public struct ArData
{
    [FirestoreProperty]
    public string UserGPS { get; set; }

    [FirestoreProperty]
    public string Compass { get; set; }

    [FirestoreProperty]
    public bool checkPoint { get; set; }
    [FirestoreProperty]
    public double pointFitness { get; set; }


    [FirestoreProperty]
    public float Gyro_w { get; set; }

    [FirestoreProperty]
    public float Gyro_x { get; set; }

    [FirestoreProperty]
    public float Gyro_y { get; set; }

    [FirestoreProperty]
    public float Gyro_z { get; set; }

    [FirestoreProperty]
    public double Latitude { get; set; }

    [FirestoreProperty]
    public double Longitude { get; set; }


    [FirestoreProperty]
    public float testPoint { get; set; }

    [FirestoreProperty]
    public bool arUseCheck { get; set; }
   
}



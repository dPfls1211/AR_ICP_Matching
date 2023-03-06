using UnityEngine;
using Firebase.Firestore;

[FirestoreData]

public struct centerPoints
{
    [FirestoreProperty]
    public float centerPointX { get; set; }
    [FirestoreProperty]
    public float centerPointY { get; set; }
    [FirestoreProperty]
    public float centerPointZ { get; set; }
}

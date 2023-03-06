using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj2user : MonoBehaviour
{
    public GameObject user;
    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.Find("AR Camera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(user.transform);
    }
}

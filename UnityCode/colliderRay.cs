using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colliderRay : MonoBehaviour
{
    public Collider coll;

    public Text Text;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Colls()
    {
        

        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;
        Text.text = ray.ToString();
        if (coll.Raycast(ray, out hit, 1000.0f))
        {
            return true;
        }
        return false;
    }
}

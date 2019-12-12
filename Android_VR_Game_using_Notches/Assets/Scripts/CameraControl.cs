using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        //transform.localPosition = new Vector3(20, 21, -55);
        //transform.localEulerAngles = new Vector3(27.071f, -34.71f, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_spinning_turtle : MonoBehaviour
{
    Vector3 currentEulerAngles;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentEulerAngles += new Vector3(0f, 0f, 30f) * Time.deltaTime;

        transform.eulerAngles = currentEulerAngles;

    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class scr_oldman_explinationmark : MonoBehaviour
{
    public bool can_see_turtle = false;
    public GameObject explinationMark;
    public float explinationMark_cooldown = 10;

    float cooldown_counter;

    // Start is called before the first frame update
    void Start()
    {
        cooldown_counter = explinationMark_cooldown;
    }

    // Update is called once per frame
    void Update()
    {
       if (can_see_turtle == false) 
        {
            explinationMark.SetActive(false);
        }
        else
        {
            explinationMark.SetActive(true);
        }

       if (can_see_turtle == true)
        {
            cooldown_counter = cooldown_counter - 1*Time.deltaTime;
            if (cooldown_counter < 0)
            {
                can_see_turtle=false;
                cooldown_counter = explinationMark_cooldown;
            }
        }
    }
}

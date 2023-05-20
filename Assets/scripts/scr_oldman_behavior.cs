using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class scr_oldman_behavior : MonoBehaviour
{
    Vector2 oldman_position;
    Vector2 turtle_position;
    Vector2 home_position;

    float oldman_turtle_angle;
    float oldman_home_angle;
    float verticle_component;
    float horizontal_component;

    public GameObject oldman;
    public GameObject turtle;
    public GameObject home;

    public float oldman_speed = 1f;

    public bool turtle_in_shell = false;
    bool can_see_turtle = false;

    float distance_to_turtle;
    public float oldman_vision_radius = 8;

    public GameObject explinationMark;
    public float explinationMark_cooldown = 10;

    float cooldown_counter;

    // Start is called before the first frame update
    void Start()
    {
        oldman_position = Vector2.zero;
        turtle_position = Vector2.zero;
        home_position = Vector2.zero;

        oldman_turtle_angle = 0;

        cooldown_counter = explinationMark_cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        

        oldman_position = new Vector2(oldman.transform.position.x, oldman.transform.position.y);
        turtle_position = new Vector2(turtle.transform.position.x, turtle.transform.position.y);
        home_position = new Vector2(home.transform.position.x, home.transform.position.y);

        
        //Debug.Log("Old Man Position" + oldman_position);
        //Debug.Log("turtle Position" + turtle_position);


        oldman_turtle_angle = Mathf.Atan((turtle_position.y - oldman_position.y) / (turtle_position.x - oldman_position.x));
        oldman_turtle_angle = oldman_turtle_angle * 180 / Mathf.PI;

        oldman_home_angle = Mathf.Atan((home_position.y - oldman_position.y) / (home_position.x - oldman_position.x));
        oldman_home_angle = oldman_home_angle * 180 / Mathf.PI;
       
        distance_to_turtle = Vector2.Distance(oldman_position, turtle_position);
        //Debug.Log("Distance from old man to turtle" +  distance_to_turtle);

        if (distance_to_turtle < oldman_vision_radius && turtle_in_shell == false )
        {
            can_see_turtle = true;
        }
        else
        {
            can_see_turtle = false;
        }



        if (can_see_turtle == true)
        {
            verticle_component = Mathf.Sin(oldman_turtle_angle);
            horizontal_component = Mathf.Cos(oldman_turtle_angle);
            transform.Translate(oldman_speed * Time.deltaTime * horizontal_component, oldman_speed * Time.deltaTime * verticle_component, 0f);
        }
        else
        {
            verticle_component = Mathf.Sin(oldman_home_angle);
            horizontal_component = Mathf.Cos(oldman_home_angle);
            transform.Translate(oldman_speed * Time.deltaTime * horizontal_component, oldman_speed * Time.deltaTime * verticle_component, 0f);
        }

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
            cooldown_counter = cooldown_counter - 1 * Time.deltaTime;
            if (cooldown_counter < 0)
            {
                can_see_turtle = false;
                cooldown_counter = explinationMark_cooldown;
            }
        }

    }
}

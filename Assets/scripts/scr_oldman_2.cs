using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class scr_oldman_2 : MonoBehaviour
{
   
    //Old man behavior
    public float vision_radius = 6f;
    public float hearing_range = 8f;
    bool can_see_turtle = false;
    public bool turtle_in_shell = false;

    public GameObject turtle;
    public GameObject home;
    public GameObject oldman;

    public GameObject exclamation_mark;
    public GameObject question_mark;
    public GameObject music_mark;
    bool just_saw_turtle = false;
    bool just_lost_turtle = false;
    bool just_got_home = false;
    bool left_home = true;

    Vector2 oldman_position;
    Vector2 turtle_position;
    Vector2 home_position;

    float distance_to_turtle;
    float distance_to_home;

    public float thought_cooldown = 1f;
    float thougt_cooldown_counter;

    public GameObject target;


    public AIDestinationSetter destinationSetter;

    void Start()
    {
        oldman_position = Vector2.zero;
        turtle_position = Vector2.zero;
        home_position = Vector2.zero;

        thougt_cooldown_counter = thought_cooldown;

        target = turtle;

    }

    // Update is called once per frame
    void Update()
    {
        oldman_position = new Vector2(oldman.transform.position.x, oldman.transform.position.y);
        turtle_position = new Vector2(turtle.transform.position.x, turtle.transform.position.y);
        home_position = new Vector2(home.transform.position.x, home.transform.position.y);  

        distance_to_turtle = Vector2.Distance(turtle_position, oldman_position);


        //Behavior Tree
        if (distance_to_turtle < vision_radius && turtle_in_shell == false)
        {
            if (can_see_turtle == false)
            {
                just_saw_turtle = true;
            }
            can_see_turtle = true;
        }
        else
        {
            if (can_see_turtle == true)
            {
                just_lost_turtle = true;
            }
            can_see_turtle = false;
        }

        //emote tree
        if (just_saw_turtle == true)
        {
            exclamation_mark.SetActive(true);
            question_mark.SetActive(false);
            music_mark.SetActive(false);
            thougt_cooldown_counter = thought_cooldown;
            just_saw_turtle = false;
        }

        if (just_lost_turtle == true)
        {
            exclamation_mark.SetActive(false);
            question_mark.SetActive(true);
            music_mark.SetActive(false);
            thougt_cooldown_counter = thought_cooldown;
            just_lost_turtle=false;
        }

        distance_to_home = Vector2.Distance(home_position, oldman_position);
        if (distance_to_home > 1 && left_home == false)
        {
            left_home = true;
        }


        if (distance_to_home < 1 && just_got_home == false && left_home == true)
        {
            just_got_home = true;
            left_home = false;
        }

        if (just_got_home == true)
        {
            exclamation_mark.SetActive(false);
            question_mark.SetActive(false);
            music_mark.SetActive(true);
            thougt_cooldown_counter = thought_cooldown;
            just_got_home = false;
        }

        if (thougt_cooldown_counter > 0)
        {
            thougt_cooldown_counter = thougt_cooldown_counter - 1f*Time.deltaTime;
        }
        else
        {
            exclamation_mark.SetActive(false);
            question_mark.SetActive(false);
            music_mark.SetActive(false);
        }


        if (can_see_turtle == true)
        {
            target = turtle;
            destinationSetter.target = turtle.transform;
            Debug.Log("trying to get turtle");
        }
        else
        {
            target = home;
            destinationSetter.target = home.transform;
            Debug.Log("trying to go home");
        }




        Debug.Log("cooldown counter" + thougt_cooldown_counter);



    }
}

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

    public List<lettuce> lettuces;

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

    public float walk_animation_speed = 0.1f;
    float walk_animation_counter;
    public GameObject first_walk_animation;
    public GameObject second_walk_animation;
    public GameObject rocking_chair;
    bool walk_flip = true;

    private turtleController tutel;
    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> angrySounds;
    [SerializeField] private List<AudioClip> confusedSounds;
    [SerializeField] private List<AudioClip> idleSounds;
    [SerializeField] private AudioClip placeholder;

    [SerializeField] private float soundFrequency;
    [SerializeField] [Range(0f, 1f)] private float frequencyRandomness;
    public float cooldownTimer;
    public bool makeNoise = true;
    void Start()
    {
        tutel = turtle.GetComponent<turtleController>();
        audioSource = GetComponent<AudioSource>();
        oldman_position = Vector2.zero;
        turtle_position = Vector2.zero;
        home_position = Vector2.zero;

        thougt_cooldown_counter = thought_cooldown;

        target = turtle;

        walk_animation_counter = walk_animation_speed;
        first_walk_animation.SetActive(true);
        second_walk_animation.SetActive(false);

        cooldownTimer = coolDown();
    }

    // Update is called once per frame
    void Update()
    {

        turtle_in_shell = tutel.inShell;
        oldman_position = new Vector2(oldman.transform.position.x, oldman.transform.position.y);
        turtle_position = new Vector2(turtle.transform.position.x, turtle.transform.position.y);
        home_position = new Vector2(home.transform.position.x, home.transform.position.y);  

        distance_to_turtle = Vector2.Distance(turtle_position, oldman_position);

        cooldownTimer -= Time.deltaTime;
        if (can_see_turtle && (cooldownTimer < 0f))
        {
            playSound(0);
            cooldownTimer = coolDown();
        }
        if(!can_see_turtle && left_home && cooldownTimer < 0f)
        {
            playSound(1);
            cooldownTimer = coolDown();
        }
        if(!left_home && cooldownTimer < 0f)
        {
            playSound(2);
            cooldownTimer = coolDown();
        }


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

        GameObject tmp = checkLettuces();
        if (can_see_turtle == true)
        {
            target = turtle;
            destinationSetter.target = turtle.transform;
            if(tmp!= null) tmp.GetComponent<lettuce>().currentlyCrunched = false;
            Debug.Log("trying to get turtle");
        }
        else if(tmp != null )
        {
            if(Vector2.Distance(tmp.transform.position, oldman_position) < hearing_range)
            {
                target = tmp;
                destinationSetter.target = tmp.transform;
                Debug.Log("trying to get to crunchered lettuce uwu");
            }
            else
            {
                tmp.GetComponent<lettuce>().currentlyCrunched = false;
            }
        }
        else
        {
            target = home;
            destinationSetter.target = home.transform;
            Debug.Log("trying to go home");
        }
        

        if(distance_to_home > 0.2)
        {
            walk_animation_counter = walk_animation_counter - 1f * Time.deltaTime;
            //Debug.Log("walk animation counter" + walk_animation_counter);
            if (walk_animation_counter < 0)
            {
                walk_animation_counter = walk_animation_speed;
                if (walk_flip == false)
                {
                    first_walk_animation.SetActive(true);
                    second_walk_animation.SetActive(false);
                    rocking_chair.SetActive(false);
                    walk_flip = true;
                }
                else
                {
                    first_walk_animation.SetActive(false);
                    second_walk_animation.SetActive(true);
                    rocking_chair.SetActive(false);
                    walk_flip = false;
                }
            }   
        }
        else
        {
            first_walk_animation.SetActive(false);
            second_walk_animation.SetActive(false);
            rocking_chair.SetActive(true);
        }


        //Debug.Log("cooldown counter" + thougt_cooldown_counter);



    }

    private float coolDown()
    {
        float min, max;
        min = soundFrequency - (soundFrequency * frequencyRandomness);
        max = soundFrequency + (soundFrequency * frequencyRandomness);

        float ret = Random.Range(min, max);
        if (audioSource.clip != null)
        {
            ret += audioSource.clip.length;
        }
        return ret;
    }

    private void playSound(int emotion)
    {
        AudioClip soundToPlay = placeholder;
        switch (emotion)
        {
            case 0: //Agner
                if (angrySounds.Count > 0) soundToPlay = angrySounds[Random.Range(0, angrySounds.Count - 1)];
                break;
            case 1: //Confuzzlesion
                if (confusedSounds.Count > 0) soundToPlay = confusedSounds[Random.Range(0, confusedSounds.Count - 1)];
                break;
            case 2: //Happers
                if (idleSounds.Count > 0) soundToPlay = idleSounds[Random.Range(0, idleSounds.Count - 1)];
                break;

        }
        audioSource.clip = soundToPlay;
        if(makeNoise) audioSource.Play();

    }
    private GameObject checkLettuces()
    {
        for(int i = 0; i < lettuces.Count; i++)
        {
            if (lettuces[i].currentlyCrunched)
            {
                return lettuces[i].gameObject;
            }
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Lettuce")
        {
            if (collision.gameObject.GetComponent<lettuce>().currentlyCrunched)
            {
                collision.gameObject.GetComponent<lettuce>().currentlyCrunched = false;
            }
        }
    }
}

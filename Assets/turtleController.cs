using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class turtleController : MonoBehaviour
{
    /// <summary>
    /// Things to do:
    /// </summary>

    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float rotationSpeedMax = 360f;
    [SerializeField] private float speedUpRate = 1f;

    [SerializeField] private float spriteSwitchFrequency = 0.2f;
    [SerializeField] private List<Sprite> sprites;
    private bool walkingSpriteZero = true;
    private bool correctImage = true;
    private bool justShellImage = false;
    private float switchCtr = 0f;
    private bool preSwitched = false;

    [SerializeField] private float satiety = 50f;
    [SerializeField] private float satietyPerChomp = 20f;
    [SerializeField] private float baseSatietyLossRate = 0.5f;
    [SerializeField] private float rotationSatietyLossRate = 2f;
    [SerializeField] private Slider satietyMeter;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject failimage;

    private bool noMoreHungy = false;

    private Slider satietySlider;

    private SpriteRenderer spriteRenderer;

    private float curSpinSpeed;
    private bool inShell = false;
    private bool turning = false;
    private bool win = false;

    private float burrowCooldown = 5f;

    private bool chomping = false;
    private float chompingTime;
    private float chompTimePerUnit = 3f;
    
    private float currentChompWorth;

    private GameObject lastChompedLettuce;
    public float alreadyChomped = 0f;

    // Start is called before the first frame update
    void Start()
    {
        curSpinSpeed = rotationSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!win)
        {
            if (satiety >= 100) noMoreHungy = true;
            burrowCooldown -= Time.deltaTime;
            if (burrowCooldown < 0f) text.text = "";
            move();

            if (!correctImage)
            {
                if (turning)
                {
                    spriteRenderer.sprite = sprites[2];
                    justShellImage = true;
                }
                else if (walkingSpriteZero) spriteRenderer.sprite = sprites[0];
                else spriteRenderer.sprite = sprites[1];

                correctImage = true;
            }

            if (!chomping)
            {
                if (Input.GetMouseButton(0))
                    turn(false);
                else if (Input.GetMouseButton(1))
                    turn(true);
                else
                {
                    inShell = false;
                    turning = false;
                }
            }
            if (chomping && Input.GetMouseButtonDown(2))
            {
                chomping = false;
            }


            if (!turning) curSpinSpeed = rotationSpeed;

            if (chomping) currentlyChomping();

            satietyMeter.value = satiety;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lettuce")
        {
            chomping = true;
            chompingTime = chompTimePerUnit * (collision.gameObject.GetComponent<lettuce>().lettuceDurability / 100);
            lastChompedLettuce = collision.gameObject;
        }
        else if(collision.gameObject.tag == "Burrow")
        {
            if(burrowCooldown > 0f)
            {
                transform.Rotate(new Vector3(0f, 0f, 180f));
            }
            else
            {
                burrowCooldown = 5f;
                if (satiety >= 100)
                {
                    text.text = "Yay :D";
                    win = true;
                }
                else
                {
                    text.text = "Too Hungy :(";
                    transform.Rotate(new Vector3(0f, 0f, 180f));
                }
            }
            
            
        }
        else if(collision.gameObject.tag == "Old Man")
        {
            failimage.SetActive(true);
            win = true;
        }
    }

    private void currentlyChomping()
    {
        chompingTime -= Time.deltaTime;
        float tmpPerc = (chompingTime / chompTimePerUnit) * 100;
        lastChompedLettuce.GetComponent<lettuce>().lettuceDurability = tmpPerc;
        if (lastChompedLettuce.GetComponent<lettuce>().lettuceDurability < 0f)
        {
            chomping = false;
            alreadyChomped++;
            Destroy(lastChompedLettuce.gameObject);
            satiety += satietyPerChomp;
        }
    }
    private void move()
    {
        
        if (justShellImage) correctImage = false;
        
        Vector3 dir = transform.up;
        if (!inShell && !chomping)
        {
            if (!noMoreHungy)
            {
                satiety -= baseSatietyLossRate * Time.deltaTime;
                if (satiety < 0f) satiety = 0f;
            }

            float tMult = 1f / spriteSwitchFrequency;
            switchCtr += Time.deltaTime;
            float ctrMult = switchCtr * tMult;
            if (ctrMult > 0.5) transform.position = (transform.position + (dir * Mathf.SmoothStep(movementSpeed, 0f, ctrMult) * Time.deltaTime));
            else                transform.position = (transform.position + (dir * Mathf.SmoothStep(0f, movementSpeed, ctrMult) * Time.deltaTime));
            
            
            if(switchCtr > (spriteSwitchFrequency * .2f) && !preSwitched)
            {
                walkingSpriteZero = !walkingSpriteZero;
                correctImage = false;
                preSwitched = true;
            }
            if (switchCtr > spriteSwitchFrequency)
            {
                switchCtr = 0f;
                preSwitched = false;
            }
        }
    }

    private void turn(bool turnRight)
    {
        inShell = true;
        turning = true;
        correctImage = false;
        
        if(!noMoreHungy)
        {
            satiety -= rotationSatietyLossRate * Time.deltaTime;
            if (satiety < 0f) satiety = 0f;
        }
        

        curSpinSpeed = curSpinSpeed * (1 + (speedUpRate * Time.deltaTime));
        if (curSpinSpeed > rotationSpeedMax) curSpinSpeed = rotationSpeedMax;

        if(turnRight)
            transform.Rotate(new Vector3(0f, 0f, -curSpinSpeed * Time.deltaTime));
        else
            transform.Rotate(new Vector3(0f, 0f, curSpinSpeed * Time.deltaTime));

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lettuce : MonoBehaviour
{
    public float lettuceSize = 1;
    public float lettuceDurability = 100f;

    [SerializeField] private List<Sprite> chompStages;
    [SerializeField] private Sprite regrowStage;
    [SerializeField] private float respawnTimer = 10f;
    [SerializeField] private bool doRespawn = false;

    private float respawnCtr = 0f;

    private float chompStageNumber;
    private float chompStageSize;

    private SpriteRenderer spriteRenderer;

    public bool currentlyCrunched = false;


    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        transform.Rotate(new Vector3(0f, 0f, Random.Range(-180f, 180f)));
        chompStageNumber = chompStages.Count;
        chompStageSize = 100 / chompStageNumber;
    }
    private void Update()
    {
        if(doRespawn && lettuceDurability <= 0f)
        {
            respawnCtr += Time.deltaTime;
            if (respawnCtr >= respawnTimer)
            {
                respawnCtr = 0f;
                lettuceDurability = 99f;
            }
        }
        if (lettuceDurability <= 0  )
            spriteRenderer.sprite = regrowStage;
        else        
            for(int i = 0; i < chompStageNumber; i++)
            {
            
                if (lettuceDurability < (chompStageSize * (i + 1)))
                {
                    spriteRenderer.sprite = chompStages[i];
                    break;
                }
            }
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lettuce : MonoBehaviour
{
    public float lettuceSize = 1;
    public float lettuceDurability = 100;

    [SerializeField] private List<Sprite> chompStages;
    private float chompStageNumber;
    private float chompStageSize;

    private SpriteRenderer spriteRenderer;

    
    private void Start()
    {
        transform.Rotate(new Vector3(0f, 0f, Random.Range(-180f, 180f)));
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        chompStageNumber = chompStages.Count;
        chompStageSize = 100 / chompStageNumber;
    }
    private void Update()
    {
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

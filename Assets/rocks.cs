using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class rocks : MonoBehaviour
{
    [SerializeField] private List<Sprite> rockSprites;
    
    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = rockSprites[Random.Range(0, rockSprites.Count)];    
    }
}

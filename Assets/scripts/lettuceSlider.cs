using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lettuceSlider : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private List<Image> lettuces;
    public float value;
    private float perStage;

    [SerializeField] private Sprite inactive;
    [SerializeField] private Sprite active;

    // Start is called before the first frame update
    void Start()
    {
        perStage = 100f / lettuces.Count;
    }

    // Update is called once per frame
    void Update()
    {
        updateLettuces();
        slider.value = value;
    }

    private void updateLettuces()
    {
        for(int i = 0; i < lettuces.Count; i++)
        {
            if(value > perStage * i)
                lettuces[i].sprite = active;
            else
                lettuces[i].sprite = inactive;
        }
    }
}

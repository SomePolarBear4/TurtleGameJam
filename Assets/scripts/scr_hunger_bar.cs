using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_hunger_bar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHunger(float maxHunger)
    {
        slider.maxValue = maxHunger;
        slider.value = maxHunger;
    }

    public void SetHunger(float hunger)
    {
        slider.value = hunger;
    }
}

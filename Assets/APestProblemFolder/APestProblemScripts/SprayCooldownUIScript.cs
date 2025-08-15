using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprayCooldownUIScript : MonoBehaviour
{

    public Slider cooldownSlider;
    //public Gradient gradient;
    public Image fill;

    public void SetMaxCooldown(float cooldown)
    {

        cooldownSlider.maxValue = cooldown;
        cooldownSlider.value = cooldown;

        //fill.color = gradient.Evaluate(1f);

    }


    public void SetCurrentCooldown(float cooldown)
    {

        cooldownSlider.value = cooldown;
       // fill.color = gradient.Evaluate(cooldownSlider.normalizedValue);
    }

}

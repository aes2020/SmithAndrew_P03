using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public Slider slider;

    public float _energy = 0f;

    public Gradient gradient;
    public Image fill;


    public void SetMaxEnergy(int energy)
    {
        slider.maxValue = energy;
        slider.value = energy;

        fill.color = gradient.Evaluate(1f);
    }

    public void GainEnergy(float power)
    {  
            _energy += power;
            fill.fillAmount = _energy / 100f;
            Debug.Log("Rising Energy");
    }

    public void SetEnergy(int startEnergy)
    {
        slider.value = startEnergy;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}

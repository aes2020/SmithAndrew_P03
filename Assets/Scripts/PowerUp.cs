using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public Slider slider;

    public float _energy = 200f;

    public Gradient gradient;
    public Image fill;

    public Character _character;

    public int currentEnergy;

    [SerializeField] private float timerSpeed = 5f;

    private float elapsed;

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
        /*
        if(_energy >= 100)
        {
            _energy = 100f;
        }
        */
            Debug.Log("Rising Energy");
    }

    public void SetEnergy(int energy)
    {
        slider.value = energy;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void EnergyReset(float power)
    {
        if (slider.value >= 200 && _character.isTransformed)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= timerSpeed)
            {
                elapsed = 0f;
                SetEnergy(currentEnergy);
                _character.isTransformed = false;
                _character.notTransformed = true;


                _character.artToDisable.SetActive(false);
                _character.artToEnable.SetActive(true);

                slider.value = _energy;
                _energy += power;

                fill.fillAmount = _energy /100f;

                //fill.color = gradient.Evaluate(slider.normalizedValue);

                Debug.Log("Energy is reset");
            }
        }
    }
}

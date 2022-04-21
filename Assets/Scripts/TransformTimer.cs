using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTimer : MonoBehaviour
{
    [SerializeField] private float timerSpeed = 2f;

    private float elapsed;

    public Character _character;

    public PowerUp _powerUp;

    public float _energy = 200f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        Character character
           = this.gameObject.GetComponent<Character>();

        EnergyDecrease();

        /*
        if(_powerUp.slider.value >= 200)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= timerSpeed)
            {
                elapsed = 0f;

            }
        }
        */
    }

    private void EnergyDecrease()
    {
        if (_powerUp.slider.value >= 200 && _character.isTransformed)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= timerSpeed)
            {
                elapsed = 0f;
                _powerUp.slider.value--;


                Debug.Log("Energy is dropping");
            }
        }
    }
}

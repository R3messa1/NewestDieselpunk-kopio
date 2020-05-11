using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar instance;

    private Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        Destroy(instance);
        instance = this;
        healthSlider = GetComponent<Slider>();
    }

    public void SetValue(float value)
    {
        healthSlider.value = value;
    }
}

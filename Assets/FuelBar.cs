using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    public static FuelBar instance;

    private Slider fuelSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        fuelSlider = GetComponent<Slider>();
    }

    public void SetValue(float value)
    {
        fuelSlider.value = value;
    }
}

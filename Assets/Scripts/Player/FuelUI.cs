using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{

    public static FuelUI instance;

    Text FueltTxt;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        FueltTxt = GetComponent<Text>();
    }

    public void UpdateFuel(int remainingFuel, int totalFuel)
    {
        FueltTxt.text = remainingFuel.ToString() + " / " + totalFuel.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    public static AmmoCounter instance;

    [SerializeField] private Text ammoCounter;

    private int _ammo;
    private int _w1;
    private int _w2;

    public void Start()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    public void Update()
    {
        LetsCount();
    }

    public void UpdateAmmo1(int w1)
    {
        _w1 = w1;
    }

    public void UpdateAmmo2(int w2)
    {
        _w2 = w2;
    }

    public void LetsCount()
    {
        _ammo = _w1 + _w2;
        ammoCounter.text = _ammo.ToString();
    }
}

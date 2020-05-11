using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public static DamageEffect instance;

    public float maxAlpha = 0.4f;
    public float damageAlphaIncrement = 0.1f;

    Image myImg;

    float currentAlpha;

    private void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        myImg = GetComponent<Image>();
    }

    private void Update()
    {
        myImg.color = new Color(1, 0, 0, currentAlpha);
        currentAlpha -= Time.deltaTime * 0.05f;
    }

    public void TakeDamage()
    {
        currentAlpha = Mathf.Clamp(currentAlpha + damageAlphaIncrement, 0, maxAlpha);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    [SerializeField]
    private GameObject _impactPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            RichAI rAI = other.GetComponent<RichAI>();

            rAI.HomeRun();

            sound();

            Instantiate(_impactPrefab, transform.position, Camera.main.transform.rotation);
        }
    }

    void sound()
    {
        SoundManager.sndMan.PlayHitSound();
    }
}

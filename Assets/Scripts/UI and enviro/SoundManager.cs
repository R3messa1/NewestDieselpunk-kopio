using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sndMan;

    private AudioSource audioSrc;
    private AudioClip[] GunSound;
    private AudioClip[] WooshSound;
    private AudioClip[] SmackSound;

    private int randomGunSound;
    private int randomWooshSound;
    private int randomBatSound;
    // Start is called before the first frame update
    void Start()
    {
        sndMan = this;
        audioSrc = GetComponent<AudioSource>();
        GunSound = Resources.LoadAll<AudioClip>("GunSound");
        WooshSound = Resources.LoadAll<AudioClip>("WooshSound");
        SmackSound = Resources.LoadAll<AudioClip>("BaseballHitSound");

    }


    public void PlayGunSound() // <--- Tätä kutsumalla tää tekee ton gunsoundin
    {
        randomGunSound = Random.Range(0,1);
        audioSrc.PlayOneShot(GunSound[randomGunSound]);
        audioSrc.clip = GunSound[Random.Range(0, GunSound.Length)];
        audioSrc.Play();

    }

  

    public void PlayWooshSound() // <--- Tätä kutsumalla tää tekee ton wooshsoundin
    {
        randomGunSound = Random.Range(0,1);
        audioSrc.PlayOneShot(WooshSound[randomWooshSound]);
        audioSrc.clip = WooshSound[Random.Range(0, WooshSound.Length)];
        audioSrc.Play();

    }

    public void PlayHitSound()
    {
        randomBatSound = Random.Range(0, 1);
        audioSrc.PlayOneShot(SmackSound[randomBatSound]);
        audioSrc.clip = SmackSound[Random.Range(0, SmackSound.Length)];
        audioSrc.Play();
    }
}

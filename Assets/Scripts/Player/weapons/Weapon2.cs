using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon2 : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] int damage = 25;
    [SerializeField] LayerMask ignoreLayer;
    public Animator _animator;

    [SerializeField]
    private GameObject _muzzleFlashPrefabLeft;

    [SerializeField]
    private int _maxAmmo = 100;
    [SerializeField]
    private int _startingAmmo = 20;
    private int _ammo;
    [SerializeField]
    private int _ammoPickupAmount = 8;
    private bool _canFire = true;
    [SerializeField]
    private float _timeBetweenShots = 0.21f;
    [SerializeField]
    private GameObject AmmoHUD;

    private void Start()
    {
        AmmoHUD = GameObject.Find("AmmoHUD");
        _animator = this.GetComponent<Animator>();
        _ammo = _startingAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        AmmoCounter _ammoCounter = AmmoHUD.GetComponent<AmmoCounter>();

        _ammoCounter.UpdateAmmo2(_ammo);

        if (Input.GetMouseButton(0) && _ammo > 0 && _canFire)
        {
            _ammo -= 1;
            StartCoroutine(FireRate());

        }
        /*else if(Input.GetButtonUp("Fire1") || _ammo <= 0)
         {
             _muzzleFlashPrefabLeft.SetActive(false);
             _muzzleFlashPrefabRight.SetActive(false);
         }*/
        Debug.Log("ammo: " + _ammo);
    }

    IEnumerator FireRate()
    {
        
        _canFire = false;
        Shoot();
        yield return new WaitForSeconds(_timeBetweenShots);
        _canFire = true;
        _muzzleFlashPrefabLeft.SetActive(false);
    }

    void sound()
    {
        SoundManager.sndMan.PlayGunSound();
    }

    private void Shoot()
    {
        _muzzleFlashPrefabLeft.SetActive(true);

        //sound();

        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range, ignoreLayer))
        {
            Debug.Log("Kuole saatanan " + hit.transform.name);
            Poll targetPol = hit.transform.GetComponent<Poll>();
            if (targetPol)
                targetPol.Hit();
            //TODO: add some hit effect for visual players
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            // call a method on EnemyHealth that decreases the enemy's health
            Debug.Log("Damaging this soB");
            target.TakeDamage(damage, false);
        }
        else
        {
            return;
        }
    }

    public void AddAmmo()
    {
        _ammo += _ammoPickupAmount;
    }
}

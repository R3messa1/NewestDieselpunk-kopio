using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    [SerializeField]
    private Collider _hitBox;

    // Start is called before the first frame update
    void Start()
    {
        _hitBox = this.GetComponent<CapsuleCollider>();
        _hitBox.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitBoxActive()
    {
        _hitBox.isTrigger = true;
        StartCoroutine(AbleToDeal());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();

            if(player != null)
            {
                player.TakeDamage(33f);
                _hitBox.isTrigger = false;
            }
        }
    }
    
    IEnumerator AbleToDeal()
    {
        yield return new WaitForSeconds(2);
        _hitBox.isTrigger = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ProjectileShooter : MonoBehaviour
{
    [SerializeField] GameObject Projectile;
    [SerializeField] float FiringRate;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FireProjectile",1,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FireProjectile()
    {
        GameObject FiredProjectile = Instantiate(Projectile, transform.position, transform.rotation);
        FiredProjectile.transform.name = "Projectile";
        Destroy(FiredProjectile, 1);
    }
}

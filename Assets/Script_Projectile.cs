using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Projectile : MonoBehaviour
{
    [SerializeField] float ProjectileSpeed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.back * ProjectileSpeed*Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.name == "Player")
        {
            Destroy(collision.gameObject);
        }
    }
}

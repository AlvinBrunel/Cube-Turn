using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Platform_PreGame : MonoBehaviour
{
    Vector3 FinalPos;

    public Material InitialMaterial;
    [SerializeField] int DropSpeed = 100;
    [SerializeField] bool isFlashingPlatform;
    [SerializeField] bool isPlayerOn = false;

    [SerializeField] GameObject PlatformDebris;

    GameObject Player;
    GameObject GameEngine;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        GameEngine = GameObject.Find("Game");
        GetComponent<MeshRenderer>().material = GameEngine.GetComponent<Script_Game>().PlatformMat;
        GetComponent<MeshRenderer>().material.name = GameEngine.GetComponent<Script_Game>().PlatformMat.name;
        InitialMaterial = GetComponent<MeshRenderer>().material;

        FinalPos = transform.position;
        FinalPos.y = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y + 10,transform.position.z);
        StartCoroutine(DropPlatform(DropSpeed));

        if(isFlashingPlatform == true)
        {
            InvokeRepeating("FlashPlatform", 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<MeshRenderer>().enabled == false)
        {
            if (isPlayerOn == true)
            {
                //CancelInvoke("FlashPlatform");
                Player.transform.GetComponent<BoxCollider>().enabled = false;
                isPlayerOn = false;
                Player.GetComponent<Script_PlayerMovement>()._isGrounded = false;
                Debug.Log("Falling");

            }
        }
    }
    IEnumerator DropPlatform(int DropSpeed)
    {
        yield return new WaitForSeconds(transform.position.x/10);
        while (transform.position.y > FinalPos.y)
        {
            transform.position += Vector3.down * DropSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }

        transform.position = FinalPos;

    }
    void FlashPlatform()
    {
        if (GetComponent<MeshRenderer>().enabled == true)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else if (GetComponent<MeshRenderer>().enabled == false)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Player")
        {
            isPlayerOn = true;
            Player.GetComponent<Script_PlayerMovement>()._isGrounded = true;
        }


    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.name == "Player")
        {
            isPlayerOn = true;
            Player.GetComponent<Script_PlayerMovement>()._isGrounded = true;
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.name == "Player")
        {
            isPlayerOn = false;
            collision.transform.GetComponent<Script_PlayerMovement>()._isGrounded = false;
        }
    }
    private void OnDestroy()
    {
        GameObject SpawnPlatformDebris = Instantiate(PlatformDebris,transform.position,transform.rotation);

        var ps = SpawnPlatformDebris.transform.GetComponent<ParticleSystem>();
        //Material m_Material = GetComponent<Renderer>().material;
        var psr = SpawnPlatformDebris.transform.GetComponent<ParticleSystemRenderer>();
        psr.material = GetComponent<MeshRenderer>().material;
    }
}

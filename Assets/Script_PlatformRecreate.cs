using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlatformRecreate : MonoBehaviour
{
    [SerializeField] GameObject Platform;
    GameObject GameEngine;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyParticle", 2f);

        GameEngine = GameObject.Find("Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {

    }
    void DestroyParticle()
    {
        GameObject SpawnPlatform = Instantiate(Platform, transform.position + (Vector3.up * 15), transform.rotation);
        SpawnPlatform.transform.name = "Platform";
        SpawnPlatform.transform.parent = GameObject.Find("Platforms").transform;

        if(GameObject.Find("Cube") == null && (SpawnPlatform.transform.position.x == 0 && SpawnPlatform.transform.position.z == 0))
        {
            GameEngine.GetComponent<Script_Game>().Invoke("SpawnCube",1f);
            GameEngine.GetComponent<Script_Game>().Invoke("StartGameGUI", 2.3f);
        }
    }
}

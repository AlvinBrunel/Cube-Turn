using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Script_PlayerSide : MonoBehaviour
{
    public GameObject GameEngine;
    GameObject Cam;

    AudioSource AS;
    public Material InitialSideMaterial;

    [SerializeField] GameObject ComboScore;
    // Start is called before the first frame update
    void Start()
    {
        GameEngine = GameObject.Find("Game");
        Cam = GameObject.Find("Main Camera");

        AS = GetComponent<AudioSource>();
        //GetComponent<MeshRenderer>().material.color = transform.parent.GetComponent<Script_PlayerMovement>().PlayerColour;

        GetComponent<MeshRenderer>().material = GameEngine.GetComponent<Script_Game>().PlayerMat;
        InitialSideMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Platform")
        {
            if((other.gameObject.GetComponent<MeshRenderer>().material.name == GetComponent<MeshRenderer>().material.name) && GetComponent<MeshRenderer>().material != InitialSideMaterial)
            {
                other.gameObject.GetComponent<MeshRenderer>().material = other.gameObject.GetComponent<Script_Platform_PreGame>().InitialMaterial;
                other.gameObject.GetComponent<MeshRenderer>().material.name = other.gameObject.GetComponent<Script_Platform_PreGame>().InitialMaterial.name;
                GetComponent<MeshRenderer>().material = InitialSideMaterial;
                Debug.Log("Working");
                GameEngine.GetComponent<Script_Game>().Score++;
                if (PlayerPrefs.GetInt("PBScore") < GameEngine.GetComponent<Script_Game>().Score && PlayerPrefs.GetInt("PBScore") !=0 && GameEngine.GetComponent<Script_Game>().ScoreText.GetComponent<TextMeshProUGUI>().color != Color.yellow)
                {
                    GameEngine.GetComponent<Script_Game>().ScoreText.GetComponent<TextMeshProUGUI>().color = Color.yellow;
                    Cam.GetComponent<AudioSource>().Play();

                }
                AS.Play();
                if(ComboScore)
                {
                    ShowComboScore(other.transform.gameObject);
                }
                GameEngine.GetComponent<Script_Game>().LevelUp(GameEngine.GetComponent<Script_Game>().Score);
                Cam.GetComponent<ScreenShakeController>().Shake("VerySmall");

                    for (int i = 0; i < transform.parent.childCount; i++)
                    {
                        if (transform.parent.GetChild(i).GetComponent<MeshRenderer>().material != transform.parent.GetChild(i).GetComponent<Script_PlayerSide>().InitialSideMaterial)
                        {
                            break;
                        }
                        if(i == transform.parent.childCount-1)
                        {
                            GameEngine.GetComponent<Script_Game>().CancelInvoke("ChangePlatform");
                            GameEngine.GetComponent<Script_Game>().ChangePlatform();
                        }
                    }
            }
        }
    }
    void ShowComboScore(GameObject Platform)
    {
        if(!GameObject.Find("Combo Score"))
        {
            transform.parent.GetComponent<Script_PlayerMovement>().CurrentComboScore = 1;
        }
        else
        {
            transform.parent.GetComponent<Script_PlayerMovement>().CurrentComboScore++;
        }
            GameObject InsGameScoreText = Instantiate(ComboScore, new Vector3(Platform.transform.position.x, Platform.transform.position.y + 1, Platform.transform.position.z), Quaternion.identity);
            InsGameScoreText.name = "Combo Score";
        if(transform.parent.GetComponent<Script_PlayerMovement>().CurrentComboScore >= 3)
        {
            InsGameScoreText.GetComponent<TextMeshPro>().text = (transform.parent.GetComponent<Script_PlayerMovement>().CurrentComboScore).ToString();
        }
        else
        {
            InsGameScoreText.GetComponent<TextMeshPro>().text = "";
        }

    }
}

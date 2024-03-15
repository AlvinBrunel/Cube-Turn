using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Script_GUI : MonoBehaviour
{
    GameObject GameEngine;
    // Start is called before the first frame update
    void Start()
    {
        GameEngine = GameObject.Find("Game");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = (GameEngine.GetComponent<Script_Game>().Score).ToString();
    }
}

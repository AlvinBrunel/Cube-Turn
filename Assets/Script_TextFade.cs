using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_TextFade : MonoBehaviour
{
    byte Transparency = 255;
    byte TransparencyInterval = 5;
    [SerializeField] float TimeInterval;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FadeIn()
    {
        Transparency += TransparencyInterval;
        if (Transparency > 250)
        {
            Invoke("FadeOut", TimeInterval);
        }
        else
        {
            GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, Transparency);
            Invoke("FadeIn", TimeInterval);
        }
    }
    void FadeOut()
    {
        Transparency -= TransparencyInterval;
        if (Transparency < 50)
        {
            Invoke("FadeIn", TimeInterval);
        }
        else
        {
            GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, Transparency);
            Invoke("FadeOut", TimeInterval);

        }
    }
}

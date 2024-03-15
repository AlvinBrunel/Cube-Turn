using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_FloatingText : MonoBehaviour
{
    Vector3 InitialPos;
    Color InitialColour;

    float CurrentAlpha = 1f;
    [SerializeField] float Speed;

    RectTransform RT;

    // Start is called before the first frame update
    void Start()
    {
        RT = GetComponent<RectTransform>();
        InitialPos = RT.localPosition;

        StartCoroutine(FloatText());
        InitialColour = GetComponent<TextMeshPro>().color;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator FloatText()
    {
        while (transform.position.y <= InitialPos.y + 2.7f)
        {
            RT.localPosition += Vector3.up*Speed*Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);

        }
        while (transform.position.y <= InitialPos.y + 2.8f)
        {
            RT.localPosition += Vector3.up * (Speed/3) * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);

        }
        while (transform.position.y <= InitialPos.y + 3.2f || CurrentAlpha >= 0f)
        {
            RT.localPosition += Vector3.up * (Speed/5) * Time.deltaTime;
            CurrentAlpha -= 0.04f;
            GetComponent<TextMeshPro>().color = new Color(InitialColour.r, InitialColour.g, InitialColour.b,CurrentAlpha);
            yield return new WaitForSeconds(Time.deltaTime);

        }
        Destroy(gameObject);
    }
}

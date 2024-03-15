using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    private float shakeTimeRemaining, shakePower, shakeFadeTime,shakeRotation;
    public float rotationMultiplier = 15f;

    Vector3 InitialPos;
    // Start is called before the first frame update
    void Start()
    {
        InitialPos = transform.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if(shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f,1f)*shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);

            shakePower = Mathf.MoveTowards(shakePower,0f,shakeFadeTime*Time.deltaTime);
            shakeRotation = Mathf.MoveTowards(shakeRotation, 0, shakeFadeTime*rotationMultiplier*Time.deltaTime);
        }
        else
        {
            transform.position = InitialPos;
        }
        //transform.rotation = Quaternion.Euler(0,0,shakeRotation*Random.Range(-1f,1f));
    }
    void StartShake(float length,float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;
    }

    public void Shake(string Size)
    {
        if(Size == "Big")
        {
            StartShake(.5f, 1f);
        }
        if (Size == "Medium")
        {
            StartShake(.3f, .6f);
        }
        if (Size == "Small")
        {
            StartShake(.2f, .4f);
        }
        if (Size == "VerySmall")
        {
            StartShake(.04f, .08f);
        }
    }
}

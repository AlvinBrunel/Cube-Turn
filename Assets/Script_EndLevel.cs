using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EndLevel : MonoBehaviour
{
    MeshRenderer MR;
    [SerializeField] float RotationSpeed;
    public string NextLevel;
    Vector3 spinDir;
    // Start is called before the first frame update
    void Start()
    {
        MR = GetComponent<MeshRenderer>();
        spinDir = Vector3.right * RotationSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(spinDir*Time.deltaTime);
    }
}

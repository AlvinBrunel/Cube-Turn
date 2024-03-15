using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerMovement : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private Vector3 Anchor;
    private Vector3 Axis;
    private Vector3 Dir;

    float xScale;
    float yScale;

    public bool _isRolling;
    public bool _isGrounded = false;

    [SerializeField] float RollSpeed = 5;
    [SerializeField] GameObject PlayerDebris;
    [SerializeField] Material[] SideMaterials;

    public int CurrentComboScore = 0;


    GameObject GameEngine;
    GameObject Cam;

    // Start is called before the first frame update
    void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen

        xScale = this.transform.localScale.x;
        yScale = this.transform.localScale.y;

        GameEngine = GameObject.Find("Game");
        
    }

    // Update is called once per frame
    void Update()
    {
        //MobileMovement();

        if (transform.position.y < -2.5f)
        {
            GameEngine.GetComponent<Script_Game>().EndGame();
        }
        if ((_isRolling) || !_isGrounded || Time.timeScale == 0) return;
            
        if (Input.GetKeyDown("a") || MobileMovement() == Vector2.left) Assemble(Vector3.left);
        else if (Input.GetKeyDown("d") || MobileMovement() == Vector2.right) Assemble(Vector3.right);
        else if (Input.GetKeyDown("w") || MobileMovement() == Vector2.up) Assemble(Vector3.forward);
        else if (Input.GetKeyDown("s") || MobileMovement() == Vector2.down) Assemble(Vector3.back);
        void Assemble(Vector3 Dir)
        {
            if(GameEngine.GetComponent<Script_Game>().isPlayingGame == false)
            {
                GameEngine.GetComponent<Script_Game>().StartGame();
            }
            Anchor = transform.position + (Vector3.down + Dir) * (xScale/2);
            Axis = Vector3.Cross(Vector3.up, Dir);
            StartCoroutine(Roll(Anchor, Axis));
        }

    }
    private Vector2 MobileMovement()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
                return Vector2.zero;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
                return Vector2.zero;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                            return Vector2.right;
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                            return Vector2.left;
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                            return Vector2.up;
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");
                            return Vector2.down; 
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    return Vector2.zero;

                }
            }
            return Vector2.zero;
        }
        else
        {
            return Vector2.zero;
        }
    }
    IEnumerator Roll(Vector3 Anchor, Vector3 Axis)
    {
        _isRolling = true;

        for(int i=0;i<90/RollSpeed;i++)
        {
            transform.RotateAround(Anchor,Axis,RollSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _isRolling = false;
        transform.position = new Vector3(Mathf.Round(transform.position.x),transform.position.y, Mathf.Round(transform.position.z));

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Platform")
        {
            _isGrounded = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Platform")
        {
            _isGrounded = true;
            if (!_isRolling)
            {

            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Platform")
        {
            _isGrounded = false;
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SideMaterials[i] = transform.GetChild(i).GetComponent<MeshRenderer>().material;
        }
        GameObject SpawnPlayerDebris = Instantiate(PlayerDebris, transform.position, Quaternion.Euler(0,0,0));
        for(int i = 0;i<PlayerDebris.transform.childCount;i++)
        {
            var ps = SpawnPlayerDebris.transform.GetChild(i).GetComponent<ParticleSystem>();
            //Material m_Material = GetComponent<Renderer>().material;
            var psr = SpawnPlayerDebris.transform.GetChild(i).GetComponent<ParticleSystemRenderer>();
            psr.material = SideMaterials[i];
        }
    }
}

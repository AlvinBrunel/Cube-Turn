using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Script_Game : MonoBehaviour
{
    [SerializeField] Material[] Skyboxes;
    [SerializeField] Material[] PlatformMats;
    [SerializeField] Material[] PlayerMats;

    [SerializeField] Material Skybox;
    public Material PlatformMat;
    public Material PlayerMat;

    int CurrentBackground = 0;

    public static bool gameIsPaused = false;
    [SerializeField] GameObject GameTitleText;
    public GameObject ScoreText;
    [SerializeField] GameObject SwipeToStart;
    [SerializeField] GameObject PersonalBestText;
    [SerializeField] GameObject PreviousScoreText;
    [SerializeField] GameObject PauseMenu;

    [SerializeField] GameObject PlayerObject;
    GameObject Player;

    [SerializeField] GameObject[] Platforms;
    //public Color TileColour;
    GameObject[] Sides;

    [SerializeField] AudioSource ASMain;
    [SerializeField] AudioSource AS;
    public AudioClip sfxEndGame, sfxLevelUp, sfxNewTile, sfxPB;

    public Material[] GameMaterials;

    public int Score = 0;
    public int Level = 1;


    public bool isPlayingGame = false;
    //public bool PBMet = false;

    public int PreviousScore;

    //Difficulties
    int ColourSelection = 2;
    public float PlatformInterval = 5f;

    // Start is called before the first frame update
    void Start()
    {
        ChangeBackground(Skyboxes[CurrentBackground],PlatformMats[CurrentBackground],PlayerMats[CurrentBackground]);

        AS = GetComponent<AudioSource>();
        if(PlayerPrefs.HasKey("PBScore") && PersonalBestText.activeSelf)
        {
            if (PlayerPrefs.GetInt("PBScore") > 0)
            {
                PersonalBestText.GetComponent<TextMeshProUGUI>().text = "Personal Best: " + PlayerPrefs.GetInt("PBScore").ToString();
            }
        }
        else
        {
            PersonalBestText.SetActive(false);
            PlayerPrefs.SetInt("PBScore", 0);
        }
        SpawnCube();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlayingGame == true)
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }
    public void ChangePlatform()
    {
        GameObject SpawnPlatform = Platforms[Random.Range(0, Platforms.Length)];
        Debug.Log(SpawnPlatform.GetComponent<MeshRenderer>().material.name);
        Debug.Log(PlatformMat.name);
        while (SpawnPlatform.GetComponent<MeshRenderer>().material.name != PlatformMat.name)
        {
            SpawnPlatform = Platforms[Random.Range(0, Platforms.Length)];
        }
        int RandNum = Random.Range(0, ColourSelection);
        SpawnPlatform.GetComponent<MeshRenderer>().material = GameMaterials[RandNum];
        SpawnPlatform.GetComponent<MeshRenderer>().material.name = "Side " + RandNum;



        for (int i =0;i<Sides.Length;i++)
        {
            if (Sides[i].GetComponent<MeshRenderer>().material== Sides[i].GetComponent<Script_PlayerSide>().InitialSideMaterial)
            {
                Sides[i].GetComponent<MeshRenderer>().material = SpawnPlatform.GetComponent<MeshRenderer>().material;
                Sides[i].GetComponent<MeshRenderer>().material.name = "Side " + RandNum;
                PlaySound(sfxNewTile);
                break;
            }
            if(i == Sides.Length-1)
            {
                EndGame();
            }
        }
        Invoke("ChangePlatform", PlatformInterval);
    }
    public void LevelUp(int Score)
    {
        if(Score == 15)
        {
            PlatformInterval = 4.5f;
            Level++;
            PlaySound(sfxLevelUp);
        }
        if (Score == 25)
        {
            ColourSelection = 3;
            Level++;
            PlaySound(sfxLevelUp);
        }
        if (Score == 40)
        {
            PlatformInterval = 4f;
            Level++;
            PlaySound(sfxLevelUp);
        }
        if (Score == 60)
        {
            PlatformInterval = 3.5f;
            ColourSelection = 4;
            Level++;
            PlaySound(sfxLevelUp);
        }
        if (Score == 85)
        {
            PlatformInterval = 3f;
            ColourSelection = 5;
            Level++;
            PlaySound(sfxLevelUp);
        }
        if (Score == 115)
        {
            PlatformInterval = 2.5f;
            ColourSelection = 6;
            Level++;
            PlaySound(sfxLevelUp);
        }
        if (Score == 150)
        {
            PlatformInterval = 2f;
            Level++;
            PlaySound(sfxLevelUp);
        }
        if (Score == 190)
        {
            PlatformInterval = 1.5f;
            Level++;
            PlaySound(sfxLevelUp);
        }
        if (Score == 235)
        {
            PlatformInterval = 1.5f;
            Level++;
            PlaySound(sfxLevelUp);
        }
        if (Score == 285)
        {
            PlatformInterval = 1f;
            Level++;
            PlaySound(sfxLevelUp);
        }
    }
    public void EndGame()
    {
        PlaySound(sfxEndGame);
        ScoreText.SetActive(false);
        ScoreText.GetComponent<TextMeshProUGUI>().color = Color.white;
        isPlayingGame = false;
        Destroy(Player);
        foreach (GameObject Platform in Platforms)
        {
            Destroy(Platform);
        }
        if (PlayerPrefs.GetInt("PBScore") < Score)
        {
            PlayerPrefs.SetInt("PBScore", Score);
        }
        PreviousScore = Score;
        Score = 0;
        Level = 1;
        PlatformInterval = 4.5f;
        ColourSelection = 2;
        CancelInvoke("ChangePlatform");
    }
    public void StartGame()
    {
        isPlayingGame = true;
        Platforms = GameObject.FindGameObjectsWithTag("Platform");
        Sides = GameObject.FindGameObjectsWithTag("Side");

        foreach (GameObject Platform in GameObject.FindGameObjectsWithTag("Platform"))
        {
            Platform.GetComponent<Renderer>().material = PlatformMat;
            Platform.GetComponent<Renderer>().material.name = PlatformMat.name;
            Platform.GetComponent<Script_Platform_PreGame>().InitialMaterial = PlatformMat;
        }

        ChangePlatform();
        InGameGUI();

    }
    public void SpawnCube()
    {
        Player = Instantiate(PlayerObject, new Vector3(0, 10f, 0), Quaternion.Euler(0, 0, 0));
        Player.transform.name = "Cube";
    }
    public void StartGameGUI()
    {
        SwipeToStart.SetActive(true);
        ScoreText.SetActive(false);
        PersonalBestText.SetActive(true);
        PreviousScoreText.SetActive(true);
        GameTitleText.SetActive(true);

        PreviousScoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + PreviousScore.ToString();
        PersonalBestText.GetComponent<TextMeshProUGUI>().text = "Personal Best: " + PlayerPrefs.GetInt("PBScore").ToString();
    }
    public void InGameGUI()
    {
        SwipeToStart.SetActive(false);
        ScoreText.SetActive(true);
        PersonalBestText.SetActive(false);
        PreviousScoreText.SetActive(false);
        GameTitleText.SetActive(false);
    }
    public void PlaySound(AudioClip AC)
    {
        if (AC == sfxLevelUp)
        {
            ASMain.Play();
        }
        else
        {
            AS.clip = AC;
            AS.Play();
        }
    }
    void PauseGame()
    {
        if (gameIsPaused)
        {
            PauseMenu.SetActive(true);
            ScoreText.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            PauseMenu.SetActive(false);
            ScoreText.SetActive(true);
            Time.timeScale = 1;
        }
    }
    void ChangeBackground(Material SkyBox, Material PlatformMat, Material PlayerMat)
    {
        RenderSettings.skybox = SkyBox;

        foreach(GameObject Platform in GameObject.FindGameObjectsWithTag("Platform"))
        {
            Platform.GetComponent<Renderer>().material = PlatformMat;
            Platform.GetComponent<Renderer>().material.name = PlatformMat.name;
            Platform.GetComponent<Script_Platform_PreGame>().InitialMaterial = PlatformMat;
        }
        if(Player != null)
        {
            for(int i = 0; i < Player.transform.childCount;i++)
            {
                Player.transform.GetChild(i).GetComponent<MeshRenderer>().material = PlayerMat;
            }  
        }
    }
}

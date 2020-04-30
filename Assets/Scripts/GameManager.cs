using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WindowsType
{
    GameOver,
    Menu,
    Game,
    GamePause,
    None = -1
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private WindowsType currectWindows = WindowsType.None;
    [SerializeField] private GameObject[] uiWindows;
    [SerializeField] private Text ScoreTextView;
    [SerializeField] private Text GameOverScoreTextView;
    [SerializeField] private Text DebugView;
    [SerializeField] private Text TimeLeft;
    [SerializeField] private Text BestScoreView;
    [SerializeField] private GameObject spawnBallPrefabs;
    [SerializeField] private GameObject rootSpawn;
    [SerializeField] private int CountEnemyObject;
    [SerializeField] private int CountGoodObject;
    [SerializeField] private GameObject[] FingerSpawn;
    [SerializeField] private int CurrentScore;
    [SerializeField] private List<GameObject> allSpawnBall;
    [SerializeField] private bool GamePause;
    [SerializeField] private GameObject light_ball;
    [SerializeField] private int TotalSeconds;
    private int CurrentTotalSeconds;
    
    private bool currentFingers;

    public static GameManager instance = null;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        InitializeManager();
    }
    /// <summary>
    /// First initialization
    /// </summary>
    private void InitializeManager()
    {
        if (!PlayerPrefs.HasKey("Top"))
            PlayerPrefs.SetInt("Top", 0);
        BestScoreView.text = "Best Score: " + PlayerPrefs.GetInt("Top"); 
        StartCoroutine(TimeLeftCoroutine());
        allSpawnBall= new List<GameObject>();
        for (int i = 0; i < CountEnemyObject; i++)
        {
            GameObject ball = Instantiate(spawnBallPrefabs, Vector3.zero, Quaternion.identity);
            ball.GetComponent<Ball>().Initialization(BallType.Enemy);
            ball.transform.parent = rootSpawn.transform;
            ball.SetActive(false);
            allSpawnBall.Add(ball);
        }
        for (int i = 0; i < CountGoodObject; i++)
        {
            GameObject ball = Instantiate(spawnBallPrefabs, Vector3.zero, Quaternion.identity);
            ball.GetComponent<Ball>().Initialization(BallType.Good);
            ball.transform.parent = rootSpawn.transform;
            ball.SetActive(false);
            allSpawnBall.Add(ball);
        }
        UpdateWindows(WindowsType.Menu);
    }
    /// <summary>
    /// Action new game
    /// </summary>
    public void StartNewGame()
    {
        
        CurrentTotalSeconds = 0;
        currentFingers = false;
        GamePause = false;
        CurrentScore = 0;
        UpdateScore();
        UpdateWindows(WindowsType.Game);
        SetVisible(true);
        SetPause(GamePause);
    }
    /// <summary>
    /// Action back to main menu
    /// </summary>
    public void ExitToMenu()
    {
        SetVisible(false);
        UpdateWindows(WindowsType.Menu);
    }
    /// <summary>
    /// Action for quit application
    /// </summary>
    public void ExitApp()
    {
        Application.Quit();
    }
    /// <summary>
    /// Event Kill ball
    /// </summary>
    /// <param name="type"></param>
    public void KillBall(BallType type)
    {
        if (type == BallType.Enemy)
            CurrentScore++;
        else
            CurrentScore -= 3;
        UpdateScore();
    }
    
    /// <summary>
    /// Check fingers and execute logic
    /// </summary>
    void Update()
    {
        if (currectWindows != WindowsType.Game) return;
        
        DebugView.text = "";
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (i == 2) break; // only two fingers
            switch (Input.GetTouch(i).phase)
            {
                case TouchPhase.Began:
                    FingerSpawn[i].gameObject.SetActive(true);
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    FingerSpawn[i].gameObject.transform.position = 
                        new Vector3(pos.x,pos.y,FingerSpawn[i].gameObject.transform.position.z);
                    break;
                case TouchPhase.Moved:
                    Vector3 pos1 = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    FingerSpawn[i].gameObject.transform.position = 
                        new Vector3(pos1.x,pos1.y,FingerSpawn[i].gameObject.transform.position.z);
                    break;
                case TouchPhase.Ended:
                    FingerSpawn[i].gameObject.SetActive(false);
                    ChangeTriggers(false);
                    break;
            }
        }
        ChangeTriggers(FingerSpawn[0].gameObject.activeSelf && FingerSpawn[1].gameObject.activeSelf);

    }
    /// <summary>
    /// Action show Game Pause
    /// </summary>
    public void ClickGamePause()
    {
        GamePause = !GamePause;
        SetPause(GamePause);

        if (GamePause)
        {
            UpdateWindows(WindowsType.GamePause);
        }
        else
        {
            UpdateWindows(WindowsType.Game);
        }
    }
    /// <summary>
    /// Action Show Game Over Menu
    /// </summary>
    public void GameOverMenu()
    {
        if (CurrentScore > PlayerPrefs.GetInt("Top"))
        {
            PlayerPrefs.SetInt("Top", CurrentScore);
        }
        SetVisible(false);
        UpdateWindows(WindowsType.GameOver);
        GameOverScoreTextView.text = "Total Score : " + CurrentScore.ToString();
    }
    /// <summary>
    /// Action restart gmae
    /// </summary>
    public void RestartGame()
    {
        SetVisible(false);
        StartNewGame();
    }
    /// <summary>
    /// Utils for control windows
    /// </summary>
    /// <param name="_typeWindows"></param>
    public void UpdateWindows(WindowsType _typeWindows)
    {
        if (_typeWindows == currectWindows) return;
        if (currectWindows  != WindowsType.None)
            uiWindows[(int)currectWindows].SetActive(false);
        currectWindows = _typeWindows;
        uiWindows[(int)currectWindows].SetActive(true);
    }
    /// <summary>
    /// Mass apply visible objects
    /// </summary>
    /// <param name="visible"></param>
    private void SetVisible(bool visible)
    {
        foreach (GameObject o in allSpawnBall)
        {
            o.SetActive(visible);
        }
        if (!visible)
        {
            light_ball.SetActive(false);
            FingerSpawn[0].SetActive(false);
            FingerSpawn[1].SetActive(false);
        }
    }
    /// <summary>
    /// Mass send pause\unpause for all moved objects
    /// </summary>
    /// <param name="pause"></param>
    private void SetPause(bool pause)
    {
        BallLighting look = light_ball.GetComponent<BallLighting>();
        if (look != null)
        {
            look.GamePause = pause;    
        }
        foreach (GameObject o in allSpawnBall)
        {
            Actor actor = o.GetComponent<Actor>();
            if (actor != null)
            {
                actor.GamePause = pause;
            }
        }
    }
    /// <summary>
    /// Coroutine for check time left of game
    /// </summary>
    /// <returns></returns>
    private IEnumerator  TimeLeftCoroutine()
    {
        while (true)
        {
            if (!GamePause && currectWindows == WindowsType.Game)
            {
                CurrentTotalSeconds++;
                UpdateScore();
                if (CurrentTotalSeconds == TotalSeconds)
                    GameOverMenu();
            }            
            yield return new WaitForSeconds(1);
        }
    }
    
    /// <summary>
    /// Enabled triggers for logic OnTriggers event
    /// </summary>
    /// <param name="fingers"></param>
    private void ChangeTriggers(bool fingers)
    {
        if (fingers == currentFingers) return;
        currentFingers = fingers;
        light_ball.gameObject.SetActive(fingers);
        light_ball.GetComponent<CircleCollider2D>().isTrigger = fingers;
        FingerSpawn[0].GetComponent<CircleCollider2D>().isTrigger = !fingers;
        FingerSpawn[1].GetComponent<CircleCollider2D>().isTrigger = !fingers;
    }
    /// <summary>
    /// Update current information time left, current score
    /// </summary>
    private void UpdateScore()
    {
        TimeLeft.text = "Time Left: " + (TotalSeconds - CurrentTotalSeconds);
        ScoreTextView.text = "Total Score : " + CurrentScore.ToString();
    }
}

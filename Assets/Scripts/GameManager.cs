using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum Windows
{
    GameOver,
    Menu,
    Game
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] uiWindows;
    [SerializeField] private Text ScoreTextView;
    [SerializeField] private GameObject spawnBallPrefabs;
    [SerializeField] private GameObject rootSpawn;
    [SerializeField] private int CountEnemyObject;
    [SerializeField] private int CountGoodObject;
    [SerializeField] private GameObject[] FingerSpawn;
    [SerializeField] private int CurrentScore;

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

    private void InitializeManager()
    {
        for (int i = 0; i < CountEnemyObject; i++)
        {
            GameObject ball = Instantiate(spawnBallPrefabs, Vector3.zero, Quaternion.identity);
            ball.GetComponent<Ball>().Initialization(BallType.Enemy);
            ball.transform.parent = rootSpawn.transform;
        }

        for (int i = 0; i < CountGoodObject; i++)
        {
            GameObject ball = Instantiate(spawnBallPrefabs, Vector3.zero, Quaternion.identity);
            ball.GetComponent<Ball>().Initialization(BallType.Good);
            ball.transform.parent = rootSpawn.transform;
        }

        CurrentScore = 0;
        UpdateScore();
    }

    public void KillBall(BallType type)
    {
        if (type == BallType.Enemy)
            CurrentScore++;
        else
            CurrentScore -= 3;
        UpdateScore();
    }
    private void UpdateScore()
    {
        ScoreTextView.text = "Total Score : " + CurrentScore;
    }
    private RaycastHit2D[] hit;
    private int number;
    void Update()
    {
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
                    break;
            }
        }
/*
        if (FingerSpawn[0].gameObject.activeSelf && FingerSpawn[1].gameObject.activeSelf)
        {
            
            Debug.DrawLine(FingerSpawn[0].gameObject.transform.position, FingerSpawn[1].gameObject.transform.position, Color.white);
            float distance =
                (FingerSpawn[0].gameObject.transform.position - FingerSpawn[1].gameObject.transform.position).magnitude;
            RaycastHit2D[] hits = Physics2D.RaycastAll(FingerSpawn[0].gameObject.transform.position,
                FingerSpawn[1].gameObject.transform.position, distance);
            foreach (RaycastHit2D h in hits)
            {
                Ball ball = h.collider.gameObject.GetComponent<Ball>();
                if (ball != null)
                {
                    ball.Kick(Random.Range(5,15));
                    UpdateScore();
                }
            }
        }
        */
    }
}

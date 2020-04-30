using UnityEngine;

public enum BallType{
    Enemy,
    Good
}

public class Ball : Actor
{
    [SerializeField] private float speed;
    private Vector2 target;
    [SerializeField] private Rect area;
    [SerializeField] [Range(20, 100)] private float HP;
    [SerializeField] private BallType _ballType;
    [SerializeField] [Range(20, 100)] private int MinimumViewHp = 20;
    [SerializeField] private Sprite[] sprites;
    private int maxHp = 100;

    /// <summary>
    /// First Initialization of object
    /// </summary>
    /// <param name="_type"></param>
    public void Initialization(BallType _type)
    {
        GamePause = false;
        _ballType = _type;
        GetComponent<SpriteRenderer>().sprite = sprites[(int)_ballType];
    }
    /// <summary>
    /// Event enable object (means new init position)
    /// </summary>
    void OnEnable()
    {
        transform.position = new Vector2(Random.Range(area.x, area.width), Random.Range(area.y, area.height));
        HP = _ballType == BallType.Good ? 80 : Random.Range(50, 70);
        speed = (maxHp - HP) / 10;
        updateScale();
    }
    /// <summary>
    /// if two objects bump
    /// </summary>
    /// <param name="kick"></param>
    public void Kick(int kick)
    {
        HP -= kick;
        updateScale();
        if (HP <= 0)
        {
            GameManager.instance.KillBall(_ballType);
            Destroy(gameObject);
        }
        speed = (maxHp - HP) / 10;
    }
    /// <summary>
    /// Update scale objects
    /// </summary>
    void updateScale()
    {
        gameObject.transform.localScale = new Vector3(
            0.3f * ((HP <= MinimumViewHp ? MinimumViewHp : HP) / 100), 
            0.3f * ((HP <= MinimumViewHp ? MinimumViewHp : HP) / 100),
            1);
    }
    /// <summary>
    /// Move object to position
    /// </summary>
    void Update()
    {
        if (GamePause) return;
        
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if (Vector2.Distance(transform.position, target) == 0.0f)
        {
            target = new Vector2(Random.Range(area.x, area.width), Random.Range(area.y, area.height));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType{
    Enemy,
    Good
}

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 target;
    [SerializeField] private Rect area;
    [SerializeField] [Range(20, 100)] private float HP;
    [SerializeField] private BallType _ballType;
    [SerializeField] [Range(20, 100)] private int MinimumViewHp = 20;
    [SerializeField] private Sprite[] sprites;
    private int maxHp = 100;

    public void Initialization(BallType _type)
    {
        _ballType = _type;
        GetComponent<SpriteRenderer>().sprite = sprites[(int)_ballType];
        transform.position = new Vector2(Random.Range(area.x, area.width), Random.Range(area.y, area.height));
        HP = _ballType == BallType.Good ? 80 : Random.Range(50, 70);
        speed = (maxHp - HP) / 10;
    }
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

    void updateScale()
    {
        gameObject.transform.localScale = new Vector3(
            0.3f * ((HP <= MinimumViewHp ? MinimumViewHp : HP) / 100), 
            0.3f * ((HP <= MinimumViewHp ? MinimumViewHp : HP) / 100),
            1);
    }
    void Update()
    {
        return;
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if (Vector2.Distance(transform.position, target) == 0.0f)
        {
            target = new Vector2(Random.Range(area.x, area.width), Random.Range(area.y, area.height));
        }
    }
}

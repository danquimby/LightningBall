using UnityEngine;

public class BallLighting  : Actor
{
    [SerializeField] private int speed = 12;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject root_target;

    /// <summary>
    /// event move ball lighting 
    /// </summary>
    void Update()
    {
        if (GamePause) return;
        
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        if (Vector2.Distance(transform.position, target.transform.position) == 0.0f)
        {
            transform.position = root_target.transform.position;
        }
    }
    /// <summary>
    /// Event bump to ball
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GamePause) return;
        Ball ball = other.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            ball.Kick(Random.Range(5,15));
        }
    }
}

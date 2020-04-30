using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public float maxdistance = 10;
    float temp;
    [SerializeField] private int speed = 30;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject root_target;
    private Rigidbody2D rb2D;
    private Vector2 velocity;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        velocity = new Vector2(1.75f, 1.1f);
        Vector3 mosuerPostion = target.transform.position;
        Vector2 direction = new Vector2(
            mosuerPostion.x - transform.position.x,
            mosuerPostion.y - transform.position.y);
        transform.up = direction;
    }
    // Update is called once per frame
    void Update()
    {
        //rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);
        
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        if (Vector2.Distance(transform.position, target.transform.position) == 0.0f)
        {
            //target = new Vector2(Random.Range(area.x, area.width), Random.Range(area.y, area.height));
            transform.position = root_target.transform.position;
        }
    }
}

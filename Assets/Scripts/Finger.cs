using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Finger : MonoBehaviour
{
    [SerializeField] private Text _text;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                ball.Kick(Random.Range(5,15));
            }
        }
        catch (Exception e)
        {
            _text.text = e.ToString();
        }
    }
}

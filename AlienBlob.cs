/*
 * Script by Chris McVickar AKA Lotkey/Synth Chris
 * Github: https://github.com/lotkey
 * Website: https://synthchrismusic.wixsite.com/music
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBlob : MonoBehaviour
{
    public Rigidbody2D body;
    public Rigidbody2D playerBody;

    public float speed = 10;
    public bool playerInRadius = false;
    private Vector2 vectorFromPlayer;
    private float distanceFromPlayer;
    public float radius = 1500f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerInRadius = CheckIfPlayerInRadius();
        if (playerInRadius)
        {
            MoveTowardsPlayer();
        }
    }

    bool CheckIfPlayerInRadius()
    {
        vectorFromPlayer = body.position - playerBody.position;
        distanceFromPlayer = (float)Math.Sqrt(vectorFromPlayer.x * vectorFromPlayer.x + vectorFromPlayer.y * vectorFromPlayer.y);
        return distanceFromPlayer < radius;
    }

    void MoveTowardsPlayer()
    {
        if(vectorFromPlayer.x > 0)
        {
            Vector2 velocity = body.velocity;
            velocity.x = -speed;
            body.velocity = velocity;
        }
        else if(vectorFromPlayer.x < 0)
        {
            Vector2 velocity = body.velocity;
            velocity.x = speed;
            body.velocity = velocity;
        }
        else
        {
            Vector2 velocity = body.velocity;
            velocity.x = 0;
            body.velocity = velocity;
        }
    }
}

/*
 * Script by Chris McVickar AKA Lotkey/Synth Chris
 * Github: https://github.com/lotkey
 * Website: https://synthchrismusic.wixsite.com/music
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public HealthBar healthbar;
    public int maxHealth = 100;
    public int health = 100;
    public float attackDistance;
    public int attackDamage = 10;
    // Start is called before the first frame update
    void Start()
    {
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Die()
    {
        healthbar.GetComponent<Canvas>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        this.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthbar.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }
}

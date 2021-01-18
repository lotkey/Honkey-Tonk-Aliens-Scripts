/*
 * Script by Chris McVickar AKA Lotkey/Synth Chris
 * Github: https://github.com/lotkey
 * Website: https://synthchrismusic.wixsite.com/music
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    public PlayerMovement movement;
    public HealthBar healthbar;
    public int maxHealth = 100;
    public int health = 100;
    public float attackDistance = .5f;
    public float attackRange = 1f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;
    #region KeyCodes
    KeyCode ATTACK = KeyCode.Space;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ATTACK))
        {
            Attack();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    private void Attack()
    {
        Vector2 position = movement.body.position;
        if (movement.isFacingRight)
        {
            position.x += attackDistance;
        }
        else
        {
            position.x -= attackDistance;
        }
        Collider2D[] enemies = Physics2D.OverlapCircleAll(position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnDrawGizmosSelected()
    {
        Vector2 position = movement.body.position;
        if (movement.isFacingRight)
        {
            position.x += attackDistance;
        }
        else
        {
            position.x -= attackDistance;
        }
        Gizmos.DrawWireSphere(position, attackRange);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] float canBeHurtDelay = 1f;
    public int currentHealth;
    public bool hasBeenHit = false;
    public bool hasDied = false;
    public bool hasReborn = false;
    Animator animator;
    CapsuleCollider2D myBodyCollider;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = health;
        myBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
                if (!hasBeenHit)
                {
                    TakeDamage();
                }

        }
    }
    public void TakeDamage()
    {
        hasBeenHit = true;
        currentHealth--;
        StartCoroutine(DelayCanBeHurt());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        StartCoroutine(Rebirth());

    }

    private IEnumerator Rebirth()
    {
        yield return new WaitForSeconds(4.5f);
      
        animator.SetBool("rebirthing", true);
        StartCoroutine(RebirthComplete());
      
    }

    private IEnumerator RebirthComplete()
    {
        yield return new WaitUntil(() => hasReborn = true);
        currentHealth = health;
        animator.SetBool("hasReborn", true);
    }

    IEnumerator DelayCanBeHurt()
    {
        yield return new WaitForSeconds(canBeHurtDelay);
        hasBeenHit = false;
    }
}


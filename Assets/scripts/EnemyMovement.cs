using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] AudioClip hitSFX;
    Rigidbody2D myRigidbody;
    Vector2 direction; 
    BoxCollider2D myBoxColl;
    bool isAttacked = false;
    GameObject enemy;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
        Attacked();
    }

    private void OnTriggerEnter2D(Collider2D other) {

        Debug.Log("Trigger Collider Event");
        if(!myBoxColl.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            moveSpeed = -(moveSpeed);
            FlipEnemyFacing();
        }
    }

    void Attacked()
    {
        if(myBoxColl.IsTouchingLayers(LayerMask.GetMask("Attack")))
        {
            if(!isAttacked)
            {
                AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position); 
                isAttacked = !isAttacked;
                Destroy(this.gameObject);
            }
        }
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)),1f);
        Debug.Log("Position: "+transform.localScale.x);
    }
}

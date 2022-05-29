using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidbody;
    BoxCollider2D myBoxColl;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        Debug.Log("Trigger Collider Event");
        if(!myBoxColl.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            moveSpeed = -(moveSpeed);
            FlipEnemyFacing();
        }

    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)),1f);
        Debug.Log("Position: "+transform.localScale.x);
    }
}

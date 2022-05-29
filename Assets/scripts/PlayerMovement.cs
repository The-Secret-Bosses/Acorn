using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    [SerializeField] float slideSpeed = 5f;
    [SerializeField] int hitpoint = 2;
    [SerializeField] float knockback = 5f;
    [SerializeField] float kbTime = 2f;
    private float offSetY = -0.855f;
    private float offSetX = 0.184f;

    GameObject attack;

    bool isAlive = true;
    bool isAttacked = false;
    private int direction;
    int totalJump;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myBoxColl;
    BoxCollider2D boxAttack;
    float gravityScaleAtStart;
    // Start is called before the first frame update
    void Start()
    {
        totalJump = 0;
        attack = GameObject.FindGameObjectsWithTag("Attack")[0];
        attack.SetActive(false);
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myBoxColl = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        boxAttack = attack.GetComponent(typeof(BoxCollider2D)) as BoxCollider2D;
    }

    bool HorizontalSpeed()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        return playerHasHorizontalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive){return;}
        Run();
        Sliding();
        FlipSprite();
        IsRunning();
        Attacked();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
    }

    void IsRunning()
    {

        if(!myAnimator.GetBool("isSliding"))
        {
            myAnimator.SetBool("isRunning",HorizontalSpeed());
        }
    }

    void FlipSprite()
    {
        if(HorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x),1f);

            float flipped = transform.localScale.x * boxAttack.offset.x;

            boxAttack.offset = new Vector2(flipped, boxAttack.offset.y);
        }
    }

    void OnJump(InputValue value)
    {
        if(!isAlive){return;}
        if(isAttacked){return;}
        bool isTouchingGround = myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if(value.isPressed && isTouchingGround)
        {
            myAnimator.SetTrigger("Jumping");
            myRigidbody.velocity += new Vector2(0f,jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if(value.isPressed)
        {
            myAnimator.SetTrigger("Attack!!");
        }
    }

    void Sliding()
    {
        if(myAnimator.GetBool("isSliding"))
        {
            float xVector = (transform.localScale.x * slideSpeed);

            Vector2 playerVelocity = new Vector2(xVector, myRigidbody.velocity.y);
            myRigidbody.velocity = playerVelocity;

        }

    }

    void OnSlide(InputValue value)
    {
        if(!isAlive){return;}
        if(isAttacked){return;}
        if(!myAnimator.GetBool("isSliding"))
        {
            myAnimator.SetBool("isSliding",true);
        }
    }

    void Attacked()
    {
        if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            if((!isAttacked && !(myAnimator.GetBool("isSliding"))))
            {
                if(hitpoint > 1)
                {
                    hitpoint--;
                    Debug.Log("Got attacked!!");
                    myAnimator.SetTrigger("Attacked");
                    myAnimator.SetBool("isRunning",false);
                    StartCoroutine("StopSlide");
                    isAttacked = !isAttacked;
                    float xVector = (transform.localScale.x * knockback);
                    Debug.Log("X-Vector: "+xVector);
                    Vector2 forcePush = new Vector2(6000f,20f);
                    float forceMagnitude =1f;
                    // Vector2 playerVelocity = new Vector2(xVector+knockback, myRigidbody.velocity.y+15f);
                    // myRigidbody.velocity = playerVelocity;

                    myRigidbody.AddForce(forcePush*forceMagnitude,ForceMode2D.Impulse);
                }
                else
                {
                    if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
                    {
                        myAnimator.SetTrigger("Dying");
                        myAnimator.SetBool("isRunning",false);
                        myAnimator.SetBool("isSliding",false);
                        myRigidbody.bodyType = RigidbodyType2D.Static;
                        myCapsuleCollider.enabled = false;
                    }
                }
            }
        }
    }

    

    IEnumerator Respawn()
    {
        yield return new WaitUntil(()=>{
            Debug.Log("Testing");

            myRigidbody.bodyType = RigidbodyType2D.Dynamic;
            myCapsuleCollider.enabled = true;
            hitpoint = 3;
            return true;
        });
    }

    IEnumerator StopKnockback()
    {
        yield return new WaitUntil(()=>{
            isAttacked = false;
            return true;
        });
    }

    IEnumerator Invincible()
    {
        yield return new WaitUntil(()=>{

            myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            myCapsuleCollider.isTrigger = true;
            
            return true;

        });
    }

    IEnumerator StopSlide()
    {

        yield return new WaitUntil(()=>{
            myAnimator.SetBool("isSliding",false);
            myCapsuleCollider.isTrigger = false;
            myBoxColl.enabled = false;
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;
            return true;
        });
        
    }
}

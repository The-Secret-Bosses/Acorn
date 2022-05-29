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
    [SerializeField] float slideTime = 0.4f;
    [SerializeField] int hitpoint = 2;
    [SerializeField] float knockback = 100f;
    [SerializeField] float kbTime = 0.4f;
    private float offSetY = -0.855f;
    private float offSetX = 0.184f;

    private GameObject player;
    private GameObject rebirth;
    bool isAlive = true;
    bool isAttacked = false;
    private int direction;
    int totalJump;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    float gravityScaleAtStart;
    // Start is called before the first frame update
    void Start()
    {
        totalJump = 0;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        rebirth = GameObject.FindGameObjectsWithTag("Respawn")[0];
        Debug.Log(player);
        Debug.Log(rebirth);
        rebirth.SetActive(false);
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
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
        if(!myAnimator.GetBool("Sliding"))
        {
            myAnimator.SetBool("isRunning",HorizontalSpeed());
        }
        
    }

    void FlipSprite()
    {
        if(HorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x),1f);
        }
    }

    void OnJump(InputValue value)
    {
        if(!isAlive){return;}
        bool isTouchingGround = myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if(value.isPressed)
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

            myRigidbody.AddForce(new Vector2(xVector,myRigidbody.velocity.y));

            StartCoroutine("StopSlide");
        }

    }

    void OnSlide(InputValue value)
    {
        
        if(!myAnimator.GetBool("isSliding"))
        {
            myAnimator.SetBool("isSliding",true);
        }
    }

    void Attacked()
    {
        if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            if((!isAttacked))
            {
                if(hitpoint > 0)
                {
                    hitpoint--;
                    Debug.Log("Got attacked!!");
                    myAnimator.SetTrigger("Attacked");
                    isAttacked = !isAttacked;
                    float xVector = (transform.localScale.x * knockback);
                    Debug.Log("X-Vector: "+xVector);
                    Debug.Log("X-Vector neg: "+(-(xVector)));
                    myRigidbody.AddForce(new Vector2(-(xVector),myRigidbody.velocity.y*3f));
                    StartCoroutine("StopKnockback");
                }
                else
                {
                    myAnimator.SetBool("isDead",true);

                    myRigidbody.bodyType = RigidbodyType2D.Static;
                    myCapsuleCollider.enabled = false;

                    StartCoroutine("CancelDeath");
                }
            }
        }
    }

    IEnumerator CancelDeath()
    {
        yield return new WaitForSeconds(0.4f);
        myAnimator.SetBool("isDead",false);
        player.SetActive(false);
        float xSet = 0f;
        float ySet = 0f;
        if(myRigidbody.velocity.x >0)
        {
            xSet = myRigidbody.velocity.x + offSetX;
        }
        else
        {
            xSet = myRigidbody.velocity.x - offSetX;
        }
        if(myRigidbody.velocity.y > 0)
        {
            ySet = myRigidbody.velocity.y - offSetY;
        }
        else
        {
            ySet = myRigidbody.velocity.y + offSetY;
        }

        rebirth.transform.Translate(new Vector2(xSet, ySet));
        rebirth.SetActive(true);
        myRigidbody.velocity = new Vector2(0,2f);
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;
        myCapsuleCollider.enabled = true;
        hitpoint = 1;
    }

    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(kbTime);
        isAttacked = false;
    }

    IEnumerator StopSlide()
    {

        yield return new WaitForSeconds(slideTime);
        myAnimator.SetBool("isSliding",false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    BoxCollider2D myBoxColl;
    // Start is called before the first frame update
    void Start()
    {
        myBoxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        FlipTheBox();
    }

    void FlipTheBox()
    {
        float flipped = transform.localScale.x * myBoxColl.offset.x;

        myBoxColl.offset = new Vector2(flipped, myBoxColl.offset.y);
    }

    // IEnumerator OnHit()
    // {
    //     yield return new WaitUntil(()=>{

    //         myBoxColl.enabled = true;
    //         StartCoroutine("ResetHit");
    //         return true;
    //     });
    // }


    // IEnumerator ResetHit()
    // {
    //     yield return new WaitForSeconds(1f);
    //     myBoxColl.enabled = false;
    // }
}

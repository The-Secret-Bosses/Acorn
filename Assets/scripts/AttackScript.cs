using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
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
        
    }


    IEnumerator OnHit()
    {
        yield return new WaitUntil(()=>{

            myBoxColl.enabled = true;
            StartCoroutine("ResetHit");
            return true;
        });
    }


    IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(1f);
        myBoxColl.enabled = false;
    }
}

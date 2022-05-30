using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPower : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(myBoxColl.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            StartCoroutine("DelayConsume");
        }
    }

    IEnumerator DelayConsume()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}

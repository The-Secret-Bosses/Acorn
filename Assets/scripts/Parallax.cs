using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Transform player;

    
    Vector2 startPosition;

    float startZ;

    Vector2 travel =>(Vector2) cam.transform.position - startPosition;

    float distanceFromPlayer => transform.position.z - player.position.z;
    float clippingPlane => (cam.transform.position.z) + (distanceFromPlayer > 0 ? cam.farClipPlane : cam.nearClipPlane);
    float parallaxFactor => Mathf.Abs(distanceFromPlayer) / clippingPlane;
    public void Start()
    {
        startPosition = transform.position;
        startZ = transform.transform.position.z;
    }

    public void Update()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}

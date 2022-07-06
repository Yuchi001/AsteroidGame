using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Mechanics : MonoBehaviour
{
    public float speed;
    public float range;

    private Rigidbody2D rb2d;

    private Vector2 startPos; 

    public GameObject explosionParticles;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }
    void Update()
    {
        rb2d.velocity = transform.up * speed;
        if (Vector2.Distance(startPos, transform.position) >= range) 
        {
            DestroyBullet();
        }
    }
    public void DestroyBullet()
    {
        ToolClass.SpawnParticles(explosionParticles, transform.position, Color.blue); 
        Destroy(gameObject);
    }
}

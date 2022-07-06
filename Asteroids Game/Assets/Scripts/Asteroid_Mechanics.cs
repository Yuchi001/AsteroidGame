using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Mechanics : MonoBehaviour
{
    [HideInInspector, Range(0,2)] public int asteroidSize;

    public Sprite[] asteroidSprites; 

    public float asteroidMoveSpeed; 
    public float asteroidRotationSpeed; 
    public float maxAsteroidRange; 

    private Vector2 spawnPos;

    public GameObject explosionParticles; 

    public Transform[] asteroidSpawnPos;

    public SpriteRenderer spriteRenderer; // dziecko asteroidy, rotacja

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spawnPos = transform.position;
        HealthManager.noHpLeft += DestroyAsteroid;
        SetRandomStats();
    }
    void Update()
    {
        rb2d.velocity = transform.up * asteroidMoveSpeed;
        spriteRenderer.transform.Rotate(0, 0, asteroidRotationSpeed);
        if(Vector2.Distance(transform.position, spawnPos) > maxAsteroidRange)
        {
            DestroyAsteroid();
        }
    }
    public void SetRandomStats()
    {
        float scaleModyficator = Random.Range(0.3f, 0.5f) * (1 + asteroidSize);

        transform.localScale = new Vector2(scaleModyficator, scaleModyficator);  // wielkość asteroidy
        spriteRenderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)]; // losowy sprite asteroidy
        asteroidMoveSpeed = Random.Range(asteroidMoveSpeed / 2, asteroidMoveSpeed); // pręskość asteroidy
    }
    public void DestroyAsteroid(bool muteAudio = false)
    {
        if (!muteAudio) ToolClass.PlayAudioClip(ToolClass.AudioLibrary.asteroidDestroyed);
        ToolClass.SpawnParticles(explosionParticles, transform.position, Color.grey, transform.localScale.x);
        Destroy(gameObject);
    }
    public void SpawnNewAsteroids()
    {
        for (int i = 0; i < 2; i++)
        {
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            Asteroid_Mechanics asteroid = Instantiate(this, asteroidSpawnPos[i].position, randomRotation);
            asteroid.asteroidSize = asteroidSize - 1;

            // upewniam się że nowe asteroidy na pewno mają aktywne komponenty (BARDZO WAŻNE)
            asteroid.enabled = true;
            asteroid.GetComponent<Collider2D>().enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("bullet"))
        {
            collider2D.GetComponent<Bullet_Mechanics>().DestroyBullet(); 

            ToolClass.PlayAudioClip(ToolClass.AudioLibrary.asteroidDestroyed);
            ToolClass.SpawnParticles(explosionParticles, transform.position, Color.grey);

            if(asteroidSize != 0)
            {
                SpawnNewAsteroids();
            }
            ScoreCalculation();
            DestroyAsteroid();
        }
    }
    public void ScoreCalculation()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null ? playerObject : false)
        {
            Vector2 playerPos = playerObject.transform.position;
            ScoreScript scoreScript = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>();
            scoreScript.ScoreFunction(transform.position, playerPos, asteroidSize);
        }
    }
    private void OnDestroy()
    {
        HealthManager.noHpLeft -= DestroyAsteroid;
    }
}

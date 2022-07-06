using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoScript : MonoBehaviour
{
    private Vector2 startPos;

    public GameObject healthUp;
    public GameObject shootUp;
    public GameObject explosionParticles;

    public float speed;
    public float range;

    private bool powerUpSpawned = false;
    private void Start()
    {
        startPos = transform.position;
        HealthManager.noHpLeft += DestroyUfo;
    }
    void Update()
    {
        if (Vector2.Distance(startPos, transform.position) > range) 
        {
            Destroy(gameObject);
        }
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }
    public void DestroyUfo(bool muteAudio = false)
    {
        if (!muteAudio) ToolClass.PlayAudioClip(ToolClass.AudioLibrary.asteroidDestroyed);
        ToolClass.SpawnParticles(explosionParticles, transform.position, Color.red);
        Destroy(gameObject);
    }
    public void SpawnPowerUp(ToolClass.PowerUpTypes powerUp)
    {
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        switch (powerUp)
        {
            case ToolClass.PowerUpTypes.healthUp:
                Instantiate(healthUp, transform.position, randomRotation);
                break;
            case ToolClass.PowerUpTypes.shootUp:
                Instantiate(shootUp, transform.position, randomRotation);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("bullet") && !powerUpSpawned)
        {
            powerUpSpawned = true;

            ToolClass.PowerUpTypes randomPowerUpType = (ToolClass.PowerUpTypes)Random.Range(0, 2);
            SpawnPowerUp(randomPowerUpType);
            DestroyUfo();
            Destroy(collider2D.gameObject);
        }
    }
    private void OnDestroy()
    {
        HealthManager.noHpLeft -= DestroyUfo;
    }
}

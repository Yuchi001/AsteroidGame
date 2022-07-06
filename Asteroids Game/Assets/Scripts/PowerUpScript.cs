using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public GameObject explosionParticles;

    public GameObject itemSprite; // chcemy aby obrót itemu nie wpływał na jego trajektorie więc, obracamy jego dziecko

    private Vector2 startPos;

    public Color particlesColor;

    public float range;
    public float speed;
    public float rotationSpeed;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        HealthManager.noHpLeft += DestroyPowerUp;
    }
    void Update()
    {
        if(Vector2.Distance(startPos, transform.position) > range)
        {
            DestroyPowerUp();
        }
        rb2d.velocity = transform.up * speed;
        itemSprite.transform.Rotate(0, 0, rotationSpeed);
    }
    public void DestroyPowerUp(bool muteAudio = false)
    {
        if (!muteAudio) ToolClass.PlayAudioClip(ToolClass.AudioLibrary.asteroidDestroyed);
        ToolClass.SpawnParticles(explosionParticles, transform.position, particlesColor);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        HealthManager.noHpLeft -= DestroyPowerUp;
    }
}

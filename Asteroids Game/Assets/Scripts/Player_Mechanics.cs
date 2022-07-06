using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mechanics : MonoBehaviour
{
    public Player_ScriptableObject spaceShip_properties; // specjalna zmienna przechowująca wszystkie podstawowe stałe dane gracza (maxZdrowie, maxPrędkość etc)

    private Rigidbody2D rb2d;

    private SpriteRenderer spriteRenderer;

    public GameObject explosionParticles;

    public Transform shootPosMain;
    public Transform shootPosLeft;
    public Transform shootPosRight;

    public Animator anim;

    public Vector2 mapBorders; // używam tylko jednej zmiennej bo kamera znajduje się w punkcie 0,0

    // statystyki gracza w czasie rzeczywistym  (line 46)
    [HideInInspector] public int player_hp;
    [HideInInspector] public float player_attackSpeed;
    [HideInInspector] public float player_movementSpeed;
    [HideInInspector] public bool canShoot = true; // tą zmienną manipulujemy w skrypcie "Player_ScriptableObject"

    [Range(0, 2)] public int shoot_PowerUpLevel = 0;
    public float spawnProtectionTime;
    private bool spawnProtection;

    private const int powerUpScore = 100;

    public delegate void GetHurt();
    public static event GetHurt getHurt;

    public delegate void GainHeart();
    public static event GainHeart gainHeart;

    void Start()
    {
        ToolClass.PlayAudioClip(ToolClass.AudioLibrary.respawn);

        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        StartCoroutine(SetSpawnProtection());
        SetPlayerStats(); 
    }
    void Update()
    {
        if(Input.GetMouseButton(0) && canShoot)
        {
            spaceShip_properties.Shoot(this);
        }
        if(Input.GetMouseButton(1))
        {
            rb2d.velocity += (Vector2)transform.up * player_movementSpeed * Time.deltaTime;
        }

        SetPlayerPosition();
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, GetAngle()); // gracz patrzy na myszkę
    }
    private void PlayerDeath()
    {
        shoot_PowerUpLevel = 0;
        getHurt?.Invoke();
        ToolClass.PlayAudioClip(ToolClass.AudioLibrary.playerHit);
        ToolClass.SpawnParticles(explosionParticles, transform.position, spaceShip_properties.spaceShipParticlesColor);
        Destroy(gameObject);
    }
    private void SetPlayerPosition()
    {
        Vector2 newPosition = CheckIfPlayerIsOutOfMap();
        if (newPosition != new Vector2(0,0)) transform.position = CheckIfPlayerIsOutOfMap();
    }
    private Vector2 CheckIfPlayerIsOutOfMap() // jeżeli gracz nie wykroczył poza granice mapy, funkcja zwraca aktualną pozycje gracza
    {
        if(transform.position.x > mapBorders.x)
        {
            return new Vector2(-mapBorders.x + 0.1f, transform.position.y);
        }
        if (transform.position.x < -mapBorders.x)
        {
            return new Vector2(mapBorders.x - 0.1f, transform.position.y);
        }
        if (transform.position.y > mapBorders.y)
        {
            return new Vector2(transform.position.x, -mapBorders.y + 0.1f);
        }
        if (transform.position.y < -mapBorders.y)
        {
            return new Vector2(transform.position.x, mapBorders.y - 0.1f);
        }
        return new Vector2(0, 0);
    }
    private void SetPlayerStats()
    {
        player_hp = spaceShip_properties.hp;
        player_attackSpeed = spaceShip_properties.attackSpeed;
        player_movementSpeed = spaceShip_properties.movementSpeed;
        spriteRenderer.sprite = spaceShip_properties.spaceShipSprite;
    }
    public float GetAngle() // ta funkcja zwraca kąt potrzebny do ustawienia gracza prostopadle do kursora
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 diff = (Vector2)transform.position - mousePos;

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90;
        return angle;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "asteroid":
                if(!spawnProtection)
                {
                    PlayerDeath();
                    collision.GetComponent<Asteroid_Mechanics>().DestroyAsteroid();
                }
                break;
            case "healthUp": // power up
                gainHeart?.Invoke();
                ToolClass.PlayAudioClip(ToolClass.AudioLibrary.powerUp_healthUp);
                collision.GetComponent<PowerUpScript>().DestroyPowerUp();
                GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>().AddAndPopUpScore(powerUpScore, transform.position);
                break;
            case "shootUp": // power up
                ToolClass.PlayAudioClip(ToolClass.AudioLibrary.powerUp_shoot);
                if (shoot_PowerUpLevel < 2) shoot_PowerUpLevel += 1;
                collision.GetComponent<PowerUpScript>().DestroyPowerUp();
                GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>().AddAndPopUpScore(powerUpScore, transform.position);
                break;
            case "ufo": 
                if (!spawnProtection)
                {
                    PlayerDeath();
                    collision.GetComponent<UfoScript>().DestroyUfo();
                }
                break;
        }
    }

    private IEnumerator SetSpawnProtection()
    {
        spawnProtection = true;
        yield return new WaitForSeconds(spawnProtectionTime);
        spawnProtection = false;
        anim.SetTrigger("endSpawnProtection");
    }
}

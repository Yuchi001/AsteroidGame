using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ta klasa jest przypięta do obiektu "healthManager" na scenie
public class HealthManager : MonoBehaviour
{
    public int playerHearts;

    public GameObject[] heartsObj; 
    public GameObject menu;

    public Spawner_Mechanics spawnerScript;

    private const float playerRespawnTime = 1.5f;
    private const bool muteAudioOnEndGame = true;

    public delegate void NoHpLeft(bool muteAudio = muteAudioOnEndGame);
    public static event NoHpLeft noHpLeft;
    void Start()
    {
        Player_Mechanics.getHurt += LoseHearts;
        Player_Mechanics.gainHeart += GainHearts;
        SetHearts();
    }
    public void EndGame()
    {
        noHpLeft?.Invoke();
        menu.SetActive(true);
    }
    public void LoseHearts()
    {
        playerHearts--;
        SetHearts();
        if(playerHearts == 0)
        {
            EndGame();
        }
        else StartCoroutine(RespawnPlayer());
    }
    public void GainHearts()
    {
        Player_Mechanics playerMechanics = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Mechanics>();
        int heartsQuantity = playerMechanics.spaceShip_properties.hp;
        if (playerHearts < heartsQuantity)
        {
            playerHearts++;
            SetHearts();
        }
    }
    public void SetHearts()
    {
        for (int i = playerHearts; i < 4; i++)
        {
            heartsObj[i].SetActive(false);
        }
        for (int i = 0; i < playerHearts; i++)
        {
            heartsObj[i].SetActive(true);
        }
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(playerRespawnTime);
        spawnerScript.StartSpawning();
    }

    private void OnDestroy()
    {
        Player_Mechanics.getHurt -= LoseHearts;
        Player_Mechanics.gainHeart -= GainHearts;
    }
}

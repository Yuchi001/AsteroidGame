using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Mechanics : MonoBehaviour
{
    public GameObject asteroidObj;
    public GameObject playerObj;
    public GameObject ufoObj;

    public Transform[] asteroidSpawnPos;

    public Transform[] ufoSpawnPos;

    private Player_Mechanics player_mechanics;
    IEnumerator Spawn()
    {
        while(GameObject.FindGameObjectWithTag("Player"))
        {
            if(SpawnUfo()) 
            {
                Vector2 ufoSpawnPosition = ufoSpawnPos[Random.Range(0, ufoSpawnPos.Length)].position;
                GameObject ufo = Instantiate(ufoObj, ufoSpawnPosition, Quaternion.identity);
                UfoScript ufoScript = ufo.GetComponent<UfoScript>();
                if (ufo.transform.position.x > GameObject.FindGameObjectWithTag("Player").transform.position.x)
                {
                    ufoScript.speed *= -1;
                }
            }
            else
            {
                Vector2 asteroidSpawnPosition = asteroidSpawnPos[Random.Range(0, asteroidSpawnPos.Length)].position;
                GameObject asteroid = Instantiate(asteroidObj, asteroidSpawnPosition, Quaternion.identity);
                SetAsteroidStats(asteroid);
                yield return new WaitForSeconds(1f);
            }
        }
    }
    public void SetAsteroidStats(GameObject go)
    {
        Asteroid_Mechanics asteroid = go.GetComponent<Asteroid_Mechanics>();
        asteroid.transform.rotation = Quaternion.Euler(0, 0, GetAngle(asteroid.gameObject));
        asteroid.asteroidSize = Random.Range(0, 3);
    }
    public bool SpawnUfo()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber < 5) return true;
        return false;
    }
    public void StartSpawning()
    {
        player_mechanics = Instantiate(playerObj, new Vector2(0,0), Quaternion.identity).GetComponent<Player_Mechanics>();
        StartCoroutine(Spawn());
    }
    public float GetAngle(GameObject asteroid) // ta funkcja zwraca kąt potrzebny do odwórcenia asteroidy w stronę gracza 
    {
        Vector2 playerPos = player_mechanics.transform.position;

        Vector2 diff = (Vector2)asteroid.transform.position - playerPos;

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90;
        return angle;
    }
}

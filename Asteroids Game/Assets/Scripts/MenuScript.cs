using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    private const float waitBeforeExit = 0.3f;
    public Spawner_Mechanics spawner;
    public HealthManager healthManager;
    public void OnPlay()
    {
        ToolClass.PlayAudioClip(ToolClass.AudioLibrary.button);
        healthManager.playerHearts = 3;
        healthManager.SetHearts();
        spawner.StartSpawning();
        gameObject.SetActive(false);
    }
    public void OnExit()
    {
        ToolClass.PlayAudioClip(ToolClass.AudioLibrary.button);
        StartCoroutine(WaitBeforeExiting());
    }
    private IEnumerator WaitBeforeExiting()
    {
        yield return new WaitForSeconds(waitBeforeExit);
        Application.Quit();
    }
}

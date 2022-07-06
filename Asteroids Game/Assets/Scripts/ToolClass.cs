using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// klasa z pomocnymi narzędziami
public static class ToolClass
{
    public enum PowerUpTypes {healthUp, shootUp};
    public enum AudioLibrary {playerHit, asteroidDestroyed, powerUp_healthUp, powerUp_shoot, button, shoot, respawn};
    public static void SpawnParticles(GameObject particles, Vector2 position, Color color, float scale = 1)
    {
        GameObject particlesObject = GameObject.Instantiate(particles, position, Quaternion.identity);
        particlesObject.transform.localScale = new Vector2(scale, scale);
        ParticleSystem.MainModule particlesMainModule = particlesObject.GetComponent<ParticleSystem>().main;
        particlesMainModule.startColor = color;
        GameObject.Destroy(particlesObject, 3f);
    }
    public static void PlayAudioClip(AudioLibrary audioClipType)
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        audioManager.PlayAudioClip(audioClipType);
    }
}

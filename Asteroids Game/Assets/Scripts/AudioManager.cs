using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioPropertiesClass[] audioProperties;
        
    public AudioSource audioSourceObj;

    private const float volume = 0.4f;
    private const float audioSourceLifeTime = 1f;

    public void PlayAudioClip(ToolClass.AudioLibrary audioClipType)
    {
        foreach(AudioPropertiesClass clipProps in audioProperties)
        {
            if(clipProps.clipType == audioClipType && clipProps.audioClips.Length > 0)
            {
                int randomIndex = Random.Range(0, clipProps.audioClips.Length);
                AudioSource audioSource = Instantiate(audioSourceObj, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(clipProps.audioClips[randomIndex], volume);
                Destroy(audioSource.gameObject, audioSourceLifeTime);
            }
        }
    }

    [System.Serializable]
    public class AudioPropertiesClass
    {
        public ToolClass.AudioLibrary clipType;
        public AudioClip[] audioClips;
    }
}

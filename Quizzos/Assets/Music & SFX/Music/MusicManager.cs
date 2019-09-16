using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicTrack { Menu, WorldMap, Combat, BossFight};

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip menuTrack;
    [SerializeField] AudioClip worldMapTrack;
    [SerializeField] AudioClip combatTrack;
    [SerializeField] AudioClip bossFightTrack;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menuTrack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMusicTrack(int trackToPlay)
    {
        switch ((MusicTrack)trackToPlay)
        {
            case MusicTrack.Menu:
                audioSource.volume = 1f;
                audioSource.clip = menuTrack;
                break;
            case MusicTrack.WorldMap:
                audioSource.volume = 1f;
                audioSource.clip = worldMapTrack;
                break;
            case MusicTrack.Combat:
                audioSource.volume = 0.35f;
                audioSource.clip = combatTrack;
                break;
            case MusicTrack.BossFight:
                audioSource.volume = 0.7f;
                audioSource.clip = bossFightTrack;
                break;
        }
        audioSource.Play();
    }
}

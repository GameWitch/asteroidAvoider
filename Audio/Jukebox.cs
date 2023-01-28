using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    AudioController audioController;
    List<AudioType> audioTypes;

    private void Start()
    {
        audioController = GameObject.FindGameObjectWithTag("Core").GetComponent<AudioController>();
        audioTypes = audioController.GetMusicPlaylist();

        audioController.PlayAudio(audioTypes[0]);
    }
}

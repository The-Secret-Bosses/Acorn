using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    MusicPlayer musicPlayer;
    [SerializeField] AudioClip newLevelMusic;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        musicPlayer.levelMusic = newLevelMusic;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

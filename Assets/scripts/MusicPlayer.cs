
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip levelMusic;
    AudioSource audioSource;
    private void Awake()
    {


        int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
        if (numMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        AudioSource.PlayClipAtPoint(levelMusic, Camera.main.transform.position);
    }
}
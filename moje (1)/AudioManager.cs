using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound[] music;
    public Sound[] sounds;
    private int songIndex;
    bool isPaused = false;
    [SerializeField]
    private GameObject musicPanel;
    [SerializeField]
    private TMP_Text nowPlayingText;
    private float timer = 3;
    public float currentVolume;
    private float timePlaying;

  

    private void Awake()
    {
        foreach(Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.playOnAwake = false;
        }
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        songIndex = Random.Range(0, music.Length);
        PlaySong(songIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextSong();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            PreviousSong();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseSong();
        }
        SongEnds();

    }

    public void PlaySong(int index)
    {
        timePlaying = 0;
        if (music[index] != null)
        {
            music[index].source.Play();
        }
        nowPlayingText.text = "Now Playing:\n " + music[index].name + "\n by\n " + music[index].artist; 
        isPaused = false;
        timer = 3;
        StopAllCoroutines();
        StartCoroutine(ShowMusicUI());
    }
    public void NextSong()
    {
            songIndex++;
            foreach (Sound s in music)
            {
                s.source.Stop();
            }
            if (songIndex > music.Length - 1)
            {
                songIndex = 0;
            }
            PlaySong(songIndex);
    }

    public void PreviousSong()
    { 
        songIndex--;
        foreach (Sound s in music)
        {
        s.source.Stop();
        }
        if (songIndex < 0)
        {
            songIndex = music.Length - 1;
        }
        PlaySong(songIndex);
    }

    public void PauseSong()
    {
        if (!isPaused)
        {
            if (music[songIndex] != null)
            {
                music[songIndex].source.Pause();
                isPaused = true;
            }
        }
        else
        {
            music[songIndex].source.UnPause();
            isPaused = false;
        }
        timer = 3;
        StopAllCoroutines();
        StartCoroutine(ShowMusicUI());
    }


    public IEnumerator ShowMusicUI()
    {
        musicPanel.SetActive(true);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return false;
        }
        musicPanel.SetActive(false);
    }

    public void SetVolume(float value)
    {
        foreach(Sound s in music)
        {
            s.source.volume = value;
            currentVolume = value;
        }
    }

    public void SongEnds()
    {
        if (!isPaused)
        {
            timePlaying += Time.deltaTime;
        }
        if (timePlaying >= music[songIndex].audioClip.length)
            NextSong();
    }

}

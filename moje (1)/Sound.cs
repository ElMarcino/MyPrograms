using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{

    public string name;
    public string artist;

    public AudioClip audioClip;

    [HideInInspector]
    public AudioSource source;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeValue : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _musicVolume = 1f;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("VolumeSave"))
        {
            _musicVolume = PlayerPrefs.GetFloat("VolumeSave");
        }    
    }

    private void Update()
    {
        _audioSource.volume = _musicVolume;
    }

    public void SetVolume(float vol)
    {
        _musicVolume = vol;
        PlayerPrefs.SetFloat("VolumeSave", vol);
        PlayerPrefs.Save();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private FMOD.Studio.Bus m_musicBus;
    private FMOD.Studio.Bus m_sfxBus;
    private FMOD.Studio.Bus m_masterBus;

    private float m_musicvolume = 1f;
    private float m_sfxVolume = 1f;
    private float m_masterVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", m_musicvolume);
            PlayerPrefs.SetFloat("sfxVolume", m_musicvolume);
            PlayerPrefs.SetFloat("masterVolume", m_musicvolume);
            Load();
        }

        else
        {
            Load();
        }

        m_musicBus = FMODUnity.RuntimeManager.GetBus("Bus:/Music");
        m_sfxBus = FMODUnity.RuntimeManager.GetBus("Bus:/SFX");
        m_masterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        SetBusVolumes();
    }

    private void SetBusVolumes() {
        m_musicBus.setVolume(m_musicvolume);
        m_sfxBus.setVolume(m_sfxVolume);
        m_masterBus.setVolume(m_masterVolume);
    }

    public void ChangeMasterVolume(float newVolume) {
        m_masterVolume = newVolume;
        SetBusVolumes();
        Save();
    }
    
    public void ChangeMusicVolume(float newVolume) {
        m_musicvolume = newVolume;
        SetBusVolumes();
        Save();
    }
    
    public void ChangeSFXVolume(float newVolume) {
        m_sfxVolume = newVolume;
        SetBusVolumes();
        Save();
    }

    private void Load()
    {
        m_musicvolume = PlayerPrefs.GetFloat("musicVolume");
        m_sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        m_masterVolume = PlayerPrefs.GetFloat("masterVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", m_musicvolume);
        PlayerPrefs.SetFloat("sfxVolume", m_sfxVolume);
        PlayerPrefs.SetFloat("masterVolume", m_masterVolume);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuSettings : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] GameObject saveButton;

    [Header("Controls")]
    [SerializeField] AudioMixer audioMixer;
    [Space]
    [SerializeField] Slider masterSlider, musicSlider, sfxSlider;

    [Header("Save")]
    [SerializeField] string saveName;
    [SerializeField] SettingsData settingsData;

    void Start()
    {
        Load();
        LoadAllVolume();
    }

    public void SetMasterVolume(float value)
    {
        SetVolume("MasterVolume", masterSlider.value);
        settingsData.masterVolume = value;
    }

    public void SetMusicVolume(float value)
    {
        SetVolume("MusicVolume", musicSlider.value);
        settingsData.musicVolume = value;
    }

    public void SetSfxVolume(float value)
    {
        SetVolume("SfxVolume", sfxSlider.value);
        settingsData.sfxVolume = value;
    }
    void LoadAllVolume()
    {
        LoadVolume(settingsData.masterVolume, "MasterVolume", masterSlider);
        LoadVolume(settingsData.musicVolume, "MusicVolume", musicSlider);
        LoadVolume(settingsData.sfxVolume, "SfxVolume", sfxSlider);
    }

    public void SaveAllVolume()
    {
        SaveVolume("MasterVolume", masterSlider.value);
        SaveVolume("MusicVolume", musicSlider.value);
        SaveVolume("SfxVolume", sfxSlider.value);
    }

    void SetVolume(string parameter, float value)
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(value) * 20);
    }

    void SaveVolume(string parameter, float value)
    {
        PlayerPrefs.SetFloat(parameter, value);
    }

    public void LoadVolume(float savedValue,string parameter, Slider slider)
    {
        audioMixer.SetFloat(parameter, savedValue);
        slider.value = savedValue;
    }

    public void Load()
    {
       SettingsData savedSettingsData = JsonSaveSystem.Load<SettingsData>(saveName);
       if (savedSettingsData != null) settingsData = savedSettingsData;
    }

    public void Save()
    {
        JsonSaveSystem.Save<SettingsData>(saveName, settingsData);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadScene(int scene)
    {
        Resume();
        SceneManager.LoadScene(scene);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        SaveAllVolume();

        Application.Quit();
    }


    [System.Serializable]
    public class SettingsData
    {
        public float masterVolume = 1;
        public float musicVolume = 1;
        public float sfxVolume = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Image soundOn;
    [SerializeField] Image soundOff;
    [SerializeField] private AudioSource soundAudioSource; // Gunakan AudioSource terpisah untuk SFX
    private bool muted = false;
    private static SoundManager soundManager;

    private void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("soundMuted"))
        {
            PlayerPrefs.SetInt("soundMuted", 0);
            Load();
        }
        else
        {
            Load();
        }

        UpdateButtonIcon();
        soundAudioSource.mute = muted; // Gunakan mute pada AudioSource untuk mengontrol SFX
    }

    public void OnButtonPress()
    {
        muted = !muted; // Toggle status muted
        soundAudioSource.mute = muted; // Mengubah mute pada AudioSource

        Save();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        soundOn.enabled = !muted;
        soundOff.enabled = muted;
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("soundMuted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("soundMuted", muted ? 1 : 0);
    }
}

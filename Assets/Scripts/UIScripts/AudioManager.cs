using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

    public bool isMusicMuted;
    public bool isSFXMuted;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

    private bool _musicOn = true;
    public void ChangeMusicState()
    {
        if (_musicOn)
        {
            SetSoundMuted("Music", true);
        }
        else
        {
            SetSoundMuted("Music", false);
        }
        _musicOn = !_musicOn;
    }

    private bool _sfxOn = true;
    public void ChangeSFXState()
    {
        if (_sfxOn)
        {
            SetSoundMuted("Click", true);
            sounds[0].volume = 0f;
        }
        else
        {
            SetSoundMuted("Click", false);
            sounds[0].volume = 0.64f;
        }
        _sfxOn = !_sfxOn;
    }

    public void SetSoundMuted(string sound, bool state)
    {
        Array.Find(sounds, item => item.name == sound).source.mute = state;
    }
    

	public void Play(string sound)
	{
        if (isMusicMuted && sound.Contains("Music") || isSFXMuted && !sound.Contains("Music"))
            return;

        Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
        
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		s.source.PlayOneShot(s.clip);
	}
}

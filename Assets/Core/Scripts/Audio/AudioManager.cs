using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Instance
    private static AudioManager m_instance;
    public static AudioManager Use { get => m_instance; }

    private void OnEnable ()
    {
        m_instance = this;
    }
    #endregion

    [SerializeField] private string m_resourcesFolder = "Audio";
    [SerializeField] private string m_musicFolder = "Music";
    [SerializeField] private string m_soundsFolder = "Sounds";

    [Header("Preferenses")]
    [SerializeField] [Range(0f, 10f)] private float m_smoothSpeed = 3f;
    [SerializeField] private AudioMixerGroup m_musicAudioMixer;
    [SerializeField] private AudioMixerGroup m_soundsAudioMixer;

    private float m_musicVolume = 1f;
    private float m_soundsVolume = 1f;

    public bool MuteMusic { get; private set; } = false;
    public bool MuteSounds { get; private set; } = false;

    private AudioSource m_lastMusicSource;
    private AudioSource m_currentMusicSource;
    public AudioSource CachedSoundSource { get; private set; }

    private List<Ambient> m_ambients = null;

    private void LateUpdate ()
    {
        Attenuation();
    }

    public void SetMusicVolume (float volume)
    {
        m_musicVolume = volume;
        m_currentMusicSource.volume = volume;
    }

    public void SetMuteMusic (bool state)
    {
        MuteMusic = state;

        if (m_currentMusicSource != null)
            m_currentMusicSource.mute = state;
    }

    public void SetMuteSounds (bool state)
    {
        MuteSounds = state;

        if (m_ambients != null)
            foreach (var Ambient in m_ambients)
                Ambient.UpdateMute();
    }

    public void PlayMusic (string trackName, bool loop = false, float volume = 1f, float delay = 0f)
    {
        m_musicVolume = volume;

        if (string.IsNullOrEmpty(trackName)) return;
        StartCoroutine(GetMusicTrack(trackName, loop, delay));
    }

    public void PlaySound (string soundName, bool goCache = false, float volume = 1f)
    {
        m_soundsVolume = volume;

        if (string.IsNullOrEmpty(soundName)) return;
        StartCoroutine(GetSoundEffect(soundName, goCache));
    }

    public void StopSound ()
    {
        try
        {
            CachedSoundSource.Stop();
        } catch { }
    }

    public void KillAllAmbients () { KillAllAmbients(0f); }
    public void KillAllAmbients (float duration)
    {
        if (m_ambients == null || m_ambients.Count == 0)
            return;

        foreach (var ambient in m_ambients)
            ambient.Kill(duration);

        m_ambients = new List<Ambient>();
    }

    public void PlayAmbient (string ambient, float volume) { PlayAmbient(ambient, volume, 0f); }
    public void PlayAmbient (string ambient, float volume, float delay)
    {
        if (m_ambients == null)
            m_ambients = new List<Ambient>();

        GameObject Object = new GameObject(string.Format("{0} (Ambient)", ambient));
        Ambient Ambient = Object.AddComponent<Ambient>();
        Ambient.Play(ambient, volume, delay);

        Object.transform.parent = transform;
        m_ambients.Add(Ambient);
    }

    private void Attenuation ()
    {
        if (m_lastMusicSource != null)
        {
            m_lastMusicSource.volume = Mathf.Lerp(m_lastMusicSource.volume, 0f, m_smoothSpeed * Time.deltaTime);
            m_currentMusicSource.volume = Mathf.Lerp(m_currentMusicSource.volume, m_musicVolume,
                m_smoothSpeed * Time.deltaTime);

            if (m_lastMusicSource.volume < 0.05f)
            {
                m_lastMusicSource.volume = 0f;
                Destroy(m_lastMusicSource.gameObject);
            }
        }
    }

    private IEnumerator GetMusicTrack (string trackName, bool loop, float delay)
    {
        ResourceRequest Request = LoadAsync(string.Format("{0}/{1}", m_musicFolder, trackName));
        while (!Request.isDone) yield return null;

        AudioClip Clip = (AudioClip)Request.asset;
        if (Clip == null) yield break;

        yield return new WaitForSeconds(delay);

        m_lastMusicSource = m_currentMusicSource;

        GameObject Object = new GameObject(string.Format("{0} (Music Track)", trackName));
        AudioSource Source = Object.AddComponent<AudioSource>();
        Object.transform.parent = transform;
        Source.outputAudioMixerGroup = m_musicAudioMixer;
        Source.playOnAwake = false;
        Source.loop = loop;
        Source.mute = MuteMusic;
        Source.volume = (m_lastMusicSource == null) ? m_musicVolume : 0f;
        Source.clip = Clip;
        Source.Play();
        m_currentMusicSource = Source;
    }
    
    private IEnumerator GetSoundEffect (string soundName, bool goCache = false)
    {
        ResourceRequest Request = LoadAsync(string.Format("{0}/{1}", m_soundsFolder, soundName));
        while (!Request.isDone) yield return null;

        AudioClip Clip = (AudioClip)Request.asset;
        if (Clip == null) yield break;

        GameObject Object = new GameObject(string.Format("{0} (Sound Effect)", soundName));
        AudioSource Source = Object.AddComponent<AudioSource>();

        if (goCache)
            CachedSoundSource = Source;
        
        Object.transform.parent = transform;
        Source.outputAudioMixerGroup = m_soundsAudioMixer;
        Source.playOnAwake = false;
        Source.loop = false;
        Source.mute = MuteSounds;
        Source.volume = m_soundsVolume;
        Source.clip = Clip;
        Source.Play();
        Destroy(Object, Clip.length);
    }

    private ResourceRequest LoadAsync (string resourceName)
    {
        string path = string.Format("{0}/{1}", m_resourcesFolder, resourceName);
        return Resources.LoadAsync<AudioClip>(path);
    }
}
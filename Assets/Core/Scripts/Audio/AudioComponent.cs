using UnityEngine;
using UnityEngine.Audio;

public class AudioComponent : MonoBehaviour
{
    [SerializeField]
    private AudioMixerGroup m_AudioMixer = null;

    [SerializeField]
    private AudioClip m_AudioClip = null;
    private AudioSource m_AudioSource;

    [SerializeField]
    private bool m_Loop = false;

    [SerializeField] [Range(0f, 1f)]
    private float m_Volume = .7f;

    private void Start()
    {
        m_AudioSource = gameObject.AddComponent<AudioSource>();
        m_AudioSource.clip = m_AudioClip;
        m_AudioSource.outputAudioMixerGroup = m_AudioMixer;
        m_AudioSource.playOnAwake = false;
        m_AudioSource.loop = m_Loop;
        m_AudioSource.mute = SaveManager.Use.GetBool("mute-sfx");
        m_AudioSource.volume = m_Volume;
        m_AudioSource.Play();
    }

    public void UpdateMute()
    {
        m_AudioSource.mute = SaveManager.Use.GetBool("mute-sfx");
    }
}
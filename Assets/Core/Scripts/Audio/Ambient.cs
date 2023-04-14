using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Ambient : MonoBehaviour
{
    public void Play (string ambient, float volume, float delay)
    {
        if (string.IsNullOrEmpty(ambient)) return;
        StartCoroutine(GetAmbient(ambient, volume, delay));
    }

    public void UpdateMute ()
    {
        var Source = GetComponent<AudioSource>();
        if (Source != null)
            Source.mute = AudioManager.Use.MuteSounds;
    }

    public void Kill (float duration)
    {
        var Source = GetComponent<AudioSource>();
        Source.DOFade(0f, duration);

        Destroy(gameObject, duration);
    }

    private IEnumerator GetAmbient (string ambient, float volume, float delay)
    {
        ResourceRequest Request = LoadAsync(ambient);
        while (!Request.isDone) yield return null;

        AudioClip Clip = (AudioClip)Request.asset;
        if (Clip == null) yield break;

        yield return new WaitForSeconds(delay);

        AudioSource Source = gameObject.AddComponent<AudioSource>();
        Source.playOnAwake = false;
        Source.loop = true;
        Source.mute = AudioManager.Use.MuteSounds;
        Source.volume = volume;
        Source.clip = Clip;
        Source.Play();
    }

    private ResourceRequest LoadAsync (string ambient)
    {
        string path = string.Format("Audio/Ambient/{0}", ambient);
        return Resources.LoadAsync<AudioClip>(path);
    }
}

[System.Serializable]
public struct AmbientData
{
    public string Ambient;

    [Range(0f, 1f)] public float Volume;
    [Range(0f, 10f)] public float Delay;
}
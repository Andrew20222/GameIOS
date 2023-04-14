using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

[RequireComponent(typeof(Toggle))]
public class SoundToggle : MonoBehaviour
{
    private enum Mode
    {
        Classic,
        Extended
    }

    [SerializeField]
    private Mode m_Mode = Mode.Classic;
    private bool m_IsClassic => m_Mode == Mode.Classic;

    [SerializeField] [HideIf(nameof(m_IsClassic))]
    private List<string> m_AudioClips;

    [SerializeField] [ShowIf(nameof(m_IsClassic))]
    private string m_AudioClip = "UI Click";

    [SerializeField] [HideIf(nameof(m_IsClassic))] [Space]
    private bool m_Randomize;

    [SerializeField] [Range(0f, 1f)]
    private float m_Volume = .3f;
    private int m_Index = 0;

    private void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(OnButtonClick);
    }

    private void OnButtonClick(bool value)
    {
        if (m_Mode == Mode.Classic)
        {
            if (!string.IsNullOrEmpty(m_AudioClip))
                AudioManager.Use.PlaySound(m_AudioClip, false, m_Volume);
        }
        else
        {
            if (m_Randomize)
            {
                int index = Random.Range(0, m_AudioClips.Count);
                AudioManager.Use.PlaySound(m_AudioClips[index], false, m_Volume);
            }
            else
            {
                AudioManager.Use.PlaySound(m_AudioClips[m_Index], false, m_Volume);

                if (m_Index + 1 == m_AudioClips.Count - 1)
                    m_Index = 0;
                else m_Index++;
            }
        }

        AudioManager.Use.PlaySound(m_AudioClip, false, m_Volume);
    }

    private void OnDestroy()
    {
        GetComponent<Toggle>()?.onValueChanged.RemoveListener(OnButtonClick);
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Use;
    public string Data => JsonConvert.SerializeObject(m_Data);
    public Dictionary<string, object> LocalData => m_Data;

    private Dictionary<string, object> m_Data;

    public void Awake()
    {
        Use = this;

        m_Data =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString(SavesConstants.SAVE_NAME))
            ?? new Dictionary<string, object>();

        AudioManager.Use.SetMuteSounds(GetBool(SavesConstants.MUTE_SOUNDS));
        AudioManager.Use.SetMuteMusic(GetBool(SavesConstants.MUTE_MUSIC));
    }

    public void Replace(Dictionary<string, object> data)
    {
        m_Data = new Dictionary<string, object>(data);
        Save();
    }

    public void Replace(string rawData)
    {
        m_Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(rawData);
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetString(SavesConstants.SAVE_NAME, JsonConvert.SerializeObject(m_Data));
        PlayerPrefs.Save();
    }

    public bool HasKey(string key)
    {
        return m_Data.ContainsKey(key);
    }

    public void DeleteKey(string key)
    {
        if (!HasKey(key))
            return;

        m_Data.Remove(key);
        Save();
    }

    public void SetValue(string key, object value)
    {
        if (HasKey(key))
            m_Data[key] = value;
        else m_Data.Add(key, value);

        switch (key)
        {
            case SavesConstants.MUTE_MUSIC:
                AudioManager.Use.SetMuteMusic((bool)value);
                break;

            case SavesConstants.MUTE_SOUNDS:
                AudioManager.Use.SetMuteSounds((bool)value);
                break;
        }

        Save();
    }

    public object GetValue(string key, object value = null)
    {
        return HasKey(key) ? m_Data[key] : value;
    }

    public void SetVector(string key, Vector3 value)
    {
        SetValue(key, JsonConvert.SerializeObject(new VectorData(value)));
    }

    public Vector3 GetVector3(string key, Vector3 value)
    {
        try
        {
            VectorData vectorData = JsonConvert.DeserializeObject<VectorData>((string)GetValue(key));
            return new Vector3(vectorData!.X, vectorData!.Y, vectorData!.Z);
        }
        catch
        {
            return value;
        }
    }

    public int GetInt(string key, int value = 0) => Convert.ToInt32(GetValue(key, value));
    public float GetFloat(string key, float value = 0f) => Convert.ToSingle(GetValue(key, value));
    public byte GetByte(string key, byte value = 0) => Convert.ToByte(GetValue(key, value));
    public string GetString(string key, string value = "") => (string)GetValue(key, value);
    public bool GetBool(string key, bool value = false) => (bool)GetValue(key, value);
    public Vector2 GetVector2(string key, Vector2 value) => GetVector3(key, value);

    public class VectorData
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public VectorData(Vector3 input)
        {
            X = input.x;
            Y = input.y;
            Z = input.z;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // 用于播放背景音效
    [SerializeField]
    private AudioSource m_BgmAudioSource;

    // 用于播放特效音乐
    [SerializeField]
    private AudioSource m_EffectAudioSource;

    void Start()
    {
        m_BgmAudioSource.loop = true;
        m_BgmAudioSource.playOnAwake = true;

        m_EffectAudioSource.loop = false;
        m_EffectAudioSource.playOnAwake = false;
    }

    #region 背景音乐

    /// <summary>
    /// 播放背景音乐 
    /// </summary>
    /// <param name="clip"></param>
    public void PlayBgMusic(AudioClip clip)
    {
        if(clip == null)
        {
            Log.Error("没有找到背景音乐");
            return;
        }

        m_BgmAudioSource.clip = clip;
        m_BgmAudioSource.Play();
    }

    /// <summary>
    /// 停止背景音乐 
    /// </summary>
    public void StopBgMusic()
    {
        m_BgmAudioSource.clip = null;
        m_BgmAudioSource.Stop();
    }

    #endregion

    #region 音效音乐

    /// <summary>
    /// 根据资源名称播放音效 
    /// </summary>
    /// <param name="name"></param>
    public void PlayEffectMusic(string name)
    {
        AudioClip asset = ResourcesManager.Instance.GetAsset(name) as AudioClip;
        if (asset == null)
        {
            Log.Error("没有找到音效:" + name);
            return;
        }
        PlayEffectMusic(asset);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clip"></param>
    public void PlayEffectMusic(AudioClip clip)
    {
        m_EffectAudioSource.clip = clip;
        m_EffectAudioSource.Play();
    }

    /// <summary>
    /// 停止播放音效
    /// </summary>
    public void StopEffectMusic()
    {
        m_EffectAudioSource.Stop();
        m_EffectAudioSource.clip = null;
    }

    #endregion
}

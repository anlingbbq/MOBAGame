using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // 用于播放背景音效
    [SerializeField]
    private AudioSource BgmAudioSource;

    // 用于播放特效音乐
    [SerializeField]
    private AudioSource EffectAudioSource;

    void Start()
    {
        BgmAudioSource.loop = true;
        BgmAudioSource.playOnAwake = true;

        EffectAudioSource.loop = false;
        EffectAudioSource.playOnAwake = false;
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

        BgmAudioSource.clip = clip;
        BgmAudioSource.Play();
    }

    /// <summary>
    /// 停止背景音乐 
    /// </summary>
    public void StopBgMusic()
    {
        BgmAudioSource.clip = null;
        BgmAudioSource.Stop();
    }

    #endregion

    #region 音效音乐

    /// <summary>
    /// 播放音效 
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

    public void PlayEffectMusic(AudioClip clip)
    {
        EffectAudioSource.clip = clip;
        EffectAudioSource.Play();
    }

    #endregion
}

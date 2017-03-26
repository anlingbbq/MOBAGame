using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>, IResourceListener
{
    // 用于播放背景音效
    [SerializeField]
    private AudioSource BgmAudioSource;

    // 用于播放特效音乐
    [SerializeField]
    private AudioSource EffectAudioSource;

    // 保存加载的音效
    //private Dictionary<string, AudioClip> m_EffectDict = new Dictionary<string, AudioClip>();

    void Start()
    {
        BgmAudioSource.loop = true;
        BgmAudioSource.playOnAwake = true;

        EffectAudioSource.loop = false;
        EffectAudioSource.playOnAwake = false;
    }

    // 加载登陆界面的声音文件
    public void LoadLoginSound()
    {
        ResourcesManager.Instance.Load(Paths.UI_LOGIN_BG, typeof(AudioClip), this, AssetType.SoundBGM);
        ResourcesManager.Instance.Load(Paths.UI_ENTERGAME, typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.UI_CLICK, typeof(AudioClip), this);
    }

    public void OnLoaded(string assetName, object asset, AssetType assetType)
    {
        //Log.Debug("已加载音乐资源 ： " + assetName);

        // 自动播放背景音乐
        if (assetType == AssetType.SoundBGM)
        {
            PlayBgMusic(asset as AudioClip);
        }
    }

    #region 背景音乐

    // 播放背景音乐
    public void PlayBgMusic(AudioClip clip)
    {
        if(clip == null)
        {
            return;
        }

        BgmAudioSource.clip = clip;
        BgmAudioSource.Play();
    }

    // 停止背景音乐
    public void StopBgMusic()
    {
        BgmAudioSource.clip = null;
        BgmAudioSource.Stop();
    }

    #endregion

    #region 音效音乐

    // 播放音效
    public void PlayEffectMusic(string name)
    {
        AudioClip asset = ResourcesManager.Instance.GetAsset(name) as AudioClip;
        PlayEffectMusic(asset);
    }

    public void PlayEffectMusic(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        EffectAudioSource.clip = clip;
        EffectAudioSource.Play();
    }

    #endregion
}

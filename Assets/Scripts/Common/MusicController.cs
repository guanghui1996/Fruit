/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-09 21:41:54
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public static MusicController Music;

    /// <summary>
    /// 音乐 用于在不同界面播放的音乐集合
    /// </summary>
    [SerializeField]
    private AudioClip[] MusicClips;

    /// <summary>
    /// 音乐播放组件
    /// </summary>
    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        if (Music == null)
        {
            DontDestroyOnLoad(gameObject);
            Music = this;
        }
        else if(Music != this)
        {
            Destroy(gameObject);
        }
    }

    public void MusicOn() {
        audioSource.mute = false;
    }

    public void MusicOff() {
        audioSource.mute = true;
    }

    public void BG_menu() {
        audioSource.clip = MusicClips[0];
        audioSource.Play();
    }

    public void BG_play() {
        audioSource.clip = MusicClips[1];
        audioSource.Play();
    }
}

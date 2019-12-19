/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-09 21:41:39
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public static SoundController Sound;

    /// <summary>
    /// 存储 按键点击 特效 胜利失败等游戏音效
    /// </summary>
    [SerializeField]
    private AudioClip[] SoundClips;

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        if (Sound == null)
        {
            DontDestroyOnLoad(gameObject);
            Sound = this;
        }
        else if (Sound != this) {
            Destroy(gameObject);
        }
    }

    public void SoundOn() {
        audioSource.mute = false;
    }

    public void SoundOff() {
        audioSource.mute = true;
    }

    public void Click() {
        audioSource.PlayOneShot(SoundClips[0]);
    }

}

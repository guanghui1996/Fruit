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
    private AudioClip[] soundClips;

    [SerializeField]
    private AudioSource audioSource;

    public AudioClip[] SoundClips
    {
        get
        {
            return soundClips;
        }

        set
        {
            soundClips = value;
        }
    }

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

    public void Lose()
    {
        audioSource.PlayOneShot(SoundClips[5]);
    }

    public void Win()
    {
        audioSource.PlayOneShot(SoundClips[5]);
    }

    public void FruitCrash()
    {
        audioSource.PlayOneShot(SoundClips[1]);
    }

    public void LockCrash()
    {
        audioSource.PlayOneShot(SoundClips[2]);
    }
    public void IceCrash()
    {
        audioSource.PlayOneShot(SoundClips[3]);
    }
    public void StarIn()
    {
        audioSource.PlayOneShot(SoundClips[6]);
    }
    public void Fire()
    {
        audioSource.PlayOneShot(SoundClips[7]);
    }
    public void Gun()
    {
        audioSource.PlayOneShot(SoundClips[8]);
    }
    public void Boom()
    {
        audioSource.PlayOneShot(SoundClips[9]);
    }

}

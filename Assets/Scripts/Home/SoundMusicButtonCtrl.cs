/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-15 15:34:26
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMusicButtonCtrl : MonoBehaviour {

    public UnityEngine.UI.Image Sound;
    public UnityEngine.UI.Image Music;

	// Use this for initialization
	void Start () {
        SetButonState();
	}

    void SetButonState() {
        if (PlayerPrefs.GetInt("MUSIC", 0) != 1)
        {
            Music.sprite = ButtonActionController.Click.ButtonSprites[0];
            MusicController.Music.MusicOn();
        }
        else
        {
            Music.sprite = ButtonActionController.Click.ButtonSprites[1];
            MusicController.Music.MusicOff();
        }
        if (PlayerPrefs.GetInt("SOUND", 0) != 1)
        {
            Sound.sprite = ButtonActionController.Click.ButtonSprites[2];
            SoundController.Sound.SoundOn();
        }
        else
        {
            Sound.sprite = ButtonActionController.Click.ButtonSprites[3];
            SoundController.Sound.SoundOff();
        }
    }

    public void MusicBtnClick() {
        SoundController.Sound.Click();
        if (PlayerPrefs.GetInt("MUSIC", 0) != 1)
        {
            Music.sprite = ButtonActionController.Click.ButtonSprites[1];
            PlayerPrefs.SetInt("MUSIC", 1);
            Debug.Log("MUSIC OFF");
            MusicController.Music.MusicOff();
        }
        else
        {
            Music.sprite = ButtonActionController.Click.ButtonSprites[0];
            PlayerPrefs.SetInt("MUSIC", 0);
            Debug.Log("MUSIC ON");
            MusicController.Music.MusicOn();
        }
    }

    public void SoundBtnClick()
    {
        if (PlayerPrefs.GetInt("SOUND", 0) != 1)
        {
            Sound.sprite = ButtonActionController.Click.ButtonSprites[3];
            PlayerPrefs.SetInt("SOUND", 1);
            Debug.Log("SOUND OFF");
            SoundController.Sound.SoundOff();
        }
        else
        {
            Sound.sprite = ButtonActionController.Click.ButtonSprites[2];
            PlayerPrefs.SetInt("SOUND", 0);
            Debug.Log("SOUND ON");
            SoundController.Sound.SoundOn();
        }
        SoundController.Sound.Click();
    }


}

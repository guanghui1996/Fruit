/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-11 20:45:50
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour {

    [SerializeField]
    private GameObject btnActionController;
    [SerializeField]
    private GameObject musicController;
    [SerializeField]
    private GameObject soundController;

	// Use this for initialization
	void Start ()
    {
        GameObject btn = Instantiate(btnActionController);
        btn.name = "ButtonActionController";
        GameObject music = Instantiate(musicController);
        music.name = "Music";
        GameObject sound = Instantiate(soundController);
        sound.name = "Sound";
        MusicController.Music.BG_menu();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitOK();
        }
	}

    public void ExitOK()
    {
        Application.Quit();
    }
}

/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-11 20:16:58
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWaiting : MonoBehaviour {

    [SerializeField]
    private UnityEngine.UI.Image loadBar;

	
	IEnumerator Start () {
        for (int i = 0; i < 120; i++)
        {
            loadBar.fillAmount += 1 / 120f;
            yield return new WaitForEndOfFrame();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
	}
	
}

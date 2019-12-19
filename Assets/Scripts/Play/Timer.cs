/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-11 22:27:48
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public static Timer timer;

    private bool isreq;

    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




 #region 属性
    public bool Isreq
    {
        get
        {
            return isreq;
        }

        set
        {
            isreq = value;
        }
    }
#endregion
}

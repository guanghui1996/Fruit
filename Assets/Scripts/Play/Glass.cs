/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2020-01-06 22:53:20
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour {

    public Animator amin;

    /// <summary>
    /// disable animator
    /// </summary>
    public void DisableAnimator()
    {
        amin.enabled = false;
    }
}

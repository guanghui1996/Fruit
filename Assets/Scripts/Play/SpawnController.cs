/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-24 23:54:18
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    public float DELAY;

    private void Update()
    {
        DELAY -= Time.deltaTime;
        if (DELAY < 0)
        {

        }
    }

}

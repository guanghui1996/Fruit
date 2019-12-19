/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-19 00:32:32
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulti {

    public static List<FruitObj> ListPlus(List<FruitObj> l1, List<FruitObj> l2, FruitObj bonus)
    {
        List<FruitObj> tmp = new List<FruitObj>();
        for (int i = l1.Count - 1; i >= 0; i--)
        {
            tmp.Add(l1[i]);
        }
        for (int i = 0; i < l2.Count; i++)
        {
            tmp.Add(l2[i]);
        }
        return tmp;
    }
	
}

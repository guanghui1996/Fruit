/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-15 23:48:13
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fruit {

    public int fruitType;
    public Vector2 fruitPosition;
    public int fruitPower;

    public int FruitType
    {
        get
        {
            return fruitType;
        }

        set
        {
            fruitType = value;
        }
    }

    public Vector2 FruitPosition
    {
        get
        {
            return fruitPosition;
        }

        set
        {
            fruitPosition = value;
        }
    }

    public int FruitPower
    {
        get
        {
            return fruitPower;
        }

        set
        {
            fruitPower = value;
        }
    }
}

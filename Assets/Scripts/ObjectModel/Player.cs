/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-11 21:33:55
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int Level;
    public string Name;
    public bool Locked;
    public int Stars;
    public int HightScore;
    public int Background;
    public string ToSaveString() {
        string s = Locked + "," + Stars + "," + HightScore +  "," + Background + ",";
        return s;
    }
}

public class PlayerUtils
{
    private string KEY_DATA = "DATA";
    private string data = "";
    private string[] dataSplit;
    private Player p;

    public void Save(List<Player> Maps)
    {
        foreach (var item in Maps)
        {
            data += item.ToSaveString();
        }
        PlayerPrefs.SetString(KEY_DATA, data);
    }


    public List<Player> Load()
    {
        List<Player> list = new List<Player>();
        string data  = PlayerPrefs.GetString(KEY_DATA, "");

        dataSplit = data.Split(',');

        for (int i = 0; i < 297; i++)
        {
            p = new Player();
            p.Level = i + 1;
            p.Name = (i + 1).ToString();
            p.Locked = bool.Parse(dataSplit[i * 4]);
            p.Stars = int.Parse(dataSplit[i * 4 + 1]);
            p.HightScore = int.Parse(dataSplit[i * 4 + 2]);
            p.Background = int.Parse(dataSplit[i * 4 + 3]);
            list.Add(p);
        }

        return list;
    }
}

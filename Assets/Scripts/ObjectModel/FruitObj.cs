/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-15 23:50:22
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitObj : MonoBehaviour {

    [SerializeField]
    private Fruit fruit;

    [SerializeField]
    private SpriteRenderer render;

    private Animation anim;

    private const float DELAY = 0.2f;

    public bool Checked;

    public bool isMove;

    private void Awake()
    {
        anim = Render.GetComponent<Animation>();
    }

    public void Destroy()
    {
        RemoveFromList((int)Fruit.FruitPosition.x, (int)Fruit.FruitPosition.y);
        StartCoroutine(_Destroy());
    }


    private void RemoveFromList(int x, int y)
    {

    }

    IEnumerator _Destroy()
    {
        yield return null;
    }

    public void RuleChecker()
    {

        if (fruit.FruitType != 99)
        {
            List<FruitObj> list = Ulti.ListPlus(GetRow(Fruit.FruitPosition, Fruit.FruitType, null),
                                                      GetCollumn(Fruit.FruitPosition, Fruit.FruitType, null),
                                                      this);

            if (list.Count >= 3)
            {
                listProcess(list);
                Checked = true;
            }
        }
        else
        {
            //GameController.gameController.WinChecker();
        }
    }

    void listProcess(List<FruitObj> list)
    {
        List<int> _listint = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].Checked)
                _listint.Add(list[i].getListcount());
            else
                _listint.Add(list.Count);
        }
        int max = Mathf.Max(_listint.ToArray());
        int idx = _listint.IndexOf(max);
        GameController.gameController.FruitProcess(list[idx].getList(), this.gameObject);
    }

    public int getListcount()
    {
        List<FruitObj> list = Ulti.ListPlus(GetRow(Fruit.FruitPosition, Fruit.FruitType, null),
                                            GetCollumn(Fruit.FruitPosition, Fruit.FruitType, null),
                                            this);

        return list.Count;
    }

    public List<FruitObj> getList()
    {
        List<FruitObj> list = Ulti.ListPlus(GetRow(Fruit.FruitPosition, Fruit.FruitType, null),
                                            GetCollumn(Fruit.FruitPosition, Fruit.FruitType, null),
                                            this);

        return list;
    }

    #region 动画相关

    public void Bounce()
    {
        if (GameController.gameController.GameState == (int)Timer.GameState.PLAYING && !Supporter.supporter.IsNoMove)
        {
            Animation anim = render.GetComponent<Animation>();
            anim.enabled = true;
            anim.Play("bounce");
        }
    }

    public void FruitDisable()
    {
        anim.enabled = true;
        anim.Play("Disable");
    }

    public void FruitEnable()
    {
        anim.enabled = true;
        anim.Play("Enable");
    }

    public void FruitSuggesttion()
    {
        anim.enabled = true;
        anim.Play("Suggesttion");
    }

    public void FruitStopSuggesttion()
    {
        if (anim.IsPlaying("Suggesttion"))
        {
            anim.Stop("Suggesttion");
            anim.enabled = false;
            transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void SetBackAnimation(GameObject Obj)
    {
        if (!Supporter.supporter.IsNoMove)
        {
            Vector2 ObjPos = Obj.GetComponent<FruitObj>().Fruit.FruitPosition;
            Animation anim = transform.GetChild(0).GetComponent<Animation>();
            anim.enabled = true;

            if (ObjPos.x == Fruit.FruitPosition.x)
            {
                if (ObjPos.y > Fruit.FruitPosition.y)
                {
                    anim.Play("MoveBack_Up");
                }
                else
                {
                    anim.Play("MoveBack_Down");
                }
            }
            else
            {
                if (ObjPos.x > Fruit.FruitPosition.x)
                {
                    anim.Play("MoveBack_Right");
                }
                else
                {
                    anim.Play("MoveBack_Left");
                }
            }
        }
    }

    #endregion

    #region 相连列表
    /// <summary>
    /// 获取同列相连
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="type"></param>
    /// <param name="bonus"></param>
    /// <returns></returns>
    public List<FruitObj> GetCollumn(Vector2 Pos, int type, FruitObj bonus)
    {
        List<FruitObj> tmp1 = GetTop(Pos, type);
        List<FruitObj> tmp2 = GetBot(Pos, type);
        if (tmp1.Count + tmp2.Count > 1)
        {
            return Ulti.ListPlus(tmp1, tmp2, bonus);
        }
        else
            return new List<FruitObj>();
    }
    /// <summary>
    /// 获取同行相连
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="type"></param>
    /// <param name="bonus"></param>
    /// <returns></returns>
    public List<FruitObj> GetRow(Vector2 Pos, int type, FruitObj bonus)
    {
        List<FruitObj> tmp1 = GetLeft(Pos, type);
        List<FruitObj> tmp2 = GetRight(Pos, type);
        if (tmp1.Count + tmp2.Count > 1)
        {
            return Ulti.ListPlus(tmp1, tmp2, bonus);
        }
        else
            return new List<FruitObj>();
    }

    private List<FruitObj> GetTop(Vector2 Pos,int type)
    {
        List<FruitObj> tmp = new List<FruitObj>();
        for (int y = (int)Pos.y + 1; y < 9; y++)
        {
            if (y != Fruit.FruitPosition.y && FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y] != null && FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y].Fruit.FruitType == type && GridManager.grid.GridCellObj[(int)Pos.x, y].Cell.CellEffect == 0)
            {
                tmp.Add(FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y]);
            }
            else
                return tmp;
        }
        return tmp;
    }
    private List<FruitObj> GetBot(Vector2 Pos, int type)
    {
        List<FruitObj> tmp = new List<FruitObj>();
        for (int y = (int)Pos.y - 1; y >= 0; y--)
        {
            if (y != Fruit.FruitPosition.y && FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y] != null && FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y].Fruit.FruitType == type && GridManager.grid.GridCellObj[(int)Pos.x, y].Cell.CellEffect == 0)
            {
                tmp.Add(FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y]);
            }
            else
                return tmp;
        }
        return tmp;
    }
    private List<FruitObj> GetLeft(Vector2 Pos, int type)
    {
        List<FruitObj> tmp = new List<FruitObj>();
        for (int x = (int)Pos.x - 1; x >= 0; x--)
        {
            if (x != Fruit.FruitPosition.x && FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y] != null && FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y].Fruit.FruitType == type && GridManager.grid.GridCellObj[x, (int)Pos.y].Cell.CellEffect == 0)
            {
                tmp.Add(FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y]);
            }
            else
                return tmp;
        }
        return tmp;
    }
    private List<FruitObj> GetRight(Vector2 Pos, int type)
    {
        List<FruitObj> tmp = new List<FruitObj>();
        for (int x = (int)Pos.x + 1; x < 7; x++)
        {
            if (x != Fruit.FruitPosition.x && FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y] != null && FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y].Fruit.FruitType == type && GridManager.grid.GridCellObj[x, (int)Pos.y].Cell.CellEffect == 0)
            {
                tmp.Add(FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y]);
            }
            else
                return tmp;
        }
        return tmp;
    }
    #endregion


    #region 属性
    public Fruit Fruit
    {
        get
        {
            return fruit;
        }

        set
        {
            fruit = value;
        }
    }

    public SpriteRenderer Render
    {
        get
        {
            return render;
        }

        set
        {
            render = value;
        }
    }
    #endregion
}

/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-16 00:11:24
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public static GridManager grid;

    [SerializeField]
    private GameObject[,] gridCell;

    [SerializeField]
    private CellObj[,] gridCellObj;

    [SerializeField]
    private GameObject gridParent;

    [SerializeField]
    private GameObject cellPrefab;

    [SerializeField]
    private Sprite[] cellSprite;

    [SerializeField]
    private GameObject[] border;

    [SerializeField]
    private GameObject[] corner;

    [SerializeField]
    private GameObject borderParent;

    [SerializeField]
    private int[,] map;

    [SerializeField]
    private GameObject objTmp;

    private CellObj cellScript;

    private const string path = "Assets/Resources/Map/";

    private void Awake()
    {
        grid = this;
    }

    public IEnumerator GridMapCreate(string MapName)
    {
        GridCell = new GameObject[7, 9];
        Map = MapReader(MapName);
        yield return new WaitForEndOfFrame();
        GridCreate(Map);
        yield return new WaitForEndOfFrame();
        BorderCreate(Map);
        yield return new WaitForSeconds(1f);
        FruitSpawner.spawn.FruitMapCreate(Map);
        yield return new WaitForEndOfFrame();
        FruitSpawner.spawn.EnableAllFruit();
    }


    /// <summary>
    /// 根据文本文档生成基本地图表格
    /// </summary>
    /// <param name="mapName"></param>
    /// <returns></returns>
    int[,] MapReader(string mapName) {
        int[,] tmp = new int[7, 9];
        string mapStringData = "";
#if UNITY_EDITOR
        TextAsset txtass = (TextAsset)Resources.Load("Maps/" + mapName, typeof(TextAsset));
        mapStringData = txtass.ToString();
        //mapStringData = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(@path + mapName + ".txt").ToString();
#else
        TextAsset txtass = (TextAsset)Resources.Load("Maps/" + mapName, typeof(TextAsset));
        mapStringData = txtass.ToString();
#endif
        string[] stringResult = mapStringData.Split(new char[] { '\t', '\n' });
        int dem = 0;
        for (int y = 8; y >= 0; y--)
        {
            for (int x = 0; x < 7; x++)
            {
                tmp[x, y] = int.Parse(stringResult[dem]);
                dem++;
            }
        }
        return tmp;
    }

    /// <summary>
    /// 根据Map来生成对应元素表格
    /// </summary>
    /// <param name="map"></param>
    void GridCreate(int[,] map)
    {
        GameController.gameController.CellNotEmpty = 0;
        GridCellObj = new CellObj[7, 9];
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (map[x, y] > 1) {
                    GameController.gameController.CellNotEmpty++;
                }
                if (map[x, y] > 0) {
                    CellInstantiate(x, y, map[x, y]);
                }

            }
        }
    }


    void CellInstantiate(int x, int y, int type)
    {
        ObjTmp = Instantiate(CellPrefab) as GameObject;
        ObjTmp.transform.SetParent(GridParent.transform, false);
        ObjTmp.transform.localPosition = new Vector3(x, y);
        cellScript = ObjTmp.GetComponent<CellObj>();
        cellScript.CellCode = type;
        cellScript.Cell = SetCell(type, x, y);
        cellScript.SetSprite(cellScript.Cell.CellType - 1);
        GridCell[x, y] = objTmp;
        GridCellObj[x, y] = cellScript;
    }

    
    Cell SetCell(int type, int x, int y)
    {
        Cell tcell = new Cell();

        if (type > 10)
        {
            tcell.CellType = type / 10;
            tcell.CellEffect = type % 10;
        }
        else
        {
            tcell.CellType = type % 100 % 10;
            tcell.CellEffect = 0;
        }
        tcell.CellPosition = new Vector2(x, y);
        return tcell;
    }
    
    /// <summary>
    /// 边界生成
    /// </summary>
    /// <param name="map"></param>
    void BorderCreate(int[,] map)
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                int i = map[x, y];
                if (i > 0)
                {
                    borderins(GridCell[x, y], left(x, y), right(x, y), top(x, y), bot(x, y));
                    CornerOutCheck(GridCell[x, y], topLeft(x, y), topRight(x, y), botLeft(x, y), botRight(x, y), x, y);
                }
                else
                {
                    borderInChecker(map, x, y);
                }
            }
        }
    }

    /// <summary>
    /// 判断是否处于边界位置
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="top"></param>
    /// <param name="bot"></param>
    void borderins(GameObject parent, bool left, bool right, bool top, bool bot)
    {
        if (left && !top)
        {
            ObjTmp = Instantiate(border[2]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            //ObjTmp.transform.localPosition += parent.transform.localPosition;
            ObjTmp.transform.localPosition += new Vector3(parent.transform.localPosition.x,
                                                          parent.transform.localPosition.y + 0.5f,
                                                          parent.transform.localPosition.z);
        }
        if (right && !top)
        {
            ObjTmp = Instantiate(border[3]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            //ObjTmp.transform.localPosition += parent.transform.localPosition;
            ObjTmp.transform.localPosition += new Vector3(parent.transform.localPosition.x,
                                                          parent.transform.localPosition.y + 0.5f,
                                                          parent.transform.localPosition.z);
        }
        if (top && !left)
        {
            ObjTmp = Instantiate(border[1]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            //ObjTmp.transform.localPosition += parent.transform.localPosition;
            ObjTmp.transform.localPosition += new Vector3(parent.transform.localPosition.x - 0.5f,
                                                          parent.transform.localPosition.y,
                                                          parent.transform.localPosition.z);
        }
        if (bot && !left)
        {
            ObjTmp = Instantiate(border[0]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            //ObjTmp.transform.localPosition += parent.transform.localPosition;
            ObjTmp.transform.localPosition += new Vector3(parent.transform.localPosition.x - 0.5f,
                                                          parent.transform.localPosition.y,
                                                          parent.transform.localPosition.z);
        }
    }

    /// <summary>
    /// 边界顶点生成
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="topLeft"></param>
    /// <param name="topRight"></param>
    /// <param name="botLeft"></param>
    /// <param name="botRight"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void CornerOutCheck(GameObject parent, bool topLeft, bool topRight, bool botLeft, bool botRight, int x, int y)
    {
        bool _top = top(x, y);
        bool _bot = bot(x, y);
        bool _left = left(x, y);
        bool _right = right(x, y);
        if (topLeft && _top && _left)
        {
            ObjTmp = Instantiate(corner[0]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            ObjTmp.transform.localPosition += parent.transform.localPosition;
        }
        if (topRight && _top && _right)
        {
            ObjTmp = Instantiate(corner[1]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            ObjTmp.transform.localPosition += parent.transform.localPosition;
        }
        if (botLeft && _bot && _left)
        {
            ObjTmp = Instantiate(corner[2]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            ObjTmp.transform.localPosition += parent.transform.localPosition;
        }
        if (botRight && _bot && _right)
        {
            ObjTmp = Instantiate(corner[3]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            ObjTmp.transform.localPosition += parent.transform.localPosition;
        }
    }

    void borderInChecker(int[,] map, int x, int y)
    {
        if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1, y] > 0 && map[x, y - 1] > 0)
        {
            ObjTmp = Instantiate(corner[6]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            ObjTmp.transform.localPosition += new Vector3(x - 1, y);
        }
        if (x - 1 >= 0 && y + 1 < 9 && map[x - 1, y] > 0 && map[x, y + 1] > 0)
        {
            ObjTmp = Instantiate(corner[4]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            ObjTmp.transform.localPosition += new Vector3(x - 1, y);
        }
        if (x + 1 < 7 && y - 1 >= 0 && map[x + 1, y] > 0 && map[x, y - 1] > 0)
        {
            ObjTmp = Instantiate(corner[7]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            ObjTmp.transform.localPosition += new Vector3(x - 1, y);
        }
        if (x + 1 < 0 && y + 1 < 9 && map[x + 1, y] > 0 && map[x, y + 1] > 0)
        {
            ObjTmp = Instantiate(corner[5]) as GameObject;
            ObjTmp.transform.SetParent(BorderParent.transform, false);
            ObjTmp.transform.localPosition += new Vector3(x - 1, y);
        }
    }

    #region 边缘检查
    bool left(int x, int y)
    {
        if (x == 0)
            return true;
        else if (x - 1 >= 0 && Map[x - 1, y] == 0)
            return true;
        return false;
    }
    bool right(int x, int y)
    {
        if (x == 6)
            return true;
        else if (x + 1 <= 6 && Map[x + 1, y] == 0)
            return true;
        return false;
    }
    bool top(int x, int y)
    {
        if (y == 8)
            return true;
        else if (y + 1 <= 8 && Map[x, y + 1] == 0)
            return true;
        return false;
    }
    bool bot(int x, int y)
    {
        if (y == 0)
            return true;
        else if (y - 1 >= 0 && Map[x, y - 1] == 0)
            return true;
        return false;
    }
    bool topLeft(int x, int y)
    {
        if (x - 1 < 0 || y + 1 > 8)
        {
            return true;
        }
        else if (x - 1 >= 0 && y + 1 <= 8 && Map[x - 1, y + 1] == 0)
        {
            return true;
        }
        return false;
    }
    bool topRight(int x, int y)
    {
        if (x + 1 > 6 || y + 1 > 8)
        {
            return true;
        }
        else if (x + 1 <= 6 && y + 1 <= 8 && Map[x + 1, y + 1] == 0)
        {
            return true;
        }
        return false;
    }
    bool botLeft(int x, int y)
    {
        if (x - 1 < 0 || y - 1 < 0)
            return true;
        else if (x - 1 >= 0 && y - 1 >= 0 && Map[x - 1, y - 1] == 0)
            return true;
        return false;
    }
    bool botRight(int x, int y)
    {
        if (x + 1 > 6 || y - 1 < 0)
            return true;
        else if (x + 1 <= 6 && y - 1 >= 0 && Map[x + 1, y - 1] == 0)
            return true;
        return false;
    }
    #endregion

    #region 属性
    public GameObject[,] GridCell
    {
        get
        {
            return gridCell;
        }

        set
        {
            gridCell = value;
        }
    }

    public CellObj[,] GridCellObj
    {
        get
        {
            return gridCellObj;
        }

        set
        {
            gridCellObj = value;
        }
    }

    public GameObject GridParent
    {
        get
        {
            return gridParent;
        }

        set
        {
            gridParent = value;
        }
    }

    public GameObject CellPrefab
    {
        get
        {
            return cellPrefab;
        }

        set
        {
            cellPrefab = value;
        }
    }

    public Sprite[] CellSprite
    {
        get
        {
            return cellSprite;
        }

        set
        {
            cellSprite = value;
        }
    }

    public GameObject[] Border
    {
        get
        {
            return border;
        }

        set
        {
            border = value;
        }
    }

    public GameObject[] Corner
    {
        get
        {
            return corner;
        }

        set
        {
            corner = value;
        }
    }

    public GameObject BorderParent
    {
        get
        {
            return borderParent;
        }

        set
        {
            borderParent = value;
        }
    }

    public int[,] Map
    {
        get
        {
            return map;
        }

        set
        {
            map = value;
        }
    }

    public GameObject ObjTmp
    {
        get
        {
            return objTmp;
        }

        set
        {
            objTmp = value;
        }
    }
#endregion
}

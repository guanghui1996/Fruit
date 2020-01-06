/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-15 23:57:39
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {

    public static FruitSpawner spawn;

    [SerializeField]
    private GameObject[,] fruitGrid;

    [SerializeField]
    private FruitObj[,] fruitGridScript;

    [SerializeField]
    private GameObject FruitParent;

    [SerializeField]
    private GameObject FruitObject;

    [SerializeField]
    private Sprite[] FruitSprite;

    [SerializeField]
    private GameObject Star;

    [SerializeField]
    private GameObject StarEffect;

    [SerializeField]
    private GameObject FruitColor;

    [SerializeField]
    private GameObject NoSelect;

    private const float BaseDistance = 1f;

    private GameObject ObjTmp;

    private FruitObj FruitScript;

    int index = 0;

    [SerializeField]
    private List<GameObject>[] preSpawnList = new List<GameObject>[7];

    public FruitObj[,] FruitGridScript
    {
        get
        {
            return fruitGridScript;
        }

        set
        {
            fruitGridScript = value;
        }
    }

    public GameObject[,] FruitGrid
    {
        get
        {
            return fruitGrid;
        }

        set
        {
            fruitGrid = value;
        }
    }

    private void Awake()
    {
        spawn = this;
        for (int i = 0; i < 7; i++)
        {
            preSpawnList[i] = new List<GameObject>();
        }
    }

    public void FruitMapCreate(int[,] Map)
    {
        FruitGrid = new GameObject[7, 9];

        FruitGridScript = new FruitObj[7, 9];

        for (int x = 0; x < 7; x++)
        {
            int s = 0;
            for (int y = 0; y < 9; y++)
            {
                if (GridManager.grid.GridCellObj[x, y] != null && GridManager.grid.GridCellObj[x, y].Cell.CellEffect == 4)
                    s = y;
            }
            for (int y = s; y < 9; y++)
            {
                if (Map[x, y] > 0)
                {
                    G_FruitInstaniate(x, y);
                }
            }
        }
        while (!Supporter.supporter.IsNoMoreMove() && index < 20)
        {
            Debug.Log("Hello");
            index++;
            RemakeGrid();
            FruitMapCreate(Map);
        }
        SpawnController.isSpawned = true;
    }

    void RemakeGrid()
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (FruitGrid[x, y] != null && fruitGridScript[x, y] != GameController.gameController.FruitStar)
                {
                    Destroy(FruitGrid[x, y]);
                    FruitGridScript[x, y] = null;
                }
            }
        }
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < preSpawnList[i].Count; j++)
            {
                if (preSpawnList[i][j] != null)
                    Destroy(preSpawnList[i][j]);
            }
            preSpawnList[i].Clear();
        }
    }

    public void EnableAllFruit()
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (FruitGridScript[x, y] != null && FruitGridScript[x, y] != GameController.gameController.FruitStar)
                {
                    FruitGridScript[x, y].FruitEnable();
                }
            }
        }
    }

    public GameObject FruitInstantiate(int x, int y)
    {
        ObjTmp = Instantiate(FruitObject) as GameObject;
        FruitScript = ObjTmp.GetComponent<FruitObj>();
        ObjTmp.transform.SetParent(FruitParent.transform,false);
        ObjTmp.transform.localPosition = new Vector3(ObjTmp.transform.localPosition.x + x * BaseDistance, ObjTmp.transform.localPosition.y + y * BaseDistance);
        
        FruitGrid[x, y] = ObjTmp;
        FruitGridScript[x, y] = FruitScript;

        int r = 0;

        if (PlayerInfo.MODE == 1)
            r = Random.Range(0, 6);
        else
            r = Random.Range(0, 7);


        FruitScript.Render.sprite = FruitSprite[r];
        FruitScript.Fruit.FruitPosition = new Vector2(x, y);
        FruitScript.Fruit.FruitType = r;

        return ObjTmp;
    }

    GameObject RFruitInstantiate(int x, int y)
    {
        ObjTmp = Instantiate(FruitObject) as GameObject;
        FruitScript = ObjTmp.GetComponent<FruitObj>();
        ObjTmp.transform.SetParent(FruitParent.transform, false);
        ObjTmp.transform.localPosition = new Vector3(x, y);
        ObjTmp.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0, 0, 1);
        FruitGrid[x, y] = ObjTmp;
        FruitGridScript[x, y] = FruitScript;

        int r = RandomFruit(x, y);

        FruitScript.Render.sprite = FruitSprite[r];
        FruitScript.Fruit.FruitPosition = new Vector2(x, y);
        FruitScript.Fruit.FruitType = r;

        return ObjTmp;
    }

    public GameObject SpawnFruitPower(int type, int power, Vector3 pos)
    {
        GameObject tmp;
        int x = (int)pos.x;
        int y = (int)pos.y;
        if (FruitGrid[x, y] != null)
            Destroy(FruitGrid[x, y]);
        if (type == 8)
        {
            tmp = Instantiate(FruitColor) as GameObject;
        }
        else
        {
            tmp = Instantiate(FruitObject) as GameObject;
        }
        FruitScript = tmp.GetComponent<FruitObj>();
        FruitScript.Render.enabled = true;
        tmp.transform.SetParent(FruitParent.transform, false);
        tmp.transform.localPosition = new Vector3(x, y, pos.z);
        FruitGrid[x, y] = tmp;
        FruitGridScript[x, y] = FruitScript;
        if (type != 8)
            FruitScript.Render.sprite = FruitSprite[type];
        FruitScript.Fruit.FruitPosition = new Vector2(x, y);
        FruitScript.Fruit.FruitType = type;
        FruitScript.Fruit.FruitPower = power;
        tmp.GetComponent<Collider2D>().enabled = false;
        if (power == (int)GameController.Power.BOOM)
            EffectSpawner.effect.Enchant(tmp.transform.GetChild(0).gameObject);


        return tmp;
    }

    GameObject G_FruitInstaniate(int x, int y)
    {
        ObjTmp = Instantiate(FruitObject) as GameObject;
        ObjTmp.transform.SetParent(FruitParent.transform, false);
        ObjTmp.transform.localPosition = new Vector3(x, y);
        ObjTmp.transform.GetChild(0).gameObject.transform.localScale = new Vector3(1, 1, 1);
        FruitScript = ObjTmp.GetComponent<FruitObj>();
        FruitGrid[x, y] = ObjTmp;
        FruitGridScript[x, y] = FruitScript;

        int r = RandomFruit(x, y);
        FruitScript.Render.sprite = FruitSprite[r];
        FruitScript.Fruit.FruitPosition = new Vector2(x, y);
        FruitScript.Fruit.FruitType = r;


        return ObjTmp;
    }

    int RandomFruit(int x, int y)
    {
        int r = -1;
        int dem = 0;
        while (true)
        {
            if (PlayerInfo.MODE == 1)
                r = Random.Range(0, 6);
            else
                r = Random.Range(0, 7);
            if (x < 2 || FruitGridScript[x - 1, y] == null || FruitGridScript[x - 2, y] == null || r != FruitGridScript[x - 1, y].Fruit.FruitType || FruitGridScript[x - 2, y].Fruit.FruitType != r)
            {
                if (y < 2 || FruitGridScript[x, y - 1] == null || FruitGridScript[x, y - 2] == null || r != FruitGridScript[x, y - 1].Fruit.FruitType || FruitGridScript[x, y - 2].Fruit.FruitType != r)
                {
                    return r;
                }
            }
            dem++;
            if (dem > 100)
                return 0;
        }
    }

    public void SpawnStar(Vector2 pos)
    {
        if (FruitGridScript[(int)pos.x, (int)pos.y] != null)
            Destroy(FruitGrid[(int)pos.x, (int)pos.y]);

        GameObject tmp = Instantiate(Star) as GameObject;
        tmp.name = "FruitStar";
        tmp.transform.SetParent(FruitParent.transform, false);
        tmp.transform.localPosition = new Vector3(pos.x, pos.y);
        tmp.transform.GetChild(0).gameObject.SetActive(false);

        FruitScript = tmp.GetComponent<FruitObj>();
        FruitScript.Fruit.FruitPosition = pos;
        FruitGridScript[(int)pos.x, (int)pos.y] = FruitScript;
        FruitGrid[(int)pos.x, (int)pos.y] = tmp;
        GameController.gameController.FruitStar = FruitScript;

        StarEffect.SetActive(true);
    }

    public IEnumerator ReSpawn()
    {
    loop:
        RemakeGrid();
        for (int x = 0; x < 7; x++)
        {
            int s = 0;
            for (int y = 0; y < 9; y++)
            {
                if (GridManager.grid.GridCellObj[x, y] != null && GridManager.grid.GridCellObj[x, y].Cell.CellEffect == 4)
                    s = y;
            }
            for (int y = s; y < 9; y++)
            {
                if (GridManager.grid.Map[x, y] > 0 && fruitGridScript[x, y] == null)
                {
                    RFruitInstantiate(x, y);
                }
            }
        }

        while (!Supporter.supporter.IsNoMoreMove())
        {
            Debug.Log(Supporter.supporter.IsNoMoreMove());
            goto loop;
        }

        EnableAllFruit();
        yield return new WaitForSeconds(0.75f);
        Timer.timer.NoSelect.SetActive(false);
        Timer.timer.NoMove.SetActive(false);
    }

}

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

    public static bool isSpawned = false;


    private void Update()
    {
        DELAY -= Time.deltaTime;
        if (DELAY < 0 && isSpawned)
        {
            StartCoroutine(DropAndSpawn());
            this.enabled = false;
        }
    }

    IEnumerator DropAndSpawn()
    {

        Drop();
        yield return new WaitForEndOfFrame();
        Spawn();
        BonusPower();
        ShowStar();
    }


    void Drop()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                if (FruitSpawner.spawn.FruitGridScript[x, y] != null && GridManager.grid.GridCellObj[x, y].Cell.CellEffect != 4)
                    FruitSpawner.spawn.FruitGridScript[x, y].getNewPosition();
            }
        }
    }

    void Spawn()
    {
        int[] h = new int[7];

        for (int x = 0; x < 7; x++)
        {
            int s = 0;
            for (int y = 0; y < 9; y++)
            {
                if (GridManager.grid.GridCellObj[x, y] != null && GridManager.grid.GridCellObj[x, y].Cell.CellEffect == 4)
                    s = y + 1;
            }
            for (int y = s; y < 9; y++)
            {
                if (GameController.gameController.GameState == (int)Timer.GameState.PLAYING)
                {
                    if (GridManager.grid.GridCellObj[x, y] != null && FruitSpawner.spawn.FruitGridScript[x, y] == null)
                    {
                        GameObject tmp = FruitSpawner.spawn.FruitInstantiate(x, y);
                        if (PlayerInfo.MODE == 1 && Random.value > 0.99f)
                        {
                            tmp.GetComponent<FruitObj>().Fruit.FruitType = 4;

                        }
                        tmp.transform.localPosition = new Vector3(tmp.transform.localPosition.x, 10 + h[x]);
                        h[x]++;
                        StartCoroutine(Ulti.IEDrop(tmp, new Vector2(x, y), GameController.DROP_SPEED));
                        FruitObj script = tmp.GetComponent<FruitObj>();
                        script.Render.enabled = true;
                    }
                }
            }
        }

        StartCoroutine(checkNoMoreMove());

    }

    void BonusPower()
    {
        if (GameController.gameController.IsAddPower)
        {
            GameController.gameController.AddBonusPower();
            GameController.gameController.IsAddPower = false;
        }
    }

    void ShowStar()
    {
        if (GameController.gameController.IsShowStar)
        {
            GameController.gameController.IsShowStar = false;
            GameController.gameController.ShowStar();
            GameController.gameController.IsStar = true;
        }
    }

    IEnumerator checkNoMoreMove()
    {
        yield return new WaitForSeconds(0.5f);
        if (!Supporter.supporter.IsNoMoreMove())
        {
            if (PlayerInfo.MODE == 1)
            {
                Timer.timer.NoSelect.SetActive(true);
                StartCoroutine(ReSpawnGrid());
            }
            else
            {
                Timer.timer.NoSelect.SetActive(true);
                Timer.timer.Lost();
            }
        }
    }

    IEnumerator ReSpawnGrid()
    {
        Timer.timer.NoMove.SetActive(true);
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (FruitSpawner.spawn.FruitGridScript[x, y] != null && FruitSpawner.spawn.FruitGridScript[x, y].Fruit.FruitType != 99)
                    FruitSpawner.spawn.FruitGridScript[x, y].FruitDisable();
            }
        }
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(FruitSpawner.spawn.ReSpawn());
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : Pathfinder
{
    protected override IEnumerator Search(TileLogic start) // we are using enumerator instead a normal method, so we can slow it down and watch it happen
    {
        tileSearch = new List<TileLogic>();
        int iterationCount = 0;

        tileSearch.Add(start);
        Board.Instance.ClearSearch();
        Queue<TileLogic> checkNow = new Queue<TileLogic>();
        Queue<TileLogic> checkNext = new Queue<TileLogic>();

        start.CostFromOrigin = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            TileLogic tile = checkNow.Dequeue();
            Board.Instance.PaintTile(tile.Position, Color.green);
            for (int i = 0; i < Board.Directions.Length; i++)
            {
                TileLogic next = Board.GetTile(tile.Position + Board.Directions[i]);
                yield return new WaitForSeconds(0.025f);
                iterationCount++;

                if (next == null || next.CostFromOrigin <= tile.CostFromOrigin + next.MoveCost)
                    continue;
                next.CostFromOrigin = tile.CostFromOrigin + next.MoveCost;
                if (ValidateMovement(tile, next))
                {
                    checkNext.Enqueue(next);
                    next.Previous = tile;
                    tileSearch.Add(next);
                    Board.Instance.PaintTile(next.Position, Color.yellow);

                }
            }
            if (checkNow.Count == 0)
            {
                SwapReferences(ref checkNow, ref checkNext);
            }
        }
        Debug.Log("Iterations :" + iterationCount);
        //yield return new WaitForSeconds(1);
    }
    void SwapReferences(ref Queue<TileLogic> checkNow, ref Queue<TileLogic> checkNext)
    {
        Queue<TileLogic> temp = checkNow;
        checkNow = checkNext;
        checkNext = temp;
    }
}


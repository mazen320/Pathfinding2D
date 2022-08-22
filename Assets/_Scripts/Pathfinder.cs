using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : MonoBehaviour
{
    public Vector3Int InitialPosition;
    public Vector3Int ObjectivePosition;
    public int SearchLength;
    protected List<TileLogic> tileSearch;
    [ContextMenu("Search")]
    void TriggerSearch()
    {
        StartCoroutine(Search(Board.GetTile(InitialPosition)));
    }
    [ContextMenu("Print Path")]
    void TriggerPrintPath()
    {
        TileLogic objective = Board.GetTile(ObjectivePosition);
        if (tileSearch.Contains(objective))
        {
            List<TileLogic> path = BuildPath(objective);
            PrintPath(path);
        }
        else
        {
            Debug.Log("Objective not found");
        }
    }
    protected abstract IEnumerator Search(TileLogic start);

    List<TileLogic> BuildPath(TileLogic lastTile)
    {
        List<TileLogic> path = new List<TileLogic>();
        TileLogic temp = lastTile;
        while (temp.Previous != null)
        {
            path.Add(temp);
            temp = temp.Previous;
        }
        path.Add(temp);
        path.Reverse();
        return path;
    }
    void PrintPath(List<TileLogic> path)
    {
        foreach (TileLogic t in path)
        {
            Debug.Log(t.Position);
        }
    }
    protected bool ValidateMovement(TileLogic from, TileLogic to)
    {
        if (to.CostFromOrigin > SearchLength)
        {
            return false;
        }
        return true;
    }
}

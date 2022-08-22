using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : Pathfinder // sort the list and give prirorty to the lowest score
{
    protected override IEnumerator Search(TileLogic start) // we are using enumerator instead a normal method, so we can slow it down and watch it happen
    {
        tileSearch = new List<TileLogic>();
        int iterationCount = 0;

        tileSearch.Add(start);
        Board.Instance.ClearSearch();

        TileLogic objective = Board.GetTile(ObjectivePosition);
        TileLogic current;

        List<TileLogic> openSet = new List<TileLogic>();
        openSet.Add(start);
        start.CostFromOrigin = 0;

        while (openSet.Count > 0)
        {
            openSet.Sort((x, y) => x.Score.CompareTo(y.Score));
            current = openSet[0];
            Board.Instance.PaintTile(current.Position, Color.green);

            if (current == objective)
            {
                Debug.Log("Objective found");
                break;
            }

            openSet.RemoveAt(0);
            tileSearch.Add(current);
            for (int i = 0; i < Board.Directions.Length; i++)
            {
                TileLogic next = Board.GetTile(current.Position + Board.Directions[i]);
                yield return new WaitForSeconds(0.025f);
                iterationCount++;

                if (next == null || next.CostFromOrigin <= current.CostFromOrigin + next.MoveCost)
                    continue;
                next.CostFromOrigin = current.CostFromOrigin + next.MoveCost;
                if (ValidateMovement(current, next))
                {
                    next.Previous = current;
                    next.CostToObjective = Vector3Int.Distance(next.Position, objective.Position);
                    next.Score = next.CostFromOrigin + next.CostToObjective;

                    if (!tileSearch.Contains(next))
                    {
                        openSet.Add(next);
                    }
                    Board.Instance.PaintTile(next.Position, Color.yellow);

                }
            }
        }
    }
}

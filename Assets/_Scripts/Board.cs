using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    //singleton
    public static Board Instance;
    public static Vector3Int[] Directions = new Vector3Int[4]
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left
    };
    public Tilemap Tilemap;
    public Vector3Int Size;
    public Dictionary<Vector3Int, TileLogic> Tiles;

    void Awake()
    {
        //so everyone can access static stuff through static - what singleton are for
        Instance = this;
        Tiles = new Dictionary<Vector3Int, TileLogic>();
        CreateTileLogics();
        Debug.Log(Tiles.Count);
    }

    public static TileLogic GetTile(Vector3Int position)
    {
        TileLogic tile;

        if (Instance.Tiles.TryGetValue(position, out tile)) // this is a fix if the tiles are outside the map 
            return tile;
        return null;

        return Instance.Tiles[position];
    }
    public void PaintTile(Vector3Int position, Color color)
    {
        Tilemap.SetColor(position, color);
    }

    public void ClearSearch()
    {
        foreach (TileLogic t in Tiles.Values)
        {
            t.CostFromOrigin = int.MaxValue;
            t.CostToObjective = int.MaxValue;
            t.Score = int.MaxValue;
            t.Previous = null;
        }
    }
    void CreateTileLogics()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                TileLogic tile = new TileLogic();
                tile.Position = new Vector3Int(x, y, 0);
                Tiles.Add(tile.Position, tile);
                SetTile(tile);
            }
        }
    }

    void SetTile(TileLogic tile)
    {
        {
            string tileType = Tilemap.GetTile(tile.Position).name;
            switch (tileType)
            {
                case "blockedTile":
                    tile.MoveCost = int.MaxValue;
                    break;
                case "2Tile":
                    tile.MoveCost = 2;
                    break;
                case "3Tile":
                    tile.MoveCost = 3;
                    break;
                default:
                    tile.MoveCost = 1;
                    break;
            }
        }
    }
}

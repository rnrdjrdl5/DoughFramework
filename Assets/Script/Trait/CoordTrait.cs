using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CoordTrait : Trait
{
    [SerializeField] Tilemap tileMap;

    Dictionary<Vector3Int, TileInfo> tileInfos = new();

    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

        tileInfos.Clear();

        var bounds = tileMap.cellBounds;

        for (int curX = bounds.xMin; curX < bounds.xMax; curX++)
        {
            for (int curY = bounds.yMin; curY < bounds.yMax; curY++)
            {
                var tilePosition = new Vector3Int(curX, curY);
                var tileData = GetTileByTileMap(tilePosition);

                TileInfo tileInfo = new TileInfo();

                tileInfo.tilePosition = tilePosition;
                tileInfo.tileData = tileData;

                tileInfos.Add(tilePosition ,tileInfo);
            }
        }
    }

    public bool TryGetTileInfo(int cellX, int cellY, out TileInfo result)
    {
        var tilePosition = new Vector3Int(cellX, cellY, 0);

        return tileInfos.TryGetValue(tilePosition, out result);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        var tilePosition = new Vector3Int(x, y, 0);

        return tileMap.CellToWorld(tilePosition);
    }

    public bool TryGetTileByWorldPosition(Vector3 worldPosition, out TileInfo result)
    {
        var tilePosition = tileMap.WorldToCell(worldPosition);

        if (!TryGetTileInfo(tilePosition.x, tilePosition.y, out result))
        {
            return false;
        }

        return true;
    }

    TileData GetTileByTileMap(int x, int y)
    {
        var position = new Vector3Int(x, y, 0);

        return GetTileByTileMap(position);
    }

    TileData GetTileByTileMap(Vector3Int position)
    {
        var tileBase = tileMap.GetTile(position);

        var tileData = new TileData();
        tileBase.GetTileData(position, tileMap, ref tileData);

        return tileData;
    }

    public struct TileInfo
    {
        public TileData tileData;
        public Vector3Int tilePosition;

        public object objectData;
    }
}

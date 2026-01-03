using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public Grid grid;
    public Tilemap tilemap;
    public TileBase grassTile;
    public TileBase stoneTile;
    public TileBase waterTile;
    
    [Header("Grid Size")]
    public int gridWidth = 10;
    public int gridHeight = 10;
    
    void Start()
    {
        GenerateBasicMap();
    }
    
    void GenerateBasicMap()
    {
        // Create basic Elarion-style map
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                
                // Create varied terrain
                if (x == 0 || x == gridWidth - 1 || y == 0 || y == gridHeight - 1)
                    tilemap.SetTile(position, stoneTile); // Borders
                else if (Random.Range(0f, 1f) < 0.1f)
                    tilemap.SetTile(position, waterTile); // Water patches
                else
                    tilemap.SetTile(position, grassTile); // Grass
            }
        }
    }
    
    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return grid.CellToWorld(new Vector3Int(gridPos.x, gridPos.y, 0));
    }
    
    public Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        Vector3Int cellPos = grid.WorldToCell(worldPos);
        return new Vector2Int(cellPos.x, cellPos.y);
    }
    
    public bool IsValidPosition(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.x >= gridWidth || gridPos.y < 0 || gridPos.y >= gridHeight)
            return false;
            
        Vector3Int tilePos = new Vector3Int(gridPos.x, gridPos.y, 0);
        TileBase tile = tilemap.GetTile(tilePos);
        
        return tile != waterTile && tile != null;
    }
    
    public bool CanMoveTo(Vector2Int from, Vector2Int to)
    {
        // Check if move is adjacent (1 tile distance)
        int distance = Mathf.Abs(to.x - from.x) + Mathf.Abs(to.y - from.y);
        return distance == 1 && IsValidPosition(to);
    }
}
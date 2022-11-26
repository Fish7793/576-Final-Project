using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CanvasGraph : MonoBehaviour
{
    public Dictionary<Vector3Int, CanvasBlock> blocks = new();
    public BlockGraph graph = new();
    public Agent Agent { get; set; }

    public void Awake()
    {
        foreach (Transform child in transform)
        {
            var block = child.GetComponent<CanvasBlock>();
            block.transform.localPosition = ToGrid(block.transform.localPosition);
            block.queryTile = QueryTile;
            block.Begin(this);
            blocks.Add(block.transform.localPosition.ToVector3Int(), block);
            UpdateGraph();
        }
    }

    public void UpdateGraph()
    {
        graph = new BlockGraph();
        foreach (var kv in blocks)
        {
            graph.blocks.Add(kv.Value.Block);
            if (kv.Value.Block is StartBlock start)
            {
                graph.start = start;
            }
        }

        foreach (var kv in blocks)
        {
            kv.Value.RefreshIO();
        }
    }

    public CanvasBlock QueryTile(Vector3 pos)
    {
        var grd = ToGrid(transform.InverseTransformPoint(pos));
        return blocks.ContainsKey(grd) ? blocks[grd] : null;    
    }

    public static Vector3Int ToGrid(Vector3 pos)
    {
        return ((Vector3)(pos / LevelManager.gridScale).ToVector3Int() * LevelManager.gridScale).ToVector3Int();
    }

    public void AddToVisualGraph(CanvasBlock prefab, Vector3 pos)
    {
        var obj = GameObject.Instantiate(prefab, transform).GetComponent<CanvasBlock>();
        obj.transform.position = pos;
        obj.transform.localPosition = ToGrid(obj.transform.localPosition);
        obj.queryTile = QueryTile;
        obj.Begin(this);
        blocks.Add(obj.transform.localPosition.ToVector3Int(), obj);
        UpdateGraph();
    }

    public CanvasBlock RemoveFromVisualGraph(Vector3 pos)
    {
        var tile = QueryTile(pos);
        if (tile != null)
        {
            blocks.Remove(tile.transform.localPosition.ToVector3Int());
        }
        UpdateGraph();
        return tile;
    }

    public void UpdateVisualGraph(Vector3Int oldPos, Vector3Int newPos)
    {
        var tile = RemoveFromVisualGraph(oldPos);
        if (tile != null)
        {
            AddToVisualGraph(tile, newPos);
        }
    }

    public void Refresh()
    {
        foreach (var block in blocks)
        {
            block.Value.Begin(this);
            block.Value.queryTile = QueryTile;
        }
    }
}

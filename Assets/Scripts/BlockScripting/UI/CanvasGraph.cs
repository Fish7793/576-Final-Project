using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CanvasGraph : MonoBehaviour
{
    public Dictionary<Vector3Int, CanvasBlockBase> blocks = new();
    public HashSet<CanvasBlockBase> immune = new();
    public BlockGraph graph = new();
    public Agent Agent { get; set; }

    public void Awake()
    {
        foreach (Transform child in transform)
        {
            var block = child.GetComponent<CanvasBlockBase>();
            block.transform.localPosition = ToGrid(block.transform.localPosition);
            block.queryTile = QueryTile;
            block.Begin(this);
            blocks.Add(block.transform.localPosition.ToVector3Int(), block);
            immune.Add(block);
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

    public CanvasBlockBase QueryTile(Vector3 pos)
    {
        var grd = ToGrid(transform.InverseTransformPoint(pos));
        return blocks.ContainsKey(grd) ? blocks[grd] : null;    
    }

    public static Vector3Int ToGrid(Vector3 pos)
    {
        return ((Vector3)(pos / LevelManager.gridScale).ToVector3Int() * LevelManager.gridScale).ToVector3Int();
    }

    public bool AddToVisualGraph(CanvasBlockBase prefab, Vector3 pos, Vector3 eulerAngles)
    {
        bool add = true;
        if (QueryTile(pos) != null)
            add = RemoveFromVisualGraph(pos);

        if (add)
        {
            var obj = GameObject.Instantiate(prefab, transform).GetComponent<CanvasBlockBase>();
            obj.transform.position = pos;
            obj.transform.eulerAngles = eulerAngles;
            obj.transform.localPosition = ToGrid(obj.transform.localPosition);
            obj.queryTile = QueryTile;
            obj.Begin(this);
            blocks.Add(obj.transform.localPosition.ToVector3Int(), obj);
            UpdateGraph();
        }
        return true;
    }

    public bool RemoveFromVisualGraph(Vector3 pos)
    {
        var tile = QueryTile(pos);
        if (tile != null && !immune.Contains(tile))
        {
            blocks.Remove(tile.transform.localPosition.ToVector3Int());
            Destroy(tile.gameObject);
            UpdateGraph();

            return true;
        }
        return false;
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
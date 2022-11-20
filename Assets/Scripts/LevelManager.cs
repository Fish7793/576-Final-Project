using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public List<Tilemap> groundLayers;
    public List<Tilemap> propLayers;
    public Dictionary<PropInfo, Tilemap> info;
    public List<GameObject> active;

    public BlockGraph graph = new();
    public Agent playerAgent;

    void Start()
    {
        info = new Dictionary<PropInfo, Tilemap>();
        foreach (var prop in propLayers)
            foreach (Transform child in prop.transform)
            {
                info.Add(child.GetComponent<Prop>().GetInfo(), prop);
                active.Add(child.gameObject);
                if (child.name.ToLower().Contains("player"))
                {
                    playerAgent = child.GetComponent<Agent>();
                }
            }

        ActionBlock block = new()
        {
            action = playerAgent.Move
        };
        graph.blocks.Add(block);
        graph.start = block;

        ActionBlock rotate = new()
        {
            action = () => playerAgent.Rotate(90)
        };
        block.next = rotate;
        graph.blocks.Add(rotate);

        StartLevel();
    }

    public void StartLevel()
    {
        StartCoroutine(LevelLogic());
    }

    public IEnumerator LevelLogic()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            graph.Evaluate();

            foreach (var obj in active)
            {
                if (obj.TryGetComponent(out Prop p))
                {
                    yield return p.Step();
                }
            }
        }
    }

    public void ResetLevel()
    {
        StopCoroutine("LevelLogic");

        foreach (var gameObject in active)
            if (gameObject != null)
                Destroy(gameObject);
        
        active.Clear();

        foreach (var kv in info)
        {
            var layer = kv.Value;
            var propInfo = kv.Key;
            var prop = Instantiate(propInfo.prefab, layer.transform);
            prop.transform.position = propInfo.pos;
            prop.transform.eulerAngles = propInfo.rot;
            active.Add(prop.gameObject);
        }
    }
}

[System.Serializable]
public struct PropInfo 
{
    public Vector3Int pos;
    public Vector3 rot;
    public GameObject prefab;

    public PropInfo(Prop prop)
    {
        pos = prop.transform.position.ToVector3Int();
        rot = prop.transform.eulerAngles;
        prefab = prop.prefab;
    }
}
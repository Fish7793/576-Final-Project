using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public List<Tilemap> groundLayers;
    public List<Tilemap> propLayers;
    public Dictionary<Tilemap, PropInfo> info;
    public List<GameObject> active;

    void Start()
    {
        info = new Dictionary<Tilemap, PropInfo>();
        foreach (var prop in propLayers)
            foreach (Transform child in prop.transform)
            {
                info.Add(prop, child.GetComponent<Prop>().GetInfo());
                active.Add(child.gameObject);
            }
    }

    public void StartLevel()
    {

    }

    public void ResetLevel()
    {
        foreach (var gameObject in active)
            if (gameObject != null)
                Destroy(gameObject);
        
        active.Clear();

        foreach (var kv in info)
        {
            var layer = kv.Key;
            var propInfo = kv.Value;
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
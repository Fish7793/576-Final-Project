using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<string, GameObject> prefabs = new();

    public static void LoadAllResources<T>(string dir, Dictionary<string, T> dict) where T : Object
    {
        foreach (var item in Resources.LoadAll<T>(dir))
            dict.Add(item.name, item);
    }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            LoadAllResources("_Prefabs", prefabs);
        }
    }
}

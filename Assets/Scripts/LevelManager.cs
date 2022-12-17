using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public new Camera camera;

    [Header("Level Stuff")]
    public List<Tilemap> groundLayers;
    public List<Tilemap> propLayers;
    public Dictionary<PropInfo, Tilemap> info;
    public Dictionary<Vector3Int, GameObject> map;
    public List<GameObject> active;
    public Prop goal;

    public Agent playerAgent;

    bool running = false;
    public bool Running { get => running; }

    [Header("Block Editor Stuff")]
    public CanvasGraph canvasGraph;
    GameObject ghost;

    public MouseData mouseData;
    public Transform buttonRoot;
    public GameObject buttonPrefab;
    public List<CanvasBlockFolderData> blockFolders;
    bool placing = false;
    public static float gridScale = 50;

    void SetAgent(Agent a)
    {
        a.moveCheck = (v) =>
        {
            var sensed = active.FirstOrDefault(x => x.transform.position.ToVector3Int() == v);
            return sensed == null || (sensed != null && sensed.TryGetComponent(out Prop p) && !p.propTags.Contains(PropType.Wall));
        };
        a.isGround = (v) =>
        {
            return map.ContainsKey(v - new Vector3Int(0, 1, 0));
        };
        a.sense = (v) =>
        {
            var sensed = active.Where(x => x.transform.position.ToVector3Int() == v).ToList();
            if (sensed.Count > 0)
            {
                var obj = sensed[0];
                if (obj.TryGetComponent(out Prop prop))
                {
                    return prop.propTags;
                }
            }
            else if (map.TryGetValue(v - new Vector3Int(0, 1, 0), out GameObject _))
            {
                return new PropType[] { PropType.Floor };
            }

            return new PropType[] { PropType.None };
        };
    }

    void SetPlayer(GameObject player_object)
    {
        playerAgent = player_object.GetComponent<Agent>();
        SetAgent(playerAgent);
        playerAgent.stoppingCondition = (agent) =>
        {
            return agent.transform.position.ToVector3Int() == goal.transform.position.ToVector3Int();
        };
        canvasGraph.Agent = playerAgent;
    }

    void SetEnemy(GameObject enemy_object)
    {
        var e = enemy_object.GetComponent<EnemyAgent>();
        SetAgent(e);
        e.getPlayer = () => playerAgent;
        e.pathCost = (v) =>
        {
            float cost = 0.1f;
            if (!map.ContainsKey(v - new Vector3Int(0, 1, 0)))
            {
                cost += 1000f;
            }

            var sensed = active.Where(x => x.transform.position.ToVector3Int() == v).ToList();
            if (sensed.Count > 0)
            {
                if (sensed.First().TryGetComponent(out Prop prop))
                {
                    if (prop.propTags.Contains(PropType.Wall))
                        cost += 10f;
                }
            }
            return cost;
        };
    }

    public void Win()
    {
        AudioManager.Play(GameManager.sounds["win"]);
        var winPanel = GameObject.Instantiate(GameManager.prefabs["Win Canvas"]).GetComponent<WinPanel>();
        winPanel.nextLevelButton.onClick.AddListener(() => 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }

    void CreateFolders()
    {
        foreach (var folder in blockFolders)
        {
            var f = GameObject.Instantiate(GameManager.prefabs["Folder Prefab"], buttonRoot).GetComponent<CanvasBlockFolder>();
            f.Begin(this, folder);
        }
    }

    public void StartPlacement()
    {
        placing = true;
        if (mouseData.selection != null)
        {
            print("Creating Ghost!");
            ghost = GameObject.Instantiate(mouseData.selection);
            ghost.transform.SetParent(canvasGraph.transform);
        } 
        else
        {
            mouseData = new MouseData(MouseState.None, null);
        }
    }

    public void EndPlacement()
    {
        if (mouseData.selection != null)
        {
            canvasGraph.AddToVisualGraph(mouseData.selection.GetComponent<CanvasBlockBase>(), Input.mousePosition, ghost.transform.eulerAngles);
        }

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Destroy(ghost);
            placing = false;
            mouseData = new MouseData(MouseState.None, null);
        }
    }

    private void Update()
    {
        if (!running)
        {
            if (placing)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    int mod = -1;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        mod = 1;
                    }
                    ghost.transform.eulerAngles += new Vector3(0, 0, 90 * mod);
                }

                ghost.transform.position = Input.mousePosition;
                if (Input.GetMouseButtonDown(0))
                {
                    GameManager.instance.PlayMenuSelect();

                    EndPlacement();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    AudioManager.Play(GameManager.sounds["menu_back"], 1f);

                    mouseData = new MouseData(MouseState.None, null);
                    EndPlacement();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    var tile = canvasGraph.QueryTile(Input.mousePosition);
                    if (tile != null)
                    {
                        AudioManager.Play(GameManager.sounds["menu_back"], 1f);

                        mouseData = new MouseData(
                            MouseState.Placing, 
                            GameManager.prefabs.Where(
                                x => x.Value.name.Contains(tile.name.Replace("(Clone)", "").Trim())
                                  && x.Value.TryGetComponent(tile.GetType(), out _)
                            ).FirstOrDefault().Value
                        );
                        canvasGraph.RemoveFromVisualGraph(Input.mousePosition);
                        StartPlacement();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    GameManager.instance.PlayMenuSelect();

                    var tile = canvasGraph.QueryTile(Input.mousePosition);
                    if (tile != null)
                    {
                        mouseData = new MouseData(
                            MouseState.Placing,
                            GameManager.prefabs.Where(
                                x => x.Value.name.Contains(tile.name.Replace("(Clone)", "").Trim())
                                  && x.Value.TryGetComponent(tile.GetType(), out _)
                            ).FirstOrDefault().Value
                        );
                        StartPlacement();
                    }
                }

                if (Input.GetMouseButton(1))
                {
                    if (canvasGraph.RemoveFromVisualGraph(Input.mousePosition))
                        AudioManager.Play(GameManager.sounds["menu_back"], 1f);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Win();
        }
    }

    public void StartLevel()
    {
        if (!running)
        {
            AudioManager.Play(GameManager.sounds["menu_accept"], 0.8f);

            running = true;
            StartCoroutine(LevelLogic());
        }
    }

    public IEnumerator LevelLogic()
    {
        canvasGraph.Refresh();
        canvasGraph.UpdateGraph();
        while (running)
        {
            canvasGraph.graph.Evaluate();

            foreach (var obj in active)
            {
                if (obj != null && obj.TryGetComponent(out Prop p))
                {
                    p.Step();
                }
            }

            yield return new WaitForSeconds(2.5f);
            
            if (playerAgent.stopped)
            {
                if (playerAgent.positionTarget == goal.transform.position.ToVector3Int())
                    Win();
                break;
            }
        }
    }

    void Start()
    {
        info = new Dictionary<PropInfo, Tilemap>();
        map = new Dictionary<Vector3Int, GameObject>();

        foreach (var layer in groundLayers)
            foreach (Transform child in layer.transform)
            {
                map.Add(child.position.ToVector3Int(), child.gameObject);
            }

        foreach (var prop in propLayers)
            foreach (Transform child in prop.transform)
            {
                var cprop = child.GetComponent<Prop>();
                info.Add(cprop.GetInfo(), prop);
                active.Add(child.gameObject);
                if (child.name.ToLower().Contains("player"))
                {
                    SetPlayer(child.gameObject);
                }

                if (cprop.propTags.Contains(PropType.Goal))
                {
                    goal = cprop;
                }

                if (cprop is EnemyAgent e)
                {
                    SetEnemy(e.gameObject);
                }
            }

        CreateFolders();
    }

    public void ResetLevel()
    {
        AudioManager.Play(GameManager.sounds["reset"], 0.75f);
        running = false;
        StopAllCoroutines();
        foreach (var gameObject in active)
            if (gameObject != null)
                Destroy(gameObject);
        
        active.Clear();

        foreach (var kv in info)
        {
            var layer = kv.Value;
            var propInfo = kv.Key;
            var prop = Instantiate(GameManager.prefabs[propInfo.prefabName], layer.transform);
            prop.transform.position = propInfo.pos;
            prop.transform.eulerAngles = propInfo.rot;
            active.Add(prop);
            if (prop.name.ToLower().Contains("player"))
            {
                SetPlayer(prop);
            }

            if (prop.TryGetComponent(out Prop cprop) && cprop.propTags.Contains(PropType.Goal))
            {
                goal = cprop;
            }

            if (cprop is EnemyAgent e)
            {
                SetEnemy(e.gameObject);
            }
        }
    }
}

[System.Serializable]
public struct PropInfo 
{
    public Vector3Int pos;
    public Vector3 rot;
    public GameObject prefab;
    public string prefabName;

    public PropInfo(Prop prop)
    {
        pos = prop.transform.position.ToVector3Int();
        rot = prop.transform.eulerAngles;
        prefab = prop.prefab;
        prefabName = prop.prefab.name;
    }
}

[System.Serializable]
public struct MouseData
{
    public bool holding;
    public MouseState state;
    public GameObject selection;

    public MouseData(MouseState state, GameObject selection)
    {
        this.state = state;
        this.selection = selection;
        holding = selection != null;
    }
}

public enum MouseState
{
    None, Placing, Selecting
}
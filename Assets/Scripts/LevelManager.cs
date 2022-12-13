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
    public List<CanvasBlockBase> blockPrefabs;
    List<Button> buttons;
    bool placing = false;
    public static float gridScale = 50;

    void SetPlayer(GameObject player_object)
    {
        playerAgent = player_object.GetComponent<Agent>();

        /**
         * 
         */
        playerAgent.moveCheck = (v) =>
        {
            return map.ContainsKey(v - new Vector3Int(0, 1, 0));
        };

        playerAgent.isGround = (v) =>
        {
            return map.ContainsKey(v - new Vector3Int(0, 1, 0));
        };

        /**
         * 
         */
        playerAgent.sense = (v) =>
        {
            var sensed = active.Where(x => x.transform.position.ToVector3Int() == v).ToList();
            if (sensed.Count > 0)
            {
                var obj = sensed[0];
                if (obj.TryGetComponent(out Prop prop))
                {
                    print("Found " + prop);
                    return prop.propTags;
                }
            } 
            else if (map.TryGetValue(v - new Vector3Int(0, 1, 0), out GameObject _))
            {
                print("Floor");
                return new PropType[] { PropType.Floor };
            }

            return new PropType[] { PropType.None };
        };

        /**
         * 
         */
        playerAgent.stoppingCondition = (agent) =>
        {
            return agent.positionTarget == goal.transform.position.ToVector3Int();
        };
        canvasGraph.Agent = playerAgent;
    }

    public void Win()
    {
        var winPanel = GameObject.Instantiate(GameManager.prefabs["Win Canvas"]).GetComponent<WinPanel>();
        winPanel.nextLevelButton.onClick.AddListener(() => 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }

    void CreateButtons()
    {
        foreach (var block in blockPrefabs)
        {
            var button = GameObject.Instantiate(buttonPrefab, buttonRoot).GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                if (!Running)
                {
                    Debug.Log(block.name);
                    mouseData = new MouseData(MouseState.Placing, block.gameObject);
                    StartPlacement();
                }
            });

            var text = button.GetComponentInChildren<TMP_Text>();
            text.text = block.name
                .Replace("Prefab", "")
                .Replace("Block", "")
                .Trim();
        }
    }

    void StartPlacement()
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

    void EndPlacement()
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
            }

        CreateButtons();
    }

    private void Update()
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
                EndPlacement();
            } 
            else if (Input.GetMouseButtonDown(1))
            {
                mouseData = new MouseData(MouseState.None, null);
                EndPlacement();
            }
        } 
        else
        {
            if (Input.GetMouseButton(1))
            {
                canvasGraph.RemoveFromVisualGraph(Input.mousePosition);
            }
        }
    }

    public void StartLevel()
    {
        running = true;
        StartCoroutine(LevelLogic());
    }

    public IEnumerator LevelLogic()
    {
        canvasGraph.Refresh();
        canvasGraph.UpdateGraph();
        while (running)
        {

            if (playerAgent.stopped)
            {
                if (playerAgent.positionTarget == goal.transform.position.ToVector3Int())
                    Win();
                break;
            }

            canvasGraph.graph.Evaluate();

            foreach (var obj in active)
            {
                if (obj != null && obj.TryGetComponent(out Prop p))
                {
                    yield return p.Step();
                }
            }

            yield return new WaitForSeconds(2.5f);
        }
    }

    public void ResetLevel()
    {
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
            active.Add(prop.gameObject);
            if (prop.name.ToLower().Contains("player"))
            {
                SetPlayer(prop);
            }

            if (prop.TryGetComponent(out Prop cprop) && cprop.propTags.Contains(PropType.Goal))
            {
                goal = cprop;
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
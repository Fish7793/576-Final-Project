using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBlockFolder : MonoBehaviour
{
    public List<CanvasBlockBase> blockPrefabs;

    public TMPro.TMP_Text label;
    public RectTransform contentRoot;
    public Toggle toggle;
    public Image toggleImage;

    RectTransform rt;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Begin(LevelManager levelManager, CanvasBlockFolderData data)
    {
        label.text = data.name;
        blockPrefabs = data.blockPrefabs;

        foreach (var block in blockPrefabs)
        {
            var button = Instantiate(GameManager.prefabs["Component Button Prefab"], contentRoot).GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                GameManager.instance.PlayMenuSelect();
                if (!levelManager.Running)
                {
                    Debug.Log(block.name);
                    levelManager.mouseData = new MouseData(MouseState.Placing, block.gameObject);
                    levelManager.StartPlacement();
                }
            });

            button.GetComponentInChildren<TMPro.TMP_Text>().text = block.name.Replace("Block", "").Replace("Prefab", "").Trim();
            button.GetComponent<Image>().color = block.GetComponentInChildren<Image>().color;
        }

        toggle.onValueChanged.AddListener(x =>
        {
            if (x)
            {
                GameManager.instance.PlayMenuSelect();

                toggleImage.transform.localScale = new Vector3(1, -1, 1);
                contentRoot.sizeDelta = new Vector2(0, blockPrefabs.Count * 25 + 12);
                rt.sizeDelta = new Vector2(60, blockPrefabs.Count * 25 + 12 + 30 + 20 + 2);
            }
            else
            {
                AudioManager.Play(GameManager.sounds["menu_back"], 1f);

                toggleImage.transform.localScale = Vector3.one;
                contentRoot.sizeDelta = new Vector2(0, 0);
                rt.sizeDelta = new Vector2(60, 30 + 20 + 2);
            }
        });
    }
}

[System.Serializable]
public struct CanvasBlockFolderData
{
    public string name;
    public List<CanvasBlockBase> blockPrefabs;
}
using UnityEngine;

public class HowToMenu : MonoBehaviour
{
    public Transform contentRoot;
    public HowToInfo info;
    public int index;

    public void Start()
    {
        Populate();
    }

    public void Populate()
    {
        foreach (Transform t in contentRoot)
            Destroy(t.gameObject);
        GameObject.Instantiate(info.items[index], contentRoot);
        GameManager.instance.PlayMenuSelect();
    }

    public void Next()
    {
        index++;
        if (index >= info.items.Count)
        {
            index = 0;
        }

        Populate();
    }

    public void Prev()
    {
        index--;
        if (index < 0) 
        { 
            index = info.items.Count - 1;
        }

        Populate();
    }
}

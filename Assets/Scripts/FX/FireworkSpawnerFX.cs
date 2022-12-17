using System.Collections;
using UnityEngine;

public class FireworkSpawnerFX : MonoBehaviour
{
    public GameObject fireworkPrefab;

    public void Start()
    {
        StartCoroutine(Routine());
    }

    public IEnumerator Routine()
    {
        while (true)
        {
            var r = Random.insideUnitCircle;
            Instantiate(fireworkPrefab).transform.position = new Vector3(r.x, 1, r.y) * 5;
            yield return new WaitForSeconds(0.2f + Random.value / 4);
        }
    }
}

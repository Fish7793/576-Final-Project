using System.Linq;
using UnityEngine;

public class Mine : Prop
{
    public float explosionForce = 2;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Agent a))
        {
            Explode();
            a.stopped = true;
            var rb = a.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.AddForce((-(transform.position - collision.transform.position).normalized / 3 + new Vector3(0, 1, 0)) * explosionForce, ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.value * 2 - 1, Random.value * 2 - 1, Random.value * 2 - 1) / 3, ForceMode.Impulse);
            GameManager.instance.StartCoroutine(GameManager.Delay(() => { if (a != null) a.Die(); }, 1));
        }
    }

    public void Explode()
    {
        var i = Instantiate(GameManager.prefabs["ExplodeFX"], transform);
        i.transform.SetParent(null);
        i.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
        Destroy(gameObject);
    }
}
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public GameObject hitVFX;
    public Rigidbody2D rb;
    public float launchForce;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        rb.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        Invoke(nameof(OnDespawn), 4f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Character>().OnHit(10f);
            Instantiate(hitVFX, transform.position, transform.rotation);
            OnDespawn();
        }
    }
}

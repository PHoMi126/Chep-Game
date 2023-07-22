using UnityEngine;

public class LineController : MonoBehaviour
{
    public GameObject hitVFX;
    public float power = 5;
    private LineRenderer lineRenderer;
    public Rigidbody2D rb;
    Vector2 DragStartPos;

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 DragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 velocity = (DragEndPos - DragStartPos) * power;

            Vector2[] trajectory = Plot(rb, (Vector2)transform.position, velocity, 500);
            lineRenderer.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }
            lineRenderer.SetPositions(positions);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 DragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 velocity = (DragEndPos - DragStartPos) * power;

            rb.velocity = velocity;
        }
    }

    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] result = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            result[i] = pos;
        }

        return result;
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

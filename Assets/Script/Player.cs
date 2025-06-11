using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    Rigidbody2D rb;
    private Animator anim;
    private bool Swipe = false;
    private float previousDirection = 0f;
    [SerializeField] private UIManager uiManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float targetX = touchPosition.x;
            float currentX = transform.position.x;

            float direction = Mathf.Sign(targetX - currentX);
            float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            if (!Swipe)
            {
                Blink();
                Swipe = true;
                previousDirection = direction;
            }
            else
            {
                if (direction != previousDirection)
                {
                    Blink();
                    previousDirection = direction;
                }
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            Swipe = false;
        }
    }

    private void Blink()
    {
        if (anim != null)
        {
            anim.SetTrigger("Blink");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Block")
        {
            uiManager.ShowGameOver();
            Time.timeScale = 0f;
        }
    }
}
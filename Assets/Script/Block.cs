using UnityEngine;
using UnityEngine.Pool;

public class Block : MonoBehaviour
{
    private Transform player;
    private ObjectPool<Block> pool;
    private bool hasScored = false;
    private Rigidbody2D rb; 

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void SetPool(ObjectPool<Block> objectPool)
    {
        pool = objectPool;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        if (!hasScored && transform.position.y < player.position.y)
        {
            hasScored = true;
            Object.FindAnyObjectByType<GameManager>().AddScore();
        }
        if (transform.position.y < -4f)
        {
            VFXPool.Instance.PlayVFX(transform.position); 
            AudioManager.Instance.PlayBlockImpact();
            pool.Release(this);
        }
    }

    public void SetFallGravity(float gravity)
    {
        rb.gravityScale = gravity;
    }
    
    public void ResetState()
    {
        hasScored = false;
    }
}

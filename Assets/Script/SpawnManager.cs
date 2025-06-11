using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float maxX;

    private float spawnInterval = 1.5f;
    private float minSpawnInterval = 0.4f;
    private float difficultyRamp = 0.05f; // cuanto se reduce cada escalado

    private ObjectPool<Block> blockPool;
    private Transform playerTransform;
    private Coroutine spawnCoroutine;

    void Awake()
    {
        blockPool = new ObjectPool<Block>(
            createFunc: CreateBlock,
            actionOnGet: OnGetBlock,
            actionOnRelease: OnReleaseBlock,
            actionOnDestroy: OnDestroyBlock,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 30
        );
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnBlock();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnBlock()
    {
        Vector3 pos = spawnPoint.position;
        pos.x = Random.Range(-maxX, maxX);

        Block block = blockPool.Get();
        block.transform.position = pos;
        block.transform.rotation = Quaternion.identity;
        block.gameObject.SetActive(true);
        block.SetPlayer(playerTransform);
        block.SetPool(blockPool);
    }

    public void IncreaseDifficulty()
    {
        if (spawnInterval > minSpawnInterval)
        {
            spawnInterval -= difficultyRamp;
            spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval);
            Debug.Log("Increased difficulty. New spawn interval: " + spawnInterval);
        }
    }

    private Block CreateBlock() => Instantiate(blockPrefab).GetComponent<Block>();
    private void OnGetBlock(Block block) => block.ResetState();
    private void OnReleaseBlock(Block block) => block.gameObject.SetActive(false);
    private void OnDestroyBlock(Block block) => Destroy(block.gameObject);
}

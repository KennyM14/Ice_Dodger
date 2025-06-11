using UnityEngine;
using UnityEngine.Pool;
using System.Collections; 

public class VFXPool : MonoBehaviour
{

    public static VFXPool Instance;
    [SerializeField] private ParticleSystem breakVFXPrefab;
    private ObjectPool<ParticleSystem> vfxPool;

     void Awake()
    {
        Instance = this;

        vfxPool = new ObjectPool<ParticleSystem>(
            createFunc: () => Instantiate(breakVFXPrefab),
            actionOnGet: (vfx) => vfx.gameObject.SetActive(true),
            actionOnRelease: (vfx) => vfx.gameObject.SetActive(false),
            actionOnDestroy: (vfx) => Destroy(vfx.gameObject),
            defaultCapacity: 10,
            maxSize: 30
        );
    }

    public void PlayVFX(Vector3 position)
    {
        ParticleSystem vfx = vfxPool.Get();
        vfx.transform.position = position;
        vfx.transform.rotation = Quaternion.identity;

        vfx.Play();

        StartCoroutine(ReleaseAfterDuration(vfx));
    }

    private IEnumerator ReleaseAfterDuration(ParticleSystem vfx)
    {
        yield return new WaitForSeconds(vfx.main.duration);
        vfxPool.Release(vfx);
    }
}

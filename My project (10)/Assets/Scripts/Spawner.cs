using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _delay;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => GetAction(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    } 

    private void OnDisabled(Cube cube)
    {
        cube.Returned -= ReturnToPool;       
    }

    private void GetAction(Cube cube)
    {
        cube.Returned += ReturnToPool;
        cube.gameObject.SetActive(true);
        cube.gameObject.transform.position = GetPosition();
    }

    private void ReturnToPool(Cube cube)
    {
        _pool.Release(cube);
    }

    private Vector3 GetPosition()
    {
        float minXValue = 0;
        float maxXValue = 20;
        float minYValue = 2;
        float maxYValue = 15;
        float minZValue = -10;
        float maxZValue = 10;

        Vector3 spawnPosition = new Vector3(Random.Range(minXValue, maxXValue), Random.Range(minYValue, maxYValue), Random.Range(minZValue, maxZValue));

        return spawnPosition;
    }

    private IEnumerator SpawnCubes()
    {
        var wait = new WaitForSeconds(_delay);

        while (true)
        {
            _pool.Get();
            yield return wait;
        }
    }
}

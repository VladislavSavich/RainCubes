using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    private ObjectPool<Cube> _pool;
    private int _poolCapacity = 10;
    private int _poolMaxSize = 10;
    private float _repeatRate = 1f;
    private Vector3 _maximumSpawnCoordinates = new Vector3(50, 30, 50);
    private Vector3 _minimumSpawnCoordinates = new Vector3(20, 25, 20);

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_cubePrefab),
        actionOnGet: (cube) => ActionOnGet(cube),
        actionOnRelease: (cube) => cube.gameObject.SetActive(false),
        actionOnDestroy: (cube) => Destroy(cube),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void ActionOnGet(Cube cube) 
    {
        cube.transform.position = GenerateRandomPosition();
        cube.gameObject.SetActive(true);
        cube.CubeFallenDown += ReleaseCube;
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private IEnumerator SpawnCubes() 
    {
        while (enabled)  
        {
            yield return new WaitForSeconds(_repeatRate);

            if (_pool.CountActive < _poolMaxSize)
            {
                _pool.Get();
            }
        }
    }

    private void ReleaseCube(Cube cube)
    {
        cube.CubeFallenDown -= ReleaseCube;
        _pool.Release(cube);
        cube.ResetÑondition();
    }

    private Vector3 GenerateRandomPosition() 
    {
        float positionX = Random.Range(_minimumSpawnCoordinates.x, _maximumSpawnCoordinates.x);
        float positionY = Random.Range(_minimumSpawnCoordinates.y, _maximumSpawnCoordinates.y);
        float positionZ = Random.Range(_minimumSpawnCoordinates.z, _maximumSpawnCoordinates.z);

        return new Vector3(positionX, positionY, positionZ);
    }
}

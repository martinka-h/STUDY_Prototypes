using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {
    #region Private members

    private static ObjectPoolManager _instance;
    private readonly Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();
    private HashSet<GameObject> _activeObjects = new HashSet<GameObject>();

    #endregion Private Members

    #region Unity Callbacks

    private void Awake()
    {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    #endregion Unity Callbacks

    #region Spawning

    public static GameObject SpawnGameObject(GameObject prefab, bool setActive = true)
    {
        if (_instance == null) return null;

        GameObject go = _instance.DequeGameObject(prefab);
        if (go != null) {
            go.SetActive(setActive);
        } else {
            go = _instance.InstantiateGameObject(prefab, setActive);
        }

        _instance._activeObjects.Add(go);

        return go;
    }

    public static GameObject SpawnGameObject(GameObject prefab, Vector3? position, Quaternion? rotation, bool setActive = true)
    {
        if (_instance == null) return null;
        GameObject go = _instance.DequeGameObject(prefab);
        if (go != null) {
            if (position != null) {
                go.transform.position = position.Value;
            }

            if (rotation != null) {
                go.transform.rotation = rotation.Value;

            }
            go.SetActive(setActive);
        } else {
            go = _instance.InstantiateGameObject(prefab, position ?? Vector3.zero, rotation ?? Quaternion.identity, setActive);
        }

        _instance._activeObjects.Add(go);

        return go;
    }

    #endregion Spawning

    #region Despawning / Destroying

    public static void DespawnGameObject(GameObject go)
    {
        if (go == null || !_instance._activeObjects.Contains(go)) {
            return;
        }
        go.SetActive(false);
        _instance._activeObjects.Remove(go);
        var pool = _instance.GetPool(go);
        pool.Enqueue(go);
    }

    public static void PermanentlyDestroyGameObjectsOfType(GameObject prefab)
    {
        if (_instance == null) return;
        var queue = _instance.GetPool(prefab);
        GameObject go;
        while (queue?.Count > 0) {
            go = queue.Dequeue();
            if (go != null) {
                if (go.activeSelf) {
                    go.SetActive(false);
                }

                Destroy(go);
            }
        }
    }

    public static void EmptyPool()
    {
        if (_instance == null) return;
        foreach (Queue<GameObject> pool in _instance._pool.Values) {
            foreach (var go in pool) {
                Destroy(go);
            }
        }
        _instance._pool.Clear();
    }

    #endregion

    #region Private methods

    private GameObject DequeGameObject(GameObject prefab)
    {
        var queue = GetPool(prefab);
        if (queue.Count < 1) return null;
        GameObject go = queue.Dequeue();
        if (go == null) {
            Debug.LogWarning("Dequeued null gameObject (" + prefab.name + ") from pool.");
        }

        return go;
    }

    private GameObject InstantiateGameObject(GameObject prefab, bool setActive)
    {
        var queue = GetPool(prefab);
        var go = Instantiate(prefab);
        DontDestroyOnLoad(go);
        go.SetActive(setActive);
        return go;
    }

    private GameObject InstantiateGameObject(GameObject prefab, Vector3 position, Quaternion rotation, bool setActive)
    {
        GameObject go = InstantiateGameObject(prefab, setActive);
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }

    private GameObject InstantiateGameObject(GameObject prefab, Transform parentTransform, bool setActive)
    {
        GameObject go = InstantiateGameObject(prefab, parentTransform.position, parentTransform.rotation, setActive);
        go.transform.SetParent(parentTransform);
        return go;
    }

    private Queue<GameObject> GetPool(GameObject prefab)
    {
        if (_pool.TryGetValue(prefab.BaseName(), out var pool)) {
            return pool;
        }

        pool = new Queue<GameObject>();
        _pool.Add(prefab.BaseName(), pool);
        return pool;
    }

    #endregion
}

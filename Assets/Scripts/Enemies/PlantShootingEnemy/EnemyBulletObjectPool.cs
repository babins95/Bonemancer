using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBulletObjectPool : MonoBehaviour
{
    [SerializeField] private PoolableObject originalPrefab;
    [SerializeField] private int instances;

    private PoolableObject[] _objects;
    void Start()
    {
        _objects = new PoolableObject[instances];
        for (int i = 0; i < instances; i++)
        {
            PoolableObject newObject = Instantiate(originalPrefab);

            _objects[i] = newObject;
            newObject.gameObject.SetActive(false);

#if UNITY_EDITOR
            newObject.name = $"{originalPrefab.name} (Clone #{i})";
#endif
        }
    }
    public GameObject Get()
    {
        PoolableObject obj = _objects.FirstOrDefault(o => !o.gameObject.activeSelf);
        if (obj != null)
        {
            obj.gameObject.SetActive(true);
            obj.OnCloneActivated();

            return obj.gameObject;
        }

        return null;
    }
    public void Release(GameObject obj)
    {
        obj.SetActive(false);
    }
}

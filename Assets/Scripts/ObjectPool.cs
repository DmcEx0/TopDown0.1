using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    public T Prefab { get; private set; }
    public bool IsAutoExpand { get; set; }
    public Transform Container { get; private set; }

    private List<T> _pool;

    public ObjectPool(T prefab, int count, Transform container)
    {
        Prefab = prefab;
        Container = container;

        CreatePool(count);
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var elementInPool in _pool)
        {
            if (elementInPool.gameObject.activeInHierarchy == false)
            {
                element = elementInPool;
                elementInPool.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
            return element;

        if (IsAutoExpand)
            return CreateObject(true);

        throw new Exception($"There is no free element in pool of type {typeof(T)}");
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>();

        for (int i = 0; i < count; i++)
            CreateObject();
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = GameObject.Instantiate(Prefab, Container);
        createdObject.gameObject.SetActive(isActiveByDefault);

        _pool.Add(createdObject);
        return createdObject;
    }
}

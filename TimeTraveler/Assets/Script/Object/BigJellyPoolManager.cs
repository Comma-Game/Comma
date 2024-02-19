using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigJellyPoolManager : MonoBehaviour
{
    GameObject _obj;
    int _count;

    GameObject _parent;
    Queue<GameObject> _queue, _usedQueue;

    public void Awake()
    {
        _parent = GameObject.Find("BigJellyPoolManager");
    }

    public void SetObject(GameObject obj)
    {
        _queue = new Queue<GameObject>();
        _usedQueue = new Queue<GameObject>();

        _obj = obj;

        _count = 10;

        MakeObject();
    }

    void MakeObject()
    {
        for (int i = 0; i < _count; i++)
        {
            GameObject obj = Instantiate(_obj);
            obj.SetActive(false);
            obj.transform.SetParent(_parent.transform);
            _queue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        GameObject obj;
        if (_queue.Count > 0) obj = _queue.Dequeue();
        else
        {
            obj = Instantiate(_obj);
            obj.SetActive(false);
        }

        obj.transform.SetParent(_parent.transform);
        _usedQueue.Enqueue(obj);

        return obj;
    }

    public void ReturnObject(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject obj = _usedQueue.Dequeue();
            obj.SetActive(false);
            _queue.Enqueue(obj);
        }
    }
}

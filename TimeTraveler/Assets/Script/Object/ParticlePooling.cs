using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFooling : MonoBehaviour
{
    GameObject _particle;
    int _count;

    GameObject _parent;
    Queue<GameObject> _queue;

    public void Awake()
    {
        _parent = GameObject.Find("JellyPoolManager");
    }

    public void SetParticle(GameObject obj)
    {
        _queue = new Queue<GameObject>();
        _particle = obj;

        _count = 3;
        
        StopAllCoroutines();
        MakeParticle();
    }

    void MakeParticle()
    {
        for(int i = 0; i < _count; i++)
        {
            GameObject obj = Instantiate(_particle);
            obj.SetActive(false);
            obj.transform.SetParent(_parent.transform);
            _queue.Enqueue(obj);
        }
    }

    public void EnableParticle(Vector3 pos, Transform parent)
    {
        GameObject obj;
        if (_queue.Count > 0) obj = _queue.Dequeue();
        else obj = Instantiate(_particle);
        
        obj.transform.position = pos;
        obj.transform.SetParent(parent);
        obj.SetActive(true);
    }
}

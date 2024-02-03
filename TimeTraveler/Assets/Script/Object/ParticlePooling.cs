using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFooling : MonoBehaviour
{
    GameObject[] _particle;
    int[] _count;

    Queue<GameObject>[] _queue;

    public void SetParticle(GameObject[] objs)
    {
        _queue = new Queue<GameObject>[2];
        for (int i = 0; i < 2; i++) _queue[i] = new Queue<GameObject>();
        _count = new int[2];

        _particle = objs;

        _count[0] = 3;
        _count[1] = 1;

        StopAllCoroutines();
        MakeParticle();
    }

    void MakeParticle()
    {
        for(int i = 0; i < 2; i++)
        {
            for (int j = 0; j < _count[0]; j++)
            {
                GameObject obj = Instantiate(_particle[i]);
                obj.SetActive(false);
                _queue[i].Enqueue(obj);
            }
        }
    }

    public void EnableParticle(int index, Vector3 pos)
    {
        GameObject obj;
        if (_queue[index].Count > 0) obj = _queue[index].Dequeue();
        else obj = Instantiate(_particle[index]);
        
        obj.transform.position = pos;
        obj.SetActive(true);
    }
}

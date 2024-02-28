using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObjectManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> _staticObject;

    [SerializeField]
    int _isRandomCount;

    GameObject _parent;
    List<int> _child;
    List<Transform> _usedObject;

    void Awake()
    {
        _usedObject = new List<Transform>();
    }

    private void OnEnable()
    {
        foreach (Transform obj in _usedObject)
        {
            obj.SetParent(transform);
            obj.gameObject.SetActive(true);
            obj.GetComponent<FallObject>().enabled = false;
        }
        _usedObject.Clear();

        //무조건 떨어져야 하는 오브젝트
        if (_staticObject != null)
        {
            foreach (Transform obj in _staticObject)
            {
                obj.gameObject.SetActive(true);
                obj.GetComponent<FallObject>().enabled = true;

                _usedObject.Add(obj);
            }
        }

        int cnt = 0;
        _child = new List<int>();

        while (cnt++ < _isRandomCount)
        {
            List<int> tempChild = new List<int>();
            for (int i = 0; i < transform.childCount; i++) tempChild.Add(i);

            int random = Random.Range(0, tempChild.Count);
            _child.Add(tempChild[random]);
            tempChild.RemoveAt(random);
        }

        //랜덤으로 떨어지는 오브젝트
        foreach (int i in _child)
        {
            Transform obj = transform.GetChild(i);
            obj.gameObject.SetActive(true);
            obj.GetComponent<FallObject>().enabled = true;

            _usedObject.Add(obj);
        }
    }

    private void OnDisable()
    {
        foreach (Transform obj in _usedObject)
        {
            if (obj.gameObject.activeSelf)
            {
                obj.GetComponent<FallObject>().enabled = false;
                obj.gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        _parent = GameObject.Find("StageParent");
    }

    public Transform GetStageParent() { return _parent.transform; }
}

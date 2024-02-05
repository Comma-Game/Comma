using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFish : MoveObstacle
{
    [SerializeField]
    int _moveX;
    
    [SerializeField]
    int _moveZ;

    [SerializeField]
    bool _compareX;

    protected override bool CompareX { get { return _compareX; } }

    [SerializeField]
    bool _compareY;

    protected override bool CompareY { get { return _compareY; } }

    [SerializeField]
    bool _compareZ;

    protected override bool CompareZ { get { return _compareZ; } }

    [SerializeField]
    float _moveSpeed;

    protected override float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField]
    float _minPos;

    protected override float MinPos { get { return _minPos; } }

    [SerializeField]
    float _maxPos;

    protected override float MaxPos { get { return _maxPos; } }

    Vector3 _sPos;

    private void Awake()
    {
        _sPos = transform.localPosition;
    }

    private void Start()
    {
        StartCoroutine(Move());
    }

    override protected IEnumerator Move()
    {
        float pos;
        while (true)
        {
            if (_compareX) pos = transform.localPosition.x;
            else if (_compareY) pos = transform.localPosition.y;
            else pos = transform.localPosition.z;

            if (pos <= _minPos || pos >= _maxPos) transform.localPosition = _sPos;

            transform.Translate(new Vector3(_moveSpeed * _moveX, 0, _moveSpeed * _moveZ));
            yield return new WaitForSeconds(0.02f);
        }
    }
}

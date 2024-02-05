using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MoveObstacle : MonoBehaviour
{
    abstract protected bool CompareX { get; }

    abstract protected bool CompareY { get; }

    abstract protected bool CompareZ { get; }

    abstract protected float MoveSpeed { get; }

    abstract protected float MinPos { get; }

    abstract protected float MaxPos { get; }

    abstract protected IEnumerator Move();
}

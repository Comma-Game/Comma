using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRange : MonoBehaviour
{
    [SerializeField]
    Material[] _materials;

    public void SetInvincible()
    {
        gameObject.GetComponent<MeshRenderer>().material = _materials[1];
    }

    public void SetSkill()
    {
        gameObject.GetComponent<MeshRenderer>().material = _materials[2];
    }

    public void ReSetColor()
    {
        gameObject.GetComponent<MeshRenderer>().material = _materials[0];
    }
}

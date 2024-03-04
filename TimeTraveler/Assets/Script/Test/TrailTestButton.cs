using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTestButton : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    [SerializeField]
    int _num;

    GameObject _trail;

    public void ChangeTrail()
    {
        if (_trail != null) _trail.SetActive(false);
        GameObject trail = Resources.LoadAll<GameObject>("TrailItem")[_num];
        _trail = Instantiate(trail, _player.transform);
    }
}

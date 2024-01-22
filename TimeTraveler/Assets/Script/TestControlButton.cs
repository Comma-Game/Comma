using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControlButton : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    public void TogglePlayerScript()
    {
        _player.GetComponent<TestMovePlayer>().enabled = !_player.GetComponent<TestMovePlayer>().enabled;
        _player.GetComponent<MovePlayer>().enabled = !_player.GetComponent<MovePlayer>().enabled;
    }
}

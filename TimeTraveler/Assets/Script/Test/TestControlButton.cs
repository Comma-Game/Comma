using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestControlButton : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    [SerializeField]
    TextMeshProUGUI _text;

    bool _toggle;

    private void Start()
    {
        _toggle = _player.GetComponent<MovePlayer>().enabled;
        _text.text = "Change To Mobile Mode";
    }

    private void Update()
    {
        if(!_toggle)
        {
            _text.text = "Change To Mobile Mode";
        }
        else
        {
            _text.text = "Change To PC Mode";
        }
        _text.text += "\n" + StageController.Instance.GetStageVelocity();
    }

    public void TogglePlayerScript()
    {
        if(!_toggle)
        {
            _player.GetComponent<TestMovePlayer>().enabled = false;
            _player.GetComponent<MovePlayer>().enabled = true;
        }
        else
        {
            _player.GetComponent<TestMovePlayer>().enabled = true;
            _player.GetComponent<MovePlayer>().enabled = false;
        }
        _toggle = !_toggle;
    }
}

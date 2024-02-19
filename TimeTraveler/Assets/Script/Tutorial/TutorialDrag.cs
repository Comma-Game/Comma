using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDrag : MonoBehaviour
{
    [SerializeField]
    Slider _slider;

    [SerializeField]
    int _x;
    
    [SerializeField]
    int _z;

    [SerializeField]
    int _power;

    [SerializeField]
    GameObject _arrow;

    private void OnEnable()
    {
        TutorialManager.Instance.SetDragTrue();
        ResetFunctionUI();
        _slider.onValueChanged.AddListener(OnDrag);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                if (_slider.value == 1)
                {
                    TutorialManager.Instance.SetDragFalse();
                    TutorialManager.Instance.SetStopFalse();
                    TutorialManager.Instance.MovePlayer(new Vector3(_x, 0, _z) * _power);
                }
                else
                {
                    _slider.value = 0;
                    _arrow.SetActive(false);
                }
            }
        }
    }

    void OnDrag(float v)
    {
        _arrow.SetActive(true);
    }

    void ResetFunctionUI()
    {
        _slider.onValueChanged.RemoveAllListeners();
        _slider.value = 0;
        _arrow.SetActive(false);
    }
}

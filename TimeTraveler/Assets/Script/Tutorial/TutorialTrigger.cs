using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject _ui;

    [SerializeField]
    bool _isEnergyZone;

    [SerializeField]
    bool _setPlayerPosition;

    [SerializeField]
    bool _isLastStop;

    [SerializeField]
    bool _isExit;

    [SerializeField]
    bool _isBackground;

    [SerializeField]
    bool _isCheckPortal;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (_isExit)
            {
                TutorialManager.Instance.ExitTutorial();
                return;
            }

            TutorialManager.Instance.SetCurrentUI(_ui);
            TutorialManager.Instance.SetStopTrue();

            if (_isEnergyZone) TutorialManager.Instance.ChargeEnergy();
            if (_setPlayerPosition) TutorialManager.Instance.SetPlayerPosition();
            if (_isLastStop) TutorialManager.Instance.SetMovePlayer();
            if (_isBackground) TutorialManager.Instance.EnableBackgroundUI();
            if (_isCheckPortal) TutorialManager.Instance.EnablePortalImage();
        }
    }
}

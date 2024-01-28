using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] public GameObject settingPanel;

    public void OnSettingPanel(bool isActive){
        AudioManager.Instance.PlayGameButtonClick();
        settingPanel.SetActive(isActive);
    }
}

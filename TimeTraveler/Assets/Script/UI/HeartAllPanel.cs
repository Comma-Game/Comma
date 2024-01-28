using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAllPanel : MonoBehaviour
{
    [SerializeField] public GameObject HeartPanel;
    private HeartPanel HeartPanelCS;
    [SerializeField] public GameObject HeartShopPanel;
    private HeartShopPanel HeartShopPanelCS;

    void Start()
    {
        HeartPanelCS = HeartPanel.GetComponent<HeartPanel>();
        HeartShopPanelCS = HeartShopPanel.GetComponent<HeartShopPanel>();
    }

    public void OnHeartShopPanel(bool isActive){
        AudioManager.Instance.PlayGameButtonClick();
        HeartShopPanel.SetActive(isActive);
    }

}

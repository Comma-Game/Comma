using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 필요

public class MainSceneController : MonoBehaviour
{
    [SerializeField]
    public GameObject background;
    [SerializeField]
    public float rotationSpeed = 50f;

    void Update()
    {
        // 회전 속도에 따라 물체를 자기 중심으로 회전
        background.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    public void GamePlay(){
        Debug.Log("GamePlay");
        SceneManager.LoadScene(1);
	}

    public void StatShop(){
        Debug.Log("StatShop");
	}

	public void BoxShop(){
        Debug.Log("BoxShop");
	}

	public void GameSetting(){
        Debug.Log("GameSetting");
	}

    public void UpgradeStatHP(){
        Debug.Log("UpgradeStatHP");
	}

    public void UpgradeStatEnergy(){
        Debug.Log("UpgradeStatEnergy");
	}

	public void UpgradeStat(){
        Debug.Log("UpgradeStat");
	}

	public void GetMysteryBox(){
        Debug.Log("GetMysteryBox");
	}
}

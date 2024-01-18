using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    public GameObject playerStatPanel;
    private PlayerStatPanel playerStatPanelCS;

    [SerializeField]
    public GameObject gamePanel;
    private GamePanel gamePanelCS;

    private static CanvasController instance;
    // singleton
    public static CanvasController Instance
    {
        get
        {
            // 인스턴스가 없으면 생성
            if (instance == null)
            {
                instance = FindObjectOfType<CanvasController>();

                // 만약 Scene에 GameManager가 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject("CanvasController");
                    instance = obj.AddComponent<CanvasController>();
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerStatPanelCS = playerStatPanel.GetComponent<PlayerStatPanel>();
        gamePanelCS = gamePanel.GetComponent<GamePanel>();
    }

    public void PlayerGetDamgeHP(float damge){
        playerStatPanelCS.TakeDamage(damge);
    }

    public void PlayerRestoreHP(float healAmount){
        playerStatPanelCS.RestoreHealth(healAmount);
    }

    public void PlayerUpEnergy(float Amount){
        playerStatPanelCS.UpEnergy(Amount);
    }

    public void PlayerDownEnergy(float Amount){
        playerStatPanelCS.DownEnergy(Amount);
    }

    public void ChangeCoinText(float coin){
        playerStatPanelCS.ChangeCoinText(coin);
    }

    public void ChangeScoreText(float score){
        playerStatPanelCS.ChangeScoreText(score);
    }

    public void ChangeState(float stage){
        gamePanelCS.ChangeStageText(stage);
    }

}

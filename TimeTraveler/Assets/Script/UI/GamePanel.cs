using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    public TextMeshProUGUI stageText;
    [Header("Button Settings")]
    [SerializeField]    
    public Button gameButton;
    public TextMeshProUGUI gameButtonText;
    public Image imageToChange;
    public Sprite oriImage;
    public Sprite changeImage;
    private bool isGameButtonClick = false;

    // Start is called before the first frame update
    void Start()
    {
        gameButton.onClick.AddListener(ChangeImage);
        ChangeStageText(1);
    }

    public void ChangeStageText(float stage){
        stageText.text = "Stage " + stage.ToString();
    }

    private void ChangeImage()
    {
        AudioManager.Instance.PlayGameButtonClick();
        if(isGameButtonClick){
            // 이미지 변경
            imageToChange.sprite = oriImage;
            isGameButtonClick = false;
            gameButtonText.text = "off";
        }else{
            // 이미지 변경
            imageToChange.sprite = changeImage;
            isGameButtonClick = true;
            gameButtonText.text = "on";
        }
    }

}

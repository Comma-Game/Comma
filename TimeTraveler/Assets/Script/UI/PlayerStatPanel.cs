using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerStatPanel : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;

    [Header("Health Settings")]
    private float health;
    private float healthLerpTimer;
    [SerializeField]
    public float maxHealth = 100;
    public float healthSpeed = 2;
    [SerializeField] private Image backgroundImg;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    private RectTransform backgroundImgRect;
    private RectTransform frontHealthBarRect;
    private RectTransform backHealthBarRect;
    public bool checkHealthDamage = false;
    public bool checkHealthRestore = false;

    [Header("Energy Settings")]
    private float energy;
    private float energyLerpTimer;
    public float maxEnergy = 100;
    public float energySpeed = 2;
    [SerializeField] private Image backgroundEnergyImg;
    [SerializeField] private Image frontEnergyBar;
    [SerializeField] private Image backEnergyBar;
    private RectTransform backgroundEnergyImgRect; 
    private RectTransform frontEnergyBarRect;
    private RectTransform backEnergyBarRect;
    public bool checkEnergyDown = false;
    public bool checkEnergyUp = false;

    [Header("buff Settings")]
    public GameObject[] buffs;

    [Header("another Settings")]
    [SerializeField]    
    public TextMeshProUGUI scoreText;
    public GameObject scoreUpObj;
    public TextMeshProUGUI scoreUpText;

    [Header("collect")]
    [SerializeField] public Image[] collectImgs;
    [SerializeField] public Sprite originSprite;
    [SerializeField] public Sprite changeSprite;

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (checkHealthDamage)
        {
            TakeDamage(Random.Range(5, 10));
            checkHealthDamage = false;
        }

        if (checkHealthRestore)
        {
            RestoreHealth(Random.Range(5, 10));
            checkHealthRestore = false;
        }

        energy = Mathf.Clamp(energy, 0, maxEnergy);
        UpdateEnergyUI();

        if (checkEnergyDown)
        {
            DownEnergy(Random.Range(5, 10));
            checkEnergyDown = false;
        }

        if (checkEnergyUp)
        {
            UpEnergy(Random.Range(5, 10));
            checkEnergyUp = false;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    /// HP 관련

    private void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            healthLerpTimer += Time.deltaTime;
            float percentComplete = healthLerpTimer / healthSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            healthLerpTimer += Time.deltaTime;
            float percentComplete = healthLerpTimer / healthSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void RestoreHealth(float healAmount)
    {
        //Debug.Log("RestoreHealth");
        health += healAmount;
        healthLerpTimer = 0;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        //Debug.Log("TakeDamage health : " + health);
        healthLerpTimer = 0;
    }

    /////////////////////////////////////////////////////////////////////////////////
    /// 에너지 관련

    private void UpdateEnergyUI()
    {
        float fillF = frontEnergyBar.fillAmount;
        float fillB = backEnergyBar.fillAmount;
        float eFraction = energy / maxEnergy;

        if (fillB > eFraction)
        {
            frontEnergyBar.fillAmount = eFraction;
            energyLerpTimer += Time.deltaTime;
            float percentComplete = energyLerpTimer / energySpeed;
            percentComplete = percentComplete * percentComplete;
            backEnergyBar.fillAmount = Mathf.Lerp(fillB, eFraction, percentComplete);
        }

        if (fillF < eFraction)
        {
            backEnergyBar.fillAmount = eFraction;
            energyLerpTimer += Time.deltaTime;
            float percentComplete = energyLerpTimer / energySpeed;
            percentComplete = percentComplete * percentComplete;
            frontEnergyBar.fillAmount = Mathf.Lerp(fillF, backEnergyBar.fillAmount, percentComplete);
        }
    }

    public void UpEnergy(float Amount)
    {
        //Debug.Log("UpEnergy");
        //Debug.Log("UpEnergy : " + Amount);
        energy += Amount;
        energyLerpTimer = 0;
    }

    public void DownEnergy(float damage)
    {
        //Debug.Log("DownEnergy");
        //Debug.Log("DownEnergy : " + damage);
        energy -= damage;
        energyLerpTimer = 0;
    }

    public void initSettingHealth(float maxHp){
        maxHealth = maxHp;
        //Debug.Log("maxHealth : " + maxHealth);
        health = maxHealth;
        ChangeScoreText(0);
        backgroundImgRect = backgroundImg.GetComponent<RectTransform>();
        frontHealthBarRect = frontHealthBar.GetComponent<RectTransform>();
        backHealthBarRect = backHealthBar.GetComponent<RectTransform>();
        if(maxHealth > 100){
            float diff = maxHealth - 100;
            if(backgroundImgRect != null){
                backgroundImgRect.sizeDelta = new Vector2(backgroundImgRect.sizeDelta.x + diff, backgroundImgRect.sizeDelta.y);
                frontHealthBarRect.sizeDelta = new Vector2(frontHealthBarRect.sizeDelta.x + diff, frontHealthBarRect.sizeDelta.y);
                backHealthBarRect.sizeDelta = new Vector2(backHealthBarRect.sizeDelta.x + diff, backHealthBarRect.sizeDelta.y);
            }
        }
    }

    public void initSettingEnergy(float maxenergy){
        energy = 0;
        maxEnergy = maxenergy;
        backgroundEnergyImgRect = backgroundEnergyImg.GetComponent<RectTransform>();
        frontEnergyBarRect = frontEnergyBar.GetComponent<RectTransform>();
        backEnergyBarRect = backEnergyBar.GetComponent<RectTransform>();
        if(maxEnergy != 100){
            float diff = (maxEnergy - 100)*(backgroundEnergyImgRect.sizeDelta.x/100);
            if(backgroundImgRect != null){
                backgroundEnergyImgRect.sizeDelta = new Vector2(backgroundEnergyImgRect.sizeDelta.x + diff, backgroundEnergyImgRect.sizeDelta.y);
                frontEnergyBarRect.sizeDelta = new Vector2(frontEnergyBarRect.sizeDelta.x + diff, frontEnergyBarRect.sizeDelta.y);
                backEnergyBarRect.sizeDelta = new Vector2(backEnergyBarRect.sizeDelta.x + diff, backEnergyBarRect.sizeDelta.y);
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    ////// buff
    
    public void ChangeBuffImage(int num){
        //Debug.Log("ChangeBuffImage : " + num);
        if(num != 0){
            buffs[num-1].SetActive(true);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    ////// 그 밖에

    public void ChangeScoreText(float score){
        scoreText.text = "현재 점수 : " + score.ToString();
    }

    public void ChangeScoreUpText(float scoreUp){
        scoreUpText.text = "X" + scoreUp.ToString();
    }

    public void OnScoreUpImg(bool isActive){
        scoreUpObj.SetActive(isActive);
    }

    /////////////////////////////////////////////////////////////////////////////////
    ////// collect
    
    public void isChangeCollectImg(int num, bool isChange){
        if(num > 4) return;
        if(isChange){
            collectImgs[num].sprite = changeSprite;
        }else{
            collectImgs[num].sprite = originSprite;
        }
    }

}

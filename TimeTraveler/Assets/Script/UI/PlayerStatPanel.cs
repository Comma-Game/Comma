using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;

    [Header("Health Settings")]
    private float health;
    private float healthLerpTimer;
    [SerializeField]
    public float maxHealth = 100;
    public float healthSpeed = 2;
    public Image frontHealthBar;
    public Image backHealthBar;
    public bool checkHealthDamage = false;
    public bool checkHealthRestore = false;

    [Header("Energy Settings")]
    private float energy;
    private float energyLerpTimer;
    public float maxEnergy = 100;
    public float energySpeed = 2;
    public Image frontEnergyBar;
    public Image backEnergyBar;
    public bool checkEnergyDown = false;
    public bool checkEnergyUp = false;

    [Header("another Settings")]
    [SerializeField]    
    public TextMeshProUGUI scoreText;
    [SerializeField]
    public TextMeshProUGUI coinText;

    void Start()
    {
        health = maxHealth;
        energy = maxEnergy;
        ChangeCoinText(100);
        ChangeScoreText(100);
    }

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
        Debug.Log("RestoreHealth");
        health += healAmount;
        healthLerpTimer = 0;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage");
        health -= damage;
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
        Debug.Log("UpEnergy");
        energy += Amount;
        energyLerpTimer = 0;
    }

    public void DownEnergy(float damage)
    {
        Debug.Log("DownEnergy");
        energy -= damage;
        energyLerpTimer = 0;
    }

    /////////////////////////////////////////////////////////////////////////////////
    ////// 그 밖에
    
    public void ChangeCoinText(float coin){
        coinText.text = "Coin : " + coin.ToString();
    }

    public void ChangeScoreText(float score){
        scoreText.text = "Score : " + score.ToString();
    }
}

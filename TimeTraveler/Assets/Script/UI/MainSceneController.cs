using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 필요
using TMPro;

public class MainSceneController : MonoBehaviour
{
        [Header("main Object")]
        [SerializeField] public GameObject blackHoleObj1;
        [SerializeField] public GameObject blackHoleObj2;
        [SerializeField] public GameObject background;
        [SerializeField]
        public GameObject mainPanel_Button;
        [SerializeField]
        public GameObject mainPanel_playerStat;
        [SerializeField]
        public float rotationSpeed = 50f;

        [Header("heart Panel")]
        [SerializeField] public GameObject heartPanel;
        [SerializeField] public HeartPanel heartPanelCS;

        [Header("player panel 1")]
        [SerializeField] public TextMeshProUGUI coinText_1;
        [SerializeField] public TextMeshProUGUI scoreText_1;
        [SerializeField] public TextMeshProUGUI buffText_1;

        [Header("player panel 2")]
        [SerializeField] public TextMeshProUGUI coinText_2;
        [SerializeField] public TextMeshProUGUI scoreText_2;
        [SerializeField] public TextMeshProUGUI buffText_2;

        [Header("Shop Panels")]
        [SerializeField] public TextMeshProUGUI shopHpText;
        [SerializeField] public TextMeshProUGUI shopStatText;
        [SerializeField] public TextMeshProUGUI shopEnergyText;
        [SerializeField] public TextMeshProUGUI shopHpLVText;
        [SerializeField] public TextMeshProUGUI shopStatLVText;
        [SerializeField] public TextMeshProUGUI shopEnergyLVText;
        [SerializeField] public TextMeshProUGUI mysteryBoxBuffText;

        private int hpUpgradeClass = 1;
        private int statUpgradeClass = 1;
        private int energyUpgradeClass = 1;

        private string[] mysteryBox_Buff_Texts = {
                "HP +50%",
                "Coin X1.5",
                "Score X1.5",
                "Energy Charger X2"
        };

        private int maxGradeNum = 4; // 5 업그레이드 가능하면 -1 해서 4로 설정
        private int[] hpUpgradeCoin = { 100, 200, 300, 400, 500 };
        private int[] hpUpgradeHp = { 100, 200, 300, 400, 500 };
        private int[] statUpgradeCoin = { 100, 200, 300, 400, 500 };
        private int[] statUpgradeCrack = { 100, 200, 300, 400, 500 };
         private int[] statUpgradeMemories = { 100, 200, 300, 400, 500 };
        private int[] energyUpgradeCoin = { 100, 200, 300, 400, 500 };
        private int[] energyUpgradeEnergy = { 100, 200, 300, 400, 500 };

        void Update()
        {
                // 회전 속도에 따라 물체를 자기 중심으로 회전
                background.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        /// /////////////////////////////////////////////////////////////////
        /// game object

        public void GamePlay(){
                Debug.Log("GamePlay");
                // 화면 UI 꺼주기
                mainPanel_Button.SetActive(false);
                mainPanel_playerStat.SetActive(false);
                if(heartPanelCS == null){
                        heartPanelCS = heartPanel.GetComponent<HeartPanel>();
                }
                heartPanelCS.MinusHearts(1);
                heartPanel.SetActive(false);
                // 블랙홀 켜주기
                blackHoleObj1.SetActive(true);
                blackHoleObj2.SetActive(true);
                Invoke("MoveScene", 5f);
        }

        private void MoveScene()
        {
                SceneManager.LoadScene("GameScene");
        }

        public void StatShop(){
                Debug.Log("StatShop");
        }

        /// /////////////////////////////////////////////////////////////////
        /// player panel

        public void ChangeCoinText(int coin){
                coinText_1.text = "Coin : " + coin.ToString();
                coinText_2.text = "Coin : " + coin.ToString();
        }

        public void ChangeScoreText(int score){
                scoreText_1.text = "Score : " + score.ToString();
                scoreText_2.text = "Score : " + score.ToString();
        }

        public void ChangeBuffText(string buff){
                buffText_1.text = "Get Buff : " + buff;
                buffText_2.text = "Get Buff : " + buff;
        }

        /// /////////////////////////////////////////////////////////////////
        /// store panel
        
        public void SettingClass(int hpClass, int statClass, int energyClass){
                hpUpgradeClass = hpClass;
                if(hpUpgradeClass < maxGradeNum) ChangeShopHpText(true);
                else ChangeShopHpText(false);

                statUpgradeClass = statClass;
                if(statUpgradeClass < maxGradeNum) ChangeStatText(true);
                else ChangeStatText(false);
                
                energyUpgradeClass = energyClass;
                if(statUpgradeClass < maxGradeNum) ChangeEnergyText(true);
                else ChangeEnergyText(false);
        }

        public void UpgradeStatHP(){
                Debug.Log("UpgradeStatHP "+hpUpgradeClass);
                // 스탯 올릴 수 있다면
                if(hpUpgradeClass < maxGradeNum) {
                        // 실제 스탯에 적용 코드
                        ChangeShopHpText(true);
                        hpUpgradeClass += 1; // 다음 단계 업그레이드
                }
                else{ // 없다면
                        ChangeShopHpText(false);
                }
	}

        public void UpgradeStat(){
                Debug.Log("UpgradeStat "+statUpgradeClass);
                // 스탯 올릴 수 있다면
                if(statUpgradeClass < maxGradeNum) {
                        // 실제 스탯에 적용 코드
                        ChangeStatText(true);
                        statUpgradeClass += 1; // 다음 단계 업그레이드
                }
                else{ // 없다면
                        ChangeStatText(false);
                }
                
	}

        public void UpgradeStatEnergy(){
                Debug.Log("UpgradeStatEnergy "+energyUpgradeClass);
                // 스탯 올릴 수 있다면
                if(energyUpgradeClass < maxGradeNum){
                        // 실제 스탯에 적용 코드
                        ChangeEnergyText(true);
                        energyUpgradeClass += 1; // 다음 단계 업그레이드
                }
                else{ // 없다면
                        ChangeEnergyText(false);
                }
	}

	public void GetMysteryBox(){
                string randomBuffText = GetRandomBuffText();
                mysteryBoxBuffText.text = randomBuffText;
                ChangeBuffText(randomBuffText);
                Debug.Log("GetMysteryBox");

                // buff 게임 내 적용시키는 코드 필요
	}

        private string GetRandomBuffText(){
                int randomIndex = Random.Range(0, mysteryBox_Buff_Texts.Length);
                return mysteryBox_Buff_Texts[randomIndex];
        }

        private void ChangeShopHpText(bool canUpgrade){
                if(canUpgrade) shopHpText.text = "Upgrade \n" + hpUpgradeCoin[hpUpgradeClass];
                else shopHpText.text = "Max";
                shopHpLVText.text = "LV " + (hpUpgradeClass+1).ToString();
        }

        private void ChangeStatText(bool canUpgrade){
                if(canUpgrade) shopStatText.text = "Upgrade \n" + statUpgradeCoin[statUpgradeClass];
                else shopStatText.text = "Max";
                shopStatLVText.text = "LV " + (statUpgradeClass+1).ToString();
        }

        private void ChangeEnergyText(bool canUpgrade){
                if(canUpgrade) shopEnergyText.text = "Upgrade \n" + energyUpgradeCoin[energyUpgradeClass];
                else shopEnergyText.text = "Max";
                shopEnergyLVText.text = "LV " + (energyUpgradeClass+1).ToString();
        }

        /// /////////////////////////////////////////////////////////////////

        // public void GameSetting(){
        // Debug.Log("GameSetting");
        // }
}

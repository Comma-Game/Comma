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

        private int hpUpgradeClass = 0;
        private int statUpgradeClass = 0;
        private int energyUpgradeClass = 0;

        private string[] mysteryBox_Buff_Texts = {
                "체력강화 + 50",
                "스코어 2배",
                "장애물 충돌피해 50% 감소",
                "체력감소 15% 느림",
                "물약 회복량 20% 증가",
                "가속도 +20%",
                "기본속도 +20%",
                "에너지 축적량 감소 -20%"
        };

        private int maxGradeNum = 30; // 5 업그레이드 가능하면 -1 해서 4로 설정
        private int[] hpUpgradeCoin = { 450, 490, 530, 580, 630, 690, 750, 820, 900, 990,
                                        1215, 1515, 1890, 2340, 2915, 3640, 4540, 5665, 7065, 8815,
                                        11455, 14875, 19315, 25105, 32635, 42415, 55135, 71665, 93145, 121075 };
        private int[] hpUpgradeHp = { 115, 130, 145, 160, 175, 190, 205, 220, 235, 250,
                                        260, 270, 280, 290, 300, 310, 320, 330, 340, 350,
                                        355, 360, 365, 370, 375, 380, 385, 390, 395, 400 };
        private int[] statUpgradeCoin = { 500, 550, 600, 660, 720, 790, 860, 940, 1030, 1130,
                                        1405, 1755, 2180, 2705, 3380, 4205, 5255, 6555, 8180, 10205,
                                        13265, 17225, 22385, 29075, 37775, 49085, 63785, 82895, 107735, 140045 };
        private int[] statUpgradeCrack = { 80, 90, 100, 110, 120, 130, 140, 150, 160, 170,
                                                220, 270, 320, 370, 420, 470, 520, 570, 620, 670,
                                                770, 870, 970, 1070, 1170, 1270, 1370, 1470, 1570, 1670 };
         private int[] statUpgradeMemories = { 160, 180, 200, 220, 240, 260, 280, 300, 320, 340,
                                                440, 540, 640, 740, 840, 940, 1040, 1140, 1240, 1340,
                                                1540, 1740, 1940, 2140, 2340, 2540, 2740, 2940, 3140, 3340 };
        private int[] energyUpgradeCoin = { 550, 600, 660, 720, 790, 860, 940, 1030, 1130, 1240,
                                                1540, 1915, 2390, 2965, 3690, 4590, 5715, 7140, 8915, 11140,
                                                14470, 18790, 24400, 31720, 41230, 53590, 69640, 90520, 117670, 152950 };
        private float[] energyUpgradeEnergy = { 2.06f, 2.12f, 2.18f, 2.24f, 2.3f, 2.36f, 2.42f, 2.48f, 2.54f, 2.6f,
                                                2.64f, 2.68f, 2.72f, 2.76f, 2.8f, 2.84f, 2.88f, 2.92f, 2.96f, 3f,
                                                3.02f, 3.04f, 3.06f, 3.08f, 3.1f, 3.12f, 3.14f, 3.16f, 3.18f, 3.2f };

        void Update()
        {
                // 회전 속도에 따라 물체를 자기 중심으로 회전
                background.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        /// /////////////////////////////////////////////////////////////////
        /// game object
        
        public void Start(){
                // hpUpgradeClass = 30;
                // statUpgradeClass = 29;
                // energyUpgradeClass = 4;
                SettingClass(hpUpgradeClass, statUpgradeClass, energyUpgradeClass);
        }

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
                // 스탯 올릴 수 있다면
                if((hpUpgradeClass+1) < maxGradeNum) {
                        hpUpgradeClass += 1; // 다음 단계 업그레이드
                        // 실제 스탯에 적용 코드
                        ChangeShopHpText(true);
                }
                else if((hpUpgradeClass+1) == maxGradeNum){ // 없다면
                        hpUpgradeClass += 1; // 다음 단계 업그레이드
                        ChangeShopHpText(false);
                }
	}

        public void UpgradeStat(){
                // 스탯 올릴 수 있다면
                if((statUpgradeClass+1) < maxGradeNum) {
                        statUpgradeClass += 1; // 다음 단계 업그레이드
                        // 실제 스탯에 적용 코드
                        ChangeStatText(true);
                }
                else if((statUpgradeClass+1) == maxGradeNum) { // 없다면
                        statUpgradeClass += 1; // 다음 단계 업그레이드
                        ChangeStatText(false);
                }
                
	}

        public void UpgradeStatEnergy(){
                // 스탯 올릴 수 있다면
                if((energyUpgradeClass+1) < maxGradeNum){
                        energyUpgradeClass += 1; // 다음 단계 업그레이드
                        // 실제 스탯에 적용 코드
                        ChangeEnergyText(true);
                }
                else if((energyUpgradeClass+1) == maxGradeNum){ // 없다면
                        energyUpgradeClass += 1; // 다음 단계 업그레이드
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
                Debug.Log("UpgradeStatHP "+hpUpgradeClass);
                if(canUpgrade) shopHpText.text = "Upgrade \n" + hpUpgradeCoin[hpUpgradeClass];
                else shopHpText.text = "Max";
                shopHpLVText.text = "LV " + (hpUpgradeClass).ToString();
        }

        private void ChangeStatText(bool canUpgrade){
                Debug.Log("UpgradeStat "+statUpgradeClass);
                if(canUpgrade) shopStatText.text = "Upgrade \n" + statUpgradeCoin[statUpgradeClass];
                else shopStatText.text = "Max";
                shopStatLVText.text = "LV " + (statUpgradeClass).ToString();
        }

        private void ChangeEnergyText(bool canUpgrade){
                Debug.Log("UpgradeStatEnergy "+energyUpgradeClass);
                if(canUpgrade) shopEnergyText.text = "Upgrade \n" + energyUpgradeCoin[energyUpgradeClass];
                else shopEnergyText.text = "Max";
                shopEnergyLVText.text = "LV " + (energyUpgradeClass).ToString();
        }

        /// /////////////////////////////////////////////////////////////////

        // public void GameSetting(){
        // Debug.Log("GameSetting");
        // }
}

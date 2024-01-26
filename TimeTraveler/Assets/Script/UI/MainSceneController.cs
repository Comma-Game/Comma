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
        [SerializeField] public GameObject mainPanel_Button;
        [SerializeField] public GameObject mainPanel_playerStat;
        [SerializeField] public float rotationSpeed = 50f;
        [SerializeField] public GameObject shopPanel;
        [SerializeField] public GameObject AchievementPanel;

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
        private int jellyUpgradeClass = 0;
        private int energyUpgradeClass = 0;

        private int mysteryBox_coin = 1000;
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
        private int[] jellyUpgradeCoin = { 500, 550, 600, 660, 720, 790, 860, 940, 1030, 1130,
                                        1405, 1755, 2180, 2705, 3380, 4205, 5255, 6555, 8180, 10205,
                                        13265, 17225, 22385, 29075, 37775, 49085, 63785, 82895, 107735, 140045 };
        private int[] energyUpgradeCoin = { 550, 600, 660, 720, 790, 860, 940, 1030, 1130, 1240,
                                                1540, 1915, 2390, 2965, 3690, 4590, 5715, 7140, 8915, 11140,
                                                14470, 18790, 24400, 31720, 41230, 53590, 69640, 90520, 117670, 152950 };

        void Update()
        {
                // 회전 속도에 따라 물체를 자기 중심으로 회전
                background.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        /// /////////////////////////////////////////////////////////////////
        /// game object
        
        public void Start(){
                hpUpgradeClass = (int)SaveLoadManager.Instance.GetUpgradeHP();
                jellyUpgradeClass = SaveLoadManager.Instance.GetUpgradeJelly();
                energyUpgradeClass = (int)SaveLoadManager.Instance.GetUpgradeEnergy();
                SetMysteryBox(SaveLoadManager.Instance.GetBuff());
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
                ChangeScoreText(SaveLoadManager.Instance.GetHighScore());
                SettingClass(hpUpgradeClass, jellyUpgradeClass, energyUpgradeClass);

                if(SaveLoadManager.Instance.GetCoin() == 0) SaveLoadManager.Instance.PlusCoin(10000000);
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
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

        public void ClickShop(){
                Debug.Log("ClickShop");
                shopPanel.SetActive(true);
                AchievementPanel.SetActive(false);
        }

        public void ClickAchievement(){
                Debug.Log("ClickAchievement");
                shopPanel.SetActive(false);
                AchievementPanel.SetActive(true);
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

                jellyUpgradeClass = statClass;
                if(jellyUpgradeClass < maxGradeNum) ChangeJellyText(true);
                else ChangeJellyText(false);
                
                energyUpgradeClass = energyClass;
                if(energyUpgradeClass < maxGradeNum) ChangeEnergyText(true);
                else ChangeEnergyText(false);
        }

        public void UpgradeStatHP(){
                // 소지금 확인
                if(hpUpgradeClass < maxGradeNum &&
                SaveLoadManager.Instance.GetCoin() >= hpUpgradeCoin[hpUpgradeClass])
                {
                        // 스탯 올릴 수 있다면
                        if((hpUpgradeClass+1) < maxGradeNum) {
                                // 돈 소비
                                MinusCoin(hpUpgradeCoin[hpUpgradeClass]);
                                hpUpgradeClass += 1; // 다음 단계 업그레이드
                                // 실제 스탯에 적용 코드
                                SaveLoadManager.Instance.UpgradeHP();
                                ChangeShopHpText(true);
                        }
                        else if((hpUpgradeClass+1) == maxGradeNum){ // 없다면
                                // 돈 소비
                                MinusCoin(hpUpgradeCoin[hpUpgradeClass]);
                                hpUpgradeClass += 1; // 마지막 업그레이드
                                // 실제 스탯에 적용 코드
                                SaveLoadManager.Instance.UpgradeHP();
                                ChangeShopHpText(false);
                        }
                }
	}

        public void UpgradeJelly(){
                // 소지금 확인
                if(jellyUpgradeClass < maxGradeNum &&
                SaveLoadManager.Instance.GetCoin() >= jellyUpgradeCoin[jellyUpgradeClass]){
                        // 스탯 올릴 수 있다면
                        if((jellyUpgradeClass+1) < maxGradeNum) {
                                // 돈 소비
                                MinusCoin(jellyUpgradeCoin[jellyUpgradeClass]);
                                jellyUpgradeClass += 1; // 다음 단계 업그레이드
                                // 실제 스탯에 적용 코드
                                SaveLoadManager.Instance.UpgradeJelly();
                                ChangeJellyText(true);
                        }
                        else if((jellyUpgradeClass+1) == maxGradeNum) { 
                                // 돈 소비
                                MinusCoin(jellyUpgradeCoin[jellyUpgradeClass]);
                                jellyUpgradeClass += 1; // 마지막 업그레이드
                                // 실제 스탯에 적용 코드
                                SaveLoadManager.Instance.UpgradeJelly();
                                ChangeJellyText(false);
                        }
                }                
	}

        public void UpgradeStatEnergy(){
                // 소지금 확인
                if(energyUpgradeClass < maxGradeNum &&
                SaveLoadManager.Instance.GetCoin() >= energyUpgradeCoin[energyUpgradeClass]){
                        // 스탯 올릴 수 있다면
                        if((energyUpgradeClass+1) < maxGradeNum){
                                // 돈 소비
                                MinusCoin(energyUpgradeCoin[energyUpgradeClass]);
                                energyUpgradeClass += 1; // 다음 단계 업그레이드
                                // 실제 스탯에 적용 코드
                                SaveLoadManager.Instance.UpgradeEnergy();
                                ChangeEnergyText(true);
                        }
                        else if((energyUpgradeClass+1) == maxGradeNum){ 
                                // 돈 소비
                                MinusCoin(energyUpgradeCoin[energyUpgradeClass]);
                                energyUpgradeClass += 1; // 마지막 업그레이드
                                // 실제 스탯에 적용 코드
                                SaveLoadManager.Instance.UpgradeEnergy();
                                ChangeEnergyText(false);
                        }
                }
	}

	public void GetMysteryBox(){
                string randomBuffText = GetRandomBuffText();
                mysteryBoxBuffText.text = randomBuffText;
                ChangeBuffText(randomBuffText);
                Debug.Log("GetMysteryBox");
                // 돈 소비
                MinusCoin(mysteryBox_coin);
	}

        public void SetMysteryBox(int Index){
                Debug.Log("SetMysteryBox");
                string randomBuffText = "0";
                if(Index == 0){
                        randomBuffText = "-";
                }else{
                        randomBuffText = mysteryBox_Buff_Texts[Index-1];
                }
                mysteryBoxBuffText.text = randomBuffText;
                ChangeBuffText(randomBuffText);
                Debug.Log("SetMysteryBox");
	}

        private string GetRandomBuffText(){
                int randomIndex = Random.Range(0, mysteryBox_Buff_Texts.Length);
                SaveLoadManager.Instance.SetBuff(randomIndex+1);
                return mysteryBox_Buff_Texts[randomIndex];
        }

        private void ChangeShopHpText(bool canUpgrade){
                Debug.Log("UpgradeStatHP "+hpUpgradeClass);
                if(canUpgrade) shopHpText.text = "Upgrade \n" + hpUpgradeCoin[hpUpgradeClass];
                else shopHpText.text = "Max";
                shopHpLVText.text = "LV " + (hpUpgradeClass).ToString();
        }

        private void ChangeJellyText(bool canUpgrade){
                Debug.Log("UpgradeJelly "+jellyUpgradeClass);
                if(canUpgrade) shopStatText.text = "Upgrade \n" + jellyUpgradeCoin[jellyUpgradeClass];
                else shopStatText.text = "Max";
                shopStatLVText.text = "LV " + (jellyUpgradeClass).ToString();
        }

        private void ChangeEnergyText(bool canUpgrade){
                Debug.Log("UpgradeStatEnergy "+energyUpgradeClass);
                if(canUpgrade) shopEnergyText.text = "Upgrade \n" + energyUpgradeCoin[energyUpgradeClass];
                else shopEnergyText.text = "Max";
                shopEnergyLVText.text = "LV " + (energyUpgradeClass).ToString();
        }

        private void MinusCoin(int coin){
                // 돈 소비
                SaveLoadManager.Instance.MinusCoin(coin);
                // 바뀐 금액 표시
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
        }

        /// /////////////////////////////////////////////////////////////////

        // public void GameSetting(){
        // Debug.Log("GameSetting");
        // }
}

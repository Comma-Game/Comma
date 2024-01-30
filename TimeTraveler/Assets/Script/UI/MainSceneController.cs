using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 필요
using TMPro;
using SlimUI.ModernMenu;

public class MainSceneController : MonoBehaviour
{
        [Header("main Object")]
        [SerializeField] public GameObject mainPanel_Button;
        [SerializeField] public GameObject mainPanel_playerStat;
        private float rotationSpeed = 50f;
        [SerializeField] public GameObject shopPanel;
        [SerializeField] public GameObject shop_HaertShopPanel;
        [SerializeField] public GameObject AchievementPanel;
        private AchievementManager AchievementPanelCS;
        [SerializeField] public GameObject AchievementNewText;
        [SerializeField] public GameObject AchievementNewImg;
        [SerializeField] public GameObject SettingPanel;
        [SerializeField] public GameObject CashShopPanel;
        [SerializeField] public GameObject Canv_CashShop;
        [SerializeField] public UIMenuManager uIMenuManager;

        [Header("background Obj")]
        [SerializeField] public Animator Charater;
        [SerializeField] public GameObject blackHoleObj1;
        [SerializeField] public GameObject blackHoleObj2;
        [SerializeField] public GameObject background;

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
        private bool isGameStart = false;

        private int mysteryBox_coin = 10000;
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
        private int[] hpUpgradeCoin = {1200, 1320, 1450, 1590, 1740, 1910, 2100, 2310, 2540, 2790,
                                        3465, 4315, 5390, 6715, 8390, 10465, 13065, 16315, 20390, 25465,
                                        33085, 42985, 55855, 72595, 94345, 122635, 159415, 207235, 269395, 350185};
        private int[] jellyUpgradeCoin = {1500, 1650, 1810, 1990, 2180, 2390, 2620, 2880, 3160, 3470,
                                                4320, 5395, 6720, 8395, 10470, 13070, 16320, 20395, 25470, 31820,
                                                41360, 53750, 69860, 90800, 118040, 153440, 199460, 259280, 337040, 438140};
        private int[] energyUpgradeCoin = {1000, 1100, 1210, 1330, 1460, 1600, 1760, 1930, 2120, 2330,
                                                2905, 3630, 4530, 5655, 7055, 8805, 11005, 13755, 17180, 21455,
                                                27875, 36215, 47075, 61175, 79505, 103355, 134345, 174635, 227015, 295115};

        void Update()
        {
                // 회전 속도에 따라 물체를 자기 중심으로 회전
                background.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        /// /////////////////////////////////////////////////////////////////
        /// game object
        
        public void Start(){
                AchievementPanelCS = AchievementPanel.GetComponent<AchievementManager>();
                hpUpgradeClass = (int)SaveLoadManager.Instance.GetUpgradeHP();
                jellyUpgradeClass = SaveLoadManager.Instance.GetUpgradeJelly();
                energyUpgradeClass = (int)SaveLoadManager.Instance.GetUpgradeEnergy();
                SetMysteryBox(SaveLoadManager.Instance.GetBuff());
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
                ChangeScoreText(SaveLoadManager.Instance.GetHighScore());
                AchievementPanelCS.ChangeHighScoreText(SaveLoadManager.Instance.GetHighScore());
                SettingClass(hpUpgradeClass, jellyUpgradeClass, energyUpgradeClass);

                if(SaveLoadManager.Instance.GetCoin() == 0) SaveLoadManager.Instance.PlusCoin(10000000);
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());

                
        }

        public void GamePlay(){
                Debug.Log("GamePlay");
                if(heartPanelCS.GetCurrentHearts() >= 1){
                        if(isGameStart == false){
                                isGameStart = true;
                                Charater.SetBool("Run", true);
                                // 화면 돌아가기
                                uIMenuManager.Position1();
                                // 화면 UI 꺼주기
                                mainPanel_Button.SetActive(false);
                                mainPanel_playerStat.SetActive(false);
                                SettingPanel.SetActive(false);
                                CashShopPanel.SetActive(false);
                                // 하트 값 소모 1
                                if(heartPanelCS == null){
                                        heartPanelCS = heartPanel.GetComponent<HeartPanel>();
                                }
                                heartPanelCS.MinusHearts(1);
                                heartPanel.SetActive(false);
                                // 블랙홀 켜주기
                                blackHoleObj1.SetActive(true);
                                blackHoleObj2.SetActive(true);
                                AudioManager.Instance.PlayMainScenePortal();
                                Invoke("MoveScene", 5f);
                        }
                }else{
                        shop_HaertShopPanel.SetActive(true);
                }
                SaveLoadManager.Instance.SaveData();
        }

        private void MoveScene()
        {
                SceneManager.LoadScene("GameScene");
        }

        public void ClickShop(){
                AudioManager.Instance.PlayGameButtonClick();
                Debug.Log("ClickShop");
                shopPanel.SetActive(true);
                AchievementPanel.SetActive(false);
                uIMenuManager.Position2();
                uIMenuManager.ReturnMenu();
                Canv_CashShop.SetActive(false);
        }

        public void ClickAchievement(){
                AudioManager.Instance.PlayGameButtonClick();
                Debug.Log("ClickAchievement");
                shopPanel.SetActive(false);
                AchievementPanel.SetActive(true);
                Canv_CashShop.SetActive(false);
        }

        public void ClickCashShop(){
                AudioManager.Instance.PlayGameButtonClick();
                Debug.Log("ClickCashShop");
                shopPanel.SetActive(false);
                AchievementPanel.SetActive(false);
                Canv_CashShop.SetActive(true);
        }

        /// /////////////////////////////////////////////////////////////////
        /// player panel

        private void ChangeCoinText(int coin){
                coinText_1.text = "Coin : " + coin.ToString();
                coinText_2.text = "Coin : " + coin.ToString();
        }

        private void ChangeScoreText(int score){
                scoreText_1.text = "Score : " + score.ToString();
                scoreText_2.text = "Score : " + score.ToString();
        }

        private void ChangeBuffText(string buff){
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
                        AudioManager.Instance.PlayItemUpgradeSFX();
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
                }else{
                        AudioManager.Instance.PlayGameButtonClick();
                }
	}

        public void UpgradeJelly(){
                // 소지금 확인
                if(jellyUpgradeClass < maxGradeNum &&
                SaveLoadManager.Instance.GetCoin() >= jellyUpgradeCoin[jellyUpgradeClass]){
                        AudioManager.Instance.PlayItemUpgradeSFX();
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
                }else{
                        AudioManager.Instance.PlayGameButtonClick();
                }                
	}

        public void UpgradeStatEnergy(){
                // 소지금 확인
                if(energyUpgradeClass < maxGradeNum &&
                SaveLoadManager.Instance.GetCoin() >= energyUpgradeCoin[energyUpgradeClass]){
                        AudioManager.Instance.PlayItemUpgradeSFX();
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
                }else{
                        AudioManager.Instance.PlayGameButtonClick();
                }
	}

	public void GetMysteryBox(){
                AudioManager.Instance.PlayMesteryBoxBuySFX();
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

        public void MinusCoin(int coin){
                // 돈 소비
                SaveLoadManager.Instance.MinusCoin(coin);
                // 바뀐 금액 표시
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
        }

        public void PlusCoin(int coin){
                Debug.Log("PlusCoin "+ coin);
                // 돈 소비
                SaveLoadManager.Instance.PlusCoin(coin);
                // 바뀐 금액 표시
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
        }

        /// /////////////////////////////////////////////////////////////////

        // public void GameSetting(){
        // Debug.Log("GameSetting");
        // }
}

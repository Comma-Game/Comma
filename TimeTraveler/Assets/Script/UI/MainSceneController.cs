using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 필요
using TMPro;
using SlimUI.ModernMenu;

public class MainSceneController : MonoBehaviour
{
        [Header("main Object")]
        [SerializeField] public GameObject basicButton;
        [SerializeField] public GameObject portal;
        [SerializeField] public GameObject mainPanel_Button;
        [SerializeField] public GameObject mainPanel_playerStat;
        private float rotationSpeed = 50f;
        [SerializeField] public GameObject shopPanel;
        [SerializeField] public GameObject shop_HaertShopPanel;
        [SerializeField] public GameObject AchievementPanel;
        private AchievementManager AchievementPanelCS;
        [SerializeField] public GameObject SettingPanel;
        [SerializeField] public GameObject CashShopPanel;
        [SerializeField] public GameObject Canv_CashShop;
        [SerializeField] public UIMenuManager uIMenuManager;
        [SerializeField] public GameObject FirstStoryPanel;
        [SerializeField] public GameObject loadingScene;
        [SerializeField] public LoadingSceneManager loadingSceneManager;
        [SerializeField] public GameObject mapChoosePanel;
        [SerializeField] public GameObject starFX;
        [SerializeField] public GameObject tutoralAllPanel;
        [SerializeField] public GameObject tutoralPanel;
        [SerializeField] public GameObject cheatPanel;
        [SerializeField] public GameObject cheatCheckPanel;
        [SerializeField] public NotiMessagePanel notiMessagePanel;

        [Header("background Obj")]
        [SerializeField] public Animator Charater;

        [Header("heart Panel")]
        [SerializeField] public GameObject heartPanel;
        [SerializeField] public HeartPanel heartPanelCS;

        [Header("player panel 1")]
        [SerializeField] public TextMeshProUGUI coinText_1;
        [SerializeField] public TextMeshProUGUI scoreText_1;
        [SerializeField] public TextMeshProUGUI buffText_1;
        [SerializeField] public TextMeshProUGUI hpText_1;
        [SerializeField] public TextMeshProUGUI statText_1;
        [SerializeField] public TextMeshProUGUI energyText_1;
        [SerializeField] public TextMeshProUGUI buffdetailText;

        [Header("player panel 2")]
        [SerializeField] public TextMeshProUGUI coinText_2;
        [SerializeField] public TextMeshProUGUI scoreText_2;
        [SerializeField] public TextMeshProUGUI buffText_2;
        [SerializeField] public TextMeshProUGUI hpText_2;
        [SerializeField] public TextMeshProUGUI statText_2;
        [SerializeField] public TextMeshProUGUI energyText_2;
        [SerializeField] public TextMeshProUGUI buffdetailText2;

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
        private int mysteryBox_current_num = -1;

        private bool isGameStart = false;
        private bool isGameFirst = true;
        private bool isBuyAD = false;

        private int mysteryBox_coin = 10000;
        private string[] mysteryBox_Buff_Texts = {
                "체력 강화 +50",
                "코인 1.2배 추가 획득" ,
                "장애물 충돌피해 50% 감소",
                "체력감소 15% 느림",
                "체력 회복량 20% 증가",
                "가속도 +20%",
                "기본속도 +20%",
                "에너지 축적량 20% 감소"
        };

        private int maxGradeNum = 30; // 5 업그레이드 가능하면 -1 해서 4로 설정
        private int[] hpUpgradeCoin = {1200, 1416, 1668, 1956, 2298, 2694, 3162, 3720, 4386, 5160,
                                        6435, 8035, 10035, 12535, 15660, 19560, 24435, 30535, 38160, 47685,
                                        62917, 83045, 109605, 144677, 190949, 252037, 332677, 439109, 579621, 765093};
        private int[] jellyUpgradeCoin = {1500, 1770, 2076, 2436, 2868, 3372, 3966, 4668, 5496, 6468,
                                                8068, 10068, 12568, 15693, 19593, 24468, 30568, 38193, 47718, 59643,
                                                78715, 103899, 137115, 180987, 238875, 315291, 416155, 549307, 725083, 957083};

        private int[] energyUpgradeCoin = {1000, 1180, 1378, 1612, 1900, 2242, 2638, 3106, 3664, 4312,
                                                5387, 6712, 8387, 10462, 13062, 16312, 20387, 25462, 31812, 39762,
                                                52466, 69234, 91378, 120594, 159154, 210066, 277266, 365970, 483058, 637618};

        private int[] hpUpgradeValue = { 100, 115, 130, 145, 160, 175, 190, 205, 220, 235, 250,
                                        260, 270, 280, 290, 300, 310, 320, 330, 340, 350,
                                        355, 360, 365, 370, 375, 380, 385, 390, 395, 400 };


        private int[] stat1UpgradeValue = { 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170,
                                                220, 270, 320, 370, 420, 470, 520, 570, 620, 670,
                                                770, 870, 970, 1070, 1170, 1270, 1370, 1470, 1570, 1670 };

        private int[] stat2UpgradeValue = { 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340,
                                                440, 540, 640, 740, 840, 940, 1040, 1140, 1240, 1340,
                                                1540, 1740, 1940, 2140, 2340, 2540, 2740, 2940, 3140, 3340 };

        private float[] energyUpgradeValue = { 5f, 5.1f, 5.2f, 5.3f, 5.4f, 5.5f, 5.6f, 5.7f, 5.8f, 5.9f, 6.0f,
                                                6.1f, 6.2f, 6.3f, 6.4f, 6.5f, 6.6f, 6.7f, 6.8f, 6.9f, 7.0f,
                                                7.1f, 7.2f, 7.3f, 7.4f, 7.5f, 7.6f, 7.7f, 7.8f, 7.9f, 8.0f };


        /// /////////////////////////////////////////////////////////////////
        /// game object
        
        public void Start(){
                // 데이터 불려오기
                isGameFirst = SaveLoadManager.Instance.GetIsGameFirst();
                isBuyAD = SaveLoadManager.Instance.GetIsBuyAd();
                AchievementPanelCS = AchievementPanel.GetComponent<AchievementManager>();
                hpUpgradeClass = (int)SaveLoadManager.Instance.GetUpgradeHP();
                ChangeHpText(hpUpgradeValue[hpUpgradeClass]);
                jellyUpgradeClass = SaveLoadManager.Instance.GetUpgradeJelly();
                ChangeStatText(stat1UpgradeValue[jellyUpgradeClass], stat2UpgradeValue[jellyUpgradeClass]);
                energyUpgradeClass = (int)SaveLoadManager.Instance.GetUpgradeEnergy();
                ChangeEnergyText(energyUpgradeValue[energyUpgradeClass]);
                SetMysteryBox(SaveLoadManager.Instance.GetBuff());
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
                ChangeScoreText(SaveLoadManager.Instance.GetHighScore());
                AchievementPanelCS.ChangeHighScoreText(SaveLoadManager.Instance.GetHighScore());
                SettingClass(hpUpgradeClass, jellyUpgradeClass, energyUpgradeClass);

                //if(SaveLoadManager.Instance.GetCoin() == 0) SaveLoadManager.Instance.PlusCoin(10000000);
                //ChangeCoinText(SaveLoadManager.Instance.GetCoin());

                // 만약 게임이 처음이면 스토리 panel 띄우기
                if(isGameFirst == false){
                        Invoke("FirstPopUp", 1f);
                }
        }

        public void GamePlay(){
                //Debug.Log("GamePlay");
                if(heartPanelCS.GetCurrentHearts() >= 1){
                        if(isGameStart == false){
                                // 치트 사용했는지 확인하고 모든 컨셉 열어버리기
                                if(TestConceptButton.Instance.GetTestConcept().Count > 0){
                                        SaveLoadManager.Instance.SetUnlockedConcept(9);
                                }
                                // 소지 버프가 없고 상점 버프 확정 안했지만 돌렸다면 확정시키기
                                if(mysteryBox_current_num != -1 && SaveLoadManager.Instance.GetBuff() == 0){
                                        //Debug.Log("no buff but have buff");
                                        SelectMysteryBox();
                                }
                                isGameStart = true;
                                Charater.SetBool("Run", true);
                                // 화면 돌아가기
                                uIMenuManager.Position1();
                                // 화면 UI 꺼주기
                                mainPanel_Button.SetActive(false);
                                mainPanel_playerStat.SetActive(false);
                                SettingPanel.SetActive(false);
                                CashShopPanel.SetActive(false);
                                mapChoosePanel.SetActive(false);
                                tutoralPanel.SetActive(false);
                                cheatPanel.SetActive(false);
                                cheatCheckPanel.SetActive(false);
                                basicButton.SetActive(false);
                                // 하트 값 소모 1
                                if(heartPanelCS == null){
                                        heartPanelCS = heartPanel.GetComponent<HeartPanel>();
                                }
                                heartPanelCS.MinusHearts(1);
                                heartPanel.SetActive(false);
                                AudioManager.Instance.PlayPortalSFX();
                                portal.SetActive(true);
                                Invoke("MoveScene", 3f);
                                //SceneManager.LoadScene("GameScene");
                                //PrintIntList(ChooseMapPanel.Instance.GetChooseMapList());
                        }
                }else{
                        shop_HaertShopPanel.SetActive(true);
                }
                SaveLoadManager.Instance.SaveData();
        }

        // private void PrintIntList(List<int> intList){
        //         for(int i=0; i<intList.Count; i++){
        //                 Debug.Log(intList[i]);
        //         }
        //         if(intList.Count == 0) Debug.Log("intList.Count : " + 0);
        // }

        public void TestGamePlay(){
                if(heartPanelCS.GetCurrentHearts() >= 1 && isGameStart == false) {
                        SaveLoadManager.Instance.SetUnlockedConcept(8);
                        // for(int i=0; i < 10; i++){
                        //         for(int j=0; j < 3; j++){
                        //                 SaveLoadManager.Instance.SetUnlockedMemory(i, j);
                        //         }
                        // }
                }
                GamePlay();
        }

        private void FirstPopUp(){
                FirstStoryPanel.SetActive(true);
                tutoralPanel.SetActive(true);
        }

        private void MoveScene()
        {
               // SceneManager.LoadScene("GameScene");
               starFX.SetActive(false);
                loadingScene.SetActive(true);
                loadingSceneManager.StartSceneMove();
        }

        public void ClickShop(){
                AudioManager.Instance.PlayGameButtonClick();
                //Debug.Log("ClickShop");
                shopPanel.SetActive(true);
                AchievementPanelCS.OnPanel(false);
                uIMenuManager.Position2();
                uIMenuManager.ReturnMenu();
                Canv_CashShop.SetActive(false);
        }

        public void ClickAchievement(){
                AudioManager.Instance.PlayGameButtonClick();
                //Debug.Log("ClickAchievement");
                shopPanel.SetActive(false);
                AchievementPanelCS.OnPanel(true);
                Canv_CashShop.SetActive(false);
        }

        public void ClickCashShop(){
                AudioManager.Instance.PlayGameButtonClick();
                //Debug.Log("ClickCashShop");
                shopPanel.SetActive(false);
                AchievementPanelCS.OnPanel(false);
                Canv_CashShop.SetActive(true);
        }

        /// /////////////////////////////////////////////////////////////////
        /// player panel

        private void ChangeCoinText(int coin){
                coinText_1.text = "소지금 : " + coin.ToString();
                coinText_2.text = "소지금 : " + coin.ToString();
        }

        private void ChangeScoreText(int score){
                scoreText_1.text = "최고 점수 : " + score.ToString();
                scoreText_2.text = "최고 점수 : " + score.ToString();
        }

        private void ChangeBuffText(string buff){
                buffText_1.text = ": " + buff;
                buffText_2.text = ": " + buff;
        }

        private void ChangeHpText(int hp){
                hpText_1.text = "현재 체력 : " + hp.ToString();
                hpText_2.text = "현재 체력 : " + hp.ToString();
        }

        private void ChangeStatText(int stat1, int stat2){
                statText_1.text = "균열 점수 : " + stat1.ToString() + " / 기억 점수 : " + stat2.ToString();
                statText_2.text = "균열 점수 : " + stat1.ToString() +  " / 기억 점수 : " + stat2.ToString();
        }

        private void ChangeEnergyText(float time){
                energyText_1.text = "에너지 초당 회복량 : " + time;
                energyText_2.text = "에너지 초당 회복량 : " + time;
        }

        private void ChangeBuffDetail(int num){
                if(num == 0) {
                        buffdetailText.text = "현재 캐릭터의 체력을 50만큼 추가로 올려줍니다. (Ex. 능력 적용 전 체력 100 → 능력 적용 후 체력 150)";
                        buffdetailText2.text = "현재 캐릭터의 체력을 50만큼 추가로 올려줍니다. (Ex. 능력 적용 전 체력 100 → 능력 적용 후 체력 150)";
                }else if(num == 1) {
                        buffdetailText.text = "최종 획득한 코인에서 x1.2배만큼 추가로 받습니다. (Ex. 능력 적용 전 획득코인 100 → 능력 적용 후 획득코인120)";
                        buffdetailText2.text = "최종 획득한 코인에서 x1.2배만큼 추가로 받습니다. (Ex. 능력 적용 전 획득코인 100 → 능력 적용 후 획득코인120)";
                }else if(num == 2) {
                        buffdetailText.text = "장애물에 부딪혔을 때 기존에 받던 피해량이 절반 감소합니다. (Ex. 기존 장애물에 부딪혔을 시 받는 피해량 10 → 추가된 능력 적용 시 받는 피해량 5)";
                        buffdetailText2.text = "장애물에 부딪혔을 때 기존에 받던 피해량이 절반 감소합니다. (Ex. 기존 장애물에 부딪혔을 시 받는 피해량 10 → 추가된 능력 적용 시 받는 피해량 5)";
                }else if(num == 3) {
                        buffdetailText.text = "초당 받는 지속 데미지가 15% 만큼 적게 받습니다. (Ex. 능력 적용 전 지속 데미지 10 → 능력 적용 후 지속 데미지 8.5)";
                        buffdetailText2.text = "초당 받는 지속 데미지가 15% 만큼 적게 받습니다. (Ex. 능력 적용 전 지속 데미지 10 → 능력 적용 후 지속 데미지 8.5)";
                }else if(num == 4) {
                        buffdetailText.text = "기억의 조각을 먹었을 때 회복하는 체력량이 20% 증가합니다. (Ex. 능력 적용 전 회복량 100 → 능력 적용 후 회복량 120)";
                        buffdetailText2.text = "기억의 조각을 먹었을 때 회복하는 체력량이 20% 증가합니다. (Ex. 능력 적용 전 회복량 100 → 능력 적용 후 회복량 120)";
                }else if(num == 5) {
                        buffdetailText.text = "캐릭터의 떨어지는 가속도가 20% 더 빨라집니다.";
                        buffdetailText2.text = "캐릭터의 떨어지는 가속도가 20% 더 빨라집니다.";
                }else if(num == 6) {
                        buffdetailText.text = "각 Stage마다 기본속도가 20% 더 빨라집니다.";
                        buffdetailText2.text = "각 Stage마다 기본속도가 20% 더 빨라집니다.";
                }else if(num == 7) {
                        buffdetailText.text = "에너지의 최대 축적량이 20% 감소합니다. (Ex. 능력 적용 전 축적 에너지양 100 → 능력 적용 후 축적 에너지양 80, 최대 에너지양이 줄어들면 채워야하는 에너지의 양이 줄어 더욱 자주 스킬이 자주 발동됩니다.)";
                        buffdetailText2.text = "에너지의 최대 축적량이 20% 감소합니다. (Ex. 능력 적용 전 축적 에너지양 100 → 능력 적용 후 축적 에너지양 80, 최대 에너지양이 줄어들면 채워야하는 에너지의 양이 줄어 더욱 자주 스킬이 자주 발동됩니다.)";
                }
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
                                SaveLoadManager.Instance.SaveData();
                                ChangeShopHpText(true);
                                // 올린 스탯 수치 표시
                                ChangeHpText(hpUpgradeValue[hpUpgradeClass]);
                        }
                        else if((hpUpgradeClass+1) == maxGradeNum){ // 없다면
                                // 돈 소비
                                MinusCoin(hpUpgradeCoin[hpUpgradeClass]);
                                hpUpgradeClass += 1; // 마지막 업그레이드
                                // 실제 스탯에 적용 코드
                                SaveLoadManager.Instance.UpgradeHP();
                                SaveLoadManager.Instance.SaveData();
                                ChangeShopHpText(false);
                                // 올린 스탯 수치 표시
                                ChangeHpText(hpUpgradeValue[hpUpgradeClass]);
                        }
                }else{
                        AudioManager.Instance.PlayGameButtonNoClick();
                        if(hpUpgradeClass < maxGradeNum){
                                notiMessagePanel.StartMove();
                        }
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
                                SaveLoadManager.Instance.SaveData();
                                ChangeJellyText(true);
                                // 올린 스탯 수치 표시
                                ChangeStatText(stat1UpgradeValue[jellyUpgradeClass], stat2UpgradeValue[jellyUpgradeClass]);
                        }
                        else if((jellyUpgradeClass+1) == maxGradeNum) { 
                                // 돈 소비
                                MinusCoin(jellyUpgradeCoin[jellyUpgradeClass]);
                                jellyUpgradeClass += 1; // 마지막 업그레이드
                                // 실제 스탯에 적용 코드
                                SaveLoadManager.Instance.UpgradeJelly();
                                SaveLoadManager.Instance.SaveData();
                                ChangeJellyText(false);
                                // 올린 스탯 수치 표시
                                ChangeStatText(hpUpgradeValue[jellyUpgradeClass], stat2UpgradeValue[jellyUpgradeClass]);
                        }
                }else{
                        AudioManager.Instance.PlayGameButtonNoClick();
                        if(jellyUpgradeClass < maxGradeNum){
                                notiMessagePanel.StartMove();
                        }
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
                                SaveLoadManager.Instance.SaveData();
                                ChangeEnergyText(true);
                                // 올린 스탯 수치 표시
                                ChangeEnergyText(energyUpgradeValue[energyUpgradeClass]);
                        }
                        else if((energyUpgradeClass+1) == maxGradeNum){ 
                                // 돈 소비
                                MinusCoin(energyUpgradeCoin[energyUpgradeClass]);
                                energyUpgradeClass += 1; // 마지막 업그레이드
                                // 실제 스탯에 적용 코드
                                SaveLoadManager.Instance.UpgradeEnergy();
                                SaveLoadManager.Instance.SaveData();
                                ChangeEnergyText(false);
                                // 올린 스탯 수치 표시
                                ChangeEnergyText(energyUpgradeValue[energyUpgradeClass]);
                        }
                }else{
                        AudioManager.Instance.PlayGameButtonNoClick();
                        if(energyUpgradeClass < maxGradeNum){
                                notiMessagePanel.StartMove();
                        }
                }
	}

        public void GetMysteryBox(){
                if(SaveLoadManager.Instance.GetCoin() >= 10000){
                        //Debug.Log("GetMysteryBox");
                        AudioManager.Instance.PlayMesteryBoxBuySFX();
                        string randomBuffText = mysteryBox_Buff_Texts[GetRandomBuffNum()];
                        mysteryBoxBuffText.text = randomBuffText;
                        // 돈 소비
                        MinusCoin(mysteryBox_coin);
                }else{
                        AudioManager.Instance.PlayGameButtonNoClick();
                        notiMessagePanel.StartMove();
                }
	}

	public void SelectMysteryBox(){
                if(mysteryBox_current_num != -1){
                        //Debug.Log("SelectMysteryBox : "+ (mysteryBox_current_num+1));
                        AudioManager.Instance.PlayGameButtonClick();
                        ChangeBuffText(mysteryBox_Buff_Texts[mysteryBox_current_num]);
                        ChangeBuffDetail(mysteryBox_current_num);
                        SaveLoadManager.Instance.SetBuff(mysteryBox_current_num+1);
                        SaveLoadManager.Instance.SaveData();
                }
	}

        public void SetMysteryBox(int Index){
                //Debug.Log("SetMysteryBox");
                string randomBuffText = "0";
                if(Index == 0){
                        randomBuffText = "-";
                }else{
                        randomBuffText = mysteryBox_Buff_Texts[Index-1];
                        ChangeBuffDetail(Index-1);
                }
                mysteryBoxBuffText.text = randomBuffText;
                ChangeBuffText(randomBuffText);
                //Debug.Log("SetMysteryBox");
	}

        private int GetRandomBuffNum(){
                int randomIndex = Random.Range(0, mysteryBox_Buff_Texts.Length);
                mysteryBox_current_num = randomIndex;
                return randomIndex;
        }

        private void ChangeShopHpText(bool canUpgrade){
                //Debug.Log("UpgradeStatHP "+hpUpgradeClass);
                if(canUpgrade) shopHpText.text = "강화 \n" + hpUpgradeCoin[hpUpgradeClass];
                else shopHpText.text = "Max";
                shopHpLVText.text = "LV " + (hpUpgradeClass).ToString();
        }

        private void ChangeJellyText(bool canUpgrade){
                //Debug.Log("UpgradeJelly "+jellyUpgradeClass);
                if(canUpgrade) shopStatText.text = "강화 \n" + jellyUpgradeCoin[jellyUpgradeClass];
                else shopStatText.text = "Max";
                shopStatLVText.text = "LV " + (jellyUpgradeClass).ToString();
        }

        private void ChangeEnergyText(bool canUpgrade){
                //Debug.Log("UpgradeStatEnergy "+energyUpgradeClass);
                if(canUpgrade) shopEnergyText.text = "강화 \n" + energyUpgradeCoin[energyUpgradeClass];
                else shopEnergyText.text = "Max";
                shopEnergyLVText.text = "LV " + (energyUpgradeClass).ToString();
        }

        public void MinusCoin(int coin){
                // 돈 소비
                SaveLoadManager.Instance.MinusCoin(coin);
                SaveLoadManager.Instance.SaveData();
                // 바뀐 금액 표시
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
        }

        public void PlusCoin(int coin){
                //Debug.Log("PlusCoin "+ coin);
                // 돈 소비
                SaveLoadManager.Instance.PlusCoin(coin);
                SaveLoadManager.Instance.SaveData();
                // 바뀐 금액 표시
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
        }

        public void Plus100000Coin(){
                // 돈 소비
                SaveLoadManager.Instance.PlusCoin(100000);
                SaveLoadManager.Instance.SaveData();
                // 바뀐 금액 표시
                ChangeCoinText(SaveLoadManager.Instance.GetCoin());
        }

        /// /////////////////////////////////////////////////////////////////

        // public void GameSetting(){
        // Debug.Log("GameSetting");
        // }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Doublsb.Dialog;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] public DialogManager DialogManager;
    [SerializeField] public GameObject background;
    [SerializeField] public GameObject dialogPanel;
    [SerializeField] public GameObject[] Example;
    [SerializeField] public Image backgroudImg;
    [SerializeField] public Sprite[] backgroundImgSprites;
    private List<DialogData> dialogTexts = new List<DialogData>();
    public bool isPlay = false;
    private bool isDialogActive = false;
    private int dialogCount = 0;
    private int dialogCloseCount = 3;

    private static StoryManager instance;
    // singleton
    public static StoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StoryManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("StoryManager");
                    instance = obj.AddComponent<StoryManager>();
                }
            }
            return instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(DialogManager.state);
        if(isPlay){
            //ForestStroy();
            isPlay = false;
        }

        // if(isDialogActive){
        //     if(DialogManager.state == State.Deactivate){
        //         dialogCount += 1;
        //         if(dialogCount > dialogCloseCount){
        //             isPanelOn(false);
        //         }
        //     }
        // }
    }

    private void Show_Example(int index)
    {
        Example[index].SetActive(true);
    }

    // private void ForestStroy(){
    //     dialogTexts.Add(new DialogData("I am Sa. Popped out to let you know Asset can show other characters."));
    //     dialogTexts.Add(new DialogData("I am Sa. "));
    //     dialogTexts.Add(new DialogData("Popped out to let you know Asset can show other characters."));
    //     DialogManager.Show(dialogTexts);
    // }

    public void isPanelOn(bool isActive){
        if(isActive){
            background.SetActive(true);
            dialogPanel.SetActive(true);
        }else{
            background.SetActive(false);
            dialogPanel.SetActive(false);
        }
    }

    public void ShowStory(int conceptNum, int storyNum){
        // 한번 초기화
        dialogTexts.Clear();
        isPanelOn(true);
        isDialogActive = true;
        dialogCount = 0;

        // 스토리 시작
        if(conceptNum == 0 && storyNum == 0) StoryHouse1();
        else if (conceptNum == 0 && storyNum == 1) StoryHouse2();
        else if (conceptNum == 0 && storyNum == 2) StoryHouse3();

        else if(conceptNum == 1 && storyNum == 0) StorySchool1();
        else if (conceptNum == 1 && storyNum == 1) StorySchool2();
        else if (conceptNum == 1 && storyNum == 2) StorySchool3();

        else if(conceptNum == 2 && storyNum == 0) StoryToy1();
        else if (conceptNum == 2 && storyNum == 1) StoryToy2();
        else if (conceptNum == 2 && storyNum == 2) StoryToy3();

        else if(conceptNum == 3 && storyNum == 0) StoryPlayground1();
        else if (conceptNum == 3 && storyNum == 1) StoryPlayground2();
        else if (conceptNum == 3 && storyNum == 2) StoryPlayground3();

        else if(conceptNum == 4 && storyNum == 0) StoryForest1();
        else if (conceptNum == 4 && storyNum == 1) StoryForest2();
        else if (conceptNum == 4 && storyNum == 2) StoryForest3();

        else if(conceptNum == 5 && storyNum == 0) StoryDino1();
        else if (conceptNum == 5 && storyNum == 1) StoryDino2();
        else if (conceptNum == 5 && storyNum == 2) StoryDino3();

        else if(conceptNum == 6 && storyNum == 0) StorySpace1();
        else if (conceptNum == 6 && storyNum == 1) StorySpace2();
        else if (conceptNum == 6 && storyNum == 2) StorySpace3();

        else if(conceptNum == 7 && storyNum == 0) StoryPipe1();
        else if (conceptNum == 7 && storyNum == 1) StoryPipe2();
        else if (conceptNum == 7 && storyNum == 2) StoryPipe3();

        else if(conceptNum == 8 && storyNum == 0) StoryLab1();
        else if (conceptNum == 8 && storyNum == 1) StoryLab2();
        else if (conceptNum == 8 && storyNum == 2) StoryLab3();

        else if(conceptNum == 9 && storyNum == 0) StorySea1();
        else if (conceptNum == 9 && storyNum == 1) StorySea2();
        else if (conceptNum == 9 && storyNum == 2) StorySea3();
        
        
    }

    ////////////////////////////////////////
    /// dialogTexts.Add(new DialogData("")); 

    private void StoryHouse1(){
        backgroudImg.sprite = backgroundImgSprites[0];
        dialogTexts.Add(new DialogData("나는 교통사고의 후유증으로 인해 모든 기억을 잃은 채 지난 10년동안 홀로 살아왔다.", "Sa"));
        dialogTexts.Add(new DialogData("그렇게 가족이 없던 나는 우울한 마음에 기분 좀 전환할 겸 혼자 여행 떠나기로 생각했다."));
        dialogTexts.Add(new DialogData("특별한 목적지 없이 자유롭게 여행을 하던 중 한 마을에 어디선가 익숙한 느낌의 집이 눈에 들어왔다."));
        dialogTexts.Add(new DialogData("‘잃어버린 기억의 단서가 될 수 있지 않을까’ 라는 기대감을 가지고 방문하려 했으나 날이 어두워져 근처에 숙소를 잡고 내일 가야겠다고 생각했다."));
        dialogTexts.Add(new DialogData("다음 날 어제 봤던 집 근처로 간 순간 지금껏 한 번도 느껴보지 못한 묘한 감정이 올라왔다.")); 
        dialogTexts.Add(new DialogData("그렇게 나는 집을 향해 발걸음을 옮겼다.")); 
        dialogCloseCount = 6;
        DialogManager.Show(dialogTexts);
    }

    private void StoryHouse2(){
        backgroudImg.sprite = backgroundImgSprites[0];
        dialogTexts.Add(new DialogData("평소라면 무시하고 걸었을 집이었지만, 나도 모르게 발이 이끌렸고 괜스레 올라오는 긴장감을 애써 눌렀다.", "Sa"));
        dialogTexts.Add(new DialogData("문을 두드리기 전 꽤 오랫동안 사용하지 않았다는 듯 손잡이가 녹슬어 있었고 인기척 또한 느껴지지 않았다."));
        dialogTexts.Add(new DialogData("“계세요…?”", "Li"));
        dialogTexts.Add(new DialogData("문을 두드린 후 한참을 기다리고 몇 번을 물어봤지만 안에서는 대답이 없었다.", "Sa"));
        dialogTexts.Add(new DialogData("나는 긴장해서 손에 땀이 나는 것을 보고 옷에 닦은 후 문고리를 돌렸다."));
        dialogTexts.Add(new DialogData("‘찰칵, 끼익-’"));
        dialogTexts.Add(new DialogData("문을 열고 들어가자 곳곳에 먼지가 가득 쌓여 있었고, 오랫동안 방치되었던 것으로 보이는 가구들이 있었다."));
        dialogTexts.Add(new DialogData("조금 더 용기를 내 안으로 들어오니 거실 바닥에는 깨진 액자와 사진들이 있었다."));
        dialogCloseCount = 8;
        DialogManager.Show(dialogTexts);
    }

    private void StoryHouse3(){
        backgroudImg.sprite = backgroundImgSprites[0];
        dialogTexts.Add(new DialogData("먼지에 쌓여 잘 보이지 않던 액자와 사진들을 들어 먼지를 털었다.", "Sa"));
        dialogTexts.Add(new DialogData("사진에는 바닷가에서 부모와 형과 동생으로 보이는 두 아이가 해맑게 웃고 있었고, 다른 사진에는 초등학교를 배경으로 한 두 남자 아이가 어깨 동무를 하며 찍은 사진이었다."));
        dialogTexts.Add(new DialogData("나는 사진을 가지고 집에서 나와 숙소로 돌아가 숙소의 주인장에게 사진 속 가족을 아냐고 물었다."));
        dialogTexts.Add(new DialogData("주인장은 가족은 모르고 학교는 어디인지 안다며 위치를 알려줬고 그렇게 나는 아이들이 다녔던 학교로 가게 되었다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StorySchool1(){
        backgroudImg.sprite = backgroundImgSprites[1];
        dialogTexts.Add(new DialogData("간단히 점심을 먹고 숙소 주인장이 알려준 학교를 가보니 설립된 지 오래된 느낌의 학교가 보였다.", "Sa"));
        dialogTexts.Add(new DialogData("정문에 들어서니 수업을 마치는 종소리가 들렸고, 어린 아이들이 뛰면서 학교를 나갔다."));
        dialogTexts.Add(new DialogData("나는 학교 주위를 둘러보다가 교무실에 노크를 하며 들어갔다."));
        dialogTexts.Add(new DialogData("“들어가도 될까요?”", "Li"));
        dialogTexts.Add(new DialogData("그러자 창가에 앉아 있던 50대 정도 되어 보이는 여교사가 나를 보며 일어섰다.", "Sa"));
        dialogTexts.Add(new DialogData("“안녕하세요? 무슨 일로 오셨나요?”"));
        dialogTexts.Add(new DialogData("“안녕하세요. 여쭤볼 게 있어서 찾아 뵙게 되었어요.”", "Li"));
        dialogCloseCount = 7;
        DialogManager.Show(dialogTexts);
    }

    private void StorySchool2(){
        backgroudImg.sprite = backgroundImgSprites[1];
        dialogTexts.Add(new DialogData("여교사는 자리를 안내하며 무슨 일인지 물어보았다.", "Sa"));
        dialogTexts.Add(new DialogData("나는 주머니에서 사진들을 꺼내"));
        dialogTexts.Add(new DialogData("“이 아이들을 보신 적 있으신가요? 이 학교에서 찍은 사진으로 보여서요.”", "Li"));
        dialogTexts.Add(new DialogData("선생님의 미간이 살짝 찌푸려지다가 “아-” 소리와 함께 펴지며 사진속의 아이들을 가리켰다.", "Sa"));
        dialogTexts.Add(new DialogData("“왼쪽 아이는 Aiden, 오른쪽 아이는 Ehan의 동생인 Noah이었던 걸로 기억이 나네요. 둘은 형제 관계인데, 항상 학교에 같이 다니고 우애가 깊은 아이들이었죠.”"));
        dialogTexts.Add(new DialogData("“10년 전 쯤 사고를 당해 그 자리에서 부모님은 돌아가시고 큰 아이는 의식불명 상태가 되었는데, 동생은 실종되었다고 들었어요. 참 안타까운 사고였죠...”"));
        dialogCloseCount = 6;
        DialogManager.Show(dialogTexts);
    }

    private void StorySchool3(){
        backgroudImg.sprite = backgroundImgSprites[1];
        dialogTexts.Add(new DialogData("선생님이 아이들의 대한 얘기를 할 때 집중하기가 어려웠다.", "Sa"));
        dialogTexts.Add(new DialogData("내가 병원에서 깨어난 날 의사가 ‘10년만에 깨어나셨네요. 불편한 곳은 없어요?’ 라고 말한 것이 생각났다."));
        dialogTexts.Add(new DialogData("그리고 사진 속 가족이 사고를 당한 날과 내가 병원으로 실려간 날이 비슷했고 무엇보다 아이들의 이름 중 나와 같은 이름이 있다는 것이 과연 우연의 일치일지 고민했다."));
        dialogTexts.Add(new DialogData("선생님은 나에게 깊은 고민이 있어 보인다며 아이들의 대한 내용을 좀 더 상세히 말해줬다."));
        dialogTexts.Add(new DialogData("나는 선생님께 감사하다는 말씀을 전해드리고 학교를 나왔고 다시 사진이 있던 집으로 돌아가기로 했다."));
        dialogCloseCount = 5;
        DialogManager.Show(dialogTexts);
    }

    private void StoryToy1(){
        backgroudImg.sprite = backgroundImgSprites[2];
        dialogTexts.Add(new DialogData("나는 집으로 돌아와 거실을 지나쳐 방문을 열어보니 침대와 책상, 그리고 바닥에는 여러가지 장난감들이 있었다.", "Sa"));
        dialogTexts.Add(new DialogData("아이들이 좋아할 만한 장난감과 블록들이 다양하게 있었고, 오랫동안 사람이 살지 않아 방치되었음에도 불구하고 책상 위 사진들을 보니 두 아이가 즐겁게 놀았을 방이라는 것이 느껴졌다."));
        dialogTexts.Add(new DialogData("또한, 누가 봐도 한 눈에 이 방은 부모님의 애정으로 가득 꾸며진 방이라는 것을 알 수 있는 것처럼 이 방의 주인들은 사랑받고 있었다는 것을 알 수 있었다."));
        dialogTexts.Add(new DialogData("‘나도 이런 아이들처럼 행복한 가정이었을까?’", "Li"));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryToy2(){
        backgroudImg.sprite = backgroundImgSprites[2];
        dialogTexts.Add(new DialogData("장난감들을 보니 어린 시절 나는 어떻게 지냈을 지 생각하게 되었다.", "Sa"));
        dialogTexts.Add(new DialogData("그런데 보통 아이들은 자동차나 건담 등 다양한 장난감들을 가지고 싶어 하기 마련인데 공룡 장난감만 바닥에 즐비하게 놓여 있었다."));
        dialogTexts.Add(new DialogData("그리고 벽면에는 별자리와 여러 행성에 대한 사진이 붙여져 있었고 우주 비행사와 관련된 내용의 포스터도 함께 있었다."));
        dialogTexts.Add(new DialogData("포스터 옆에는 ‘난 꼭 우주를 여행해서 자유롭게 다니는 우주 비행사가 될 거야!’ 라고 글씨가 적혀져 있어 꿈이 확고한 아이라는 것을 알 수 있었다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryToy3(){
        backgroudImg.sprite = backgroundImgSprites[2];
        dialogTexts.Add(new DialogData("책상 위를 보니 ㅁㅁㅁ의 일기장이 보였고 날짜를 보니 사고가 나기 직전에 쓴 내용이었다.", "Sa"));
        dialogTexts.Add(new DialogData("‘오늘도 형과 함께 놀이터에서 정글짐을 타며 재밌게 놀았다. 나는 형과 같이 숲에 있는 빛을 가보고 싶었는데 형은 위험하니 안된다고 한다."));
        dialogTexts.Add(new DialogData("나는 한 번만 같이 가보자고 했지만 형이 화를 내면서 안된다고 해서 아쉬웠다. 왜 안된다고 하는 걸까?’"));
        dialogTexts.Add(new DialogData("내용을 보고 무슨 말인지 이해가 잘 안 갔지만 숲에 뭔가 있다라는 느낌을 받았다. 이건 무슨 말일까 싶었지만 그냥 어린 아이가 상상력으로 쓴 것이라 생각하며 넘어갔다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryPlayground1(){
        backgroudImg.sprite = backgroundImgSprites[3];
        dialogTexts.Add(new DialogData("집을 나와 일기장의 내용을 따라 놀이터 정글짐으로 갔다.", "Sa"));
        dialogTexts.Add(new DialogData("하지만 이렇다할 단서는 찾지 못한 채 그네에 앉아서 쉬었고 아침부터 긴장되던 마음이 차츰 풀리며 피곤이 몰려왔고 나도 모르는 사이 잠에 들었다."));
        dialogTexts.Add(new DialogData("잠에 들고 꿈에서 사진 속 동생이 ‘형, 나하고 숲에 한 번만 가보면 안될까? 제발 부탁이야… 기다리고 있을게..’라는 말과 함께 놀라면서 잠에서 깼다."));
        dialogTexts.Add(new DialogData("‘사진 속 아이가 꿈에서 나오다니..’", "Li"));
        dialogTexts.Add(new DialogData("꿈 속에서 나온 곳과 현재 이 곳이 너무나도 일치했고, 동생이 기다린다는 말을 한 것에 대해 의구심을 품은 채 다시 한 번 일기장을 펼쳐봤다.", "Sa"));
        dialogCloseCount = 5;
        DialogManager.Show(dialogTexts);
    }

    private void StoryPlayground2(){
        backgroudImg.sprite = backgroundImgSprites[3];
        dialogTexts.Add(new DialogData("이전 일기의 내용보다 앞쪽 페이지를 보니", "Sa"));
        dialogTexts.Add(new DialogData("‘오늘은 형과 숲에서 숨바꼭질을 했다. 형이 술래였는데 밤이 될 때까지 나를 못 찾아서 내가 이겼다. 숲에 너무 깊숙이 들어와서 길을 잃었는데 밝은 빛이 보였다. "));
        dialogTexts.Add(new DialogData("빛을 따라가봤는데 큰 돌덩이 안에 빛이 나오고 있었다. 거기로 들어가니 다른 세상이 나왔고 놀란 나는 다시 밖으로 나와 집으로 왔다. 다음엔 형과 함께 가 봐야겠다.’ 라는 내용이 적혀 있었다."));
        dialogCloseCount = 3;
        DialogManager.Show(dialogTexts);
    }

    private void StoryPlayground3(){
        backgroudImg.sprite = backgroundImgSprites[3];
        dialogTexts.Add(new DialogData("나는 자면서 꾼 꿈과 일기장의 내용을 보고 아무리 봐도 동생이 말한 그 숲을 이상하게 생각했고, 그곳에서 단서가 될 만한 것이 있을 거라 생각했다.", "Sa"));
        dialogTexts.Add(new DialogData("그리고 꿈과 놀이터를 보니 내가 사진 속 동생의 형이라는 것을 어느 정도 확신하게 되었다."));
        dialogTexts.Add(new DialogData("하지만 일기장에서 말한 숲이 어디인지 몰라 수소문을 통해 대략적인 위치 파악이 가능했다."));
        dialogTexts.Add(new DialogData("이걸 물어봤을 때 대부분의 사람들은 되도록이면 가지 말라는 말과 함께 지난 수 년간 숲에서 실종된 사람이 여럿 있었다는 말을 듣게 되었다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryForest1(){
        backgroudImg.sprite = backgroundImgSprites[4];
        dialogTexts.Add(new DialogData("사람들이 말한 숲에 오니 사람들이 꺼리는 이유를 알았다.", "Sa"));
        dialogTexts.Add(new DialogData("인적이 드물고 나무는 우거져 최소 3m는 되어 보여 내가 어디까지 왔는지 가늠하기 힘든 곳일 거라는 불안감이 엄습했다."));
        dialogTexts.Add(new DialogData("하지만 동생을 찾기 위해서라면 뭐든 할 수 있다는 마음으로 숲을 조사하러 들어갔다."));
        dialogTexts.Add(new DialogData("그렇게 반나절이 지나도록 동생을 찾아 헤맸지만 동생에 대한 단서는 찾을 수 없었다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryForest2(){
        backgroudImg.sprite = backgroundImgSprites[4];
        dialogTexts.Add(new DialogData("일기장에서 말한 큰 돌덩이를 찾아 돌아다녀봤지만 나오라는 돌덩이는 나오지 않고 산을 오르락내리락하며 나무만 보게 되었다.", "Sa"));
        dialogTexts.Add(new DialogData("나는 어린 동생이 상상력을 통한 거짓말이라고 생각하며 이런 허구를 믿은 나를 바보라고 생각했다."));
        dialogTexts.Add(new DialogData("설상가상 밤이 깊어져 마을로 돌아가려는 순간 어두운 곳에서 희미한 빛이 나오는 것을 보게 되었다."));
        dialogCloseCount = 3;
        DialogManager.Show(dialogTexts);
    }

    private void StoryForest3(){
        backgroudImg.sprite = backgroundImgSprites[4];
        dialogTexts.Add(new DialogData("희미한 빛을 따라가보니 거대한 동굴이 있었고 나는 그제서야 동생의 일기장에서 본 큰 돌덩이가 동굴 입구라는 것을 알 수 있었다.", "Sa"));
        dialogTexts.Add(new DialogData("‘찾았다. 동생이 말한 곳이 여기였어! 어서 찾아서 데리고 돌아와야겠다.’"));
        dialogTexts.Add(new DialogData("동굴 안으로 들어가자 나무 문 틈에서 빛이 새어 나오고 있었다."));
        dialogTexts.Add(new DialogData("나는 주저없이 문을 열었고 너무 밝은 빛 때문에 눈을 감은 채 문 안으로 끌려 들어갔다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryDino1(){
        backgroudImg.sprite = backgroundImgSprites[5];
        dialogTexts.Add(new DialogData("‘쿵, 쿵’"));
        dialogTexts.Add(new DialogData("잠시 정신을 잃었던 엄청나게 큰 소리와 함께 땅이 울리는 것을 느껴 잠에서 깼다.", "Sa"));
        dialogTexts.Add(new DialogData("주변을 둘러보니 내가 살던 세상과 전혀 다른 세상이 펼쳐졌다."));
        dialogTexts.Add(new DialogData("숲에 있었던 나무들과는 비교도 안 될 만큼 높은 나무들이 있었고, 땅에 있는 발자국은 내가 살고 있던 세상에서 볼 수 없는 크기의 발자국이었다."));
        dialogTexts.Add(new DialogData("나는 주위에 나뭇잎이 떨어지는 것을 보고 위를 올려다봤는데, 나무 옆에 거대한 초식공룡이 한가롭게 먹이를 먹고 있었다."));
        dialogCloseCount = 5;
        DialogManager.Show(dialogTexts);
    }

    private void StoryDino2(){
        backgroudImg.sprite = backgroundImgSprites[5];
        dialogTexts.Add(new DialogData("나는 너무 놀라 입이 벌어졌고, 이미 멸종된 공룡의 세계에 자신이 어떻게 왔는지 이해가 되지 않아 생각을 되짚어 봤다.", "Sa"));
        dialogTexts.Add(new DialogData("‘분명…나는 그 빛나는 문을 열어 들어왔는데 여긴 대체 어디지…?’", "Li"));
        dialogTexts.Add(new DialogData("생각할 시간도 잠시, 주변에 있던 모든 초식 공룡들이 한 곳으로 일제히 움직이기 시작했고 그들은 무엇인가에 쫓기듯 도망쳤다.", "Sa"));
        dialogTexts.Add(new DialogData("영문을 모르던 나도 뒤따라 달렸는데 멀리서 엄청난 굉음과 함께 육식공룡인 티라노사우르스가 나타났고, 나와 초식공룡들을 향해 달려오고 있었다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryDino3(){
        backgroudImg.sprite = backgroundImgSprites[5];
        dialogTexts.Add(new DialogData("초식 공룡들 옆에서 같이 도망치던 도중 육식 공룡이 도망치던 나를 보고 쫓아오기 시작했다.", "Sa"));
        dialogTexts.Add(new DialogData("나는 속으로 ‘동생을 찾으러 왔는데 이런 말도 안 되는 곳에 와서 이렇게 죽을 수 없어!’ 를 외치며 죽을 힘을 다해 도망쳤다."));
        dialogTexts.Add(new DialogData("하지만 공룡 앞에서 도망치기엔 역부족이었고, 설상가상 절벽이 나타나 더 이상 도망칠 수 있는 곳이 없어지게 되었다."));
        dialogTexts.Add(new DialogData("그 때 하늘을 나는 공룡이 날아와 나를 낚아챘고 하늘에 빛이 생겨 그대로 다른 세계로 건너가게 되었다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }
    
    private void StorySpace1(){
        backgroudImg.sprite = backgroundImgSprites[6];
        dialogTexts.Add(new DialogData("눈을 떠보니 몸이 붕 떠있는 듯한 느낌이었고 주위는 한치 앞도 보이지 않는 깜깜한 곳이었다.", "Sa"));
        dialogTexts.Add(new DialogData("“아무도 없어요!!!???”", "Li"));
        dialogTexts.Add(new DialogData("누군가 있을까 싶어 소리 쳤지만 아무 소리도 들리지 않았고 돌아오는 메아리 소리조차 없어 공포감을 느꼈다.", "Sa"));
        dialogTexts.Add(new DialogData("그 때 멀리서 반짝거리는 무엇인가를 발견했고 나는 헤엄을 치듯 그곳을 향해 갔다."));
        dialogTexts.Add(new DialogData("얼마나 갔을까?", "Li"));
        dialogTexts.Add(new DialogData("잠시 뒤 수많은 빛이 주변을 지나쳐갔고 나는 그 빛이 별이라는 것을 알게 되었다.", "Sa"));
        dialogTexts.Add(new DialogData("‘설마 내가 우주에 떠있는 건가...!?’", "Li"));
        dialogCloseCount = 7;
        DialogManager.Show(dialogTexts);
    }

    private void StorySpace2(){
        backgroudImg.sprite = backgroundImgSprites[6];
        dialogTexts.Add(new DialogData("수많은 별들과 행성들을 보며 내가 우주에 있다는 것을 눈으로 보고도 믿을 수가 없었다.", "Sa"));
        dialogTexts.Add(new DialogData("아무리 눈을 씻고 볼을 꼬집어 보며 지금까지 자신이 겪은 일들이 거짓말로 느껴지지 않았다."));
        dialogTexts.Add(new DialogData("그렇게 현실에서는 볼 수 없었던 우주를 보며 신나는 우주 여행을 하다가 내가 왜 이곳에 왔는지 동생을 어떻게 찾을지에 대한 고민을 하게 되었다."));
        dialogTexts.Add(new DialogData("‘빛을 따라 갈 때마다 전혀 다른 세상이 나오고 아까는 공룡… 지금은 우주… 무슨 연관이 있는 거지?’", "Li"));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StorySpace3(){
        backgroudImg.sprite = backgroundImgSprites[6];
        dialogTexts.Add(new DialogData("한참 고민을 하게 된 끝에 동생의 방에서 본 공룡들과 행성이 실제로 나타났다는 것을 알게 되었다.", "Sa"));
        dialogTexts.Add(new DialogData("문득 ‘이건 동생의 방에 있던 것들이 실제화 된 것 같고, 그 기억을 내가 따라가고 있는 것이 아닐까?’ 라는 생각이 들었다.", "Li"));
        dialogTexts.Add(new DialogData("그리고 멀리 한 행성에서 숲과 공룡 세계에서 본 빛이 나타났고 저 빛을 따라가면 동생이 있는 곳으로 안내해줄 것이라 확신했다.", "Sa"));
        dialogCloseCount = 3;
        DialogManager.Show(dialogTexts);
    }

    private void StoryPipe1(){
        backgroudImg.sprite = backgroundImgSprites[7];
        dialogTexts.Add(new DialogData("나는 바닥과 천장이 매우 낮은 통로 같은 곳에 떨어졌다.", "Sa"));
        dialogTexts.Add(new DialogData("그곳은 퀘퀘한 냄새와 불쾌한 습기가 내 온 몸을 감싸 안았다."));
        dialogTexts.Add(new DialogData("“너무 어두운 걸?”", "Li"));
        dialogTexts.Add(new DialogData("나는 벽에 붙어있던 랜턴에 불을 켰다.", "Sa"));
        dialogTexts.Add(new DialogData("“치이익- 탁”"));
        dialogTexts.Add(new DialogData("주변이 밝아지자 복잡한 배관들과 빠르게 흘러가는 액체들이 나타났다."));
        dialogTexts.Add(new DialogData("“뭐지… 여긴? 하수구 같은 곳인가?”", "Li"));
        dialogTexts.Add(new DialogData("“고오오오오-“", "Sa"));
        dialogTexts.Add(new DialogData("뒤에서 뭔가 엄청난 소리가 들려오기 시작했다."));
        dialogTexts.Add(new DialogData("“이..일단 앞으로 가볼까..?”", "Li"));
        dialogTexts.Add(new DialogData("나는 서서히 랜턴에 불을 밝히며 앞으로 나아갔다.", "Sa"));
        dialogCloseCount = 11;
        DialogManager.Show(dialogTexts);
    }

    private void StoryPipe2(){
        backgroudImg.sprite = backgroundImgSprites[7];
        dialogTexts.Add(new DialogData("“콰직..!”", "Sa"));
        dialogTexts.Add(new DialogData("천천히 걸어가고 있던 도중 뒤에서 엄청나게 큰소리가 좁은 통로를 통해 메아리 치듯이 들려왔다."));
        dialogTexts.Add(new DialogData("나는 흠칫하여, 뒤를 돌아봤지만 어두워서 잘 보이지 않았다."));
        dialogTexts.Add(new DialogData("“쿠과과과광!”"));
        dialogTexts.Add(new DialogData("뭔가 위험한 소리가 계속 들려오기 시작했다. 이건 누가 들어봐도 뭔가 무너지는 소리였다."));
        dialogTexts.Add(new DialogData("“어..어?”", "Li"));
        dialogTexts.Add(new DialogData("나는 반사적으로 앞으로 빠르게 달려나가기 시작했다.", "Sa"));
        dialogTexts.Add(new DialogData("뒤를 돌아보니, 복잡한 배관들은 어디 가고, 천장이 무너져 내리고 있었다."));
        dialogTexts.Add(new DialogData("“쏴아아아”"));
        dialogTexts.Add(new DialogData("그 뒤로 엄청난 물살이 나를 그대로 덮쳤고, 휘말려 버리고 말았다."));
        dialogCloseCount = 10;
        DialogManager.Show(dialogTexts);
    }

    private void StoryPipe3(){
        backgroudImg.sprite = backgroundImgSprites[7];
        dialogTexts.Add(new DialogData("“으아아아! 여기서 죽고 싶진 않아”", "Li"));
        dialogTexts.Add(new DialogData("나는 물에 휩쓸려가며, 소리쳤다.", "Sa"));
        dialogTexts.Add(new DialogData("“꼬르르륵”"));
        dialogTexts.Add(new DialogData("그대로 나는 물에 잠겼고, 주마등처럼 많은 기억들이 내 머릿속을 스쳐 지나갔다."));
        dialogTexts.Add(new DialogData("나와 학교를 같이 다녔던 아이, 꿈이 우주 비행사였던 아이 그리고 나와 매일 놀이터에서 함께 놀던 그 아이는 내 친동생임이 확실했다."));
        dialogTexts.Add(new DialogData("“이대로 끝낼 수는 없어… 이제야 기억을 되찾았는데!”", "Li"));
        dialogTexts.Add(new DialogData("물 속에 잠겨 사경을 헤매고 있을 때, 통로 저 끝에서 빛이 보이기 시작했다.", "Sa"));
        dialogTexts.Add(new DialogData("나는 살기 위해 빛 쪽으로 몸을 날렸다."));
        dialogCloseCount = 8;
        DialogManager.Show(dialogTexts);
    }

    private void StoryLab1(){
        backgroudImg.sprite = backgroundImgSprites[8];
        dialogTexts.Add(new DialogData("통로의 끝으로 나오자 과학실에서나 볼 수 있을 법한 연구실이 나왔다.", "Sa"));
        dialogTexts.Add(new DialogData("많은 유리 통들이 곳곳에 있었고, 유리 통 안에는 액체로 가득 차 있었다."));
        dialogTexts.Add(new DialogData("가까이 다가가니 외계인처럼 보이는 것들이 눈을 감고 있었고 놀란 나는 뒤로 넘어졌다."));
        dialogTexts.Add(new DialogData("‘도대체 여긴 어디야!? 동생을 찾으러 왔는데 왜 이런 곳으로 온 거지? 이것도 동생의 상상한 세계라고?’", "Li"));
        dialogTexts.Add(new DialogData("말도 안 된다고 생각하며 연구실을 돌아보니 한 쪽 모퉁이에 어떠한 그림자가 보였다.", "Sa"));
        dialogCloseCount = 5;
        DialogManager.Show(dialogTexts);
    }

    private void StoryLab2(){
        backgroudImg.sprite = backgroundImgSprites[8];
        dialogTexts.Add(new DialogData("그 남자는 나를 보며", "Sa"));
        dialogTexts.Add(new DialogData("“형! 와줬구나! 와서 이것 좀 풀어줄 수 있어?”"));
        dialogTexts.Add(new DialogData("나를 형이라고 부르는 동생… 드디어 동생을 만나게 되었는데 동생의 상태는 좋지 않은 상태였고, 심지어 몸이 구속되어 움직이지 못하는 상황이었다."));
        dialogTexts.Add(new DialogData("도대체 여긴 어디이고 너를 여기에 묶은 게 누구인지 물어봤는데"));
        dialogTexts.Add(new DialogData("‘차원여행을 하던 도중 우주에서 외계인들에 의해 이 곳으로 끌려오게 되었고, 지난 10년 동안 갇혀 지냈다는 말을 듣게 되었다."));
        dialogTexts.Add(new DialogData("동생과 대화를 하던 도중 연구실 밖에서 이상한 목소리가 들려왔고, 나는 숨을 곳을 찾다가 책상 아래로 몸을 숨겼다."));
        dialogCloseCount = 6;
        DialogManager.Show(dialogTexts);
    }

    private void StoryLab3(){
        backgroudImg.sprite = backgroundImgSprites[8];
        dialogTexts.Add(new DialogData("잠시 뒤 외계인들이 동생에게 다가와 음식 같은 것을 주고 주변을 살핀 후 다시 방을 나갔다.", "Sa"));
        dialogTexts.Add(new DialogData("나는 동생을 풀 방법을 알아내기 위해 열쇠 같은 것을 찾아봤지만 나오는 것은 알 수 없는 단어로 된 외계어 서류일 뿐 열쇠 같은 것은 나오지 않았다."));
        dialogTexts.Add(new DialogData("그렇게 컴퓨터의 모니터처럼 생긴 물건에 가서 이것저것 눌러보는데 잘못 눌렀는지 비상벨이 울리기 시작했다."));
        dialogTexts.Add(new DialogData("마음이 급했던 나는 모든 버튼을 누르자 동생을 구속하던 물체가 사라졌는데 외계인들도 들어와 나를 잡으려 했다."));
        dialogTexts.Add(new DialogData("마침 빛이 나는 균열이 생겨 외계인을 따돌리고 동생과 나는 연구실을 탈출하게 되었다."));
        dialogCloseCount = 5;
        DialogManager.Show(dialogTexts);
    }

    private void StorySea1(){
        backgroudImg.sprite = backgroundImgSprites[9];
        dialogTexts.Add(new DialogData("연구실에서 탈출하니 주변은 해변가였고 안도의 한숨을 쉰 후 동생을 바라봤다.", "Sa"));
        dialogTexts.Add(new DialogData("동생도 자신이 현실로 돌아온 것에 대해 믿기지 않는 듯해 보였고 나를 보고 부둥켜안았다."));
        dialogTexts.Add(new DialogData("“살아있어줘서 정말 고마워! 정말 다행이야. 가족을 찾을 수 있어서”", "Li"));
        dialogTexts.Add(new DialogData("“나야 말로 정말 고마워 형! 형이 구하러 와줬기에 살 수 있었어…”", "Sa"));
        dialogTexts.Add(new DialogData("그렇게 나와 동생은 기쁨을 만끽하며 바닷가에서 놀던 도중 동생이 말을 걸었다."));
        dialogCloseCount = 5;
        DialogManager.Show(dialogTexts);
    }

    private void StorySea2(){
        backgroudImg.sprite = backgroundImgSprites[9];
        dialogTexts.Add(new DialogData("“형, 이 바닷가 기억나? 나의 가장 행복했던 추억이 깃든 곳이야!”", "Sa"));
        dialogTexts.Add(new DialogData("그렇게 나는 주머니 속에 있던 사진들을 보여줬고, 동생은"));
        dialogTexts.Add(new DialogData("“이 시절로 돌아가고 싶다… 엄마, 아빠를 정말 보고 싶은데 그럴 수 없다는 게 너무 슬프다…”"));
        dialogTexts.Add(new DialogData("라며 울먹였고, 나는 그러한 동생의 등을 토닥이며 위로해줬다."));
        dialogTexts.Add(new DialogData("“우리 둘이 더 힘내서 부모님 몫까지 열심히 살아가자! 부모님도 그것을 원하실 거야.”", "Li"));
        dialogTexts.Add(new DialogData("이 때 바닷가 한 편에 있던 바위에서 지금까지 차원을 이동시켜 준 빛이 나타나기 시작했다.", "Sa"));
        dialogCloseCount = 6;
        DialogManager.Show(dialogTexts);
    }

    private void StorySea3(){
        backgroudImg.sprite = backgroundImgSprites[9];
        dialogTexts.Add(new DialogData("둘은 또 이상한 곳으로 가면 어쩌지 라는 걱정이 들었지만 함께 있으면 어떠한 일이 생기든 이겨낼 수 있다고 생각하며 빛을 따라 이동했다.", "Sa"));
        dialogTexts.Add(new DialogData("눈을 뜨자 “Ehan~, Noah~ 이제 그만 집으로 가야지!” 라는 익숙한 목소리가 들려왔다."));
        dialogTexts.Add(new DialogData("동생과 나는 소리나는 방향을 보니 그리웠던 엄마와 아빠의 모습이 보였고, 자신들이 어렸을 때의 모습으로 시간이 되돌아갔다는 것을 알게 되었고 이후 행복한 나날들을 보내게 된다."));
        dialogCloseCount = 3;
        DialogManager.Show(dialogTexts);
    }

    // private void StoryForest1(){
    //     dialogTexts.Add(new DialogData(""));
    //     dialogTexts.Add(new DialogData(""));
    //     dialogTexts.Add(new DialogData(""));
    //     dialogTexts.Add(new DialogData(""));
    //     dialogTexts.Add(new DialogData(""));
    //     dialogCloseCount = 5;
    //     DialogManager.Show(dialogTexts);
    // }
}

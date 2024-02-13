using System;
using System.Collections;
using System.Collections.Generic;
using Doublsb.Dialog;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] public DialogManager DialogManager;
    [SerializeField] public GameObject background;
    [SerializeField] public GameObject dialogPanel;
    [SerializeField] public GameObject[] Example;
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
            ForestStroy();
            isPlay = false;
        }

        if(isDialogActive){
            if(DialogManager.state == State.Deactivate){
                dialogCount += 1;
                if(dialogCount > dialogCloseCount){
                    isPanelOn(false);
                }
            }
        }
    }

    private void Show_Example(int index)
    {
        Example[index].SetActive(true);
    }

    private void ForestStroy(){
        dialogTexts.Add(new DialogData("I am Sa. Popped out to let you know Asset can show other characters."));
        dialogTexts.Add(new DialogData("I am Sa. "));
        dialogTexts.Add(new DialogData("Popped out to let you know Asset can show other characters."));
        DialogManager.Show(dialogTexts);
    }

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
        else StoryHouse1();
    }

    ////////////////////////////////////////
    /// dialogTexts.Add(new DialogData("")); 

    private void StoryHouse1(){
        dialogTexts.Add(new DialogData("교통사고 후 지난 10년동안 모든 기억을 잃은 채 홀로 살아왔다."));
        dialogTexts.Add(new DialogData("가족이 없는 나는 우울한 마음에 기분 좀 전환할 겸 혼자 여행 떠나기로 생각했다."));
        dialogTexts.Add(new DialogData("특별한 목적지 없이 자유롭게 여행을 하던 중 유독 눈에 띄는 집이 있었고, 들어가보고 싶었지만 날이 어두워 근처에 숙소를 잡아 날이 밝으면 방문하기로 생각했다."));
        dialogTexts.Add(new DialogData("다음 날 숙소를 나와 어제 봤던 집 근처로 갔고 집을 본 순간 지금껏 한 번도 느껴보지 못한 묘한 감정이 올라왔다. 그렇게 나는 낯이 익은 집으로 발걸음을 옮겼다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryHouse2(){
        dialogTexts.Add(new DialogData("평소라면 무시하고 걸었을 집이었지만, 나도 모르게 발이 이끌렸고 괜스레 올라오는 긴장감을 애써 눌렀다."));
        dialogTexts.Add(new DialogData("문을 두드리기 전 꽤 오랫동안 사용하지 않았다는 듯 손잡이가 녹슬어 있었고 인기척 또한 느껴지지 않았다."));
        dialogTexts.Add(new DialogData("“계세요…?”"));
        dialogTexts.Add(new DialogData("문을 두드린 후 한참을 기다리고 몇 번을 물어봤지만 안에서는 대답이 없었다."));
        dialogTexts.Add(new DialogData("나는 긴장해서 손에 땀이 나는 것을 보고 옷에 닦은 후 문고리를 돌렸다."));
        dialogTexts.Add(new DialogData("‘찰칵, 끼익-’"));
        dialogTexts.Add(new DialogData("문을 열고 들어가자 곳곳에 먼지가 가득 쌓여 있었고, 오랫동안 방치되었던 것으로 보이는 가구들이 있었다."));
        dialogTexts.Add(new DialogData("조금 더 용기를 내 안으로 들어오니 거실 바닥에는 깨진 액자와 사진들이 있었다."));
        dialogCloseCount = 8;
        DialogManager.Show(dialogTexts);
    }

    private void StoryHouse3(){
        dialogTexts.Add(new DialogData("먼지에 쌓여 잘 보이지 않던 액자와 사진들을 들어 먼지를 털었다."));
        dialogTexts.Add(new DialogData("사진에는 바닷가에서 부모와 형과 동생으로 보이는 두 아이가 해맑게 웃고 있었고, 다른 사진에는 초등학교를 배경으로 한 두 남자 아이가 어깨 동무를 하며 찍은 사진이었다."));
        dialogTexts.Add(new DialogData("“계세요…?”"));
        dialogTexts.Add(new DialogData("나는 사진을 가지고 집에서 나와 숙소로 돌아가 숙소의 주인장에게 사진 속 가족을 아냐고 물었다."));
        dialogTexts.Add(new DialogData("주인장은 가족은 모르고 학교는 어디인지 안다며 위치를 알려줬고 그렇게 나는 아이들이 다녔던 학교로 가게 되었다."));
        dialogCloseCount = 5;
        DialogManager.Show(dialogTexts);
    }

    private void StorySchool1(){
        dialogTexts.Add(new DialogData("간단히 점심을 먹고 숙소 주인장이 알려준 학교를 가보니 설립된 지 오래된 느낌의 학교가 보였다."));
        dialogTexts.Add(new DialogData("정문에 들어서니 수업을 마치는 종소리가 들렸고, 어린 아이들이 뛰면서 학교를 나갔다. 나는 학교 주위를 둘러보다가 교무실에 노크를 하며 들어갔다."));
        dialogTexts.Add(new DialogData("“들어가도 될까요?”"));
        dialogTexts.Add(new DialogData("그러자 창가에 앉아 있던 50대 정도 되어 보이는 여교사가 나를 보며 일어섰다."));
        dialogTexts.Add(new DialogData("“안녕하세요? 무슨 일로 오셨나요?”"));
        dialogTexts.Add(new DialogData("“안녕하세요. 여쭤볼 게 있어서 찾아 뵙게 되었어요.”"));
        dialogCloseCount = 6;
        DialogManager.Show(dialogTexts);
    }

    private void StorySchool2(){
        dialogTexts.Add(new DialogData("여교사는 자리를 안내하며 무슨 일인지 물어보았다."));
        dialogTexts.Add(new DialogData("나는 주머니에서 사진들을 꺼내"));
        dialogTexts.Add(new DialogData("“이 아이들을 보신 적 있으신가요? 이 학교에서 찍은 사진으로 보여서요.”"));
        dialogTexts.Add(new DialogData("선생님의 미간이 살짝 찌푸려지다가 “아-”소리와 함께 펴지며 사진속의 아이들을 가리켰다."));
        dialogTexts.Add(new DialogData("“왼쪽 아이는 OOO, 오른쪽 아이는 OOO의 동생인 ㅁㅁㅁ이었던 걸로 기억이 나네요. 둘은 형제 관계인데, 항상 학교에 같이 다니고 우애가 깊은 아이들이었죠.”"));
        dialogTexts.Add(new DialogData("“10년 전 쯤 사고를 당해 그 자리에서 부모님은 돌아가시고 큰 아이는 의식불명 상태가 되었는데, 동생은 실종되었다고 들었어요. 참 안타까운 사고였죠...”"));
        dialogCloseCount = 6;
        DialogManager.Show(dialogTexts);
    }

    private void StorySchool3(){
        dialogTexts.Add(new DialogData("선생님이 아이들의 대한 얘기를 할 때 집중하기가 어려웠다."));
        dialogTexts.Add(new DialogData("내가 병원에서 깨어난 날 의사가 ‘10년만에 깨어나셨네요. 불편한 곳은 없어요?’ 라고 말한 것이 생각났다."));
        dialogTexts.Add(new DialogData("그리고 사진 속 가족이 사고를 당한 날과 내가 병원으로 실려간 날이 비슷했고 무엇보다 아이들의 이름 중 나와 같은 이름이 있다는 것이 과연 우연의 일치일지 고민했다."));
        dialogTexts.Add(new DialogData("선생님은 나에게 깊은 고민이 있어 보인다며 아이들의 대한 내용을 좀 더 상세히 말해줬다."));
        dialogTexts.Add(new DialogData("나는 선생님께 감사하다는 말씀을 전해드리고 학교를 나왔고 다시 사진이 있던 집으로 돌아가기로 했다."));
        dialogCloseCount = 5;
        DialogManager.Show(dialogTexts);
    }

    private void StoryToy1(){
        dialogTexts.Add(new DialogData("나는 집으로 돌아와 거실을 지나쳐 방문을 열어보니 침대와 책상, 그리고 바닥에는 여러가지 장난감들이 있었다."));
        dialogTexts.Add(new DialogData("아이들이 좋아할 만한 장난감과 블록들이 다양하게 있었고, 오랫동안 사람이 살지 않아 방치되었음에도 불구하고 책상 위 사진들을 보니 두 아이가 즐겁게 놀았을 방이라는 것이 느껴졌다."));
        dialogTexts.Add(new DialogData("또한, 누가 봐도 한 눈에 이 방은 부모님의 애정으로 가득 꾸며진 방이라는 것을 알 수 있는 것처럼 이 방의 주인들은 사랑받고 있었다는 것을 알 수 있었다."));
        dialogTexts.Add(new DialogData("‘나도 이런 아이들처럼 행복한 가정이었을까?’"));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryToy2(){
        dialogTexts.Add(new DialogData("장난감들을 보니 어린 시절 나는 어떻게 지냈을 지 생각하게 되었다."));
        dialogTexts.Add(new DialogData("그런데 보통 아이들은 다양한 장난감들을 가지고 싶어 하기 마련인데 유독 공룡 장난감이 많이 보였다. 평소에 공룡에 대한 관심이 얼마나 있는지 알 수 있었다."));
        dialogTexts.Add(new DialogData("그리고 벽지를 보니 우주 비행사와 관련된 내용의 포스터가 있었는데 포스터 옆에 ‘난 꼭 우주를 여행해서 자유롭게 다니는 우주 비행사가 될거야!’ 라는 글씨가 적혀져 있었다."));
        dialogTexts.Add(new DialogData("어린 아이지만 우주 비행사가 꿈이라는 것에 대해 귀엽고 이런 동생이 내 동생이었으면 좋겠다고 생각했다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryToy3(){
        dialogTexts.Add(new DialogData("책상 위를 보니 ㅁㅁㅁ의 일기장이 보였고 날짜를 보니 사고가 나기 직전에 쓴 내용이었다."));
        dialogTexts.Add(new DialogData("‘오늘도 형과 함께 놀이터에서 정글짐을 타며 재밌게 놀았다. 나는 형과 같이 숲에 있는 빛을 가보고 싶었는데 형은 위험하니 안된다고 한다."));
        dialogTexts.Add(new DialogData("나는 한 번만 같이 가보자고 했지만 형이 화를 내면서 안된다고 해서 아쉬웠다. 왜 안된다고 하는 걸까?’"));
        dialogTexts.Add(new DialogData("내용을 보고 무슨 말인지 이해가 잘 안 갔지만 숲에 뭔가 있다라는 느낌을 받았다. 이건 무슨 말일까 싶었지만 그냥 어린 아이가 상상력으로 쓴 것이라 생각하며 넘어갔다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryPlayground1(){
        dialogTexts.Add(new DialogData("집을 나와 일기장의 내용을 따라 놀이터 정글짐으로 갔다."));
        dialogTexts.Add(new DialogData("하지만 이렇다할 단서는 찾지 못한 채 그네에 앉아서 쉬었고 아침부터 긴장되던 마음이 차츰 풀리며 피곤이 몰려왔고 나도 모르는 사이 잠에 들었다."));
        dialogTexts.Add(new DialogData("잠에 들고 꿈에서 사진 속 동생이 ‘형, 나하고 숲에 한 번만 가보면 안될까? 제발 부탁이야… 기다리고 있을게..’라는 말과 함께 놀라면서 잠에서 깼다."));
        dialogTexts.Add(new DialogData("‘사진 속 아이가 꿈에서 나오다니..’"));
        dialogTexts.Add(new DialogData("꿈 속에서 나온 곳과 현재 이 곳이 너무나도 일치했고, 동생이 기다린다는 말을 한 것에 대해 의구심을 품은 채 다시 한 번 일기장을 펼쳐봤다."));
        dialogCloseCount = 5;
        DialogManager.Show(dialogTexts);
    }

    private void StoryPlayground2(){
        dialogTexts.Add(new DialogData("이전 일기의 내용보다 조금 더 앞으로 가니"));
        dialogTexts.Add(new DialogData("‘오늘은 형과 숲에서 숨바꼭질을 했다. 형이 술래였는데 밤이 될 때까지 나를 못 찾아서 내가 이겼다. 숲에 너무 깊숙이 들어와서 길을 잃었는데 밝은 빛이 보였다. 빛을 따라가봤는데 큰 돌덩이 안에 빛이 나오고 있었다."));
        dialogTexts.Add(new DialogData("거기로 들어가니 다른 세상이 나왔고 놀란 나는 다시 밖으로 나와 집으로 왔다. 다음엔 형과 함께 가 봐야겠다.’ 라는 내용이 적혀 있었다."));
        dialogCloseCount = 3;
        DialogManager.Show(dialogTexts);
    }

    private void StoryPlayground3(){
        dialogTexts.Add(new DialogData("나는 자면서 꾼 꿈과 일기장의 내용을 보고 아무리 봐도 동생이 말한 그 숲을 이상하게 생각했고, 그곳에서 단서가 될 만한 것이 있을 거라 생각했다."));
        dialogTexts.Add(new DialogData("그리고 꿈과 놀이터를 보니 내가 사진 속 동생의 형이라는 것을 어느 정도 확신하게 되었다."));
        dialogTexts.Add(new DialogData("하지만 일기장에서 말한 숲이 어디인지 몰라 수소문을 통해 대략적인 위치 파악이 가능했다."));
        dialogTexts.Add(new DialogData("이걸 물어봤을 때 대부분의 사람들은 되도록이면 가지 말라는 말과 함께 지난 수 년간 숲에서 실종된 사람이 여럿 있었다는 말을 듣게 되었다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryForest1(){
        dialogTexts.Add(new DialogData("사람들이 말한 숲에 오니 사람들이 꺼리는 이유를 알았다."));
        dialogTexts.Add(new DialogData("사람들의 발걸음이 별로 없었고 나무는 우거져 하늘을 덮을 정도의 높이까지 올라가 내가 어디까지 왔는지 모를 정도로 살짝 음침했다."));
        dialogTexts.Add(new DialogData("동생을 찾기 위해서라면 뭐든 할 수 있다는 마음으로 숲을 조사하러 들어갔다."));
        dialogTexts.Add(new DialogData("그렇게 반나절이 지나도록 동생을 찾아 헤맸지만 동생에 대한 단서는 찾을 수 없었다."));
        dialogCloseCount = 4;
        DialogManager.Show(dialogTexts);
    }

    private void StoryForest2(){
        dialogTexts.Add(new DialogData("일기장에서 말한 큰 돌덩이를 찾아 돌아다녀봤지만 나오라는 돌덩이는 나오지 않고 산을 오르락내리락 하며 나무만 보게 되었다."));
        dialogTexts.Add(new DialogData("나는 어린 동생이 상상력을 통한 거짓말이라고 생각하며 이런 허구를 믿은 나를 바보라고 생각했다."));
        dialogTexts.Add(new DialogData("설상가상 밤이 깊어져 마을로 돌아가려는 순간 어두운 곳에서 희미한 빛이 나오는 것을 보게 되었다."));
        dialogCloseCount = 3;
        DialogManager.Show(dialogTexts);
    }

    private void StoryForest3(){
        dialogTexts.Add(new DialogData("희미한 빛을 따라가보니 거대한 동굴이 있었고 나는 그제서야 동생의 일기장에서 본 큰 돌덩이가 동굴 입구라는 것을 알 수 있었다."));
        dialogTexts.Add(new DialogData("‘찾았다. 동생이 말한 곳이 여기였어! 어서 찾아서 데리고 돌아와야겠다.’"));
        dialogTexts.Add(new DialogData("동굴 안으로 들어가자 나무 문 틈에서 빛이 나오고 있었고 나는 주저없이 문을 열고 들어갔다."));
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

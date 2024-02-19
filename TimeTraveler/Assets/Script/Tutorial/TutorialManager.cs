using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    static TutorialManager _instance;
    public static TutorialManager Instance
    {
        get
        {
            Init_Instance();
            return _instance;
        }
    }

    static void Init_Instance()
    {
        if(_instance == null)
        {
            GameObject obj = GameObject.Find("TutorialManager");
            _instance = obj.GetComponent<TutorialManager>();
        }
    }

    GameObject _player, _curUI;
    TutorialUI _ui;
    Canvas _canvas;
    bool _isStop, _isDrag;

    private void Awake()
    {
        Init_Instance();
    }

    void Start()
    {
        _isStop = false;
        _isDrag = false;
        
        _player = GameObject.Find("Player");
        _player.GetComponent<MovePlayer>().enabled = false;

        _ui = GetComponent<TutorialUI>();

        _canvas = GameObject.Find("TutorialCanvas").GetComponent<Canvas>();
    }

    void Update()
    {
        TouchScreen();
    }

    void TouchScreen()
    {
        if (_isStop && !_isDrag && Input.touchCount > 0)
        {
            _curUI.SetActive(false);
            SetStopFalse();
        }
    }

    public void MovePlayer(Vector3 f)
    {
        f *= _canvas.scaleFactor;
        _player.GetComponent<Rigidbody>().AddForce(f);
    }

    public void SetStopTrue()
    {
        Time.timeScale = 0;

        _ui.EnableExplainUI();

        _isStop = true;
    }

    public void SetStopFalse()
    {
        Time.timeScale = 1;

        _ui.DisableExplainUI();

        _isStop = false;
    }

    public void SetCurrentUI(GameObject ui)
    {
        _curUI = ui;
        _curUI.SetActive(true);
    }

    public void SetDragTrue() 
    { 
        _isDrag = true;
        _ui.SetDragText();
    }
    
    public void SetDragFalse() 
    { 
        _isDrag = false;

        _curUI.SetActive(false);
        _ui.ResetExplainText();
    }

    public void ChargeEnergy() { _player.GetComponent<Player>().ChargeEnergy(100); }
    public void SetPlayerPosition() { _player.transform.position = new Vector3(0, 175, 0); }
    public void SetMovePlayer() { _player.GetComponent<MovePlayer>().enabled = true; }
    public void ExitTutorial() { SceneManager.LoadScene("MainScene"); }
}

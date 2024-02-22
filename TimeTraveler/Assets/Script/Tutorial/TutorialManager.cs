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

    [SerializeField]
    GameObject _backgroundUI;

    [SerializeField]
    GameObject _portalImage;

    [SerializeField]
    GameObject _restart;

    [SerializeField]
    GameObject[] _moveStage;

    GameObject _player, _curUI;
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

        _isStop = true;
    }

    public void SetStopFalse()
    {
        Time.timeScale = 1;

        DisableBackgroundUI();
        DisablePortalImage();

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
    }
    
    public void SetDragFalse() 
    { 
        _isDrag = false;

        _curUI.SetActive(false);
    }

    public void ChargeEnergy() { _player.GetComponent<Player>().ChargeEnergy(100); }
    public void SetPlayerPosition() { _player.transform.position = new Vector3(0, 175, 0); }
    public void SetMovePlayer() { _player.GetComponent<MovePlayer>().enabled = true; }
    public void ExitTutorial() { SceneManager.LoadScene("MainScene"); }
    public void EnableBackgroundUI() { _backgroundUI.SetActive(true); }
    void DisableBackgroundUI() { _backgroundUI.SetActive(false); }
    public void EnablePortalImage() { _portalImage.SetActive(true); }
    void DisablePortalImage() { _portalImage.SetActive(false); }
    public void RestartTutorial()
    {
        SetCurrentUI(_restart);
        SetStopTrue();
        SetPlayerPosition();
    }
    public GameObject[] MoveStage() { return _moveStage; }
}

using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IPublisher
{
    private void Awake()
    {
        if (FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);


    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        Screen.SetResolution(1920, 1080, true);
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

        //SceneManager.sceneLoaded += OnSceneLoaded;

        StartGame();
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.name.Equals("GameScene"))
        {
            StartGame();
            RegisterBroker();
        }
        else if (_scene.name.Equals("CampusScene"))
        {
            Broker.Clear();
            //mainMenuMng = FindAnyObjectByType<MainMenuManager>();
            //mainMenuMng.Init(isFullHD, isFullScreen);
        }
    }

    public void StartGame()
    {
        FindManager();
        InitManagers();
    }

    private void FindManager()
    {
        audioMng = FindFirstObjectByType<AudioManager>();
        bossMng = FindFirstObjectByType<BossManager>();
        uiMng = FindFirstObjectByType<UIManager>();
        camMng = FindFirstObjectByType<CameraManager>();
    }

    private void InitManagers()
    {
        //audioMng.Init();
        bossMng.Init(playerTr);
        //uiMng.Init();
        camMng.Init(playerTr);
    }

    public void RegisterBroker()
    {
        Broker.Regist(EPublisherType.GAME_MANAGER);
    }

    public void PushMessageToBroker(EMessageType _message)
    {
        Broker.AlertMessageToSub(_message, EPublisherType.GAME_MANAGER);
    }

    [SerializeField]
    private Transform playerTr = null;


    private AudioManager audioMng = null;
    private BossManager bossMng = null;
    private UIManager uiMng = null;
    private CameraManager camMng = null;


}

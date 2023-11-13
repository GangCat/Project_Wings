using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IPublisher
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        Screen.SetResolution(1920, 1080, true);
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

        StartGame();
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
        playerMng = FindFirstObjectByType<PlayerManager>();
        obstacleMng = FindFirstObjectByType<ObstacleManager>();
    }

    private void InitManagers()
    {
        //audioMng.Init();
        uiMng.Init();
        camMng.Init(playerTr, playerMng.PData, ActionFinish);
        bossMng.Init(playerTr, CameraAction, value => { uiMng.BossHpUpdate(value); }, GetRandomSpawnPoint);
        obstacleMng.Init();
        //playerMng.Init();
    }

    public void CameraAction(int _curPhaseNum)
    {
        camMng.CameraAction(_curPhaseNum);
    }

    public void ActionFinish()
    {
        bossMng.ActionFinish();
    }

    public void RegisterBroker()
    {
        Broker.Regist(EPublisherType.GAME_MANAGER);
    }

    public void PushMessageToBroker(EMessageType _message)
    {
        Broker.AlertMessageToSub(_message, EPublisherType.GAME_MANAGER);
    }

    public BossShieldGeneratorSpawnPoint[] GetRandomSpawnPoint()
    {
        return obstacleMng.GetRandomSpawnPoint();
    }

    private void Update()
    {
        if (Input.GetKeyDown(nextPhaseKeyCode))
            bossMng.ClearCurPhase();
        else if (Input.GetKeyDown(startGameKeyCode))
            bossMng.GameStart();
        else if (Input.GetKeyDown(jumpToNextPatternKeyCode))
            bossMng.JumpToNextPattern();
            
    }

    [SerializeField]
    private Transform playerTr = null;
    [SerializeField]
    private KeyCode startGameKeyCode = KeyCode.Home;
    [SerializeField]
    private KeyCode nextPhaseKeyCode = KeyCode.PageUp;
    [SerializeField]
    private KeyCode jumpToNextPatternKeyCode = KeyCode.PageDown;

    private AudioManager audioMng = null;
    private BossManager bossMng = null;
    private UIManager uiMng = null;
    private CameraManager camMng = null;
    private PlayerManager playerMng = null;
    private ObstacleManager obstacleMng = null;


}

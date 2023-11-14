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
       // pauseMng = FindFirstObjectByType<PauseManager>();
    }

    private void InitManagers()
    {
        //audioMng.Init();
        uiMng.Init();
        camMng.Init(playerTr, playerMng.PData, ActionFinish);
        bossMng.Init(playerTr, CameraAction, value => { uiMng.BossHpUpdate(value); }, value => { uiMng.BossShieldUpdate(value); }, GetRandomSpawnPoint, BossClear);
        playerMng.Init(value => { uiMng.UpdateSp(value); }, value => { uiMng.UpdateHp(value); }); 
        obstacleMng.Init();
    }

    public void CameraAction(int _curPhaseNum)
    {
        camMng.CameraAction(_curPhaseNum);
    }

    public void ActionFinish(bool _isGameClear = false)
    {
        if (!_isGameClear)
            bossMng.ActionFinish();
        else
            uiMng.GameClear();
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

    private void BossClear()
    {
        camMng.CameraAction(3);
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
    private PauseManager pauseMng = null;
}

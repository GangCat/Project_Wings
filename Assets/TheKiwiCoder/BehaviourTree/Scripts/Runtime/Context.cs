using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder {

    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context {
        public GameObject gameObject;
        public GameObject gatlingHolderGo;
        public GameObject gatlingHeadGo;
        public GameObject airPushGo;
        public Transform transform;
        public Transform playerTr;
        public Transform gunMuzzleTr;
        public Transform giantHomingMissileSpawnTr;
        public Rigidbody physics;
        public BossCollider bossCollider;
        public AnimationControllerBase anim;
        public GroupHomingMissileSpawnPos[] arrGroupHomingMissileSpawnPos;
        public CannonRainMemoryPool cannonRainMemoryPool;
        public CannonMemoryPool cannonMemoryPool;
        public GatlinMemoryPool gatlinMemoryPool;
        public GroupMissileMemoryPool groupMissileMemoryPool;
        public bool isPhaseEnd;
        public BossController bossCtrl;
        public Transform[] footWindTr;
        public GameObject footWindGo;
        public GameObject sitDownGo;
        public CannonAudioManager cannonAudioManager;
        public CustomAudioManager airPushAudioManager;
        public GameObject[] cannonSoundSpawnGOs;
        // Add other game specific systems here

        public static Context CreateFromGameObject(
            Transform _playerTr, 
            AnimationControllerBase _anim, 
            BossCollider _bossCollider, 
            Transform _giantHomingMissileSpawnTr,
            GroupHomingMissileSpawnPos[] _arrGroupHomingMissileSpawnPos,
            BossController _bossCtrl) {
            // Fetch all commonly used components   
            Context context = new Context();
            context.bossCtrl = _bossCtrl;
            context.gameObject = _bossCtrl.gameObject;
            context.transform = _bossCtrl.transform;
            context.physics = _bossCtrl.GetComponent<Rigidbody>();
            context.bossCollider = _bossCollider;
            context.playerTr = _playerTr;
            context.gatlingHolderGo = _bossCtrl.GatlingHolder;
            context.gatlingHeadGo = _bossCtrl.GatlingHead;
            context.gunMuzzleTr = _bossCtrl.GunMuzzle;
            context.anim = _anim;
            context.isPhaseEnd = false;
            context.giantHomingMissileSpawnTr = _giantHomingMissileSpawnTr;
            context.arrGroupHomingMissileSpawnPos = _arrGroupHomingMissileSpawnPos;
            context.cannonRainMemoryPool = _bossCtrl.CannonRainMemoryPool;
            context.cannonMemoryPool = _bossCtrl.CannonMemoryPool;
            context.gatlinMemoryPool = _bossCtrl.GatlinMemoryPool;
            context.groupMissileMemoryPool = _bossCtrl.GroupMissileMemoryPool;
            context.airPushGo = _bossCtrl.AirPush;
            context.footWindTr = _bossCtrl.FootWindTr;
            context.footWindGo = _bossCtrl.FootWindGo;
            context.sitDownGo = _bossCtrl.SitDownGo;
            context.cannonAudioManager = _bossCtrl.CannonAudioManager;
            context.airPushAudioManager = _bossCtrl.AirPushAudioManager;
            context.cannonSoundSpawnGOs = _bossCtrl.CannonSoundSpawnGOs;
            // Add whatever else you need here...

            return context;
        }
    }
}
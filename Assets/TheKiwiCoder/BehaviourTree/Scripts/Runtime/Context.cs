using System.Collections;
using System.Collections.Generic;
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
        public GameObject giantHomingMissileGo;
        public Transform transform;
        public Transform playerTr;
        public Transform gunMuzzleTr;
        public Transform giantHomingMissileSpawnTr;
        public Rigidbody physics;
        public BossCollider bossCollider;
        public AnimationControllerBase anim;
        public BossShieldGeneratorSpawnPointHolder secondWeakPointHolder;
        public GroupHomingMissileSpawnPos[] arrGroupHomingMissileSpawnPos;
        public bool isPhaseEnd;
        // Add other game specific systems here

        public static Context CreateFromGameObject(
            GameObject gameObject, 
            Transform _playerTr, 
            GameObject _gatlingHolder, 
            GameObject _gatlingHead,
            Transform _gunMuzzleTr, 
            AnimationControllerBase _anim, 
            BossCollider _bossCollider, 
            BossShieldGeneratorSpawnPointHolder _secondWeakPointHolder,
            GameObject _giantHomingMissileGo,
            Transform _giantHomingMissileSpawnTr,
            GroupHomingMissileSpawnPos[] _arrGroupHomingMissileSpawnPos) {
            // Fetch all commonly used components
            Context context = new Context();
            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            context.physics = gameObject.GetComponent<Rigidbody>();
            context.bossCollider = _bossCollider;
            context.playerTr = _playerTr;
            context.gatlingHolderGo = _gatlingHolder;
            context.gatlingHeadGo = _gatlingHead;
            context.gunMuzzleTr = _gunMuzzleTr;
            context.anim = _anim;
            context.isPhaseEnd = false;
            context.giantHomingMissileGo = _giantHomingMissileGo;
            context.giantHomingMissileSpawnTr = _giantHomingMissileSpawnTr;
            context.secondWeakPointHolder = _secondWeakPointHolder;
            context.arrGroupHomingMissileSpawnPos = _arrGroupHomingMissileSpawnPos;
            // Add whatever else you need here...

            return context;
        }
    }
}
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
        public Transform transform;
        public Transform playerTr;
        public Transform gunMuzzleTr;
        public Animator animator;
        public Rigidbody physics;
        public NavMeshAgent agent;
        public SphereCollider sphereCollider;
        public BoxCollider boxCollider;
        public CapsuleCollider capsuleCollider;
        public CharacterController characterController;
        public bool isPhaseEnd;
        public AnimationControllerBase anim;
        // Add other game specific systems here

        public static Context CreateFromGameObject(GameObject gameObject, Transform _playerTr = null, GameObject _gatlingHolder = null, GameObject _gatlingHead = null,Transform _gunMuzzleTr = null, AnimationControllerBase _anim = null) {
            // Fetch all commonly used components
            Context context = new Context();
            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            context.animator = gameObject.GetComponent<Animator>();
            context.physics = gameObject.GetComponent<Rigidbody>();
            context.agent = gameObject.GetComponent<NavMeshAgent>();
            context.sphereCollider = gameObject.GetComponent<SphereCollider>();
            context.boxCollider = gameObject.GetComponent<BoxCollider>();
            context.capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            context.characterController = gameObject.GetComponent<CharacterController>();
            context.playerTr = _playerTr;
            context.isPhaseEnd = false;
            context.gatlingHolderGo = _gatlingHolder;
            context.gatlingHeadGo = _gatlingHead;
            context.gunMuzzleTr = _gunMuzzleTr;
            context.anim = _anim;
            // Add whatever else you need here...

            return context;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        // Start is called before the first frame update
        public void Init(
            Transform _playerTr, 
            GameObject _gatlingHolderGo, 
            GameObject _gatlingHeadGo, 
            Transform _gunMuzzleTr, 
            BossAnimationController _anim, 
            BossCollider _bossCollider, 
            BossShieldGeneratorSpawnPointHolder _shieldGeneratorHolder,
            GameObject _giantHomingMissileGo,
            Transform _giantHomingMissileSpawnTr,
            GroupHomingMissileSpawnPos[] _arrGroupHomingMissileSpawnPos,
            CannonRainMemoryPool _cannonRainMemoryPool,
            CannonMemoryPool _cannonMemoryPool,
            GatlinMemoryPool _gatlinMemoryPool,
            GroupMissileMemoryPool _groupMissileMemoryPool)
        {
            context = CreateBehaviourTreeContext(_playerTr, _gatlingHolderGo, _gatlingHeadGo, _gunMuzzleTr, _anim, _bossCollider, _secondWeakPointHolder, _giantHomingMissileGo, _giantHomingMissileSpawnTr, _arrGroupHomingMissileSpawnPos, _cannonRainMemoryPool, _cannonMemoryPool, _gatlinMemoryPool,_groupMissileMemoryPool);
            tree = tree.Clone();
            tree.Bind(context);
            tree.blackboard.isPhaseEnd = false;
            tree.blackboard.curPhaseNum = 1;
        }

        public void FinishCurrentPhase()
        {
            tree.blackboard.isPhaseEnd = true;
        }

        public void StartNextPhase(int _newPhaseNum)
        {
            tree.blackboard.curPhaseNum = _newPhaseNum;
            tree.blackboard.isPhaseEnd = false;
        }

        // Update is called once per frame
        public void RunnerUpdate()
        {
            if (tree)
                tree.Update();
        }

        Context CreateBehaviourTreeContext(
            Transform _playerTr, 
            GameObject _gatlingHolderGo, 
            GameObject _gatlingHeadGo, 
            Transform _gunMuzzleTr, 
            BossAnimationController _anim, 
            BossCollider _bossCollider, 
            BossShieldGeneratorSpawnPointHolder _shieldGeneratorHolder,
            GameObject _giantHomingMissileGo,
            Transform _giantHomingMissileSpawnTr,
            GroupHomingMissileSpawnPos[] _arrGroupHomingMissileSpawnPos,
            CannonRainMemoryPool _cannonRainMemoryPool,
            CannonMemoryPool _cannonMemoryPool,
            GatlinMemoryPool _gatlinMemoryPool,
            GroupMissileMemoryPool _groupMissileMemoryPool)
        {
            return Context.CreateFromGameObject(gameObject, _playerTr, _gatlingHolderGo, _gatlingHeadGo, _gunMuzzleTr, _anim, _bossCollider, _secondWeakPointHolder, _giantHomingMissileGo, _giantHomingMissileSpawnTr, _arrGroupHomingMissileSpawnPos, _cannonRainMemoryPool, _cannonMemoryPool, _gatlinMemoryPool, _groupMissileMemoryPool);
        }

        private void OnDrawGizmosSelected()
        {
            if (!tree) return;

            BehaviourTree.Traverse(tree.rootNode, (n) =>
            {
                if (n.drawGizmos)
                {
                    n.OnDrawGizmos();
                }
            });
        }


        // The main behaviour tree asset
        public BehaviourTree tree;

        // Storage container object to hold game object subsystems
        Context context;
    }
}
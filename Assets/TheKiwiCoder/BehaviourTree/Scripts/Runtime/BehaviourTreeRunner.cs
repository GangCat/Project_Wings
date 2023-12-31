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
            BossShieldGeneratorSpawnPointHolder _secondWeakPointHolder,
            GameObject _giantHomingMissileGo,
            Transform _giantHomingMissileSpawnTr,
            GroupHomingMissileSpawnPos[] _arrGroupHomingMissileSpawnPos)
        {
            context = CreateBehaviourTreeContext(_playerTr, _gatlingHolderGo, _gatlingHeadGo, _gunMuzzleTr, _anim, _bossCollider, _secondWeakPointHolder, _giantHomingMissileGo, _giantHomingMissileSpawnTr, _arrGroupHomingMissileSpawnPos);
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
            BossShieldGeneratorSpawnPointHolder _secondWeakPointHolder,
            GameObject _giantHomingMissileGo,
            Transform _giantHomingMissileSpawnTr,
            GroupHomingMissileSpawnPos[] _arrGroupHomingMissileSpawnPos)
        {
            return Context.CreateFromGameObject(gameObject, _playerTr, _gatlingHolderGo, _gatlingHeadGo, _gunMuzzleTr, _anim, _bossCollider, _secondWeakPointHolder, _giantHomingMissileGo, _giantHomingMissileSpawnTr, _arrGroupHomingMissileSpawnPos);
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
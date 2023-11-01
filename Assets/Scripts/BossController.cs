using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
//using Cysharp.Threading.Tasks;

public class BossController : MonoBehaviour
{
    public void Init(BehaviourTree _tree, Transform _playerTr)
    {
        myRunner = new MyBehaviourTreeRunner(gameObject, _tree, _playerTr);
        StartCoroutine("UpdateCoroutine");
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            myRunner.Update();
            yield return null;
        }
    }

    private MyBehaviourTreeRunner myRunner = null;
}

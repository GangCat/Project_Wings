using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class PhasePatternSelectorNode : CompositeNode
{
    private int curPhaseNum = 0;

    protected override void OnStart() {
        curPhaseNum = blackboard.curPhaseNum;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        children[curPhaseNum - 1].Update();
        return State.Running;
    }


}

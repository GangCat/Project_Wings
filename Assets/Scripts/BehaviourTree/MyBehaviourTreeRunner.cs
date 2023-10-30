using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace TheKiwiCoder
{
    public class MyBehaviourTreeRunner
    {
        // Start is called before the first frame update
        public MyBehaviourTreeRunner(GameObject _go, BehaviourTree _tree)
        {
            tree = _tree;
            context = CreateBehaviourTreeContext(_go);
            tree = tree.Clone();
            tree.Bind(context);
        }

        // Update is called once per frame
        public void Update()
        {
            if (tree)
                tree.Update();
        }

        Context CreateBehaviourTreeContext(GameObject _go)
        {
            return Context.CreateFromGameObject(_go);
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
        private BehaviourTree tree;

        // Storage container object to hold game object subsystems
        Context context;
    }
}
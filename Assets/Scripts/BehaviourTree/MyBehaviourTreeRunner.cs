using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace TheKiwiCoder
{
    public class MyBehaviourTreeRunner
    {
        // Start is called before the first frame update
        public MyBehaviourTreeRunner(GameObject _go, BehaviourTree _tree, Transform _playerTr)
        {
            tree = _tree;
            context = CreateBehaviourTreeContext(_go, _playerTr);
            tree = tree.Clone();
            tree.Bind(context);
        }

        // Update is called once per frame
        public void Update()
        {
            if (tree)
                tree.Update();
        }

        Context CreateBehaviourTreeContext(GameObject _go, Transform _playerTr)
        {
            return Context.CreateFromGameObject(_go, _playerTr);
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
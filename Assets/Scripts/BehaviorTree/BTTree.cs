using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTTree : MonoBehaviour
    {
        private BTNode root = null;

        protected void Start()
        {
            root = SetupTree();
        }

        private void Update()
        {
            if(root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract BTNode SetupTree();
    }
}


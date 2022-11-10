using System;

namespace GreenBehaviors.LeafLambda {
    /// <summary>
    ///     A simple class that allows easy definition of a leaf node through delegates.
    /// </summary>
    public class Action : LambdaLeafNode {
        private readonly Func<Action, NodeState> _tickDelegate;

        public Action(
                string name,
                Func<Action, NodeState> tick,
                Action<LambdaLeafNode> start = null,
                Action<LambdaLeafNode, NodeState> finish = null,
                Action<LambdaLeafNode> cancel = null,
                Action<LambdaLeafNode> reset = null):
            base(name, start, finish, cancel, reset) {
                _tickDelegate = tick;
            }

        public override NodeState Tick() => _tickDelegate(this);
    }
}
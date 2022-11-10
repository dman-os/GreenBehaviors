using System;

namespace GreenBehaviors.LeafLambda
{
    /// <summary>
    ///     A variant of the <see cref="Action" /> node that takes a predicate as its ticking delegate.
    /// </summary>
    public class Conditional : LambdaLeafNode
    {
        private readonly Func<Conditional, bool> _conditionalDelegate;

        public Conditional(
            string name,
            Func<Conditional, bool> conditionalDelegate,
            Action<LambdaLeafNode> startDelegate = null,
            Action<LambdaLeafNode, NodeState> finishDelegate = null,
            Action<LambdaLeafNode> cancelDelegate = null,
            Action<LambdaLeafNode> resetDelegate = null) :
            base(name, startDelegate, finishDelegate, cancelDelegate, resetDelegate)
        {
            _conditionalDelegate = conditionalDelegate;
        }

        public override NodeState Tick() => _conditionalDelegate(this) ? NodeState.Success : NodeState.Failure;
    }
}
using Godot;

namespace GreenBehaviors.LeafFuncRef
{
    /// <summary>
    ///     A variant of the <see cref="Action" /> node that takes a predicate as its ticking delegate.
    /// </summary>
    public class Conditional : FuncRefLeafNode
    {
        private readonly FuncRef _conditionalDelegate;

        public Conditional(
            string name,
            FuncRef conditionalDelegate,
            FuncRef startDelegate = null,
            FuncRef finishDelegate = null,
            FuncRef cancelDelegate = null,
            FuncRef resetDelegate = null) :
            base(name, startDelegate, finishDelegate, cancelDelegate, resetDelegate)
        {
            _conditionalDelegate = conditionalDelegate;
        }

        public override NodeState Tick() =>
            (bool) _conditionalDelegate.CallFunc(this) ? NodeState.Success : NodeState.Failure;
    }
}
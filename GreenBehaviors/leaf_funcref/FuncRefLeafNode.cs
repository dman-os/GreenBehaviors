using Godot;

namespace GreenBehaviors.LeafFuncRef
{
    /// <summary>
    ///     A base class for the leaf behavior tree nodes i.e. nodes that contain
    ///     the actual behavior that make use of funcrefs.
    ///     <seealso cref="LambdaLeafNode" />
    /// </summary>
    public abstract class FuncRefLeafNode : Node
    {
        private class EmptyNodeFuncRef : Reference
        {
            public NodeState Fail() => NodeState.Failure;
            public NodeState Succeed() => NodeState.Success;
        }

        private static readonly EmptyNodeFuncRef emptyNodeInstance = new EmptyNodeFuncRef();

        /// <summary>
        ///     A node that automatically fails.
        /// </summary>
        public static Action EmptyFailureNode { get; } = new Action(
            "Empty Failure Node",
            GD.FuncRef(emptyNodeInstance, "Fail")
        );

        /// <summary>
        ///     A node that automatically succeeds.
        /// </summary>
        public static Action EmptySuccessNode { get; } = new Action(
            "Empty Success Node",
            GD.FuncRef(emptyNodeInstance, "Succeed")
        );

        private readonly FuncRef _startDelegate;
        private readonly FuncRef _finishDelegate;
        private readonly FuncRef _cancelDelegate;
        private readonly FuncRef _resetDelegate;

        protected FuncRefLeafNode(
            string name,
            FuncRef startDelegate = null,
            FuncRef finishDelegate = null,
            FuncRef cancelDelegate = null,
            FuncRef resetDelegate = null) :
            base(name)
        {
            _startDelegate = startDelegate;
            _finishDelegate = finishDelegate;
            _cancelDelegate = cancelDelegate;
            _resetDelegate = resetDelegate;
        }

        public abstract override NodeState Tick();

        public override void Start()
        {
            //if (this.State == NodeState.CANCELLED ||
            //    this.State == NodeState.FRESH)
            _startDelegate?.CallFunc(this);
            base.Start();
        }

        public override void Finish(NodeState state)
        {
            _finishDelegate?.CallFunc(this, state);
            base.Finish(state);
        }

        public override void Cancel()
        {
            _cancelDelegate?.CallFunc(this);
            base.Cancel();
        }

        public override void Reset()
        {
            _resetDelegate?.CallFunc(this);
            base.Reset();
        }
    }
}
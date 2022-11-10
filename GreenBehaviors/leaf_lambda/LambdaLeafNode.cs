using System;

namespace GreenBehaviors.LeafLambda {
    /// <summary>
    ///     A base class for the leaf behavior tree nodes i.e. nodes that contain
    ///     the actual behavior that make use of delegates.
    /// </summary>
    public abstract class LambdaLeafNode : Node {
        /// <summary>
        ///     A node that automatically fails.
        /// </summary>
        public static Action EmptyFailureNode { get; } = new Action(
            "Empty Failure Node",
            _ => NodeState.Failure
        );

        /// <summary>
        ///     A node that automatically succeeds.
        /// </summary>
        public static Action EmptySuccessNode { get; } = new Action(
            "Empty Success Node",
            _ => NodeState.Success
        );

        /// <summary>
        ///     A node that's forever running.
        /// </summary>
        public static Action EmptyRunningNode { get; } = new Action(
            "Empty Running Node",
            _ => NodeState.Running
        );

        private readonly Action<LambdaLeafNode> _startDelegate;
        private readonly Action<LambdaLeafNode, NodeState> _finishDelegate;
        private readonly Action<LambdaLeafNode> _cancelDelegate;
        private readonly Action<LambdaLeafNode> _resetDelegate;

        protected LambdaLeafNode(
                string name,
                Action<LambdaLeafNode> startDelegate = null,
                Action<LambdaLeafNode, NodeState> finishDelegate = null,
                Action<LambdaLeafNode> cancelDelegate = null,
                Action<LambdaLeafNode> resetDelegate = null):
            base(name) {
                _startDelegate = startDelegate != null ? startDelegate : _ => { };
                _finishDelegate = finishDelegate != null ? finishDelegate : (_, DISCARD) => { };
                _cancelDelegate = cancelDelegate != null ? cancelDelegate : _ => { };
                _resetDelegate = resetDelegate != null ? resetDelegate : _ => { };
            }

        public abstract override NodeState Tick();

        public override void Start() {
            //if (this.State == NodeState.CANCELLED ||
            //    this.State == NodeState.FRESH)
            _startDelegate(this);
            base.Start();
        }

        public override void Finish(NodeState state) {
            _finishDelegate(this, state);
            base.Finish(state);
        }

        public override void Cancel() {
            _cancelDelegate(this);
            base.Cancel();
        }

        public override void Reset() {
            _resetDelegate(this);
            base.Reset();
        }
    }
}
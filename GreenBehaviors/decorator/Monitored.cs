using System;
using GreenBehaviors.LeafLambda;
using Action = GreenBehaviors.LeafLambda.Action;

namespace GreenBehaviors.Decorator {
    /// <summary>
    ///     A decorator node that takes a guard node and a charge node. The charge will be ticked only
    ///     if the guard succeeds.
    /// </summary>
    public class Monitored : DecoratorNode {
        protected Node _guardNode;

        public Monitored(
            string name,
            Node guardNode = null,
            Node chargeNode = null) : base(name, chargeNode) {
            _guardNode = guardNode != null ? guardNode : LambdaLeafNode.EmptySuccessNode;
        }

        /// <param name="guardName">Name of the guard node.</param>
        /// <param name="guardConditional">
        ///     Predicate to be used for a <see cref="Conditional" /> node
        ///     that'll be set as the guard node.
        /// </param>
        public Monitored(
            string name,
            string guardName,
            Func<Conditional, bool> guardConditional,
            Node charge = null
        ) : base(name, charge) {
            _guardNode = new Conditional(guardName, guardConditional);
        }

        public Monitored(
            string name,
            string guardName,
            Func<Conditional, bool> guardConditional,
            string chargeName,
            Func<LeafLambda.Action, NodeState> chargeTick
        ) : this(name, guardName, guardConditional) {
            _childNode = new LeafLambda.Action(chargeName, chargeTick);
        }

        public DecoratorNode SetGuard(Node node) {
            if (!IsFresh)
                throw new InvalidOperationException("Node is not fresh. Can only change child when fresh");
            _guardNode = node;
            return this;
        }

        public DecoratorNode SetGuard(string name, Func<LeafLambda.Action, NodeState> func) {
            if (!IsFresh)
                throw new InvalidOperationException("Node is not fresh. Can only change child when fresh");
            _guardNode = new LeafLambda.Action(name, func);
            return this;
        }

        public DecoratorNode SetGuard(string name, Func<Conditional, bool> conditional) {
            if (!IsFresh)
                throw new InvalidOperationException("Node is not fresh. Can only change child when fresh");
            _guardNode = new Conditional(name, conditional);
            return this;
        }

        public override NodeState Tick() {
            var guardState = _guardNode.FullTick();
            switch (guardState) {
                case NodeState.Success:
                    return _childNode.FullTick();
                case NodeState.Failure:
                    _childNode.Cancel();
                    return NodeState.Failure;
                default:
                    return guardState;
            }
        }

        public override void Cancel() {
            _guardNode.Cancel();
            _childNode.Cancel();
            base.Cancel();
        }

        public override void Reset() {
            _guardNode.Reset();
            _childNode.Reset();
            base.Reset();
        }

        public override void Finish(NodeState state) {
            // TODO should we paremeterize child node cancelling?
            if (_guardNode.IsRunning) _guardNode.Cancel();
            if (_childNode.IsRunning) _childNode.Cancel();
            base.Finish(state);
        }
    }
}
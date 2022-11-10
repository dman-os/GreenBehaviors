using System;
using GreenBehaviors.LeafLambda;
using Action = GreenBehaviors.LeafLambda.Action;

namespace GreenBehaviors.Decorator
{
    /// <summary>
    ///     A base class that contains common functionality for nodes that parents
    ///     another single node.
    /// </summary>
    public abstract class DecoratorNode : Node
    {
        protected Node _childNode;

        protected DecoratorNode(string name, Node childNode = null) : base(name)
        {
            _childNode = childNode != null ? childNode : LambdaLeafNode.EmptySuccessNode;
        }

        /// <summary>
        ///     Sets the given node as the child of this decorator.
        /// </summary>
        /// <param name="node">The node to be added.</param>
        /// <returns>This node i.e. the node on which this method was called.</returns>
        /// <exception cref="InvalidOperationException">Child can only be set when decorator is fresh.</exception>
        public DecoratorNode SetChild(Node node)
        {
            if (!IsFresh)
                throw new InvalidOperationException("Node is not fresh. Can only change child when fresh");
            _childNode = node;
            return this;
        }

        /// <summary>
        ///     Creates a new <see cref="GreenBehaviors.Action" /> node based on the given
        ///     arguments and sets it as the child of this node.
        /// </summary>
        /// <seealso cref="SetChild(Node)" />
        public DecoratorNode SetChild(string name, Func<LeafLambda.Action, NodeState> func)
        {
            if (!IsFresh)
                throw new InvalidOperationException("Node is not fresh. Can only change child when fresh");
            _childNode = new LeafLambda.Action(name, func);
            return this;
        }

        public override void Cancel()
        {
            _childNode.Cancel();
            base.Cancel();
        }

        public override void Reset()
        {
            _childNode.Reset();
            base.Reset();
        }
    }
}
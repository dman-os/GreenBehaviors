using System;
using System.Collections.Generic;
using Action = GreenBehaviors.LeafLambda.Action;

namespace GreenBehaviors.Composite {
	/// <summary>
	///     A base class that contains common functionality for nodes that parent
	///     a number of other nodes.
	/// </summary>
	public abstract class CompositeNode : Node {
		protected LinkedList<Node> _childrenNodes;

		protected CompositeNode(string name, params Node[] childrenNodes) : base(name) {
			_childrenNodes = new LinkedList<Node>(childrenNodes);
		}

		protected LinkedListNode<Node> _runningNode;

		/// <summary>
		///     Add children to the composite node. Only allowed when this node is still fresh
		/// </summary>
		/// <param name="newChildren">The nodes to be added</param>
		/// <returns>This node i.e. the node this method was called on. For fluent api considerations.</returns>
		/// <exception cref="InvalidOperationException">
		///     Thrown when attempting to add
		///     to a non fresh node.
		/// </exception>
		public CompositeNode AddChild(params Node[] newChildren) {
			if (!IsFresh)
				throw new InvalidOperationException("Node is not fresh. Can only add child when fresh");
			foreach (var node in newChildren) _childrenNodes.AddLast(node);

			return this;
		}

		/// <summary>
		///     Creates a new <see cref="GreenBehaviors.Action" /> node and adds it to the
		///     this node.
		/// </summary>
		/// <param name="name">Name of the new node</param>
		/// <param name="tick">Tick delegate of the new node</param>
		/// <see cref="AddChild(Node[])" />
		public CompositeNode AddChild(
			string name,
			Func<LeafLambda.Action, NodeState> tick,
			Action<LeafLambda.LambdaLeafNode> start = null,
			Action<LeafLambda.LambdaLeafNode, NodeState> finish = null,
			Action<LeafLambda.LambdaLeafNode> cancel = null,
			Action<LeafLambda.LambdaLeafNode> reset = null
		) {
			if (!IsFresh)
				throw new InvalidOperationException("Node is not fresh. Can only add child when fresh");
			_childrenNodes.AddLast(new LeafLambda.Action(name, tick, start, finish, cancel, reset));
			return this;
		}

		public override void Start() {
			_runningNode = _childrenNodes.First;
			base.Start();
		}

		public override void Cancel() {
			if (_runningNode != null) // !IsFresh
				_runningNode.Value.Cancel();
			// _runningNode = _childrenNodes.First;

			base.Cancel();
		}

		public override void Reset() {
			foreach (var child in _childrenNodes) child.Reset();

			_runningNode = null;
			base.Reset();
		}
	}
}
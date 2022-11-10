using System;
using GreenBehaviors.LeafLambda;
using Action = GreenBehaviors.LeafLambda.Action;

namespace GreenBehaviors.Composite
{
	/// <summary>
	///     A simple variant of <see cref="Sequence" /> with utility methods to allow adding children
	///     in the beginning of the children list.
	/// </summary>
	public class Filter : Sequence
	{
		public Filter(string name) : base(name)
		{
		}

		public CompositeNode AddCondition(params Node[] newChildren)
		{
			if (!IsFresh)
				throw new InvalidOperationException("Node is not fresh. Can only add child when fresh");
			foreach (var node in newChildren) _childrenNodes.AddFirst(node);

			return this;
		}

		public CompositeNode AddCondition(string name, System.Func<LeafLambda.Action, NodeState> func)
		{
			if (!IsFresh)
				throw new InvalidOperationException("Node is not fresh. Can only add child when fresh");
			_childrenNodes.AddFirst(new LeafLambda.Action(name, func));
			return this;
		}

		public CompositeNode AddCondition(string name, Func<Conditional, bool> func)
		{
			if (!IsFresh)
				throw new InvalidOperationException("Node is not fresh. Can only add child when fresh");
			_childrenNodes.AddFirst(new Conditional(name, func));
			return this;
		}
	}
}

using System.Collections.Generic;

namespace GreenBehaviors.Composite {
	/// <summary>
	///     A variant of the <see cref="Selector" /> that starts its ticking from the first node
	///     each time it's ticked, whether there was a previously running node or not.
	/// </summary>
	/// <para>
	///     When being ticked, if there was a previously running node and if a new node different than this
	///     previously running node returns <see cref="NodeState.Running" /> this time around, it cancels
	///     the previously running node. LAS.
	/// </para>
	public class PrioritizedSelector : Selector {
		public PrioritizedSelector(string name, params Node[] children) : base(name, children) { }

		protected LinkedListNode<Node> _previouslyRunningNode;

		public override NodeState Tick() {
			_previouslyRunningNode = _runningNode;

			// start the selection from the beginning
			_runningNode = _childrenNodes.First;
			var state = base.Tick();

			if (_previouslyRunningNode != null) {
				if (_previouslyRunningNode != _runningNode)
					_previouslyRunningNode.Value.Cancel();
			}

			return state;
		}
	}
}
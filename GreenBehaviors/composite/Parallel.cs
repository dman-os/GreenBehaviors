using System;
using System.Collections.Generic;

namespace GreenBehaviors.Composite
{
	/// <summary>
	///     A composite node class that runs its children in "parallel". Not with multiple threads, mind you,
	///     it simply runs all it's nodes until it's succeeding policy is met.
	/// </summary>
	/// <remarks>
	///     All nodes that return <see cref="NodeState.Running" /> are collected and ticked the next time around.
	///     I.e. nodes that have ended (<see cref="Node.HasEnded" />) will not be re-run.
	/// </remarks>
	public class Parallel : CompositeNode
	{
		/// <summary>
		///     The succeeding behaviors of a parallel composite node.
		/// </summary>
		public enum Policy
		{
			/// <summary>
			///     Parallel node succeeds if all of it's children succeed.
			/// </summary>
			/// <see cref="Sequence" />
			Sequence,

			/// <summary>
			///     Parallel node succeeds if one of it's children succeed.
			/// </summary>
			/// <see cref="Selector" />
			Selector
		}
		//public enum Orchestrator { Resume, Join }

		/// <summary>
		///     The <see cref="Policy" /> of this parallel node.
		/// </summary>
		public Policy InstancePolicy { get; }

		// public Orchestrator InstanceOrchestrator { get; set; }
		private readonly NodeState _returnState;
		private readonly NodeState _finishState;

		private readonly Func<NodeState> _tickDelegate;

		public Parallel(
			string name,
			Policy policy = Policy.Sequence,
			//Orchestrator orchestrator = Orchestrator.Resume,
			params Node[] nodes) : base(name, nodes)
		{
			if ((InstancePolicy = policy) == Policy.Sequence)
			{
				_returnState = NodeState.Failure;
				_finishState = NodeState.Success;
			}
			else
			{
				_returnState = NodeState.Success;
				_finishState = NodeState.Failure;
			}

			/* if ((InstanceOrchestrator = orchestrator) == Orchestrator.Resume)
				_tickDelegate = ResumeTick;
			else */
			_tickDelegate = JoinTick;
		}

		public override NodeState Tick() => _tickDelegate();

		/* public NodeState ResumeTick() {
			var runningNode = _childrenNodes.First;
			var runningNodesLeft = false;

			while (runningNode != null) {

				var state = runningNode.Value.FullTick();

				if (state == _returnState)
					return _returnState;

				if (state == NodeState.RUNNING) {

					runningNodesLeft = true;
					runningNode = runningNode.Next;

				} else if (state == _finishState)
					runningNode = runningNode.Next;

				else
					throw TickException;
			}

			return runningNodesLeft? NodeState.RUNNING : _returnState;
		} */

		protected LinkedList<Node> _runningChildrenNodes = new LinkedList<Node>();

		public NodeState JoinTick()
		{
			var runningNode = _runningChildrenNodes.Count > 0 ? _runningChildrenNodes.First : _childrenNodes.First;

			// new list of running nodes
			var runningNodes = new LinkedList<Node>();

			while (runningNode != null)
			{
				var state = runningNode.Value.FullTick();

				if (state == _returnState)
					return _returnState;
				else if (state == NodeState.Running)
					runningNodes.AddFirst(runningNode);
				else
					throw TickException;
				runningNode = _runningNode.Next;
			}

			_runningChildrenNodes = runningNodes;
			return runningNodes.Count > 0 ? NodeState.Running : _finishState;
		}

		public override void Finish(NodeState state)
		{
			foreach (var node in _childrenNodes)
            {
                if (node.IsRunning)
					node.Cancel();
            }

            _runningChildrenNodes = new LinkedList<Node>();
			base.Finish(state);
		}

		public override void Cancel()
		{
			foreach (var node in _childrenNodes)
            {
                if (node.IsRunning)
					node.Cancel();
            }

            _runningChildrenNodes = new LinkedList<Node>();
			_runningNode = null;
			base.Cancel();
		}
	}
}

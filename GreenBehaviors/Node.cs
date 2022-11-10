using System;

namespace GreenBehaviors
{
	/// <summary>
	///     A base class for all types of nodes that can exist in a
	///     Behavior Tree.
	/// </summary>
	[Serializable]
	public abstract class Node
	{
		public static InvalidProgramException TickException = new InvalidProgramException(
			"All tick methods should return either SUCCESS, FAILURE or RUNNING");

		protected Node(string name)
		{
			Name = name;
		}

		/// <summary>
		///     Name of the node.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     State of the node.
		/// </summary>
		public NodeState State { get; set; }

		/// <summary>
		///     Check if the node state is <see cref="NodeState.Fresh" />
		/// </summary>
		public bool IsFresh => State == NodeState.Fresh;

		/// <summary>
		///     Check if the node state is <see cref="NodeState.Running" />
		/// </summary>
		public bool IsRunning => State == NodeState.Running;

		/// <summary>
		///     Check if the node state is <see cref="NodeState.Success" />
		/// </summary>
		public bool HasSuceeded => State == NodeState.Success;

		/// <summary>
		///     Check if the node state is <see cref="NodeState.Failure" />
		/// </summary>
		public bool HasFailed => State == NodeState.Failure;

		/// <summary>
		///     Check if the node state is either <see cref="NodeState.Success" />
		///     or <see cref="NodeState.Failure" />
		/// </summary>
		public bool HasEnded => State == NodeState.Failure || State == NodeState.Success;

		/// <summary>
		///     Check if the node state is <see cref="NodeState.Cancelled" />
		/// </summary>
		public bool WasCancelled => State == NodeState.Cancelled;

		/// <summary>
		///     Run the behavior in a node.
		/// </summary>
		public abstract NodeState Tick();

		/// <summary>
		///     Call <see cref="Start" />, <see cref="Tick" /> and <see cref="Finish" />
		///     in succession.
		/// </summary>
		/// <remarks>
		///     <para>
		///         Doesn't call <see cref="Finish" /> if the node is in the <see cref="NodeState.Running" />
		///         state after being ticked.
		///     </para>
		///     <para>Doesn't call <see cref="Start" /> if the node is already <see cref="NodeState.Running" /></para>
		/// </remarks>
		/// <returns>The state that the node at the end of this method.</returns>
		public NodeState FullTick()
		{
			if (!IsRunning) Start();
			var status = Tick();
			if (status != NodeState.Running) Finish(status);
			else State = NodeState.Running;
			return status;
		}

		/// <summary>
		///     Starts a node i.e. readies it to be ticked. Set's the node in the <see cref="NodeState.Running" /> state and nodes
		///     that need prepare anything to be <see cref="Tick" />ed start doing so.
		/// </summary>
		public virtual void Start()
		{
			State = NodeState.Running;
		}

		/// <summary>
		///     Cancels a node. Set's the node in the <see cref="NodeState.Cancelled" /> state and nodes
		///     that had any machinations running stop these machinations.
		/// </summary>
		public virtual void Cancel()
		{
			State = NodeState.Cancelled;
		}

		/// <summary>
		///     Resets a node. Set's the node in the <see cref="NodeState.Fresh" /> state and nodes
		///     that have any state that need to be cleaned out for reuse, clean out so.
		/// </summary>
		public virtual void Reset()
		{
			State = NodeState.Fresh;
		}

		/// <summary>
		///     Signifies the end of the node's run.
		/// </summary>
		/// <param name="state">The state to set the node in.</param>
		public virtual void Finish(NodeState state)
		{
			State = state;
		}
	}
}

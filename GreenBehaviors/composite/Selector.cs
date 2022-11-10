namespace GreenBehaviors.Composite
{
	/// <summary>
	///     A composite nodes that ticks all its children in turn until one of
	///     them succeeds. Returns <see cref="NodeState.Failure" /> if none succeeds.
	/// </summary>
	/// <remarks>
	///     If one of them reports <see cref="NodeState.Running" />, it returns <see cref="NodeState.Running" />
	///     and continues from this running node the next time its ticked.
	/// </remarks>
	public class Selector : CompositeNode
	{
		public Selector(string name, params Node[] nodes) : base(name, nodes)
		{
		}

		public override NodeState Tick()
		{
			while (_runningNode != null)
            {
                switch (_runningNode.Value.FullTick())
                {
                    case NodeState.Success:
                        return NodeState.Success;
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Failure:
                        _runningNode = _runningNode.Next;
                        break;
                    default:
                        throw TickException;
                }
            }

            return NodeState.Failure;
        }
    }
}

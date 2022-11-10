namespace GreenBehaviors.Composite {
    /// <summary>
    ///     A composite nodes that ticks all its children in turn until one of
    ///     them fails. Returns <see cref="NodeState.Success" /> if none fail.
    /// </summary>
    /// <remarks>
    ///     If one of them reports <see cref="NodeState.Running" />, it returns <see cref="NodeState.Running" />
    ///     and continues from this running node the next time its ticked.
    /// </remarks>
    public class Sequence : CompositeNode {
        public Sequence(string name, params Node[] nodes) : base(name, nodes) { }

        public override NodeState Tick() {
            while (_runningNode != null) {
                switch (_runningNode.Value.FullTick()) {
                    case NodeState.Failure:
                        return NodeState.Failure;
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Success:
                        _runningNode = _runningNode.Next;
                        break;
                    default:
                        throw TickException;
                }
            }

            return NodeState.Success;
        }
    }
}
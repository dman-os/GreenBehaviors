namespace GreenBehaviors.Decorator
{
    /// <summary>
    ///     A node that ticks its child and returns <see cref="NodeState.Failure" /> if
    ///     child returns <see cref="NodeState.Success" /> and vice-versa.
    /// </summary>
    public class Inverter : DecoratorNode
    {
        public Inverter(string name, Node childNode = null) : base(name, childNode)
        {
        }

        public override NodeState Tick()
        {
            switch (_childNode.FullTick())
            {
                case NodeState.Success:
                    return NodeState.Failure;
                case NodeState.Failure:
                    return NodeState.Failure;
                case NodeState.Running:
                    return NodeState.Running;
                default:
                    throw TickException;
            }
        }
    }
}
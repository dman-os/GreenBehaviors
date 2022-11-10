namespace GreenBehaviors.Decorator
{
    /// <summary>
    ///     A node repeatedly ticks it's child according to it's <see cref="BreakPolicy" /> and <see cref="Limit" />.
    /// </summary>
    public class Repeater : DecoratorNode
    {
        /// <summary>
        ///     The state of the child at which the repeater stops ticking.
        /// </summary>
        public enum BreakPolicy
        {
            /// <summary>
            ///     Won't stop ticking until the <see cref="Repeater.Limit" />.
            /// </summary>
            NoBreak,

            /// <summary>
            ///     Won't stop ticking until the child succeeds (returning <see cref="NodeState.Success" />) or until
            ///     the limit is reached (returning <see cref="NodeState.Failure" />).
            /// </summary>
            Success,

            /// <summary>
            ///     Won't stop ticking until the child fails (returning <see cref="NodeState.Success" />) or until
            ///     the limit is reached (returning <see cref="NodeState.Failure" />).
            /// </summary>
            Failure
        }

        /// <summary>
        ///     The maximum number of times this node will tick it's child.
        /// </summary>
        /// <remarks>Defaults to <see cref="float.PositiveInfinity" /></remarks>
        public float Limit { get; } = float.PositiveInfinity;

        /// <summary>
        ///     The <see cref="BreakPolicy" /> of this node.
        /// </summary>
        public BreakPolicy InstancePolicy { get; set; }

        private readonly NodeState _exitState;

        public Repeater(string name,
                        BreakPolicy instancePolicy = BreakPolicy.NoBreak,
                        Node childNode = null)
            : base(name, childNode)
        {
            switch (instancePolicy)
            {
                case BreakPolicy.NoBreak:
                    _exitState = NodeState.Fresh; // FIXME
                    break;
                case BreakPolicy.Failure:
                    _exitState = NodeState.Failure;
                    break;
                case BreakPolicy.Success:
                default:
                    _exitState = NodeState.Success;
                    break;
            }
        }

        public Repeater(string name,
                        BreakPolicy instancePolicy,
                        uint limit,
                        Node childNode = null)
            : this(name, instancePolicy, childNode)
        {
            Limit = limit;
        }

        protected uint _tickCounter;

        public override NodeState Tick()
        {
            var status = _childNode.FullTick();
            if (status         == NodeState.Running) return NodeState.Running;
            if (status         == _exitState) return NodeState.Success;
            if (++_tickCounter == Limit) return NodeState.Failure;
            return NodeState.Running;
        }
    }
}
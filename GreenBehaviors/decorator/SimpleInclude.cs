namespace GreenBehaviors.Decorator
{
    /// <summary>
    ///     A node that simply ticks it's child when ticked.
    /// </summary>
    public class SimpleInclude : DecoratorNode
    {
        public SimpleInclude(string name, Node childNode = null) : base(name, childNode)
        {
        }

        public override NodeState Tick() => _childNode.FullTick();

        public override void Start()
        {
            _childNode.Start();
            base.Start();
        }
    }
}
namespace GreenBehaviors.Decorator
{
    /*
     Experiment: Allows sub trees to be ticked somewhere else.
       Necessitates TickException(look at AbstractNode).
    */

    /// <summary>
    ///     A simple <see cref="DecoratorNode" /> that returns the state of it's child when ticked. Does not tick
    ///     it's child.
    /// </summary>
    /// <remarks>Useful if you intend to tick some parts of a tree from somewhere.</remarks>
    public class Checker : SimpleInclude
    {
        public Checker(string name, Node childNode = null) : base(name, childNode)
        {
        }

        public override NodeState Tick() => _childNode.State;
    }
}
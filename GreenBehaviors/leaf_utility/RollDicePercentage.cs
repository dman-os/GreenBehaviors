using System;

namespace GreenBehaviors.LeafUtility
{
    // TODO: accept random number generator from the user
    public class RollDicePercentage : Node
    {
        public float Percentage { get; }

        /* Expects a value between 0.0 - 1.0*/
        public RollDicePercentage(string name, float percentage) : base(name)
        {
            if (percentage > 1 || percentage < 0)
                throw new ArgumentOutOfRangeException(nameof(percentage), "Expects a value between 0.0 - 1.0");

            Percentage = percentage;
        }

        public override NodeState Tick() =>
            new Random().NextDouble() <= Percentage ? NodeState.Success : NodeState.Failure;
    }
}
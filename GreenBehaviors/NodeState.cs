namespace GreenBehaviors
{
    /// <summary>
    ///     Enums that represent the different state a node could be in.
    /// </summary>
    public enum NodeState
    {
        // these travel downtree

        /// <summary>
        ///     A node that is yet to be started. Or possibly, reset.
        /// </summary>
        Fresh,

        /// <summary>
        ///     A node that has been explicity cancelled.
        /// </summary>
        Cancelled,

        // these travel uptree

        /// <summary>
        ///     A node that has succeeded.
        /// </summary>
        Success,

        /// <summary>
        ///     A node that has failed.
        /// </summary>
        Failure,

        /// <summary>
        ///     A node that is still running.
        /// </summary>
        Running
    }
}
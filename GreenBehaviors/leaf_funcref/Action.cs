using Godot;

namespace GreenBehaviors.LeafFuncRef
{
	/// <summary>
	///     A simple class that allows easy definition of a leaf node through delegates.
	/// </summary>
	public class Action : FuncRefLeafNode
    {
        private readonly FuncRef _tickDelegate;

        public Action(
            string name,
            FuncRef tickDelegate,
            FuncRef startDelegate = null,
            FuncRef finishDelegate = null,
            FuncRef cancelDelegate = null,
            FuncRef resetDelegate = null) :
            base(name, startDelegate, finishDelegate, cancelDelegate, resetDelegate)
        {
            _tickDelegate = tickDelegate;
        }

        public override NodeState Tick() => (NodeState) _tickDelegate.CallFunc(this);
    }
}
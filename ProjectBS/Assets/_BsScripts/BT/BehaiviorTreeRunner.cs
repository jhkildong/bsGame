using System;
using System.Collections.Generic;

public interface INode
{
    public enum NodeState
    {
        Running,
        Success,
        Failure,
    }

    public NodeState Evaluate();
}

public sealed class ActionNode : INode
{
    Func<INode.NodeState> _onUpdate = null;

    public ActionNode(Func<INode.NodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.NodeState Evaluate() => _onUpdate?.Invoke() ?? INode.NodeState.Failure;
}

public sealed class SelectorNode : INode
{
    List<INode> _children;

    public SelectorNode(List<INode> childs)
    {
        _children = childs;
    }

    public INode.NodeState Evaluate()
    {
        if (_children == null)
            return INode.NodeState.Failure;

        foreach (var child in _children)
        {
            switch (child.Evaluate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;
                case INode.NodeState.Success:
                    return INode.NodeState.Success;
            }
        }
        return INode.NodeState.Failure;
    }
}

public sealed class SequenceNode : INode
{

    List<INode> _children;

    public SequenceNode(List<INode> childs)
    {
        _children = childs;
    }

    public INode.NodeState Evaluate()
    {
        if (_children == null || _children.Count == 0)
            return INode.NodeState.Failure;

        foreach (var child in _children)
        {
            switch (child.Evaluate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;
                case INode.NodeState.Success:
                    continue;
                case INode.NodeState.Failure:
                    return INode.NodeState.Failure;
            }
        }
        return INode.NodeState.Success;
    }
}

public class BehaviorTreeRunner
{
    INode _rootNode;

    public BehaviorTreeRunner(INode root)
    {
        _rootNode = root;
    }

    public void Operate()
    {
        _rootNode.Evaluate();
    }

}
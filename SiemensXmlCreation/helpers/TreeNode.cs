using CadMacro.Simulator.CommonControls.Helpers;
using SiemensXmlCreation.helpers;
using SiemensXmlCreation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace CadMacro.Simulator.Utilities
{
    [XmlRoot("Tree")]
    public class TreeNode<INode>
    {
        [XmlIgnore]
        public TreeNode<INode> Parent = null;


        [XmlIgnore]
        public string Name => CurrentValue?.ToString();


        public INode CurrentValue { get; set; }


        [XmlElement("ChildNodes")]
        public virtual ObservableCollection<TreeNode<INode>> Children { get; set; } = new ObservableCollection<TreeNode<INode>> { };
        public TreeNode(INode node)
        {
            CurrentValue = node;
        }
        public TreeNode()//for serialization purpose only
        {

        }
        /// <summary>
        /// Add child node to the current node
        /// </summary>
        /// <param name="node"></param>
        public void AddChild(TreeNode<INode> node)
        {
            node.Parent = this;
            Children.Add(node);
        }
        /// <summary>
        /// Add child nodes to the current node
        /// </summary>
        /// <param name="nodes"></param>
        public void AddChild(params TreeNode<INode>[] nodes)
        {
            foreach (var node in nodes)
            {
                node.Parent = this;
                Children.Add(node);
            }
        }
        /// <summary>
        /// Remove specific node from the tree
        /// </summary>
        /// <param name="node"></param>
        public void RemoveChild(TreeNode<INode> node)
        {
            Children.Remove(node);
        }
        /// <summary>
        /// Search specific node from the tree use query
        /// </summary>
        /// <param name="hasNode"></param>
        /// <returns></returns>
        public TreeNode<INode> FindNode(Predicate<INode> hasNode)
        {
            if (CurrentValue != null && hasNode(this.CurrentValue))
                return this;
            else
                return CheckChilds(hasNode);

        }
        /// <summary>
        /// Get specific node from the
        /// </summary>
        /// <param name="searchNode"></param>
        /// <returns></returns>
        public TreeNode<INode> FindNode(INode searchNode)
        {
            if (this.CurrentValue.Equals(searchNode))
                return this;
            else
                return CheckChilds(searchNode);

        }
        /// <summary>
        /// Get nodes of first branch
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TreeNode<INode>> GetFirstBranch()
        {
            TreeNode<INode> node = this;
            yield return node;
            while (true)
            {
                if (node.Children.Count == 0) break;
                node = node.Children[0];
                yield return node;
            }
        }
        /// <summary>
        /// Get branch from collection of nodes
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public IEnumerable<TreeNode<INode>> GetBranchNodes(List<INode> nodes)
        {
            TreeNode<INode> currentNode = this;
            int nodeIndex = 0;
            while (currentNode.Children.Count > 0)
            {
                INode searchNode = nodes[nodeIndex];
                TreeNode<INode> node = currentNode.FindNode(nodes[nodeIndex]);
                if (node is null) yield break;
                yield return node;
                currentNode = node;
                nodeIndex++;
            }
        }
        public override string ToString()
        {
            return CurrentValue?.GetType().Name;
        }
        private TreeNode<INode> CheckChilds(Predicate<INode> hasNode)
        {
            foreach (TreeNode<INode> node in Children)
            {
                TreeNode<INode> result = node.FindNode(hasNode);
                if (result != null)
                    return result;
            }
            return null;
        }
        private TreeNode<INode> CheckChilds(INode searchNode)
        {
            foreach (TreeNode<INode> node in Children)
            {
                TreeNode<INode> result = node.FindNode(searchNode);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
    //[XmlRoot("Tree")]
    //public class TreeNode<INode>:TreeNodeBase
    //{
    //    public INode CurrentValue { get; set; }
    //    public TreeNode(INode node)
    //    {
    //        CurrentValue = node;
    //    }
    //    public TreeNode()//for serialization purpose only
    //    {

    //    }
    //    /// <summary>
    //    /// Add child node to the current node
    //    /// </summary>
    //    /// <param name="node"></param>
    //    public override void AddChild(TreeNodeBase node)
    //    {
    //        node.Parent = this;
    //        Children.Add(node);
    //    }
    //    /// <summary>
    //    /// Add child nodes to the current node
    //    /// </summary>
    //    /// <param name="nodes"></param>
    //    public override void AddChild(params TreeNodeBase[] nodes)
    //    {
    //        foreach (var node in nodes)
    //        {
    //            node.Parent = this;
    //            Children.Add(node);
    //        }
    //    }
    //    /// <summary>
    //    /// Remove specific node from the tree
    //    /// </summary>
    //    /// <param name="node"></param>
    //    public override void RemoveChild(TreeNodeBase node)
    //    {
    //        Children.Remove(node);
    //    }
    //    /// <summary>
    //    /// Search specific node from the tree use query
    //    /// </summary>
    //    /// <param name="hasNode"></param>
    //    /// <returns></returns>
    //    public TreeNode<INode> FindNode(Predicate<INode> hasNode)
    //    {
    //        if (CurrentValue != null && hasNode(this.CurrentValue))
    //            return this;
    //        else
    //            return CheckChilds(hasNode);

    //    }
    //    /// <summary>
    //    /// Get specific node from the
    //    /// </summary>
    //    /// <param name="searchNode"></param>
    //    /// <returns></returns>
    //    public TreeNode<INode> FindNode(INode searchNode)
    //    {
    //        if (this.CurrentValue.Equals(searchNode))
    //            return this;
    //        else
    //            return CheckChilds(searchNode);

    //    }
    //    /// <summary>
    //    /// Get nodes of first branch
    //    /// </summary>
    //    /// <returns></returns>
    //    public IEnumerable<TreeNode<INode>> GetFirstBranch()
    //    {
    //        TreeNode<INode> node = this;
    //        yield return node;
    //        while (true)
    //        {
    //            if (node.Children.Count == 0) break;
    //            node = node.Children[0].CastTo<TreeNode<INode>>();
    //            yield return node;
    //        }
    //    }
    //    /// <summary>
    //    /// Get branch from collection of nodes
    //    /// </summary>
    //    /// <param name="nodes"></param>
    //    /// <returns></returns>
    //    public IEnumerable<TreeNode<INode>> GetBranchNodes(List<INode> nodes)
    //    {
    //        TreeNode<INode> currentNode = this;
    //        int nodeIndex = 0;
    //        while (currentNode.Children.Count > 0)
    //        {
    //            INode searchNode = nodes[nodeIndex];
    //            TreeNode<INode> node = currentNode.FindNode(nodes[nodeIndex]);
    //            if (node is null) yield break;
    //            yield return node;
    //            currentNode = node;
    //            nodeIndex++;
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return CurrentValue.GetType().Name;
    //    }
    //    private TreeNode<INode> CheckChilds(Predicate<INode> hasNode)
    //    {
    //        foreach (TreeNode<INode> node in Children)
    //        {
    //            TreeNode<INode> result = node.FindNode(hasNode);
    //            if (result != null)
    //                return result;
    //        }
    //        return null;
    //    }
    //    private TreeNode<INode> CheckChilds(INode searchNode)
    //    {
    //        foreach (TreeNode<INode> node in Children)
    //        {
    //            TreeNode<INode> result = node.FindNode(searchNode);
    //            if (result != null)
    //                return result;
    //        }
    //        return null;
    //    }
    //}
    //public abstract class TreeNodeBase
    //{
    //    private List<TreeNodeBase> _children = new List<TreeNodeBase>();
    //    [XmlIgnore]
    //    public TreeNodeBase Parent { get; set; }
    //    [XmlElement("ChildNodes")]
    //    public virtual List<TreeNodeBase> Children
    //    {
    //        get => _children;
    //        set => _children = value;
    //    }
    //    public abstract void AddChild(TreeNodeBase treeNodeBase);
    //    public abstract void RemoveChild(TreeNodeBase treeNodeBase);
    //    public abstract void AddChild(params TreeNodeBase[] nodes);
    //}
}

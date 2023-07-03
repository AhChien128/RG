using Newtonsoft.Json;
using System.Collections;

namespace prjRGsystem.Models.ViewModels
{
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        public T Data { get; set; }
        
        public TreeNode<T>? Parent { get; set; }

        [JsonIgnore]
        public ICollection<TreeNode<T>> Children { get; set; }

        [JsonIgnore]
        private ICollection<TreeNode<T>> Parents { get; set; }

        [JsonIgnore]
        public Boolean IsRoot
        {
            get { return Parent == null; }
        }

        [JsonIgnore]
        public Boolean IsLeaf
        {
            get { return Children.Count == 0; }
        }

        [JsonIgnore]
        public int Level
        {
            get
            {
                if (this.IsRoot || Parent == null)
                    return 0;
                return Parent.Level + 1;
            }
        }

        public TreeNode(T data)
        {
            this.Data = data;
            this.Children = new LinkedList<TreeNode<T>>();

            this.ElementsIndex = new LinkedList<TreeNode<T>>();
            this.ElementsIndex.Add(this);

            this.Parents = new LinkedList<TreeNode<T>>();
        }

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
            this.Children.Add(childNode);
            this.RegisterChildForSearch(childNode);

            ICollection<TreeNode<T>> childNodeParents = new LinkedList<TreeNode<T>>();
            if (Parents != null && Parents.Count > 0)
            {
                foreach (TreeNode<T> thisParent in Parents)
                {
                    childNodeParents.Add(thisParent);
                }
            }
            childNodeParents.Add(this);

            childNode.Parents = childNodeParents;
            return childNode;
        }

        public override string? ToString()
        {
            return Data != null ? Data.ToString() : "[data null]";
        }

        private ICollection<TreeNode<T>> ElementsIndex { get; set; }

        private void RegisterChildForSearch(TreeNode<T> node)
        {
            ElementsIndex.Add(node);
            if (Parent != null)
                Parent.RegisterChildForSearch(node);
        }

        public TreeNode<T>? FindTreeNode(Func<TreeNode<T>, bool> predicate)
        {
            return this.ElementsIndex.FirstOrDefault(predicate);
        }

        public TreeNode<T>? FindParentTreeNode(Func<TreeNode<T>, bool> predicate)
        {
            return this.Parents.Reverse().FirstOrDefault(predicate);
        }

        public LinkedList<TreeNode<T>> GetALLChildrenTreeNode()
        {
            LinkedList<TreeNode<T>> childrenTreeNodes = new LinkedList<TreeNode<T>>();
            if (this.Children.Count > 0)
            {
                foreach (var directChild in this.Children)
                {
                    childrenTreeNodes.AddLast(directChild);

                    if (directChild.Children.Count > 0)
                    {
                        LinkedList<TreeNode<T>> treeNodes = directChild.GetALLChildrenTreeNode();
                        if (treeNodes.Count > 0)
                        {
                            foreach (TreeNode<T> node in treeNodes)
                            {
                                childrenTreeNodes.AddLast(node);

                            }
                        }
                    }
                }
            }

            return childrenTreeNodes;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            yield return this;
            foreach (var directChild in this.Children)
            {
                foreach (var anyChild in directChild)
                    yield return anyChild;
            }
        }
    }
}

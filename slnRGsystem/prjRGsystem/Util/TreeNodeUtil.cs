using prjRGsystem.Models;
using prjRGsystem.Models.ViewModels;

namespace prjRGsystem.Util
{
    public class TreeNodeUtil
    {
        public static TreeNode<T>? TransformToTree<T>(List<T> entitys, TreeNode<T>? currentNode = null) where T : ITreeEntity, new()
        {
            if (entitys != null && entitys.Count > 0)
            {

                foreach (T entity in entitys)
                {
                    if (currentNode == null && entity.parentId == 0)
                    {
                        currentNode = new TreeNode<T>(new T());
                        currentNode.AddChild(entity);
                    }
                    else if (currentNode != null && entity.parentId == currentNode.Data.id)
                    {
                        currentNode ??= new TreeNode<T>(new T());
                        currentNode.AddChild(entity);
                    }
                }
                if (currentNode != null && currentNode.Children != null && currentNode.Children.Count > 0)
                {
                    foreach (TreeNode<T> children in currentNode.Children)
                    {
                        TransformToTree<T>(entitys, children);
                    }

                }

            }
            return currentNode;
        }
    }
}

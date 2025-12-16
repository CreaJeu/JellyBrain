using System.Linq;
using Godot;

namespace JellyBrain.Scripts.Utils;

public static class CollectionsAlternative
{
    /**
     * Calls GetChildren on a node but instead of returning an Array of node returns an array of the real type that they have
     * 
     */
    public static T[] GetChildren<T>(Node node) where T : Node
    {
        
        return node.GetChildren()
            .OfType<T>()
            .ToArray();
        // this might break if any node of the children is different from others children
    }
}
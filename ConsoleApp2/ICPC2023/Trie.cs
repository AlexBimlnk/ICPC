using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.ICPC2023;

/// <summary>
/// Простой Бор.
/// </summary>
public sealed class Trie
{
    private Node _root = new Node('$');

    public void Add(string word)
    {
        var currentNode = _root;

        foreach (var key in word)
        {
            if (currentNode.Children.ContainsKey(key))
            {
                currentNode = currentNode.Children[key];
            }
            else
            {
                var child = new Node(key);
                currentNode.Children.Add(key, child);
                child.Parent = currentNode;

                currentNode = child;
            }
        }

        currentNode.IsEnd = true;
    }

    public bool Contains(string word)
    {
        var current = _root;

        foreach (var key in word)
        {
            if (current.Children.ContainsKey(key))
            {
                current = current.Children[key];
            }
            else
            {
                return false;
            }
        }

        return current.IsEnd;
    }

    public void Remove(string word)
    {
        var currentNode = _root;

        foreach (var key in word)
        {
            currentNode = currentNode.Children[key];
        }

        if (currentNode.Children.Any())
        {
            currentNode.IsEnd = false;
        }
        else
        {
            var parent = currentNode.Parent;
            parent.Children.Remove(currentNode.Key);

            while (!parent.Children.Any())
            {
                currentNode = parent;
                parent = currentNode.Parent;

                parent.Children.Remove(currentNode.Key);
            }
        }
    }

    private class Node
    {
        public char Key { get; set; }

        public bool IsEnd { get; set; }

        public Node Parent { get; set; }

        public Dictionary<char, Node> Children { get; } = new();

        public Node(char key)
        {
            Key = key;
        }
    }

    public static class Example
    {
        public static void Start()
        {
            var words2 = new List<string> { "привет", "причал", "мир", "причалить" };

            var trie = new Trie();

            words2.ForEach(trie.Add);

            trie.Contains("мир");
            trie.Contains("прич");
            trie.Contains("причал");

            trie.Remove("причал");

            trie.Contains("причал");
            trie.Contains("причалить");
        }
    }
}

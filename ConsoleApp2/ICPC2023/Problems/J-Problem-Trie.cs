using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.ICPC2023.Problems;
internal class J_Problem_Trie
{
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

        public Node GetStartNode(string word) => _root.Children[word[0]];

        public class Node
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
    }

    private static bool PossiblePos(
        (int I, int J) pos,
        HashSet<(int, int)> pickedPos,
        int[,] answerMap,
        char[,] map,
        char key)
        => !pickedPos.Contains(pos)
           && answerMap[pos.I, pos.J] == 0
           && map[pos.I, pos.J] == key;

    private static bool TryFind(
        (int I, int J) startPos,
        string word,
        int nextPosWord,
        Trie.Node currentNode,
        char[,] map,
        int[,] answerMap,
        HashSet<(int, int)> pickedPos,
        ref int fillValue)
    {
        pickedPos.Add(startPos);
        answerMap[startPos.I, startPos.J] = fillValue;

        if (currentNode.IsEnd)
            return true;

        var nextNode = currentNode.Children[word[nextPosWord]];

        // left
        var leftPos = (startPos.I, J: startPos.J - 1);
        if (leftPos.J >= 0
            && PossiblePos(leftPos, pickedPos, answerMap, map, nextNode.Key))
        {
            if (TryFind(
                leftPos, word, nextPosWord + 1,
                nextNode, map, answerMap, pickedPos, ref fillValue))
            {
                return true;
            }

            pickedPos.Remove(leftPos);
            answerMap[leftPos.I, leftPos.J] = 0;
        }

        // down
        var downPos = (I: startPos.I + 1, startPos.J);
        if (downPos.I < 8
            && PossiblePos(downPos, pickedPos, answerMap, map, nextNode.Key))
        {
            if (TryFind(
                downPos, word, nextPosWord + 1,
                nextNode, map, answerMap, pickedPos, ref fillValue))
            {
                return true;
            }

            pickedPos.Remove(downPos);
            answerMap[downPos.I, downPos.J] = 0;
        }

        // right
        var rightPos = (startPos.I, J: startPos.J + 1);
        if (rightPos.J < 8
            && PossiblePos(rightPos, pickedPos, answerMap, map, nextNode.Key))
        {
            if (TryFind(
                rightPos, word, nextPosWord + 1,
                nextNode, map, answerMap, pickedPos, ref fillValue))
            {
                return true;
            }

            pickedPos.Remove(rightPos);
            answerMap[rightPos.I, rightPos.J] = 0;
        }

        // up
        var upPos = (I: startPos.I - 1, startPos.J);
        if (upPos.I >= 0
            && PossiblePos(upPos, pickedPos, answerMap, map, nextNode.Key))
        {
            if (TryFind(
                upPos, word, nextPosWord + 1,
                nextNode, map, answerMap, pickedPos, ref fillValue))
            {
                return true;
            }

            pickedPos.Remove(upPos);
            answerMap[upPos.I, upPos.J] = 0;
        }

        return false;
    }

    private static void FindWord(
        string word,
        int fillValue,
        char[,] map,
        Trie trie,
        int[,] answerMap)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (answerMap[i, j] == 0 && map[i, j] == word[0])
                {
                    var startPos = (i, j);
                    var startNode = trie.GetStartNode(word);

                    var pickedPos = new HashSet<(int, int)>(16);

                    if (TryFind(startPos, word, 1, startNode, map, answerMap, pickedPos, ref fillValue))
                    {
                        return;
                    }

                    answerMap[i, j] = 0;
                }
            }
        }
    }

    public static void Start()
    {
        var map = new char[8, 8];

        for (int i = 0; i < 8; i++)
        {
            var mapLine = Console.ReadLine()!;
            for (int j = 0; j < 8; j++)
            {
                map[i, j] = mapLine[j];
            }
        }

        var wordsCount = int.Parse(Console.ReadLine()!);
        var words = new Dictionary<int, string>(wordsCount);
        var trie = new Trie();

        for (int i = 0; i < wordsCount; i++)
        {
            var word = Console.ReadLine()!;

            trie.Add(word);
            words.Add(i + 1, word);
        }

        var answerMap = new int[8, 8];

        foreach (var word in words)
        {
            FindWord(word.Value, word.Key, map, trie, answerMap);
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Console.Write($"{answerMap[i, j]} ");
            }
            Console.Write(Environment.NewLine);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Iterator_DesignPattern
{
    using System;
    using System.Collections;

    class Node
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }
        public Node(string s)
        {
            name = s;
        }
    }

    class NodeCollection
    {
        private ArrayList list = new ArrayList();
        private int nodeMax = 0;

        // тут можна додати методи для видалення та редагування елементів        

        public void AddNode(Node n)
        {
            list.Add(n);
            nodeMax++;
        }
        public Node GetNode(int i)
        {
            return ((Node)list[i]);
        }

        public int NodeMax
        {
            get
            {
                return nodeMax;
            }
        }
    }

    /*
     * Логіка перебору елементів покладається на 
     * ітератор.
     */
    abstract class Iterator
    {
        abstract public Node Next();
    }

    class ReverseIterator : Iterator
    {
        private NodeCollection nodeCollection;
        private int currentIndex;

        public ReverseIterator(NodeCollection c)
        {
            nodeCollection = c;
            currentIndex = c.NodeMax - 1; // array index starts at 0!
        }

        override public Node Next()
        {
            if (currentIndex == -1)
                return null;
            else
                return (nodeCollection.GetNode(currentIndex--));
        }
    }

    public class Client
    {
        public static int Main(string[] args)
        {
            NodeCollection c = new NodeCollection();
            c.AddNode(new Node("first"));
            c.AddNode(new Node("second"));
            c.AddNode(new Node("third"));

            ReverseIterator i = new ReverseIterator(c);

            Node n;
            do
            {
                n = i.Next();
                if (n != null)
                    Console.WriteLine("{0}", n.Name);
            } while (n != null);

            Console.ReadKey();
            return 0;
        }
    }
}

/*
Output


third
second
first
*/
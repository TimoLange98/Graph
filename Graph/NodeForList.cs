using System;

namespace Graph
{
    public class Node<T>
    {
        public Node<T> Prev;
        public T Data;
        public Node<T> Next;

        public Node(Node<T> prev, T data, Node<T> next)
        {
            Prev = prev;
            Data = data;
            Next = next;
        }
        public void SwitchReferences()
        {
            var help = Prev;
            Prev = Next;
            Next = help;
        }
    }
}

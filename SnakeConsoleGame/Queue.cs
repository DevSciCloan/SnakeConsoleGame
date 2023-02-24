using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsoleGame
{
    public class Queue<E>
    {
        public E[] Items;
        public int Head = -1;
        private int Tail = -1;
        private int QSize = 0;

        public Queue(int capacity)
        {
            Items = new E[capacity];
        }

        public void Enqueue(E item) 
        {
            if (this.IsFull())
            {
                Console.WriteLine("Queue is full.");
            }
            if (this.IsEmpty())
            {
                Head = 0;
            }
            Tail = (Tail + 1) % Capacity();
            Items[Tail] = item;
            QSize++;
        }

        public int QueueSize() 
        {
            return QSize;
        }

        public E Dequeue() 
        {
            if (this.IsEmpty())
            {
                Console.WriteLine("Queue is empty");
            }
            E item = Items[Head];
            if (QSize > 1)
            {
                Head = (Head + 1) % Capacity();
            }
            else
            {
                Head = -1;
                Tail = -1;
            }
            QSize--;
            return item;
        }

        public bool IsEmpty()
        {
            return QSize == 0;
        }

        public bool IsFull()
        {
            return QSize == Capacity();
        }
        public int Capacity()
        {
            return Items.Length;
        }
    }
}

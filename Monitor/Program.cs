using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Threading.Monitor;
namespace Monitor1
{
    class InterLockedCounter
    {
        int field1;
        int field2;
        public int Field1
        {
            get {
                return field1;
            }
        }
        public int Field2
        {
            get
            {
                return field2;
            }
        }

        public void UpdateFields()
        {
            for(int i=0;i<1000000;i++)
            {

                Interlocked.Increment(ref field1);
                if(field1%2==0)
                {
                    Interlocked.Increment(ref field2);
                }
            }
        }
    }
    class MonitorCounter
    {
        int field1;
        int field2;
        public int Field1
        {
            get
            {
                return field1;
            }
        }
        public int Field2
        {
            get
            {
                return field2;
            }
        }

        public void UpdateFields()
        {
            
            try
            {
                Monitor.Enter(this);
                for (int i = 0; i < 1000000; i++)
                {

                    Interlocked.Increment(ref field1);
                    if (field1 % 2 == 0)
                    {
                        Interlocked.Increment(ref field2);
                    }
                }
            }
            finally
            {
                Monitor.Exit(this);
            }
        }
    }
    class Program
    {
        static void BadAsync()
        {
            Console.WriteLine("синхронизация Interlocked методами");
            InterLockedCounter c = new InterLockedCounter();
            Thread[] threads = new Thread[5];
            for(int i=0;i<threads.Length;i++)
            {
                threads[i] = new Thread(c.UpdateFields);
                threads[i].Start();
            }
        for(int i=0;i<threads.Length;i++)
            {
                threads[i].Join();
            }
            Console.WriteLine($"Fields: {c.Field1},{c.Field2}");
        }
        static void GoodAsync()
        {
            Console.WriteLine("синхронизация Monitor ");
            MonitorCounter m = new MonitorCounter();
            Thread[] threads = new Thread[5];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(m.UpdateFields);
                threads[i].Start();
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
            Console.WriteLine($"Fields: {m.Field1},{m.Field2}");
        }
        static void Main(string[] args)
        {
            BadAsync();
            GoodAsync();
        }
    }
}

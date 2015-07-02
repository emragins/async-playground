using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncPlayground
{
    class Program
    {
        // ****  READ MORE!  ****
        // http://blog.stephencleary.com/2012/02/async-and-await.html
        // http://blog.stephencleary.com/2011/09/async-ctp-why-do-keywords-work-that-way.html

        static void Main(string[] args)
        {
            //TestTaskConstruction();
            //Console.ReadLine();

            TestParallelForEach();
            Console.ReadLine();
        }

        private static async void TestParallelForEach()
        {
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 10     //can tweak this number to see different behavior
            };

            var stuff = Enumerable.Range(0, 10);
            await Task.Run(() =>
            {
                Parallel.ForEach(stuff, options, i =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Acting on: " + i);
                    Thread.Sleep(1000);
                });
            });

            Console.WriteLine("All finished");
        }

        private static void TestTaskConstruction()
        {
            Console.WriteLine(
                "Running Async Test 1... manual Task construction & invocation (hard to find an example online like this)");
            TestAsync1();
            Console.WriteLine("After TestAsync1() called");
            Console.ReadLine();

            Console.WriteLine("Running Async Test 2... via task-factory (more standard coding practice)");
            TestAsync2();
            Console.WriteLine("After TestAsync2() called");
            Console.ReadLine();


            Console.WriteLine("Running Async Test 3... via task-factory with a return value");
            //pull the 'Result' to use it
            int result = TestAsync3().Result;
            Console.WriteLine("After TestAsync3() called.  result = " + result);
        }

        //The 'async' keyword allows for us to use the 'await' keyword inside this method
        static async void TestAsync1()
        {
            //make a task that takes some time...
            Task t = new Task(() =>
            {
                Console.WriteLine("Starting task");
                Thread.Sleep(2000);
                Console.WriteLine("Ending task");
            });

            //start our task
            t.Start();

            //wait for the task to finish -- at this point, the execution returns to the calling thread
            //'await' can apply to any 'Task' object
            await t;

            //after our task is complete, pick back up here in the same context
            Console.WriteLine("After await");
        }


        static async void TestAsync2()
        {
            await new TaskFactory().StartNew(() =>
            {
                 Console.WriteLine("Starting task via factory");
                Thread.Sleep(2000);
                Console.WriteLine("Ending task via factory");
            });

            Console.WriteLine("After awaiting task-factory method");

        }

        //async methods must return a 'Task' but we DON'T return an actual 'Task' -- the compiler will 
        // handle any 'boxing/unboxing' type stuff (I forget the actual term)
       
        static async Task<int> TestAsync3()
        {
            int i = 0;
            await new TaskFactory().StartNew(() =>
            {
                Console.WriteLine("Starting task via factory. i = " + i);
                Thread.Sleep(2000);
                i = 1;
                Console.WriteLine("Ending task via factory... returning i = " + i);
            });

            Console.WriteLine("After awaiting task-factory method.  Returning the value of i = " + i);
            return i;
        }
    
    }

}

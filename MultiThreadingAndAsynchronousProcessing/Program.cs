using System;

namespace MultiThreadingAndAsynchronousProcessing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var choice = PrintMenu();
            var examples = new Examples();
            switch (choice)
            {
                case 1:
                    examples.SynchronizationAndContextSwitching();
                    break;
                case 2:
                    examples.BackgroundThread();
                    break;
                case 3:
                    examples.ParameterizedThreadStart();
                    break;
                case 4:
                    examples.StopThreadWithSharedVariable();
                    break;
                case 5:
                    examples.ThreadStaticVariables();
                    break;
                case 6:
                    examples.ThreadLocalExample();
                    break;
                case 7:
                    examples.ThreadPoolExample();
                    break;
                case 8:
                    examples.StartNewTask();
                    break;
                case 9:
                    examples.StartNewTaskWithReturnValue();
                    break;
                case 10:
                    examples.ContinuationTask();
                    break;
                case 11:
                    examples.SchedulingContinuationTasks();
                    break;
                case 12:
                    examples.ParentAndChildTasks();
                    break;
                case 13:
                    examples.TaskFactory();
                    break;
                case 14:
                    examples.WaitForAllTasks();
                    break;
                case 15:
                    examples.WhenAll();
                    break;
                case 16:
                    examples.WhenAny();
                    break;
                case 17:
                    examples.WaitAny();
                    break;
                case 18:
                    examples.ParallelFor();
                    break;
                case 19:
                    examples.ParallelForEach();
                    break;
                case 20:
                    examples.ParallelBreak();
                    break;
                case 21:
                    examples.ParallelStop();
                    break;
                case 22:
                    examples.AsyncAwaitOne();
                    break;
                case 23:
                    examples.ScalabilityVsResponsiveness();
                    break;
                case 24:
                    examples.ConfigureAwait();
                    break;
                case 25:
                    examples.SimplePlinq();
                    break;
                case 26:
                    examples.PlinqAsOrdered();
                    break;
                case 27:
                    examples.PlinqAsOrderedSequential();
                    break;
                case 28:
                    examples.PlinqForAll();
                    break;
                case 29:
                    examples.AggregateException();
                    break;
                case 30:
                    examples.BlockingCollection();
                    break;
                case 31:
                    examples.GetConsumingEnumerableOnBlockingCollection();
                    break;
                case 32:
                    examples.UsingAConcurrentBag();
                    break;
                case 33:
                    examples.EnumeratingAConcurrentBag();
                    break;
                case 34:
                    examples.ConcurrentStack();
                    break;
                case 35:
                    examples.ConcurrentQueue();
                    break;
                case 36:
                    examples.ConcurrentDictionary();
                    break;
                default:
                    break;
            }
            Console.WriteLine("GO pass the test!");
            Console.ReadLine();
        }

        private static void PrintStars(int num)
        {
            Console.WriteLine(new string('*', num));
        }

        private static int PrintMenu()
        {
            var goodChoice = false;
            int choice = -1;
            while (!goodChoice)
            {
                PrintStars(40);
                Console.WriteLine("What would you like to see?");
                Console.WriteLine("1) Thread Synchronization and Context Switching");
                Console.WriteLine("2) Background Thread");
                Console.WriteLine("3) Parameterized Threadstart");
                Console.WriteLine("4) Stop A Thread via a shared variable");
                Console.WriteLine("5) ThreadStatic Variables");
                Console.WriteLine("6) ThreadLocal<T> = Local Data initialized in each thread via a delegate method");
                Console.WriteLine("7) ThreadPool");
                Console.WriteLine("8) Starting a New Task");
                Console.WriteLine("9) Using a task that returns a value");
                Console.WriteLine("10) Using a continuation Task");
                Console.WriteLine("11) Scheduling Different Continuation Tasks");
                Console.WriteLine("12) Parent and Child Tasks");
                Console.WriteLine("13) Task Factory");
                Console.WriteLine("14) Wait for all Tasks");
                Console.WriteLine("15) WhenAll Continuations");
                Console.WriteLine("16) WhenAny continuation");
                Console.WriteLine("17) WaitAny");
                Console.WriteLine("18) Parallel For");
                Console.WriteLine("19) Parallel ForEach");
                Console.WriteLine("20) Parallel Break");
                Console.WriteLine("21) Parallel Stop");
                Console.WriteLine("22) Async/Await 1");
                Console.WriteLine("23) Scalability Vs Responsiveness");
                Console.WriteLine("24) Configure Await");
                Console.WriteLine("25) Simple PLINQ");
                Console.WriteLine("26) Ordered PLINQ");
                Console.WriteLine("27) Ordered Sequential");
                Console.WriteLine("28) PLINQ ForAll");
                Console.WriteLine("29) Aggregate Exception");
                Console.WriteLine("30) Blocking Collection");
                Console.WriteLine("31) GetConsumingEnumerable on Blocking Collection");
                Console.WriteLine("32) Using a concurrent bag");
                Console.WriteLine("33) Enumerating a concurrent bag");
                Console.WriteLine("34) ConcurrentStack");
                Console.WriteLine("35) ConcurrentQueue");
                Console.WriteLine("36) ConcurrentDictionary");
                goodChoice = int.TryParse(Console.ReadLine(), out choice);
                PrintStars(40);
            }
            return choice;
        }
    }
}

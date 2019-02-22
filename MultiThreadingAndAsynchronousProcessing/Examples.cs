using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingAndAsynchronousProcessing
{
    public class Examples
    {
        private int sleepyTime = 10;
        public void SynchronizationAndContextSwitching()
        {
            sleepyTime = 10;
            Thread t1 = new Thread(new ThreadStart(ThreadMethod));
            t1.Start();
            
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Main Thread: Do some work.");
                Thread.Sleep(sleepyTime);
            }

            t1.Join();
        }

        private void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Method Thread iteration {i}");
                Thread.Sleep(sleepyTime);
            }
        }

        private void ThreadMethod(object sleepInterval)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Method Thread iteration {i}");
                Thread.Sleep(Convert.ToInt32(sleepInterval));
            }
        }

        public void BackgroundThread()
        {
            sleepyTime = 1000;
            Thread t1 = new Thread(new ThreadStart(ThreadMethod));
            t1.IsBackground = false;
            t1.Start();
            t1.Join();
        }

        public void ParameterizedThreadStart()
        {
            Thread t1 = new Thread(new ParameterizedThreadStart(ThreadMethod));
            t1.Start(1000);
            t1.Join();
        }

        public void StopThreadWithSharedVariable()
        {
            bool stopped = false;

            Thread t1 = new Thread(new ThreadStart(() => {
                while (!stopped)
                {
                    Console.WriteLine("Running");
                    Thread.Sleep(1000);
                }
            }));

            t1.Start();
            Console.WriteLine("Press any key to stop processing");
            Console.ReadKey();
            stopped = true;
            t1.Join();
        }

        [ThreadStatic]
        public static int _field;
        public static int _nonTSField;

        public void ThreadStaticVariables()
        {
            new Thread(() =>
            {
                for (int x = 0; x < 10; x++)
                {
                    _field++;
                    _nonTSField++;
                    Console.WriteLine($"Thread A: {_field} | {_nonTSField}");

                }
            }).Start();

            new Thread(() =>
            {
                for (int x = 0; x < 10; x++)
                {
                    _field++;
                    _nonTSField++;
                    Console.WriteLine($"Thread B: {_field} | {_nonTSField}");

                }
            }).Start();
        }

        public static ThreadLocal<int> _t1Field =
                new ThreadLocal<int>(() => {
                    return Thread.CurrentThread.ManagedThreadId;
                });

        public void ThreadLocalExample()
        {
            new Thread(() => {
                for (int x = 0; x < _t1Field.Value; x++)
                {
                    Console.WriteLine($"Thread A: {x}");
                }
            }).Start();


            new Thread(() => {
                for (int x = 0; x < _t1Field.Value; x++)
                {
                    Console.WriteLine($"Thread B: {x}");
                }
            }).Start();
        }

        public void ThreadPoolExample()
        {
            ThreadPool.QueueUserWorkItem((s) => {
                Console.WriteLine($"working on a thread from the pool: {Thread.CurrentThread.ManagedThreadId}");
            });
        }

        public void StartNewTask() {
            Task t = Task.Run(() => {
                for (int x = 0; x < 100; x++)
                {
                    Console.Write('*');
                }
            });

            t.Wait();
        }

        public void StartNewTaskWithReturnValue() {
            Task<int> t = Task.Run(() => {
                return 42;
            });
            
            Console.WriteLine(t.Result);
        }

        public void ContinuationTask() {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            }).ContinueWith((i) =>
            {
                return i.Result * 2;
            });

            Console.WriteLine(t.Result);
        }

        public void SchedulingContinuationTasks() {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });

            t.ContinueWith((i) => {
                Console.WriteLine("Cancelled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            t.ContinueWith((i) => {
                Console.WriteLine("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            var completedTask = t.ContinueWith((i) => {
                Console.WriteLine("Completed Thread and continue task");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completedTask.Wait();
        }

        public void ParentAndChildTasks() {
            Task<int[]> parent = Task.Run(() =>
            {
                var results = new int[3];
                new Task(() => results[0] = 0, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 1, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 2, TaskCreationOptions.AttachedToParent).Start();

                return results;
            });

            var finalTask = parent.ContinueWith(parentTask => {
                foreach (int i in parentTask.Result)
                {
                    Console.WriteLine(i);
                }
            });

            finalTask.Wait();
        }

        public void TaskFactory() {
            Task<int[]> parent = Task.Run(() =>
            {
                var results = new int[3];

                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent
                                                    , TaskContinuationOptions.ExecuteSynchronously);
                tf.StartNew(() => results[0] = 0);
                tf.StartNew(() => results[1] = 1);
                tf.StartNew(() => results[2] = 2);

                return results;
            });

            var finalTask = parent.ContinueWith(parentTask => {
                foreach (int i in parentTask.Result)
                {
                    Console.WriteLine(i);
                }
            });

            finalTask.Wait();
        }

        public void WaitForAllTasks() {
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("1");
                return 1;
            });

            tasks[1] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("2");
                return 2;
            });

            tasks[2] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("3");
                return 3;
            });

            Task.WaitAll(tasks);
        }

        public void WhenAll() {
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("1");
                return 1;
            });

            tasks[1] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("2");
                return 2;
            });

            tasks[2] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("3");
                return 3;
            });

            var t = Task.WhenAll(tasks).ContinueWith(pt => {
                Console.WriteLine("All Tasks Completed");
            });

            t.Wait();
        }

        public void WhenAny() {
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("1");
                return 1;
            });

            tasks[1] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("2");
                return 2;
            });

            tasks[2] = Task.Run(() => {
                Thread.Sleep(5000);
                Console.WriteLine("3");
                return 3;
            });

            var t = Task.WhenAny(tasks).ContinueWith(pt => {
                Console.WriteLine("At least one task has Completed");
            });

            t.Wait();
        }

        public void WaitAny() {
            Task<int>[] tasks = new Task<int>[3];


            tasks[0] = Task.Run(() => { Thread.Sleep(1000); return 1; });
            tasks[1] = Task.Run(() => { Thread.Sleep(2000); return 2; });
            tasks[2] = Task.Run(() => { Thread.Sleep(4000); return 3; });

            while (tasks.Length > 0)
            {
                int i = Task.WaitAny(tasks);
                Task<int> completedTask = tasks[i];

                Console.WriteLine(completedTask.Result);

                var temp = tasks.ToList();
                temp.RemoveAt(i);
                tasks = temp.ToArray();
            }
        }

        public void ParallelFor() {
            Parallel.For(0, 10, i => {
                Thread.Sleep(1000);
                Console.WriteLine(i);
            });
        }

        public void ParallelForEach() {
            var numbers = Enumerable.Range(0, 10);
            Parallel.ForEach(numbers, i => {
                Thread.Sleep(1000);
                Console.WriteLine(i);
            });
        }

        public void ParallelBreak() {
            ParallelLoopResult result = Parallel.For(0, 1000, (int i, ParallelLoopState loopState) => {
                Console.WriteLine(i);
                if (i == 500)
                {
                    Console.WriteLine("Breaking Loop");
                    loopState.Break();
                }
                return;
            });
            Console.WriteLine(result.IsCompleted);
            Console.WriteLine(result.LowestBreakIteration);
        }

        public void ParallelStop() {
            ParallelLoopResult result = Parallel.For(0, 1000, (int i, ParallelLoopState loopState) => {
                Console.WriteLine(i);
                if (i == 500)
                {
                    Console.WriteLine("Breaking Loop");
                    loopState.Stop();
                }
                return;
            });

            Console.WriteLine(result.IsCompleted);
            Console.WriteLine(result.LowestBreakIteration);
        }

        public void AsyncAwaitOne() {
            string result = DownloadContent().Result;
            Console.WriteLine(result);
        }

        private async Task<string> DownloadContent() {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("https://www.majorguidancesolutions.com");
                return result;
            }
        }

        public void ScalabilityVsResponsiveness() {
            SleepAsyncA(1000);
            SleepAsyncB(1000);
        }

        private Task SleepAsyncA(int howLongInMilliseconds)
        {
            return Task.Run(() => { Thread.Sleep(howLongInMilliseconds);  });
        }

        private Task SleepAsyncB(int howLongInMilliseconds)
        {
            return Task.Run(() =>
            {
                TaskCompletionSource<bool> tcs = null;
                var t = new Timer(delegate { tcs.TrySetResult(true); }, null, -1, -1);
                tcs = new TaskCompletionSource<bool>(t);
                t.Change(howLongInMilliseconds, -1);
                return tcs.Task;
            });
        }

        public void ConfigureAwait() {
            Console.WriteLine(DownloadContent2().Result);
        }

        private async Task<string> DownloadContent2() {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("https://www.majorguidancesolutions.com")
                                        .ConfigureAwait(false);

                using (FileStream sourceStream = new FileStream(@"C:\Data\temp.html",
                   FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                {
                    byte[] encodedText = Encoding.Unicode.GetBytes(result);
                    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length).ConfigureAwait(false);
                }
            }

           

            return "Completed";
        }

        public void SimplePlinq() {
            var numbers = Enumerable.Range(0, 100);
            var pResult = numbers.AsParallel().Where(i => i % 2 == 0).ToArray();
            //Parallel.ForEach(pResult, i => {
            //    Console.WriteLine(i);
            //});
            foreach (var pr in pResult)
            {
                Console.WriteLine(pr);
            }
        }

        public void PlinqAsOrdered() {
            var numbers = Enumerable.Range(0, 100);
            var pResult = numbers.AsParallel().Where(i => i % 2 == 0).ToArray();
            //Parallel.ForEach(pResult, i => {
            //    Console.WriteLine(i);
            //});
            foreach (var pr in pResult)
            {
                Console.WriteLine(pr);
            }
        }

        public void PlinqAsOrderedSequential()
        {
            var numbers = Enumerable.Range(0, 100);
            var pResult = numbers.AsParallel().Where(i => i % 2 == 0).AsSequential();
            foreach (var pr in pResult.Take(25))
            {
                Console.WriteLine(pr);
            }
        }

        public void PlinqForAll() {
            var numbers = Enumerable.Range(0, 100);
            var pResult = numbers.AsParallel().Where(i => i % 2 == 0);

            pResult.ForAll(e => Console.WriteLine(e));
        }

        public void AggregateException() {
            var numbers = Enumerable.Range(0, 20);

            try
            {
                var pResult = numbers.AsParallel().Where(i => IsEven(i));

                pResult.ForAll(e => Console.WriteLine(e));
            }
            catch (AggregateException aex)
            {
                Console.WriteLine($"There were {aex.InnerExceptions.Count} exceptions");
            }
        }

        private bool IsEven(int i)
        {
            if (i % 10 == 0) throw new ArgumentException($"bad input {i}");
            return i % 2 == 0;
        }

        public void BlockingCollection()
        {
            var keepRunning = true;
            var col = new BlockingCollection<string>();
            Task read = Task.Run(() => 
            {
                while (keepRunning)
                {
                    Console.WriteLine($"You Entered: {col.Take()}");
                }
            });

            Task write = Task.Run(() => 
            {
                while (keepRunning)
                {
                    Console.WriteLine("Enter a string:");
                    var info = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(info)) break;
                    col.Add(info);
                }
            });

            write.Wait();
        }

        public void GetConsumingEnumerableOnBlockingCollection()
        {
            var keepRunning = true;
            var col = new BlockingCollection<string>();
            Task read = Task.Run(() =>
            {
                foreach (var s in col.GetConsumingEnumerable()) Console.WriteLine($"You Entered: {s}");
            });

            Task write = Task.Run(() =>
            {
                while (keepRunning)
                {
                    Console.WriteLine("Enter a string:");
                    var info = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(info)) break;
                    col.Add(info);
                }
            });

            write.Wait();
        }

        public void UsingAConcurrentBag()
        {
            var baggie = new ConcurrentBag<int>();
            baggie.Add(44);
            baggie.Add(11774);
            int result;

            if (baggie.TryTake(out result))
            {
                Console.WriteLine(result);
            }

            if (baggie.TryPeek(out result))
            {
                Console.WriteLine($"Next item in baggie is {result}");
            }
        }

        public void EnumeratingAConcurrentBag()
        {
            var baggie = new ConcurrentBag<int>();
            Task.Run(() => {
                baggie.Add(44);
                Thread.Sleep(2000);
                baggie.Add(11774);
            });
            Thread.Sleep(500);
            Task.Run(() => {
                foreach (var i in baggie) {
                    Console.WriteLine(i);
                }
            }).Wait();
            //11774 will not be shown
        }

        public void ConcurrentStack()
        {
            var cs = new ConcurrentStack<int>();
            cs.Push(44);

            int result;
            if (cs.TryPop(out result))
            {
                Console.WriteLine($"Popped: {result}");
            }

            cs.PushRange(new int[] { 1, 2, 3 });

            int[] values = new int[3];
            cs.TryPopRange(values);
            foreach (var i in values)
            {
                Console.WriteLine($"Value popped: {i}");
            }
        }

        public void ConcurrentQueue()
        {
            var q = new ConcurrentQueue<int>();
            q.Enqueue(25);
            int result;
            if (q.TryDequeue(out result))
            {
                Console.WriteLine($"Dequeued {result}");
            }
        }

        public void ConcurrentDictionary()
        {
            var dict = new ConcurrentDictionary<string, int>();
            if (dict.TryAdd("red", 1))
            {
                Console.WriteLine("Added Red");
            }

            if (dict.TryUpdate("red", 23, 1))
            {
                Console.WriteLine("red updated to 23");
            }

            dict["red"] = 10;
            Console.WriteLine($"Red updated to {dict["red"]}");

            var r1 = dict.AddOrUpdate("red", 3, (s, i) => i * 2);
            var r2 = dict.GetOrAdd("orange", 2);

            foreach (var item in dict)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }
        }
    }
}

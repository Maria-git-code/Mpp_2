using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp2
{
    public class TaskQueue
    {
        public delegate void TaskDelegate();

        private readonly Queue<TaskDelegate> _tasksQueue = new();
        private object _locker = new();
        private int _busyThreads;

        public TaskQueue(int threadsCount)
        {
            var threads = new Thread[threadsCount];

            for (int i = 0; i < threadsCount; i++)
            {
                var thread = new Thread(Process) {Name = $"{i + 1}", IsBackground = true};
                thread.Start();
                threads[i] = thread;
            }
        }

        private void Process()
        {

            while (true)
            {
                lock (_locker)
                {
                    _busyThreads++;
                }
                TaskDelegate task = null;
                lock (_tasksQueue)
                {
                    if (_tasksQueue.Count > 0)
                    {
                        task = _tasksQueue.Dequeue();
                    }
                }
                task?.Invoke();

                lock (_locker) 
                {
                    _busyThreads--;
                }
            }
        }

        public bool IsCompleted()
        {
            return _tasksQueue.Count == 0 && _busyThreads == 0;
        }
        
        public void EnqueueTask(TaskDelegate task)
        {
            lock (_tasksQueue)
            {
                _tasksQueue.Enqueue(task);
            }
        }
    }
}
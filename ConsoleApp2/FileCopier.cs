using System;
using System.IO;
using System.Threading;

namespace ConsoleApp2
{
    class FileCopier
    {
        private int _copiedFiles;
        private TaskQueue _taskQueue;
        
        public int Copy(string sourcePath, string targetPath)
        {
            _copiedFiles = 0;
            _taskQueue = new TaskQueue(5);
            
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            CopyRec(sourcePath, targetPath);
            while (!_taskQueue.IsCompleted())
            {
            }
            return _copiedFiles;
        }

        public void CopyRec(string sourcePath, string targetPath)
        {

            foreach (string directory in Directory.GetDirectories(sourcePath))
            {
                string dirName = Path.GetFileName(directory);
                string destDir = Path.Combine(targetPath, dirName);
                if (!Directory.Exists(Path.Combine(destDir)))
                {
                    Directory.CreateDirectory(Path.Combine(destDir));
                }
                CopyRec(directory, Path.Combine(targetPath, destDir));
            }
            
            if (Directory.Exists(sourcePath))
            {
                string[] paths = Directory.GetFiles(sourcePath);

                foreach (string path in paths)
                {
                    string fileName = Path.GetFileName(path);
                    string destFile = Path.Combine(targetPath, fileName);
                    _taskQueue.EnqueueTask(CopyFile(path, destFile));
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }
        }

        private TaskQueue.TaskDelegate CopyFile(string path, string destFile)
        {
            return delegate
            {
                File.Copy(path, destFile, false);
                Interlocked.Increment(ref _copiedFiles);
            };
        }


    }
}
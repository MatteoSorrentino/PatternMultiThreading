using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryTask
{
    public class WorkerProgressAsync
    {
        CancellationTokenSource _cts;

        IProgress<int> _progress;

        int _max;
        int _delay;
        Semaphore _sem;

        //costruttore
        public WorkerProgressAsync(Semaphore sem, int max, int delay, CancellationTokenSource cts, IProgress<int> progress)
        {
            _max = max;
            _delay = delay;
            _cts = cts;
            _progress = progress;
            _sem = sem;
        }

        public async Task Start()
        {
            await Task.Factory.StartNew(DoWork);
        }

        private void DoWork()
        {
            _sem.WaitOne();
            for (int i = 0; i <= _max; i++)
            {
                NotifyProgress(_progress, i);
                Thread.Sleep(_delay);
                if (_cts.IsCancellationRequested)
                    break;
            }
            _sem.Release();
        }

        private void NotifyProgress(IProgress<int> progress, int i)
        {
            progress.Report(i);
        }
    }
}

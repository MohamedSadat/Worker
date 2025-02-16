using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoastedServiceLib
{
    public interface ICounterService
    {
        int CurrentCount { get; }
        void Increment();
    }
    public class CounterService: ICounterService
    {
        private int _count;

        public int CurrentCount => _count;

        public void Increment()
        {
            // Thread-safe increment using Interlocked
            Interlocked.Increment(ref _count);
        }
    }
}

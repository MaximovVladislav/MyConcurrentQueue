using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConcurrentQueue
{
    public interface IQueue<T>
    {
        void Push(T item);
        T Pop();
    }
}

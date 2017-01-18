using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConcurrentQueue
{
    public class MultiThreadedQueue<T> : IQueue<T>, IEnumerable<T>
    {
        private volatile Queue<T> _queue;

        private static readonly object _writeLock = new object();
        private static readonly object _readLock = new object();

        public MultiThreadedQueue()
        {
            _queue = new Queue<T>();
        }

        /// <summary> 
        /// Добавляет объект в конец очереди 
        /// </summary> 
        /// <param name="item">Добавляемый объект</param> 
        public void Push(T item)
        {
            lock (_writeLock)
            {
                _queue.Enqueue(item);
            }
        }

        /// <summary> 
        /// Удаляет и возвращает объект с начала очереди. 
        /// Ожидает появления нового объекта, когда очередь пуста, пока не истечет таймаут. 
        /// </summary> 
        /// <exception cref="TimeoutException"></exception> 
        /// <returns></returns> 
        public T Pop()
        {
            T item;
            DateTime start = DateTime.Now;
            TimeSpan span = TimeSpan.FromMilliseconds(1000);

            while (!TryPop(out item))
            {
                if (DateTime.Now - start >= span)
                {
                    throw new TimeoutException("Время ожидания получения элемента превышено");
                }
            }

            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool TryPop(out T item)
        {
            lock (_readLock)
            {
                if (_queue.Count != 0)
                {
                    item = _queue.Dequeue();
                    return true;
                }
            }

            item = default(T);
            return false;
        }
    }
}

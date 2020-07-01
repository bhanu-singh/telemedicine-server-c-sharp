using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace ImTech.Notification.Server
{
    delegate void OnReceive(object o);
    public class AutoQueue<T>
    {
        Thread _drainerThread;
        Queue<T> _queue;
        OnReceive onReceiverDel;
        ManualResetEvent _mreEvent;
        object lockObject = new object();
        public AutoQueue()
        {
            _drainerThread = new Thread(new ThreadStart(StartReceiving));
            _queue = new Queue<T>();
            _mreEvent = new ManualResetEvent(false);
        }

        internal OnReceive OnReceive
        {
            get { return onReceiverDel; }
            set { onReceiverDel = value; }
        }

        public void Start()
        {

            _drainerThread.Start();
        }

        public void Enqueue(T data)
        {
            lock (lockObject)
            {
                _queue.Enqueue(data);
                _mreEvent.Set();
            }
        }


        public T Dequeue()
        {
            lock (lockObject)
            {
               return  _queue.Dequeue();               
            } 
        }

        public int Count()
        {
            lock (lockObject)
            {
                return _queue.Count();
            }
        }

        public void StartReceiving()
        {
            while (true)
            {
                if (Count() == 0)
                {
                    _mreEvent.WaitOne();
                }

                while (Count() != 0)
                {
                    T obj = Dequeue();
                    OnReceive(obj);
                }

                _mreEvent.Reset();
            }
        }
    }
}

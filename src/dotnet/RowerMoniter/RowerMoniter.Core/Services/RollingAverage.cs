using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RowerMoniter.Services
{
    public class RollingAverage
    {

        public RollingAverage(int count = 5) 
        {
            _count = count;
        }

        public decimal Value {
            get 
            {
                var local = _values.ToArray();
                return local.Sum() / local.Count();
            }
        }

        private readonly ConcurrentQueue<decimal> _values = new ConcurrentQueue<decimal>();
        private readonly int _count;

        public void Reset() 
        {
            while (_values.Any())
                _values.TryDequeue(out _);
        }

        public void Queue(decimal value) 
        {
            _values.Enqueue(value);
            while (_values.Count > _count) 
            {
                _values.TryDequeue(out _);
            }
        }
    }

}

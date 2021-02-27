using RowerMoniter.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace RowerMoniter.Services
{
    public class MomentCollection
    {
        private ImmutableList<ITimedEvent> _buffer = ImmutableList<ITimedEvent>.Empty;
        private readonly TimeSpan _span;

        public MomentCollection(TimeSpan span) 
        {
            _span = span;
        }

        public void Add(ITimedEvent timedEvent) 
        {
            TrimAndAdd(timedEvent);
        }

        public IEnumerable<ITimedEvent> Events()
        {
            var now = DateTime.Now;
            var local = _buffer;

            // if the colleciton hasn't been around long enoguh don;t return anything.
            if (now - local.Map(t => t.Time).Min() < _span) 
            {
                return Enumerable.Empty<ITimedEvent>();
            }

            var events = local.Where(time => now.Subtract(time.Time) < _span);
            return events;
        }

        private void TrimAndAdd(ITimedEvent timedEvent)
        {
            var now = DateTime.Now;
            var local = _buffer;

            var toRemove = local.Where(time => now.Subtract(time.Time) > _span);
            var builder = local.ToBuilder();

            foreach (var expiredEvent in toRemove)
            {
                builder.Remove(expiredEvent);
            }

            builder.Add(timedEvent);

            _buffer = builder.ToImmutable();

        }
    }


    public class MomentCollection<TType> 
    {
        private ImmutableList<(DateTime time, TType value)> _buffer = ImmutableList<(DateTime time, TType value)>.Empty;
        private readonly TimeSpan _span;

        public MomentCollection(TimeSpan span)
        {
            _span = span;
        }

        public void Add(TType item)
        {
            TrimAndAdd((DateTime.Now, item));
        }

        public IEnumerable<TType> Events()
        {
            var now = DateTime.Now;
            var local = _buffer;

            var events = local.Where(time => IsLessThanSpen(time, now)).Select(item => item.value).ToArray();
            return events;
        }

        private bool IsLessThanSpen((DateTime time, TType value) time, DateTime now)
        {

            var ellapsed = now.Subtract(time.time);
            return ellapsed < _span;
        }

        private void TrimAndAdd((DateTime time, TType value) value)
        {
            var now = DateTime.Now;
            var local = _buffer;

            var toRemove = local.Where(time => now.Subtract(time.time) > _span);
            var builder = local.ToBuilder();

            foreach (var expiredEvent in toRemove)
            {
                builder.Remove(expiredEvent);
            }

            builder.Add(value);

            _buffer = builder.ToImmutable();

        }
    }



}

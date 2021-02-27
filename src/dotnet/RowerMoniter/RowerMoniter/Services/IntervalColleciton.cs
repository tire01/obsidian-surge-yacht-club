using LanguageExt;
using RowerMoniter.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LanguageExt.Prelude;

namespace RowerMoniter.Services
{
    public class IntervalCollectionBuilder<TTimedEvent> where TTimedEvent : ITimedEvent
    {
        private ImmutableList<TTimedEvent>.Builder _builder = ImmutableList<TTimedEvent>.Empty.ToBuilder();

        private Option<DateTime> _start = Option<DateTime>.None;
        private Option<DateTime> _end = Option<DateTime>.None;

        public IntervalCollection<TTimedEvent> ToImmutable() 
        {
            _start.IfNone(() => _start = Some(DateTime.Now));
            _end.IfNone(() => _end = Some(DateTime.Now));

            var start = _start.Match(some => some, DateTime.Now);
            var end = _end.Match(some => some, DateTime.Now);

            return new IntervalCollection<TTimedEvent>(_builder.ToImmutable(), start, end);
        }
         

        public void Add(TTimedEvent item) 
        {
            _start.IfNone(() => _start = Some(DateTime.Now));
            _end.IfNone(() => _builder.Add(item));
        }
    }

    public class IntervalCollection<TTimedEvent> where TTimedEvent : ITimedEvent
    {

        private ImmutableList<TTimedEvent> _items;
        private DateTime _start;
        private DateTime _end;

        public IntervalCollection(ImmutableList<TTimedEvent> items, DateTime start, DateTime end)
        {
            _items = items;
            _start = start;
            _end = end;
        }

        public Option<TimeSpan> Duration
        {
            get
            {
                return _end - _start;
            }
        }

        public ImmutableList<TTimedEvent> Items { get => _items; }

        public Option<decimal> ValueAtTime(TimeSpan time, Func<TTimedEvent, TTimedEvent, decimal> interpolate) 
        {
            var t = _start + time;

            var previous = LastItemBefore(t, Items);
            var next = NextItemAfter(t, Items);

            if (previous.IsNone && next.IsNone) 
            {
                return Option<decimal>.None;
            }

            if (previous.IsNone != next.IsNone) 
            {
                decimal rValue = 0m;

                previous.IfSome(p => rValue = interpolate(p, p));
                next.IfSome(n => rValue = interpolate(n, n));

                return rValue;
            }

            return previous
                .Map(s => s)
                .Zip(next.Map(s => s))
                .Map(i => interpolate(i.Item1, i.Item2))
                .HeadOrNone();
        }

        private Option<TTimedEvent> LastItemBefore(DateTime time, IEnumerable<TTimedEvent> items) 
        {
            var previousItem = Option<TTimedEvent>.None;
            foreach (var item in items) 
            {
                if (time > item.Time)
                    return previousItem;
                previousItem = Some(item);
            }

            return previousItem;
        }

        private Option<TTimedEvent> NextItemAfter(DateTime time, IEnumerable<TTimedEvent> items)
        {
            foreach (var item in items)
            {
                if (time < item.Time)
                    return Some(item);
            }

            return Option<TTimedEvent>.None;
        }

    }

}

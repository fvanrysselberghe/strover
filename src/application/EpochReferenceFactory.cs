using System;
using Strover.Application.Interfaces;
using Strover.Models;

namespace Strover.Application
{
    /// <summary>
    /// Generates structured references that consist of a timestamp and a sequence
    /// Due to the size limitations of the StructuredReference the key is not unique
    /// Collisions will occur when:
    ///     - more than 999 references are requested within the same second
    ///     - when around 150days have passed 
    /// 
    /// It can be used when at most 999 parallel transfers are done second
    /// and the time to handle a payment is less than 150days.
    /// </summary>
    public class EpochReferenceFactory : IStructuredReferenceFactory
    {
        private struct Tick
        {

            public Tick(DateTime timestamp)
            {
                _timeValue = timestamp - DateTime.UnixEpoch;
                SequenceValue = 1;
            }
            public ulong TimeValue
            {
                get
                {
                    return (ulong)(_timeValue.Days * 24 * 60 * 60)
                        + (ulong)(_timeValue.Hours * 60 * 60)
                        + (ulong)(_timeValue.Minutes * 60)
                        + (ulong)(_timeValue.Seconds);
                }
            }

            public short SequenceValue { get; set; }
            private TimeSpan _timeValue;

            public static bool operator <(Tick left, Tick right)
            {
                if (left._timeValue < right._timeValue)
                    return true;
                else if (left._timeValue > right._timeValue)
                    return false;
                else
                    return left.SequenceValue < right.SequenceValue;
            }

            public static bool operator >(Tick left, Tick right)
            {
                if (left._timeValue > right._timeValue)
                    return true;
                else if (left._timeValue < right._timeValue)
                    return false;
                else
                    return left.SequenceValue > right.SequenceValue;
            }

            public static Tick operator ++(Tick left)
            {
                ++(left.SequenceValue);
                return left;
            }
        }

        public StructuredReference Create()
        {
            var tick = GetTick();

            return new StructuredReference(tick.TimeValue * 1000
                                           + (ulong)tick.SequenceValue);
        }

        private Tick GetTick()
        {
            lock (_guard)
            {
                Tick current = Current;
                Tick now = new Tick(DateTime.Now);

                if (current < now)
                    Current = now;
                else
                    Current = ++current;

                return Current;
            }
        }

        private readonly Object _guard = new Object();

        private Tick Current { get; set; }
    }
}
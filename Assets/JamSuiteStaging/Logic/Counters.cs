using System;
using System.Collections.Generic;

namespace JamSuite.Logic {

    public class Counter {

        public delegate void IncrementEvent(int count);
        public event IncrementEvent OnIncrement;

        public int Value;

        public Action Subscribe(IncrementEvent callback) {
            OnIncrement += callback;
            return () => OnIncrement -= callback;
        }

        public void Reset() {
            Value = 0;
        }

        public int Increase(int increment = 1) {
            Value += increment;
            if (OnIncrement != null) OnIncrement(Value);
            return Value;
        }

        public static Counter operator ++(Counter ctr) {
            ctr.Increase();
            return ctr;
        }
    }


    public class Counters {

        public static Dictionary<string, Counter> All = new Dictionary<string, Counter>();
        public static readonly Counter Empty = new Counter();

        public static bool Exists(string handle) {
            return !string.IsNullOrEmpty(handle) && All.ContainsKey(handle);
        }

        public static Counter Find(string handle) {
            if (string.IsNullOrEmpty(handle)) return Empty;

            var counter = default(Counter);
            if (All.TryGetValue(handle, out counter)) return counter;

            counter = new Counter();
            All.Add(handle, counter);
            return counter;
        }

        public static Action Subscribe(string handle, Counter.IncrementEvent callback) {
            return Find(handle).Subscribe(callback);
        }

        public static int Get(string handle) {
            return Find(handle).Value;
        }

        public static void Set(string handle, int value) {
            Find(handle).Value = value;
        }

        public static int Increase(string handle, int increment = 1) {
            return Find(handle).Increase(increment);
        }

        public static void Reset(string handle) {
            Find(handle).Reset();
        }

        public static void ResetAll() {
            foreach (var pair in All)
                pair.Value.Reset();
        }
    }
}

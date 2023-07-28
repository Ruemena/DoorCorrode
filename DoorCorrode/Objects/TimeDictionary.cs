using MEC;

// dictionary that allows for coroutines to be handled safely 


namespace DoorCorrode.Objects
{
    public class TimeDictionary<Key, Value>
    where Value : TimeValue, new()
    {
        private Dictionary<Key, Value> tvPair = new();

        public void CreateTV(Key obj)
        {
            Value tv = new Value();
            tvPair.Add(obj, tv);
        }
        public bool ContainsTV(Key obj)
        {
            if (tvPair.ContainsKey(obj)) return true; else return false;
        }
        public Value GetTV(Key obj)
        {
            return tvPair[obj];
        }
        public void SetCH(Key obj, CoroutineHandle cH)
        {
            tvPair[obj].CoroutineHandle = cH;
        }
        public void RemoveTV(Key obj)
        {
            if (tvPair.ContainsKey(obj))
            {
                Value tv = tvPair[obj];
                if (tv.CoroutineHandle != null)
                {
                    Timing.KillCoroutines((CoroutineHandle)tv.CoroutineHandle);
                }
                tvPair.Remove(obj);
            }
            else
            {
                throw new Exception($"Player {obj} not found");
            }
        }
        public void ClearTVs()
        {
            foreach (KeyValuePair<Key, Value> entry in tvPair)
            {
                Value tv = entry.Value;
                if (tv.CoroutineHandle != null)
                {
                    Timing.KillCoroutines((CoroutineHandle)tv.CoroutineHandle);
                }
            }
            tvPair = new();
        }


        private IEnumerator<float> WaitDelete(Key obj, float time)
        {
            yield return Timing.WaitForSeconds(time);
            tvPair.Remove(obj);
        }
    }
}

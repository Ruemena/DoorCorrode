namespace DoorCorrode.Types
{
    public class Larry : TimeValue
    {
        public bool IsActivelyCorroding { get; set; }
        public bool Cooldown { get; set; }

        public void ToggleCorrode()
        {
            if (IsActivelyCorroding)
            {
                IsActivelyCorroding = false;
            }
            else
            {
                IsActivelyCorroding = true;
            }
        }
    }
}

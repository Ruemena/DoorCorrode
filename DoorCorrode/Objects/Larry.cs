namespace DoorCorrode.Objects
{
    public class Larry : TimeValue
    {
        public Boolean IsActivelyCorroding { get; set; }
        public Boolean Cooldown { get; set; }

        public void ToggleCorrode()
        {
            if (IsActivelyCorroding) { 
                IsActivelyCorroding = false; 
            } else {
                IsActivelyCorroding = true;
            }
        }
    }
}

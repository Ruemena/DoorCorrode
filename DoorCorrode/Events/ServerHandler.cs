using Exiled.Events.EventArgs.Server;

namespace Exiled.DoorCorrodeEvents
{
    using DoorCorrode;
    using MEC;

    internal sealed class ServerHandler
    {
        private readonly DoorCorrode Instance = DoorCorrode.Instance;

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Instance.corrodedDoors.ClearTVs();
            Instance.larries.ClearTVs();
            Timing.KillCoroutines("GiveLarryItem");
        }
    }
}

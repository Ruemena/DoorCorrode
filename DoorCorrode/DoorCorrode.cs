namespace DoorCorrode
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.DoorCorrodeEvents;
    using Types;
        
    public class DoorCorrode : Plugin<Config>
    {
        private PlayerHandler playerHandler;
        private ServerHandler serverHandler;
        public TimeDictionary<Player, Larry> larries;
        public TimeDictionary<Door, TimeValue> corrodedDoors;

        public override string Author { get; } = "Rue";
        public override string Name { get; } = "DoorCorrode";
        public override string Prefix { get; } = "DoorCorrode";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(7, 2, 0);

        public override PluginPriority Priority { get; } = PluginPriority.Last;       
        private DoorCorrode() { }
        private static readonly DoorCorrode Singleton = new DoorCorrode();
        public static DoorCorrode Instance => Singleton;

        public override void OnEnabled()
        {
            larries = new TimeDictionary<Player, Larry>();
            corrodedDoors = new TimeDictionary<Door, TimeValue>();

            playerHandler = new PlayerHandler();
            serverHandler = new ServerHandler();

            Exiled.Events.Handlers.Server.RoundEnded += serverHandler.OnRoundEnded;

            Exiled.Events.Handlers.Player.ProcessingHotkey += playerHandler.OnProcessingHotkey;
            Exiled.Events.Handlers.Player.InteractingDoor += playerHandler.OnInteractingDoor;
            Exiled.Events.Handlers.Player.ChangingRole += playerHandler.OnChangingRole;

            Exiled.Events.Handlers.Player.Dying += playerHandler.OnDying;
            // for making the item given not usable
            Exiled.Events.Handlers.Player.ChangingItem += playerHandler.Deny106;
            Exiled.Events.Handlers.Player.DroppingItem += playerHandler.Deny106;
            Exiled.Events.Handlers.Player.UsingItem += playerHandler.Deny106;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= serverHandler.OnRoundEnded;

            Exiled.Events.Handlers.Player.InteractingDoor -= playerHandler.OnInteractingDoor;
            Exiled.Events.Handlers.Player.ProcessingHotkey -= playerHandler.OnProcessingHotkey;
            Exiled.Events.Handlers.Player.ChangingRole -= playerHandler.OnChangingRole;

            Exiled.Events.Handlers.Player.Dying -= playerHandler.OnDying;

            Exiled.Events.Handlers.Player.ChangingItem -= playerHandler.Deny106;
            Exiled.Events.Handlers.Player.DroppingItem -= playerHandler.Deny106;
            Exiled.Events.Handlers.Player.UsingItem -= playerHandler.Deny106;


            playerHandler = null;
            serverHandler = null;
            larries = null;
            corrodedDoors = null;
            base.OnDisabled();
        }
    }
}

using DoorCorrode.Types;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace DoorCorrode.Objects
{
    public static class Coroutines
    {
        public static IEnumerator<float> LockUnlockDoor(Door door, float time, DoorCorrode Instance)
        {
            Log.Debug("Locking door");
            door.Lock(time, Exiled.API.Enums.DoorLockType.AdminCommand);
            yield return Timing.WaitForSeconds(time);
            Instance.corrodedDoors.RemoveTV(door);
        }
        public static IEnumerator<float> LarryCooldown(Larry larry, float time)
        {
            larry.IsActivelyCorroding = false;
            larry.Cooldown = true;
            yield return Timing.WaitForSeconds(time);
            larry.Cooldown = false;
        }
        public static IEnumerator<float> GiveItemDelayed(Player player, ItemType item, float delay)
        {
            yield return Timing.WaitForSeconds(delay);
            if (player.Role == RoleTypeId.Scp106)
            {
                player.AddItem(item);
            }
        }
    }
}

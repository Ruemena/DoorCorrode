using Exiled.API.Features;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Mirror;
using PlayerRoles;
using UnityEngine;
using Exiled.API.Enums;

namespace DoorCorrode.Objects
{
    internal class CollisionHandler : MonoBehaviour
    {
        DoorVariant? GetClosestDoor(DoorVariant[] doorList, Vector3 pos)
        {
            float min = Mathf.Infinity;
            DoorVariant? highest = null;
            foreach (DoorVariant currentDoor in doorList)
            {
                float distance = Vector3.Distance(currentDoor.transform.position, pos);
                if (distance < min) // higher
                {
                    highest = currentDoor;
                    min = distance;
                }
            }
            return highest;
        }
        private readonly DoorCorrode Instance = DoorCorrode.Instance;
        public CorrodeDoor corrodeDoor = new CorrodeDoor();
        public Door? TryFindDoorParent(Collider col)
        {
            Door? door = Door.Get(col.GetComponentInParent<DoorVariant>()); // sometimes there are issues with getting the correct door, this makes sure that this is a door we've collided with and then we do the proper check to make sure its the right door
            if (door == null) return null;

            DoorVariant[] doorList = col.GetComponentsInParent<DoorVariant>();
            Door closestDoor = Door.Get(GetClosestDoor(doorList, col.transform.position));

            if (closestDoor.Type == DoorType.LightContainmentDoor && Door.Get(closestDoor.Transform.parent?.gameObject) != null) {
                Door higherDoor = Door.Get(closestDoor.Transform.parent?.parent?.gameObject);
                Log.Debug(higherDoor.Type);
                if (higherDoor.Type is DoorType.Scp106Primary) return higherDoor;
            }

            if (closestDoor.Type is DoorType.CheckpointEzHczA or DoorType.CheckpointEzHczB 
                or DoorType.CheckpointLczA or DoorType.CheckpointLczB)
            {
                return Door.Get(closestDoor.Transform.parent?.gameObject);
            } else
            {
                return closestDoor;
            }
        }
        public void OnTriggerEnter(Collider col)
        {
            Player player = Player.Get(this.GetComponentInParent<NetworkIdentity>());
            if (player.Role == RoleTypeId.Scp106 
                && Instance.larries.ContainsTV(player) 
                && Instance.larries.GetTV(player).IsActivelyCorroding 
                && !Instance.larries.GetTV(player).Cooldown) {

                Door? door = TryFindDoorParent(col);
                if (door != null)
                {
                    corrodeDoor.TryCorrodeDoor(door, player, Instance);
                }
            }
        }
    }
}

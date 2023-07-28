using Exiled.API.Features;
using MEC;
using UnityEngine;
using DoorCorrode;
using Exiled.API.Features.Roles;
using Interactables.Interobjects.DoorUtils;

namespace DoorCorrode.Objects
{
    internal class CorrodeDoor : MonoBehaviour
    {
        public void TryCorrodeDoor(Door? door, Player player, DoorCorrode Instance)
        {                              
            if (door != null && !door.IsElevator && !door.IsOpen && (Instance.Config.DoGates || !door.IsGate))
            {
                TimeDictionary<Door, TimeValue> cD = Instance.corrodedDoors;
                if (!cD.ContainsTV(door))
                {
                    Scp106Role role = player.Role.As<Scp106Role>();
                    if (Instance.Config.FullyClosed == true && !door.IsFullyClosed)
                    {
                        return;
                    }
                    if (role.Vigor >= (Instance.Config.VigorCost * 0.01f))
                    {
                        role.Vigor -= Instance.Config.VigorCost * 0.01f;
                    } else
                    {
                        return;
                    }
                    cD.CreateTV(door);

                    Larry larryPlayer = Instance.larries.GetTV(player);
                    larryPlayer.IsActivelyCorroding = false;

                    Hint hint = new(Instance.Config.CooldownHint, Instance.Config.Cooldown);
                    player.ShowHint(hint);
                    if (!door.IsCheckpoint && door.Type != Exiled.API.Enums.DoorType.Scp106Primary) Timing.CallDelayed(0.2f, () => door.PlaySound(Exiled.API.Enums.DoorBeepType.InteractionDenied));
                    CoroutineHandle doorCH = Timing.RunCoroutine(Coroutines.CorrodeDoor(door, Instance.Config.Length, Instance));
                    cD.GetTV(door).CoroutineHandle = doorCH;

                    CoroutineHandle larryCH = Timing.RunCoroutine(Coroutines.LarryCooldown(larryPlayer, Instance.Config.Cooldown));
                    Instance.larries.GetTV(player).CoroutineHandle = larryCH;
                }
            }
        }
    }
 }

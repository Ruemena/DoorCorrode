using Exiled.API.Features;
using Exiled.API.Enums;


using Exiled.Events.EventArgs.Player;

namespace Exiled.DoorCorrodeEvents
{
    using DoorCorrode;
    using MEC;
    using PlayerRoles;
    using Exiled.Events.EventArgs.Interfaces;
    using UnityEngine;
    using DoorCorrode.Objects;

    public class PlayerHandler : MonoBehaviour
    {
        private readonly DoorCorrode Instance = DoorCorrode.Instance;
        public Dictionary<HotkeyButton, ItemType> HotkeyItems { get; } = new Dictionary<HotkeyButton, ItemType>
        {
            {  HotkeyButton.Medical,         ItemType.Medkit          },
            {  HotkeyButton.PrimaryFirearm,  ItemType.GunCOM15        },
            {  HotkeyButton.Keycard,         ItemType.KeycardJanitor  },
            {  HotkeyButton.Grenade,         ItemType.GrenadeFlash    }
        };

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {

            if (ev.Player.CurrentHint?.Content == Instance.Config.ActiveHint)
            {
                ev.Player.ShowHint(new Hint("", 0.1f, true));
            }

            if (Instance.larries.ContainsTV(ev.Player))
            {
                Instance.larries.RemoveTV(ev.Player);

                SphereCollider[] colList = ev.Player.GameObject.GetComponents<SphereCollider>();
                foreach (SphereCollider col in colList) {
                    if (col.tag == "CorrodeCol")
                    {
                        Object.Destroy(col);
                    }
                }

                CollisionHandler[] scriptList = ev.Player.GameObject.GetComponents<CollisionHandler>();
                foreach (CollisionHandler col in scriptList)
                {
                    Object.Destroy(col);
                }
            }
            if (ev.NewRole == RoleTypeId.Scp106)
            {
                Instance.larries.CreateTV(ev.Player);

                SphereCollider col = ev.Player.GameObject.AddComponent<SphereCollider>();
                col.radius = 0.5f;
                col.center = new Vector3(0, 0, 0);
                col.tag = "CorrodeCol";
                col.isTrigger = true;
                ev.Player.GameObject.AddComponent<CollisionHandler>();

                Timing.RunCoroutine(Coroutines.GiveItemDelayed(ev.Player, HotkeyItems[Instance.Config.Hotkey], 1.5f), "GiveLarryItem");
            }
        }

        public void OnProcessingHotkey(ProcessingHotkeyEventArgs ev)
        {   
            if (ev.Hotkey == Instance.Config.Hotkey && ev.Player.Role == RoleTypeId.Scp106)
            {
                if (Instance.larries.ContainsTV(ev.Player) && !Instance.larries.GetTV(ev.Player).Cooldown)
                {
                    Instance.larries.GetTV(ev.Player).ToggleCorrode();
                    Hint hint = new(Instance.Config.ActiveHint, 999999);

                    if (ev.Player.CurrentHint?.Content == Instance.Config.ActiveHint )
                    {
                        ev.Player.ShowHint(new Hint("", 0.1f, true));
                    } else if (Instance.larries.GetTV(ev.Player).IsActivelyCorroding == true)
                    {
                        ev.Player.ShowHint(hint);
                    }
                }
                ev.IsAllowed = false;
            }
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Door.IsLocked && ev.Player.IsScp && Instance.corrodedDoors.ContainsTV(ev.Door))
            {
                ev.IsAllowed = true;
            }
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp106)
            {
                ev.ItemsToDrop.Clear();
            }
        }

        public void Deny106<T>(T ev)
        where T : IDeniableEvent, IPlayerEvent
        {
            if (ev.Player.Role == RoleTypeId.Scp106)
            {
                ev.IsAllowed = false;
            }
        }
    }
}
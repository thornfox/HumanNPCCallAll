// Requires: HumanNPC

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Oxide.Core.Plugins;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("HumanNPCCallAll", "ThornFox", "0.1.1")]
    [Description("Teleports all HumanNPCs to the caller and sets their spawn point there. / Teleportiert alle HumanNPCs zu dir und setzt ihren Spawnpunkt dorthin.")]
    public class HumanNPCCallAll : RustPlugin
    {
        [PluginReference]
        private Plugin HumanNPC;

        private const string PermUse = "humannpccallall.use";

        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private static readonly Dictionary<string, string> MessagesEn = new Dictionary<string, string>
        {
            ["NoPermission"] = "You don't have permission to use this command.",
            ["HumanNPCMissing"] = "HumanNPC is not loaded.",
            ["ReadError"] = "Could not read the NPC data from HumanNPC.",
            ["Done"] = "{0} NPC(s) teleported to you, {1} spawn point(s) updated.",
            ["Error"] = "An error occurred. See the server console for details."
        };

        private static readonly Dictionary<string, string> MessagesDe = new Dictionary<string, string>
        {
            ["NoPermission"] = "Dir fehlt die Berechtigung für diesen Befehl.",
            ["HumanNPCMissing"] = "HumanNPC ist nicht geladen.",
            ["ReadError"] = "Die NPC-Daten von HumanNPC konnten nicht gelesen werden.",
            ["Done"] = "{0} NPC(s) zu dir teleportiert, {1} Spawnpunkt(e) aktualisiert.",
            ["Error"] = "Ein Fehler ist aufgetreten. Details stehen in der Serverkonsole."
        };

        private void Init()
        {
            permission.RegisterPermission(PermUse, this);
            lang.RegisterMessages(MessagesEn, this, "en");
            lang.RegisterMessages(MessagesDe, this, "de");
        }

        //////////////////////////////////////////////////////
        ///  /callnpcs
        ///  Moves every NPC known to HumanNPC to the caller's
        ///  position and sets their spawn point there.
        //////////////////////////////////////////////////////
        [ChatCommand("callnpcs")]
        private void cmdCallNpcs(BasePlayer player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.UserIDString, PermUse))
            {
                SendReply(player, GetMsg(player, "NoPermission"));
                return;
            }

            if (HumanNPC == null || !HumanNPC.IsLoaded || HumanNPC.Object == null)
            {
                SendReply(player, GetMsg(player, "HumanNPCMissing"));
                return;
            }

            try
            {
                var host = HumanNPC.Object;
                var rawNpcs = GetFieldValue(host, "humannpcs");
                var entries = rawNpcs as IEnumerable;
                if (entries == null)
                {
                    PrintError($"HumanNPC data field has unexpected type: {rawNpcs?.GetType().FullName ?? "null"}");
                    SendReply(player, GetMsg(player, "ReadError"));
                    return;
                }

                // Collect (userid, info) pairs. Works for any dictionary-like type with KeyValuePair entries.
                var npcInfos = new List<object>();
                var npcIds = new HashSet<ulong>();
                foreach (var entry in entries)
                {
                    if (entry == null) continue;
                    var key = entry.GetType().GetProperty("Key")?.GetValue(entry, null);
                    var value = entry.GetType().GetProperty("Value")?.GetValue(entry, null);
                    if (key is ulong id) npcIds.Add(id);
                    if (value != null) npcInfos.Add(value);
                }

                var position = player.transform.position;
                var rotation = player.transform.rotation;

                // 1) Update the stored spawn point of every NPC (also disabled/despawned ones)
                int stored = 0;
                foreach (var info in npcInfos)
                {
                    var spawnField = info.GetType().GetField("spawnInfo", Flags);
                    if (spawnField == null) continue;

                    var newSpawn = Activator.CreateInstance(spawnField.FieldType, position, rotation);
                    spawnField.SetValue(info, newSpawn);
                    stored++;
                }

                // 2) Teleport the NPCs that are currently spawned on the map
                int moved = 0;
                foreach (var npc in UnityEngine.Object.FindObjectsOfType<BasePlayer>())
                {
                    if (npc == null || !npc.isServer) continue;
                    if (!npcIds.Contains(npc.userID)) continue;

                    npc.MovePosition(position);
                    npc.transform.rotation = rotation;
                    npc.SendNetworkUpdateImmediate();
                    moved++;
                }

                // 3) Persist the new spawn points
                SetFieldValue(host, "save", true);
                var saveMethod = host.GetType().GetMethod("SaveData", Flags);
                saveMethod?.Invoke(host, null);

                SendReply(player, string.Format(GetMsg(player, "Done"), moved, stored));
            }
            catch (Exception ex)
            {
                PrintError($"callnpcs failed: {ex}");
                SendReply(player, GetMsg(player, "Error"));
            }
        }

        private string GetMsg(BasePlayer player, string key) => lang.GetMessage(key, this, player.UserIDString);

        private static object GetFieldValue(object target, string name)
        {
            var field = target.GetType().GetField(name, Flags);
            return field?.GetValue(target);
        }

        private static void SetFieldValue(object target, string name, object value)
        {
            var field = target.GetType().GetField(name, Flags);
            field?.SetValue(target, value);
        }
    }
}

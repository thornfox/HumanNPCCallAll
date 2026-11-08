# HumanNPCCallAll

**Developer:** ThornFox **Version:** 0.1.1

[English](#english) | [Deutsch](#deutsch)

---

## English

Oxide plugin for Rust that adds `/callnpcs`. It teleports every NPC managed by [HumanNPC](https://umod.org/plugins/human-npc) to your position and sets their spawn point there. Useful for gathering all NPCs quickly.

### How it works

- Reads the NPC list of HumanNPC and sets the stored spawn point of every NPC to your position and rotation. This includes NPCs that are currently disabled or despawned.
- Moves all NPCs that are currently spawned on the map to you.
- Saves the HumanNPC data, so the new spawn points survive a restart.

The plugin uses reflection on private HumanNPC fields (`humannpcs`, `save`, `SaveData`). If a HumanNPC update renames them, the plugin needs an update. Tested with HumanNPC 0.6.10.

### Installation

1. Install [HumanNPC](https://umod.org/plugins/human-npc).
2. Copy `HumanNPCCallAll.cs` into `oxide/plugins/`.
3. Grant the permission to the people who should use the command.

### Commands

| Command | Description |
|---|---|
| `/callnpcs` | Teleports all HumanNPCs to you and sets their spawn point to your position |

### Permissions

| Permission | Effect |
|---|---|
| `humannpccallall.use` | Allows `/callnpcs` |

Example: `oxide.grant group admin humannpccallall.use`

The auth level does not matter. Only the permission counts.

### Configuration

None. Chat messages are available in English and German through the Oxide lang files.

### Dependencies

- [HumanNPC](https://umod.org/plugins/human-npc) (required)

### Changelog

- **0.1.1** – Initial release.

### Tested on RustRentner

All ThornFox plugins are tested extensively on [rustrentner.de](https://rustrentner.de).

### License

MIT – see [LICENSE](LICENSE).

---

## Deutsch

Oxide-Plugin für Rust, das den Befehl `/callnpcs` ergänzt. Er teleportiert alle NPCs, die [HumanNPC](https://umod.org/plugins/human-npc) verwaltet, zu deiner Position und setzt ihren Spawnpunkt dorthin. Praktisch, um alle NPCs schnell zu versammeln.

### Funktionsweise

- Liest die NPC-Liste von HumanNPC und setzt den gespeicherten Spawnpunkt jedes NPCs auf deine Position und Blickrichtung. Das gilt auch für deaktivierte oder gerade nicht gespawnte NPCs.
- Verschiebt alle NPCs, die gerade auf der Karte stehen, zu dir.
- Speichert die HumanNPC-Daten, damit die neuen Spawnpunkte einen Neustart überstehen.

Das Plugin greift per Reflection auf private HumanNPC-Felder zu (`humannpcs`, `save`, `SaveData`). Benennt ein HumanNPC-Update diese um, muss das Plugin angepasst werden. Getestet mit HumanNPC 0.6.10.

### Installation

1. [HumanNPC](https://umod.org/plugins/human-npc) installieren.
2. `HumanNPCCallAll.cs` nach `oxide/plugins/` kopieren.
3. Die Berechtigung an alle vergeben, die den Befehl nutzen sollen.

### Befehle

| Befehl | Beschreibung |
|---|---|
| `/callnpcs` | Teleportiert alle HumanNPCs zu dir und setzt ihren Spawnpunkt auf deine Position |

### Berechtigungen

| Berechtigung | Wirkung |
|---|---|
| `humannpccallall.use` | Erlaubt `/callnpcs` |

Beispiel: `oxide.grant group admin humannpccallall.use`

Das Auth-Level spielt keine Rolle. Nur die Berechtigung zählt.

### Konfiguration

Keine. Die Chat-Texte gibt es auf Englisch und Deutsch über die Oxide-Lang-Dateien.

### Abhängigkeiten

- [HumanNPC](https://umod.org/plugins/human-npc) (erforderlich)

### Changelog

- **0.1.1** – Erstveröffentlichung.

### Getestet auf RustRentner

Alle ThornFox-Plugins werden ausgiebig auf [rustrentner.de](https://rustrentner.de) getestet.

### Lizenz

MIT – siehe [LICENSE](LICENSE).

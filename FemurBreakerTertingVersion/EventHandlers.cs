using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Wrappers;
using PlayerRoles;
using ProjectMER.Events.Arguments;
using System.Collections.Generic;
using System.Linq;
using Player = LabApi.Features.Wrappers.Player;
using Random = UnityEngine.Random;

namespace FemurBreakerTertingVersion
{
    public class EventHandlers
    {
        private readonly Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;
        float playerAlive = 0;
        public void OnTrap(ButtonInteractedEventArgs ev)
        {
            if (ev.Button.GameObject.name != "Button106") return;

            switch (playerAlive)
            {
                case 0:
                    HandleFirstInteraction(ev.Player);
                    break;
                case 1:
                    HandleSecondInteraction(ev.Player);
                    break;
                case 3:
                    ev.Player.SendBroadcast(plugin.Config.OnRecontainmentRepeat, 4);
                    break;
                default:
                    ev.Player.SendBroadcast(plugin.Config.OnRequirements, 4);
                    break;
            }
        }

        private void HandleFirstInteraction(Player player)
        {
            playerAlive = 1;
            if (plugin.Config.CassieWithSacrificie)
            {
                player.Kill(plugin.Config.OnSacrificeDeathReason, plugin.Config.CassieAnounceWhitPlayerDead);
            }
            else
            {
                player.Kill(plugin.Config.OnSacrificeDeathReason);
            }
        }

        private void HandleSecondInteraction(Player player)
        {
            bool success = Random.Range(1, 101) <= plugin.Config.porcent;

            if (plugin.Config.UseGenerators)
            {
                int activeGenerators = Generator.List.Where(p => p.Activating).Count();
                if (activeGenerators != plugin.Config.Generators)
                {
                    player.SendBroadcast($"{plugin.Config.TextGenerators}{activeGenerators} / {plugin.Config.Generators}", 4);
                    return;
                }
            }

            if (!success)
            {
                playerAlive = 0;
                player.SendBroadcast(plugin.Config.OnFailure, 4);
            }
            else
            {
                playerAlive = 3;
                player.SendBroadcast(plugin.Config.OnDeath, 4);
                AffectScp106();
                Extension(plugin.Config.npc);
            }
        }

        private void AffectScp106()
        {
            List<Player> scp106 = Player.List.Where(p => p.Role == RoleTypeId.Scp106).ToList();
            foreach (Player p in scp106)
            {
                p.Kill();
            }
        }

        public void Extension(bool YT)
        {
            if (!plugin.Config.SoundOrNotsound)
            {
                Announcer.Message(plugin.Config.Cassie, plugin.Config.Cassie, true, 1, 0);
                //Cassie.Message();
                return;
            }
        }

        public void OnRestart(RoundEndedEventArgs ev)
        {
            playerAlive = 0;
        }
    }
}

namespace FemurBreakerTertingVersion
{
    using Merr = ProjectMER.Events.Handlers.Schematic;
    using System;
    using LabApi.Loader.Features.Plugins;
    using LabApi.Features;

    public class Plugin : Plugin<Config>
    {
        public override string Name => "FemurBreaker";
        public override string Author => "davilone32";
        public override Version Version => new Version(1, 4, 2);

        public override string Description => "femurBreaker";

        public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;

        private EventHandlers Handlers;
        public override void Enable()
        {

            Handlers = new EventHandlers(this);
            LabApi.Events.Handlers.ServerEvents.RoundEnded += Handlers.OnRestart;
            Merr.ButtonInteracted += Handlers.OnTrap;
        }
        public override void Disable()
        {
            LabApi.Events.Handlers.ServerEvents.RoundEnded -= Handlers.OnRestart;
            Merr.ButtonInteracted -= Handlers.OnTrap;
            Handlers = null;
        }
    }
}

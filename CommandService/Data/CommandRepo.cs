using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _ctx;

        public CommandRepo(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var platformExists = PlatformExists(platformId);
            if (platformExists)
            {
                command.PlatformId = platformId;
                _ctx.Commands.Add(command);
            }
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            _ctx.Platforms.Add(platform);
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            var result = _ctx.Platforms.Any(p => p.ExternalId == externalPlatformId);
            return result;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            var result = _ctx.Platforms.ToList();
            return result;
        }

        public Command GetCommand(int platformId, int commandId)
        {
            var result = _ctx.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
            return result;
        }

        public IEnumerable<Command> GetCommandsByPlatformId(int platformId)
        {
            var result = _ctx.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name)
                .ThenBy(c => c.Id);
            return result;
        }

        public bool PlatformExists(int platformId)
        {
            var result = _ctx.Platforms.Any(p => p.Id == platformId);
            return result;
        }

        public bool SaveChanges()
        {
            var result = (_ctx.SaveChanges() >= 0);
            return result;
        }
    }
}

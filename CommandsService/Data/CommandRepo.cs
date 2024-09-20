using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _dbContext;
        public CommandRepo(AppDbContext context)
        {
            _dbContext = context;
        }
        public void CreateCommand(Command command, int PlatformId)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.PlatformId = PlatformId;

            _dbContext.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if(platform == null) throw new ArgumentNullException(nameof(platform));

            _dbContext.Platforms.Add(platform);
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _dbContext.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _dbContext.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.platform.Name);
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            return _dbContext.Platforms.ToList();
        }

        public bool PlatformExists(int platformId)
        {
            return _dbContext.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);  
        }
    }
}

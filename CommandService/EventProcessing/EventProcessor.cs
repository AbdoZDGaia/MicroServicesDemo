using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory,
            IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventTypeEnum.PlatformPublished:
                    AddPlatforom(message);
                    break;
                default:
                    break;
            }
        }

        private EventTypeEnum DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventTypeEnum.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventTypeEnum.Undetermined;
            }
        }

        private void AddPlatforom(string platformPublishedMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublishDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage);

                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishDto);
                    if (!repo.ExternalPlatformExists(platform.ExternalId))
                    {
                        repo.CreatePlatform(platform);
                        repo.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("--> Could not add Platform to DB " + ex.Message);
                }
            }
        }
    }
}
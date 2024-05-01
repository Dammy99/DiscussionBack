using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace DiscussionBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFirebaseClient _client;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFirebaseClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpPost("decrement/{id}")]
        public async Task DecrementDiscussinById(string id)
        {
            var dis = (await _client.GetAsync("DisList/" + id)).ResultAs<Discussion>();
            dis.Votes -= 1;
            await _client.UpdateAsync("DisList/" + id, dis);
        }

        [HttpPost("increment/{id}")]
        public async Task IncrementDiscussinById(string id)
        {
            var dis = (await _client.GetAsync("DisList/" + id)).ResultAs<Discussion>();
            dis.Votes += 1;
            await _client.UpdateAsync("DisList/" + id, dis);
        }

        [HttpPost("postDiscussion")]
        public async Task<string?> PostNewDiscussionAsync([FromBody] DiscussionDto discussionRequest)
        {
            var discussion = new Discussion() { Name = discussionRequest.Name, Description = discussionRequest.Description };
            await _client.SetAsync("DisList/" + discussion.Id, discussion);
            return discussion.Description;
        }

        [HttpGet]
        public async Task<IEnumerable<Discussion>> GetListOfDiscussionsAsync()
        {
            var disList = (await _client.GetAsync("DisList")).ResultAs<IDictionary<string, Discussion>>();
            ConcurrentBag<Discussion> sortedList = new();
            if (disList != null)
            {
                var sorted = disList.OrderBy(x => x.Value.DateTime).AsEnumerable();
                foreach (var item in sorted)
                {
                    sortedList.Add(item.Value);
                }
            }
            return sortedList;
        }
    }

    public class Discussion()
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public int Votes { get; set; }
        public int VotesToDelete { get; set; }
        public DateTime PlanedTime { get; set; }
    }
    public class DiscussionDto()
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string PlanedTime { get; set; }
    }
}

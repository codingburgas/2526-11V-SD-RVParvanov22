namespace MicrobloggingSystem.Models.ViewModels
{
    public class TopUserViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int FollowersCount { get; set; }
    }
}

namespace Buccaneer.Models.DTOs;

class GetFollowerDTO
{
    public int Id { get; set; }
    public int PirateId { get; set; }
    public int FollowerId { get; set; }
    public GetFollowerPirateDTO Pirate { get; set; }
}

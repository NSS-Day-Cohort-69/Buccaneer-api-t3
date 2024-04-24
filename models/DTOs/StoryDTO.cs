namespace Buccaneer.Models.DTOs;

class StoryDTO
{
    public int Id { get; set; }
    public int PirateId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public PirateDTO Pirate { get; set; }
}

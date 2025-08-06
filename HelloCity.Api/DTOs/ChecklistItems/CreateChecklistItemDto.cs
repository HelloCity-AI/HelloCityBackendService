


namespace HelloCity.Api.DTOs.ChecklistItemDto
{
  public class CreateChecklistItemDto
  {
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Gender Gender { get; set; }
    public string? Nationality { get; set; }
    public string? City { get; set; }
    public PreferredLanguage PreferredLanguage { get; set; }
  }
}
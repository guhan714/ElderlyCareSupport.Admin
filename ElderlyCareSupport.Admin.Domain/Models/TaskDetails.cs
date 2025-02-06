using ElderlyCareSupport.Admin.Domain.ValueObjects;

namespace ElderlyCareSupport.Admin.Domain.Models;

public class TaskDetails
{
    public int TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TaskStatusId { get; set; }
    public int TaskCategoryId { get; set; }
    public long  ElderlyPersonId { get; set; }
    public int PreferredGender  { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public VolunteerDetails VolunteerAccount { get; set; } = null!;
}
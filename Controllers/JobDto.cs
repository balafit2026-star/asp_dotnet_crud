namespace JobApi.JobDto;

public class CreateJobRequest
{
    public string Title { get; set; } = string.Empty;
 }
public class UpdateJobRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
 }
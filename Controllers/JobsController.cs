namespace JobApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using JobApi.Models;
using JobApi.Data;
using JobApi.JobDto;
using Microsoft.EntityFrameworkCore;
 
[ApiController]
[Route("api/[controller]")]

public class JobsController : ControllerBase
{
    private readonly AppDbContext _context;
    public JobsController(AppDbContext context)
    {
        _context = context;
    }
     

    [HttpHead("{id}")] //for returining exist or not by status code
    public async Task<IActionResult> HeadJob(int id)
    {
        var exists = await _context.Jobs.AnyAsync(j => j.Id == id);
        return exists ? Ok() : NotFound();
    }

    [HttpDelete("{id}")] // for delete job by id
    public async Task<IActionResult> DeleteJob(int id)
    {
        var rows = await _context.Jobs
            .Where(j => j.Id == id)
            .ExecuteDeleteAsync();

        return rows == 0 ? NotFound() : NoContent();
    }


    [HttpPatch("{id}")] // for update only needed columns by id
    public async Task<IActionResult> PatchJob(int id, UpdateJobRequest request)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job is null)
            return NotFound();

        if (request.Title is not null) job.Title = request.Title;
        await _context.SaveChangesAsync();
        return NoContent();
    }


    [HttpPut("{id}")] // for update entire row by id
    public async Task<IActionResult> UpdateJobs(int id, UpdateJobRequest request)
    {


        var job = await _context.Jobs.FindAsync(id);
        if (job is null)
            return NotFound();

        job.Title = request.Title;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost] // for create  row by post
    public async Task<ActionResult<Job>> PostJobs(CreateJobRequest request)
    {
        var job = new Job
        {
            Title = request.Title,

        };

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJobs), job);

    }

    [HttpGet] //get all
    public IActionResult GetJobs()
    {
        var jobs = _context.Jobs.ToList();
        return Ok(jobs);
    }

    [HttpGet("{id}")] //get by id
    public IActionResult GetJobsById(int id)
    {
        var jobs = _context.Jobs.FindAsync(id);
        return Ok(jobs.Result);
    }


    [HttpGet("count")] // count(*)
    public IActionResult GetTest()
    {
        var jobs = _context.Jobs.ToList().Count();
        return Ok(jobs);
    }
}
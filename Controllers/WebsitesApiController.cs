using System.ComponentModel.DataAnnotations;
using FakeApi.Data;
using FakeApiSpec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fake_api.Controllers;

public class WebsitesApiController : FakeApiSpec.Controllers.WebsitesApiController
{
  private readonly WebsiteContext _context;

  public WebsitesApiController(WebsiteContext context)
  {
    _context = context;
  }

  public override async Task<IActionResult> WebsitesCreate([FromBody] Website website)
  {
    _context.Websites.Add(website);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(WebsitesRead), new { id = website.Id }, website);
  }

  public override async Task<IActionResult> WebsitesDelete([FromRoute(Name = "id"), Required] string id)
  {
    var website = await _context.Websites.FindAsync(id);
    if (website is null)
      return NotFound($"Website with id '{id}' not found.");
    _context.Websites.Remove(website);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  public override async Task<IActionResult> WebsitesList([FromQuery(Name = "name")] string? name)
  {
    return await _context.Websites
      .Where(w => string.IsNullOrEmpty(name) || w.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
      .ToArrayAsync()
      .ContinueWith(task => Ok(task.Result));
  }

  public override async Task<IActionResult> WebsitesRead([FromRoute(Name = "id"), Required] string id)
  {
    var website = await _context.Websites.FindAsync(id);
    if (website is null)
      return NotFound($"Website with id '{id}' not found.");
    return Ok(website);
  }

  public override async Task<IActionResult> WebsitesUpdate([FromRoute(Name = "id"), Required] string id, [FromBody] WebsiteMergePatchUpdate websiteMergePatchUpdate)
  {
    var website = await _context.Websites.FindAsync(id);
    if (website is null)
      return NotFound($"Website with id '{id}' not found.");
    if (websiteMergePatchUpdate.Name != null)
      website.Name = websiteMergePatchUpdate.Name;
    if (websiteMergePatchUpdate.Description != null)
      website.Description = websiteMergePatchUpdate.Description;
    if (websiteMergePatchUpdate.Url != null)
      website.Url = websiteMergePatchUpdate.Url;
    _context.Websites.Update(website);
    await _context.SaveChangesAsync();
    return Ok(website);
  }
}

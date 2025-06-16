using System.ComponentModel.DataAnnotations;
using FakeApi.Data;
using FakeApiSpec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FakeApiSpec.Models.Widget;

namespace fake_api.Controllers;

public class WidgetsApiController : FakeApiSpec.Controllers.WidgetsApiController
{
  private readonly WidgetContext _context;

  public WidgetsApiController(WidgetContext context)
  {
    _context = context;
  }

  public override async Task<IActionResult> WidgetsAnalyze([FromRoute(Name = "id"), Required] string id)
  {
    var widget = await _context.Widgets.FindAsync(id);
    if (widget is null)
      return NotFound($"Widget with id '{id}' not found.");

    return Ok(new FakeApiSpec.Models.AnalyzeResult
    {
      Id = widget.Id,
      Analysis = $"Widget '{widget.Id}' with color '{widget.Color}' analyzed successfully."
    });
  }

  public override async Task<IActionResult> WidgetsCreate([FromBody] Widget widget)
  {
    _context.Widgets.Add(widget);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(WidgetsRead), new { id = widget.Id }, widget);
  }

  public override async Task<IActionResult> WidgetsDelete([FromRoute(Name = "id"), Required] string id)
  {
    var widget = await _context.Widgets.FindAsync(id);
    if (widget is null)
      return NotFound($"Widget with id '{id}' not found.");
    _context.Widgets.Remove(widget);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  public override async Task<IActionResult> WidgetsList([FromQuery(Name = "color")] WidgetColor? color)
  {
    var widgets = await _context.Widgets
        .Where(w => (color == null) || w.Color.Equals(color))
        .ToListAsync();
    return Ok(new WidgetList { Items = widgets });
  }

  public override async Task<IActionResult> WidgetsRead([FromRoute(Name = "id"), Required] string id)
  {
    var widget = await _context.Widgets.FindAsync(id);
    if (widget is null)
      return NotFound($"Widget with id '{id}' not found.");
    return Ok(widget);
  }

  public override async Task<IActionResult> WidgetsUpdate([FromRoute(Name = "id"), Required] string id, [FromBody] WidgetMergePatchUpdate widgetMergePatchUpdate)
  {
    var widget = await _context.Widgets.FindAsync(id);
    if (widget is null)
      return NotFound($"Widget with id '{id}' not found.");

    widget.Color = widgetMergePatchUpdate.Color;
    if (widgetMergePatchUpdate.Weight.HasValue)
      widget.Weight = widgetMergePatchUpdate.Weight.Value;

    await _context.SaveChangesAsync();
    return NoContent();
  }
}

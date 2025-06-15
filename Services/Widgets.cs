//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using DemoService;
using Microsoft.EntityFrameworkCore;
using FakeApi.Data;

namespace Services
{
    public class Widgets : IWidgets
    {
        private readonly WidgetContext _context;

        public Widgets(WidgetContext context)
        {
            _context = context;
        }

        public async Task<WidgetList> ListAsync(string? color)
        {
            // Filter widgets by color if provided
            var filteredWidgets = string.IsNullOrEmpty(color)
                ? await _context.Widgets.ToListAsync()
                : await _context.Widgets
                    .Where(w => w.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                    .ToListAsync();

            //Console.WriteLine($"Listing widgets with color '{color}'...");
            return new WidgetList
            {
                Items = filteredWidgets.ToArray()
            };
        }

        public async Task<Widget> ReadAsync(string id)
        {
            var widget = await _context.Widgets.FindAsync(id);
            if (widget == null)
                throw new KeyNotFoundException($"Widget with id '{id}' not found.");
            return widget;
        }

        public async Task<Widget> CreateAsync(Widget body)
        {
            if (await _context.Widgets.AnyAsync(w => w.Id == body.Id))
                throw new InvalidOperationException($"Widget with id '{body.Id}' already exists.");
            _context.Widgets.Add(body);
            await _context.SaveChangesAsync();
            return body;
        }

        public async Task<Widget> UpdateAsync(string id, TypeSpec.Http.WidgetMergePatchUpdate body)
        {
            var widget = await _context.Widgets.FindAsync(id);
            if (widget == null)
                throw new KeyNotFoundException($"Widget with id '{id}' not found.");

            // Apply patch (assuming WidgetMergePatchUpdate has nullable properties)
            if (body.Weight != null)
                widget.Weight = body.Weight.Value;
            if (!String.IsNullOrEmpty(body.Color))
                widget.Color = body.Color;
            // Add more fields as needed

            _context.Widgets.Update(widget);
            await _context.SaveChangesAsync();
            return widget;
        }

        public async Task DeleteAsync(string id)
        {
            var widget = await _context.Widgets.FindAsync(id);
            if (widget == null)
                throw new KeyNotFoundException($"Widget with id '{id}' not found.");
            _context.Widgets.Remove(widget);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<AnalyzeResult> AnalyzeAsync(string id)
        {
            var widget = await _context.Widgets.FindAsync(id);
            if (widget == null)
                throw new KeyNotFoundException($"Widget with id '{id}' not found.");

            var result = new AnalyzeResult
            {
                Id = id,
                Analysis = $"Widget '{widget.Id}' analyzed successfully."
            };
            return result;
        }
    }
}
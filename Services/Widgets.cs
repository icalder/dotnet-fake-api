//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using DemoService;

namespace Services
{
    public class Widgets : IWidgets
    {
        // Use a generic List<Widget> as the backing store
        private static readonly List<Widget> _widgets = new List<Widget>();

        public Task<WidgetList> ListAsync(string? color)
        {
            // Filter widgets by color if provided
            var filteredWidgets = string.IsNullOrEmpty(color)
                ? _widgets
                : _widgets.Where(w => w.Color.Equals(color, StringComparison.OrdinalIgnoreCase)).ToList();

            //Console.WriteLine($"Listing widgets with color '{color}'...");
            var list = new WidgetList
            {
                Items = filteredWidgets.ToArray()
            };
            return Task.FromResult(list);
        }

        public Task<Widget> ReadAsync(string id)
        {
            var widget = _widgets.FirstOrDefault(w => w.Id == id);
            if (widget == null)
                throw new KeyNotFoundException($"Widget with id '{id}' not found.");
            return Task.FromResult(widget);
        }

        public Task<Widget> CreateAsync(Widget body)
        {
            if (_widgets.Any(w => w.Id == body.Id))
                throw new InvalidOperationException($"Widget with id '{body.Id}' already exists.");
            _widgets.Add(body);
            return Task.FromResult(body);
        }

        public Task<Widget> UpdateAsync(string id, TypeSpec.Http.WidgetMergePatchUpdate body)
        {
            var widget = _widgets.FirstOrDefault(w => w.Id == id);
            if (widget == null)
                throw new KeyNotFoundException($"Widget with id '{id}' not found.");

            // Apply patch (assuming WidgetMergePatchUpdate has nullable properties)
            if (body.Weight != null)
                widget.Weight = body.Weight.Value;
            if (!String.IsNullOrEmpty(body.Color))
                widget.Color = body.Color;
            // Add more fields as needed

            return Task.FromResult(widget);
        }

        public Task DeleteAsync(string id)
        {
            var widget = _widgets.FirstOrDefault(w => w.Id == id);
            if (widget == null)
                throw new KeyNotFoundException($"Widget with id '{id}' not found.");
            _widgets.Remove(widget);
            return Task.CompletedTask;
        }

        public Task<AnalyzeResult> AnalyzeAsync(string id)
        {
            var widget = _widgets.FirstOrDefault(w => w.Id == id);
            if (widget == null)
                throw new KeyNotFoundException($"Widget with id '{id}' not found.");

            var result = new AnalyzeResult
            {
                Id = id,
                Analysis = $"Widget '{widget.Id}' analyzed successfully."
            };
            return Task.FromResult(result);
        }
    }
}
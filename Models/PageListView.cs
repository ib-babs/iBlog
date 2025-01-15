namespace iBlog.Models
{
    public class PageListView
    {
        public IEnumerable<BlogModel>? Blogs { get; set; }
        public string? Category { get; set; }
        public List<string>? Categories { get; set; }
        public PageInfo? PageInfo { get; set; }
        [TempData]
        public string? Message { get; set; }
    }
}

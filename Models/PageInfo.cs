namespace iBlog.Models;

public class PageInfo{
    public int CurrentPage {get; set;}
    public int ItemPerPage {get; set;}
    public int TotalItem {get; set;}
    public decimal TotalPages => Math.Ceiling((decimal) TotalItem / ItemPerPage);
}
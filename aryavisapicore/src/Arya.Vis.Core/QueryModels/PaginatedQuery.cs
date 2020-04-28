namespace Arya.Vis.Core.QueryModels
{
     public class PaginatedQuery {
        public int From { get; set; }
        public int? Size { get; set; } = 10; 
    }
}
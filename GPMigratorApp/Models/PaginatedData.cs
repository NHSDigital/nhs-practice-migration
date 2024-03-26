namespace GPMigratorApp.Models;

public class PaginatedData<T> where T : class {
    
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    
        public PaginatedData(int count, IEnumerable<T> data) {
            
            TotalCount = count;
            Data = data;
        }
    
}
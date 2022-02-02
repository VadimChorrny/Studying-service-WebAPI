using System.Collections.Generic;

namespace DTO
{
    public class PaginationDto<T>
    {
        public  ICollection<T> Collection { get; set; }
        
        public MetadataPaginationDto MetadataPaginationDto { get; set; }
    }
}
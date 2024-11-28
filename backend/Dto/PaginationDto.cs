namespace Backend.Dto
{
    public class PaginationDto<T>
    {
        public int TotalPages { get; set; }
        public ICollection<T> Result { get; set; }
    }
}
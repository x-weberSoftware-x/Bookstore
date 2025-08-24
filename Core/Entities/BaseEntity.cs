namespace Core.Entities;

public class BaseEntity
{
    //entity framework(EF) is convention based and if we call this Id then EF knows this is primary key
    public int Id { get; set; }
    
}

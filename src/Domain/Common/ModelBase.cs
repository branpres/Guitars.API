namespace Domain.Common;

public abstract class ModelBase
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }

    public bool IsDeleted { get; set; }
}
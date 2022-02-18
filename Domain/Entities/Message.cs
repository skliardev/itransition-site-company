using System.ComponentModel.DataAnnotations;

namespace Company.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }

    public Guid UserFrom { get; set; }

    public Guid UserTo { get; set; }

    [Display(Name = "Title message")]
    public string Title { get; set; } = "";

    [Display(Name = "Message")]
    public string Text { get; set; } = "";

    [Display(Name = "Published")]
    public DateTime Published { get; set; }
}

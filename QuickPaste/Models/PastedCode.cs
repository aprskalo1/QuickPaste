using System;
using System.Collections.Generic;

namespace QuickPaste.Models;

public partial class PastedCode
{
    public int Id { get; set; }

    public string CodeHash { get; set; } = null!;

    public virtual ICollection<FileStorage> FileStorages { get; set; } = new List<FileStorage>();
}

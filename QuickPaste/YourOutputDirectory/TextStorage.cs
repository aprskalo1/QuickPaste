using System;
using System.Collections.Generic;

namespace QuickPaste.YourOutputDirectory;

public partial class TextStorage
{
    public int Id { get; set; }

    public DateTime? TimeUploaded { get; set; }

    public string HashedCode { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace QuickPaste.YourOutputDirectory;

public partial class FileStorage
{
    public int Id { get; set; }

    public DateTime? TimeUploaded { get; set; }

    public string Filename { get; set; } = null!;

    public string HashedCode { get; set; } = null!;
}

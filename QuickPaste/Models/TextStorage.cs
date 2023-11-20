using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace QuickPaste.Models;

public partial class TextStorage
{
    [JsonProperty("id")]
    public string Id { get; set; } = null!;

    public DateTime? TimeUploaded { get; set; }

    public string HashedCode { get; set; } = null!;

    public string TextContent { get; set; } = null!;
}

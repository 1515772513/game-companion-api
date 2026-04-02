using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

[Keyless]
[Table("recover_your_data_info")]
[MySqlCharSet("utf8mb3")]
[MySqlCollation("utf8mb3_general_ci")]
public partial class RecoverYourDataInfo
{
    [Column("READ_ME", TypeName = "text")]
    public string? ReadMe { get; set; }
}

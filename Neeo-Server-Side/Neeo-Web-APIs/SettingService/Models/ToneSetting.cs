﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SettingsService.Models
{
    public class ToneSetting : BaseRequest
    {
        [Required]
        public ushort IMTone { get; set; }
    }
}
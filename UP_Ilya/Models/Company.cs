﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace UP_Ilya.Models;

public partial class Company
{
    public int CompanyID { get; set; }

    public string CompanyName { get; set; }

    public string CompanyPhone { get; set; }

    public virtual ICollection<TV_Show> TV_Shows { get; set; } = new List<TV_Show>();
}
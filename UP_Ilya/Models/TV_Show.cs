﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace UP_Ilya.Models;

public partial class TV_Show
{
    public int TVShowID { get; set; }

    public int CompanyID { get; set; }

    public string TVShowName { get; set; }

    public string AgeRating { get; set; }

    public TimeOnly PrimeTime { get; set; }

    public DateOnly LiveDate { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<Watcher_TV_Show> Watcher_TV_Shows { get; set; } = new List<Watcher_TV_Show>();
}
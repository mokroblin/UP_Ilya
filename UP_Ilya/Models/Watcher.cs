﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace UP_Ilya.Models;

public partial class Watcher
{
    public int WatcherID { get; set; }

    public int UserID { get; set; }

    public string PersonalData { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<Watcher_TV_Show> Watcher_TV_Shows { get; set; } = new List<Watcher_TV_Show>();
}
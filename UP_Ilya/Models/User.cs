﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace UP_Ilya.Models;

public partial class User
{
    public int UserID { get; set; }

    public string UserName { get; set; }

    public string UserMail { get; set; }

    public string UserPass { get; set; }

    public string UserRole { get; set; }

    public virtual ICollection<Watcher> Watchers { get; set; } = new List<Watcher>();
}